using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    /// Lista enlazada simple con puntero al final. Igual que ListaLibros, pero para devolver categorías en orden alfabético.
    public class ListaCategorias
    {
        private NodoListaCategoria ultimo;

        public NodoListaCategoria Primero { get; private set; }
        public int Cantidad { get; private set; }

        public void Agregar(Categoria categoria)
        {
            NodoListaCategoria nuevo = new NodoListaCategoria(categoria);
            if (Primero == null)
            {
                Primero = nuevo;
            }
            else
            {
                ultimo.Siguiente = nuevo;
            }
            ultimo = nuevo;
            Cantidad++;
        }
    }
}