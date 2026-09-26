using Microsoft.AspNetCore.Mvc;
using Biblioteca.Servicios;
using Biblioteca.Dominio;
using Biblioteca.TDAs;

namespace Biblioteca.Presentacion.Controllers
{
    public class LibrosController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public LibrosController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet]
        public IActionResult Index(long? isbnBusqueda, string categoria, string subcategoria)
        {
            var indice = _catalogoService.CatalogoPrincipal.IndiceLibros;
            var categorias = ObtenerCategorias();
            Categoria categoriaSeleccionada = null;

            if (!string.IsNullOrWhiteSpace(subcategoria))
            {
                categoriaSeleccionada = _catalogoService.CatalogoPrincipal.BuscarCategoria(subcategoria);
            }
            else if (!string.IsNullOrWhiteSpace(categoria))
            {
                categoriaSeleccionada = _catalogoService.CatalogoPrincipal.BuscarCategoria(categoria);
            }

            ViewBag.Libros = categoriaSeleccionada == null
                ? indice.ObtenerAscendente()
                : categoriaSeleccionada.ObtenerLibrosConDescendientes().ObtenerAscendente();
            ViewBag.Categorias = categorias;
            ViewBag.CategoriaSeleccionada = categoria;
            ViewBag.SubcategoriaSeleccionada = subcategoria;

            // Requerimiento: Mostrar el menor y el mayor ISBN
            ViewBag.Minimo = indice.Minimo(); 
            ViewBag.Maximo = indice.Maximo();

            // Requerimiento: Buscar por ISBN
            if (isbnBusqueda.HasValue)
            {
                ViewBag.LibroBuscado = _catalogoService.CatalogoPrincipal.BuscarLibro(isbnBusqueda.Value);
                ViewBag.BusquedaRealizada = true;
                ViewBag.IsbnBuscado = isbnBusqueda.Value;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Categorias = ObtenerCategorias();
            return View();
        }

        [HttpPost]
        public IActionResult Crear(long isbn, string titulo, string autor, string categoria)
        {
            // Requerimiento: Registrar libro
            bool exito = _catalogoService.CatalogoPrincipal.AgregarLibro(isbn, titulo, autor, categoria);
            if (exito)
                TempData["Exito"] = $"Libro con ISBN {isbn} registrado correctamente.";
            else
                TempData["Error"] = $"Error al registrar. ISBN duplicado o categoría '{categoria}' inexistente.";
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(long isbn)
        {
            // Requerimiento: Eliminar libro (la lógica en tu clase Catalogo debe sacarlo de ambos árboles)
            bool exito = _catalogoService.CatalogoPrincipal.EliminarLibro(isbn);
            if (exito)
                TempData["Exito"] = $"Libro con ISBN {isbn} eliminado del catálogo global y su categoría respectiva.";
            else
                TempData["Error"] = $"No se pudo eliminar el libro. El ISBN {isbn} no existe.";

            return RedirectToAction("Index");
        }

        private ListaCategorias ObtenerCategorias()
        {
            return _catalogoService.CatalogoPrincipal.IndiceCategorias.ObtenerOrdenadas();
        }
    }
}