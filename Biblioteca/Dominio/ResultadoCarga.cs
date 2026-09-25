using Biblioteca.TDAs;

namespace Biblioteca.Servicios
{
    public class ResultadoCarga
    {
        public ListaMensajes Mensajes { get; set; }

        public ResultadoCarga()
        {
            Mensajes = new ListaMensajes();
        }

        public void AgregarMensaje(string mensaje)
        {
            Mensajes.Agregar(mensaje); 
        }
    }
}