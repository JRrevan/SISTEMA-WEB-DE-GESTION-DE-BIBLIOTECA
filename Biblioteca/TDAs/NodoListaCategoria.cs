using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    public class NodoListaCategoria
    {
        public Categoria Dato { get; }
        public NodoListaCategoria Siguiente { get; set; }

        public NodoListaCategoria(Categoria dato)
        {
            Dato = dato;
        }
    }
}