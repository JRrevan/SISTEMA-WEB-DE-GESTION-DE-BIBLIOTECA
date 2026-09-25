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
            string idPadre = Id(categoria.Nombre);
            if (categoria.Hijos != null && !categoria.Hijos.EstaVacio)
            {
                ListaCategorias hijos = categoria.Hijos.ObtenerOrdenadas();
                NodoListaCategoria actual = hijos.Primero;
                while (actual != null)
                {
                    string idHijo = Id(actual.Dato.Nombre);
                    sb.AppendLine($"  {idPadre} -> {idHijo};");
                    GenerarNodosCategoria(actual.Dato, sb);
                    actual = actual.Siguiente;
                }
            }
        }

        public string GenerarArbolLibros(ArbolAVLLibros arbol)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph ArbolAVLDeLibros {");
            sb.AppendLine("  node [shape=record, style=filled, fillcolor=lightyellow, fontname=\"Arial\"];");

            if (arbol != null && !arbol.EstaVacio)
            {
                GenerarNodosArbolLibros(arbol.Raiz, sb);
            }
            else
            {
                sb.AppendLine("  Vacio [label=\"No hay libros en el catálogo\"];");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerarNodosArbolLibros(NodoAVLLibro nodo, StringBuilder sb)
        {
            if (nodo == null) return;

            string idNodo = Id($"libro_{nodo.Dato.Isbn}");
            sb.AppendLine($"  {idNodo} [label=\"{{ISBN: {nodo.Dato.Isbn} | {Escapar(nodo.Dato.Titulo)}}}\"];");

            if (nodo.Izquierdo != null)
            {
                string idIzquierdo = Id($"libro_{nodo.Izquierdo.Dato.Isbn}");
                sb.AppendLine($"  {idNodo} -> {idIzquierdo} [label=\"Izquierdo\"];");
            }

            if (nodo.Derecho != null)
            {
                string idDerecho = Id($"libro_{nodo.Derecho.Dato.Isbn}");
                sb.AppendLine($"  {idNodo} -> {idDerecho} [label=\"Derecho\"];");
            }

            GenerarNodosArbolLibros(nodo.Izquierdo, sb);
            GenerarNodosArbolLibros(nodo.Derecho, sb);
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
                    string idNodo = Id($"libro_categoria_{actual.Dato.Isbn}");
                    sb.AppendLine($"  {idNodo} [label=\"{{ISBN: {actual.Dato.Isbn} | {Escapar(actual.Dato.Titulo)}}}\"];");
                    
                    if (nodoAnterior != null)
                    {
                        sb.AppendLine($"  {nodoAnterior} -> {idNodo} [color=blue, label=\"ISBN siguiente\"];");
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

        private static string Id(string value)
        {
            return $"\"{Escapar(value)}\"";
        }

        private static string Escapar(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "\\n");
        }
    }
}