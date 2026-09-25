using Microsoft.AspNetCore.Mvc;
using Biblioteca.Servicios;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatalogoService _catalogoService;

        // El constructor tiene singleton
        public HomeController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        // Esta acción muestra la página principal
        [HttpGet]
        public IActionResult Index()
        {
            // Pasamos datos básicos a la vista si es necesario, 
            // como la cantidad de libros actuales
            ViewBag.TotalLibros = _catalogoService.CatalogoPrincipal.IndiceLibros.Cantidad;
            ViewBag.TotalCategorias = _catalogoService.CatalogoPrincipal.IndiceCategorias.Cantidad;
            
            return View();
        }

        // Requerimiento a: Inicialización del Sistema sin información previa
        [HttpPost]
        public IActionResult InicializarSistema()
        {
            _catalogoService.InicializarSistema();
            TempData["MensajeExito"] = "El sistema se ha inicializado y borrado toda la información previa.";
            
            // Redirige de vuelta a la página principal
            return RedirectToAction("Index");
        }
    }
}