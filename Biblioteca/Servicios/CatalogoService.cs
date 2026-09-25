using Biblioteca.Dominio;

namespace Biblioteca.Servicios
{
    public class CatalogoService
    {
        public Catalogo CatalogoPrincipal { get; private set; }

        public CatalogoService()
        {
            // Se inicializa el catálogo vacío al arrancar la aplicación
            CatalogoPrincipal = new Catalogo();
        }

        // Método para reiniciar el sistema (Requerimiento "a. Inicialización")
        public void InicializarSistema()
        {
            CatalogoPrincipal = new Catalogo();
        }
    }
}