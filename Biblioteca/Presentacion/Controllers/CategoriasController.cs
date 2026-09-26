using Microsoft.AspNetCore.Mvc;
using Biblioteca.Servicios;
using Biblioteca.Dominio;
using Biblioteca.TDAs;

namespace Biblioteca.Presentacion.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CatalogoService _catalogoService;
        private readonly GraphvizService _graphvizService;

        public CategoriasController(CatalogoService catalogoService, GraphvizService graphvizService)
        {
            _catalogoService = catalogoService;
            _graphvizService = graphvizService;
        }

        [HttpGet]
        public IActionResult Index(string subcategoria = null)
        {
            Categoria nodoInicio = null;
            
            // Requerimiento: Estructura HTML con opción a partir desde una subcategoría
            if (!string.IsNullOrWhiteSpace(subcategoria))
            {
                nodoInicio = _catalogoService.CatalogoPrincipal.BuscarCategoria(subcategoria);
                if (nodoInicio == null)
                {
                    ViewBag.Error = $"La categoría '{subcategoria}' no existe. Mostrando desde la raíz.";
                    nodoInicio = _catalogoService.CatalogoPrincipal.Raiz;
                }
            }
            else
            {
                nodoInicio = _catalogoService.CatalogoPrincipal.Raiz;
            }

            ViewBag.Categorias = ObtenerCategorias();
            ViewBag.CategoriaSeleccionada = subcategoria;
            return View(nodoInicio);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Categorias = ObtenerCategorias();
            return View();
        }
        
        [HttpGet]
        public IActionResult GraficoGeneral(string subcategoria = null)
        {
            var raiz = _catalogoService.CatalogoPrincipal.Raiz;
            if (!string.IsNullOrWhiteSpace(subcategoria))
            {
                raiz = _catalogoService.CatalogoPrincipal.BuscarCategoria(subcategoria);
                if (raiz == null)
                {
                    TempData["Error"] = $"La categoría '{subcategoria}' no existe.";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.NombreRaizGrafico = raiz.Nombre;
            ViewBag.RutaSvg = _graphvizService.GenerarGraficoCategorias(raiz);
            return View();
        }

        [HttpGet]
        public IActionResult GraficoArbolLibros()
        {
            ViewBag.RutaSvg = _graphvizService.GenerarGraficoArbolLibros(
                _catalogoService.CatalogoPrincipal.IndiceLibros);
            return View();
        }

        [HttpGet]
        public IActionResult GraficoLibros(string subcategoria)
        {
            if (string.IsNullOrWhiteSpace(subcategoria))
            {
                TempData["Error"] = "Debe especificar una categoría para graficar sus libros.";
                return RedirectToAction("Index");
            }

            var categoria = _catalogoService.CatalogoPrincipal.BuscarCategoria(subcategoria);
            if (categoria == null)
            {
                TempData["Error"] = $"La categoría '{subcategoria}' no existe.";
                return RedirectToAction("Index");
            }

            ViewBag.NombreCategoria = categoria.Nombre;
            ViewBag.RutaSvg = _graphvizService.GenerarGraficoLibrosCategoria(categoria);
            return View();
        }

        private ListaCategorias ObtenerCategorias()
        {
            return _catalogoService.CatalogoPrincipal.IndiceCategorias.ObtenerOrdenadas();
        }

        [HttpPost]
        public IActionResult Crear(string nombre, string padre)
        {
            // Requerimiento: Agregar categoría
            bool exito = _catalogoService.CatalogoPrincipal.AgregarCategoria(nombre, padre);
            if (exito)
                TempData["Exito"] = $"Categoría '{nombre}' agregada al catálogo.";
            else
                TempData["Error"] = $"No se pudo agregar '{nombre}'. Ya existe o la categoría padre es inválida.";

            return RedirectToAction("Index");
        }
    }
}