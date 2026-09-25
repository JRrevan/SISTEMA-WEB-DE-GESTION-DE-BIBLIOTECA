using Biblioteca.TDAs;

namespace Biblioteca.Dominio
{
    /// Una categoría del catálogo. Puede tener subcategorías (Hijos)   libros propios (Libros). Ambos se guardan en árboles AVL para que
    /// aparezcan siempre ordenados: los hijos alfabéticamente por nombre, los libros ascendentemente por ISBN.
    public class Categoria
    {
        public string Nombre { get; }

        // Null solo para la raíz virtual del Catalogo, que no tiene padre.
        public Categoria Padre { get; }

        public ArbolAVLCategorias Hijos { get; }
        public ArbolAVLLibros Libros { get; }

        public Categoria(string nombre, Categoria padre)
        {
            Nombre = nombre;
            Padre = padre;
            Hijos = new ArbolAVLCategorias();
            Libros = new ArbolAVLLibros();
        }

        /// Todos los libros de esta categoría y de todas sus subcategorías, combinados en un solo árbol ordenado por ISBN.
        /// Sirve para "mostrar libros desde una subcategoría" y para el gráfico de Graphviz cuando se pide incluir descendientes.
        public ArbolAVLLibros ObtenerLibrosConDescendientes()
        {
            ArbolAVLLibros resultado = new ArbolAVLLibros();
            AcumularLibros(this, resultado);
            return resultado;
        }

        private static void AcumularLibros(Categoria categoria, ArbolAVLLibros destino)
        {
            ListaLibros propios = categoria.Libros.ObtenerAscendente();
            NodoListaLibro actual = propios.Primero;
            while (actual != null)
            {
                destino.Insertar(actual.Dato);
                actual = actual.Siguiente;
            }

            ListaCategorias hijas = categoria.Hijos.ObtenerOrdenadas();
            NodoListaCategoria hijo = hijas.Primero;
            while (hijo != null)
            {
                AcumularLibros(hijo.Dato, destino);
                hijo = hijo.Siguiente;
            }
        }
    }
}