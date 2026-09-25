namespace Biblioteca.Dominio
{
    public class Libro
    {
        // ISBN como long: un ISBN-13 no cabe en un int.
        public long Isbn { get; }
        public string Titulo { get; }
        public string Autor { get; }

        // Referencia a su categoría: permite eliminar el libro de su
        // árbol de categoría sin tener que buscarlo por nombre.
        public Categoria Categoria { get; set; }

        public Libro(long isbn, string titulo, string autor, Categoria categoria)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }
    }
}