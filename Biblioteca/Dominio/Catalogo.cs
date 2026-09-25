using Biblioteca.TDAs;

namespace Biblioteca.Dominio
{
    /// Raíz de todo el catálogo. Guarda una categoría raíz virtual (para que pueda haber varias categorías "de primer nivel" sin que el
    /// programa tenga que tratarlas como un caso especial) y dos índices globales que apuntan a los mismos objetos que ya cuelgan del árbol
    /// de categorías, para poder ubicarlos en O(log n) sin recorrer todo el árbol cada vez.

    public class Catalogo
    {
        // El nombre no importa: nunca se muestra ni se busca por él.
        private const string NombreRaiz = "__RAIZ__";

        public Categoria Raiz { get; private set; }

        public ArbolAVLCategorias IndiceCategorias { get; private set; }

        public ArbolAVLLibros IndiceLibros { get; private set; }

        public Catalogo()
        {
            Inicializar();
        }

        ///Deja el catálogo como recién creado, sin categorías nis libros. Es lo que ejecuta el botón "Inicialización" de la web.</summary>
        public void Inicializar()
        {
            Raiz = new Categoria(NombreRaiz, null);
            IndiceCategorias = new ArbolAVLCategorias();
            IndiceLibros = new ArbolAVLLibros();
        }

        //CATEGORÍAS

        /// Crea una categoría nueva bajo su padre (por nombre) y la agrega tanto al árbol de hijos de ese padre como al índice global.
        /// nombrePadre == null (o vacío) la cuelga directamente de la raíz.
        ///  Devuelve false si el nombre ya existe o si el padre no existe.
        public bool AgregarCategoria(string nombre, string nombrePadre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return false;
            if (IndiceCategorias.Buscar(nombre) != null) return false;

            Categoria padre = Raiz;
            if (!string.IsNullOrWhiteSpace(nombrePadre))
            {
                padre = IndiceCategorias.Buscar(nombrePadre);
                if (padre == null) return false;
            }

            Categoria nueva = new Categoria(nombre, padre);
            padre.Hijos.Insertar(nueva);
            IndiceCategorias.Insertar(nueva);
            return true;
        }

        ///Busca una categoría por nombre en todo el catálogo, sin importar en qué nivel esté.
        public Categoria BuscarCategoria(string nombre)
        {
            return IndiceCategorias.Buscar(nombre);
        }

        //LIBROS

        /// Agrega un libro a su categoría (por nombre) y lo registra en el índice global. Devuelve false si el ISBN ya existe o si la
        /// categoría no existe.
        public bool AgregarLibro(long isbn, string titulo, string autor, string nombreCategoria)
        {
            if (IndiceLibros.Buscar(isbn) != null) return false;

            Categoria categoria = IndiceCategorias.Buscar(nombreCategoria);
            if (categoria == null) return false;

            Libro libro = new Libro(isbn, titulo, autor, categoria);
            categoria.Libros.Insertar(libro);
            IndiceLibros.Insertar(libro);
            return true;
        }

        ///Busca un libro por ISBN en todo el catálogo.
        public Libro BuscarLibro(long isbn)
        {
            return IndiceLibros.Buscar(isbn);
        }

        /// Elimina un libro del índice global y del árbol de su propia categoría. Devuelve false si el ISBN no existe.
        public bool EliminarLibro(long isbn)
        {
            Libro libro = IndiceLibros.Buscar(isbn);
            if (libro == null) return false;

            libro.Categoria.Libros.Eliminar(isbn);
            IndiceLibros.Eliminar(isbn);
            return true;
        }
    }
}