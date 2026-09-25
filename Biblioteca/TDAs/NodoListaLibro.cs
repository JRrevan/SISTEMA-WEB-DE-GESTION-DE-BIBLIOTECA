using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    public class NodoListaLibro
    {
        public Libro Dato { get; }
        public NodoListaLibro Siguiente { get; set; }

        public NodoListaLibro(Libro dato)
        {
            Dato = dato;
        }
    }
}