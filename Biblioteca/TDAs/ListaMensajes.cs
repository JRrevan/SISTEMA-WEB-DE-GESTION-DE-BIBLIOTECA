namespace Biblioteca.TDAs
{
    /// Lista enlazada simple con puntero al final (agregar en O(1)).
    /// Acumula los mensajes de error y de aviso que deja la carga de un XML (registro duplicado, categoría padre inexistente, ISBN no
    /// numérico, etc.) para mostrarlos todos juntos en la vista de carga, sin que un solo error detenga el resto del procesamiento.
    public class ListaMensajes
    {
        private NodoMensaje ultimo;

        public NodoMensaje Primero { get; private set; }
        public int Cantidad { get; private set; }
        public bool EstaVacia => Primero == null;

        public void Agregar(string texto)
        {
            NodoMensaje nuevo = new NodoMensaje(texto);
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

        /// Concatena todos los mensajes en un solo texto, uno por línea. Útil para mostrarlos de una vez en la vista o en la consola.>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            NodoMensaje actual = Primero;
            while (actual != null)
            {
                sb.AppendLine(actual.Texto);
                actual = actual.Siguiente;
            }
            return sb.ToString();
        }
    }
}