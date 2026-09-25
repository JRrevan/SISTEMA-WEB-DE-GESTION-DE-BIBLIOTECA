using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    public class NodoAVLCategoria
    {
        public Categoria Dato { get; set; }
        public NodoAVLCategoria Izquierdo { get; set; }
        public NodoAVLCategoria Derecho { get; set; }

        // Igual que en NodoAVLLibro: 1 en una hoja, 0 en un nodo nulo.
        public int Altura { get; set; }

        public NodoAVLCategoria(Categoria dato)
        {
            Dato = dato;
            Altura = 1;
        }
    }
}