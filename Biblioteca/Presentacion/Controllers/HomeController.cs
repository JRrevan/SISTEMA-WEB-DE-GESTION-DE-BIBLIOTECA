using Microsoft.AspNetCore.Mvc;
using Biblioteca.Servicios;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatalogoService _catalogoService;
        private readonly IWebHostEnvironment _environment;

        // El constructor tiene singleton
        public HomeController(CatalogoService catalogoService, IWebHostEnvironment environment)
        {
            _catalogoService = catalogoService;
            _environment = environment;
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

        [HttpGet]
        public IActionResult Ayuda()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Documento()
        {
            var documento = Path.Combine(
                _environment.ContentRootPath,
                "Docs",
                "Ensayo-IPC2-lab _202400128.pdf");

            return PhysicalFile(documento, "application/pdf");
        }

    }


}