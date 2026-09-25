using System.IO;
using Biblioteca.Dominio;

namespace Biblioteca.Servicios
{
    /// Servicio orquestador encargado de coordinar la lectura del XML  y la inserción de los datos en el Catálogo principal.
    public class CargaXmlService
    {
        private readonly CatalogoService _catalogoService;

        // Inyectamos el servicio Singleton que contiene nuestro catálogo en memoria
        public CargaXmlService(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public ResultadoCarga ProcesarArchivoXml(Stream xmlStream)
        {
            LectorXmlEntrada lector = new LectorXmlEntrada();
            // Delegamos el procesamiento al lector, pasándole el stream y la instancia global del catálogo
            return lector.Procesar(xmlStream, _catalogoService.CatalogoPrincipal);
        }
    }
}