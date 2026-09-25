using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Biblioteca.Servicios;

namespace Biblioteca.Controllers
{
    public class CargaController : Controller
    {
        private readonly CargaXmlService _cargaXmlService;

        // Inyectamos el orquestador
        public CargaController(CargaXmlService cargaXmlService)
        {
            _cargaXmlService = cargaXmlService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormFile archivoXml)
        {
            if (archivoXml == null || archivoXml.Length == 0)
            {
                ViewBag.Error = "Por favor seleccione un archivo XML válido.";
                return View();
            }

            ResultadoCarga resultado;

            using (var stream = archivoXml.OpenReadStream())
            {
                // Llamamos al servicio, respetando la arquitectura de capas
                resultado = _cargaXmlService.ProcesarArchivoXml(stream);
            }

            ViewBag.Mensajes = resultado.Mensajes;
            ViewBag.Exito = $"Procesamiento finalizado. Se generaron {resultado.Mensajes.Cantidad} mensajes.";

            return View();
        }
    }
}