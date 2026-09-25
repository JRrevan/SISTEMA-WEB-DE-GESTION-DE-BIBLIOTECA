using Microsoft.AspNetCore.Mvc;
using Biblioteca.Servicios;
using Biblioteca.Dominio;

namespace Biblioteca.Presentacion.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public CategoriasController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
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

            return View(nodoInicio);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
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