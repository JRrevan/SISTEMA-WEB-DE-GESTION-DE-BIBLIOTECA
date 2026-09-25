namespace Biblioteca.Dominio
{
    /// Versión mínima para que Libro compile.  con: padre, hijos (ArbolAVLCategorias) y libros (ArbolAVLLibros).
    public class Categoria
    {
        public string Nombre { get; }

        public Categoria(string nombre)
        {
            Nombre = nombre;
        }
    }
}