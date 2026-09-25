
using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    public class NodoAVLLibro
    {
        public Libro Dato { get; set; }
        public NodoAVLLibro? Izquierdo { get; set; }
        public NodoAVLLibro? Derecho { get; set; }

        public int Altura { get; set; }

        public NodoAVLLibro(Libro dato)
        {
            Dato = dato;
            Altura = 1;
        }
    }
}