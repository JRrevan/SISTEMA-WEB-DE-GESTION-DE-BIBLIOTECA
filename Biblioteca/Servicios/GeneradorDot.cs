using System.Text;
using Biblioteca.Dominio;
using Biblioteca.TDAs;

namespace Biblioteca.Servicios
{
    public class GeneradorDot
    {
        public string GenerarEstructuraCategorias(Categoria raiz)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph EstructuraCategorias {");
            sb.AppendLine("  node [shape=box, style=filled, fillcolor=lightblue, fontname=\"Arial\"];");
            
            if (raiz != null)
            {
                GenerarNodosCategoria(raiz, sb);
            }
            
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarNodosCategoria(Categoria categoria, StringBuilder sb)
        {
            string idPadre = $"\"{categoria.Nombre}\"";
            if (categoria.Hijos != null && !categoria.Hijos.EstaVacio)
            {
                ListaCategorias hijos = categoria.Hijos.ObtenerOrdenadas();
                NodoListaCategoria actual = hijos.Primero;
                while (actual != null)
                {
                    string idHijo = $"\"{actual.Dato.Nombre}\"";
                    sb.AppendLine($"  {idPadre} -> {idHijo};");
                    GenerarNodosCategoria(actual.Dato, sb);
                    actual = actual.Siguiente;
                }
            }
        }

        public string GenerarLibrosAscendente(ArbolAVLLibros arbol, string nombreCategoria)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph LibrosCategoria {");
            sb.AppendLine("  rankdir=LR;");
            sb.AppendLine("  node [shape=record, style=filled, fillcolor=lightyellow, fontname=\"Arial\"];");
            
            if (arbol != null && !arbol.EstaVacio)
            {
                // Extraemos la lista en inorden (ascendente) desde tu TDA
                ListaLibros lista = arbol.ObtenerAscendente();
                NodoListaLibro actual = lista.Primero;
                string nodoAnterior = null;

                while (actual != null)
                {
                    string idNodo = $"\"{actual.Dato.Isbn}\"";
                    sb.AppendLine($"  {idNodo} [label=\"{{ISBN: {actual.Dato.Isbn} | {actual.Dato.Titulo}}}\"];");
                    
                    if (nodoAnterior != null)
                    {
                        sb.AppendLine($"  {nodoAnterior} -> {idNodo} [color=blue, label=\"Siguiente\"];");
                    }
                    
                    nodoAnterior = idNodo;
                    actual = actual.Siguiente;
                }
            }
            else
            {
                sb.AppendLine($"  Vacio [label=\"No hay libros en la categoría {nombreCategoria}\"];");
            }
            
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}