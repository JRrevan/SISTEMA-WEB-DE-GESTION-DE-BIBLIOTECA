using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    /// Lista enlazada simple con puntero al final (agregar en O(1)).
    /// Sirve para devolver los libros en orden ascendente y recorrerlos
    /// con un while desde las vistas o desde el generador de DOT.
    public class ListaLibros
    {
        private NodoListaLibro ultimo;

        public NodoListaLibro Primero { get; private set; }
        public int Cantidad { get; private set; }

        public void Agregar(Libro libro)
        {
            NodoListaLibro nuevo = new NodoListaLibro(libro);
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