namespace Biblioteca.TDAs
{
    public class NodoMensaje
    {
        public string Texto { get; }
        public NodoMensaje Siguiente { get; set; }

        public NodoMensaje(string texto)
        {
            Texto = texto;
        }
    }
}