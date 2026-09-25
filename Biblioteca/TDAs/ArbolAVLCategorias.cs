using System;
using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    /// Árbol AVL de categorías ordenado alfabéticamente por nombre (clave
    /// única, sin distinguir mayúsculas/minúsculas). Se usa de dos formas:
    ///   1) dentro de cada Categoria, para ordenar sus hijas directas;
    ///   2) como índice global en Catalogo, para encontrar cualquier
    ///      categoría por nombre al cargar el XML o agregar una nueva.
    /// Búsqueda e inserción en O(log n). No tiene Eliminar porque el enunciado no pide borrar categorías.
    public class ArbolAVLCategorias
    {
        public NodoAVLCategoria Raiz { get; private set; }
        public int Cantidad { get; private set; }
        public bool EstaVacio => Raiz == null;

        // INSERTAR

        ///Devuelve false si el nombre ya existe (o la categoría es nula).
        public bool Insertar(Categoria categoria)
        {
            if (categoria == null) return false;
            if (Buscar(categoria.Nombre) != null) return false;

            Raiz = Insertar(Raiz, categoria);
            Cantidad++;
            return true;
        }

        private NodoAVLCategoria Insertar(NodoAVLCategoria nodo, Categoria categoria)
        {
            if (nodo == null) return new NodoAVLCategoria(categoria);

            int comparacion = Comparar(categoria.Nombre, nodo.Dato.Nombre);
            if (comparacion < 0)
                nodo.Izquierdo = Insertar(nodo.Izquierdo, categoria);
            else
                nodo.Derecho = Insertar(nodo.Derecho, categoria);

            return Balancear(nodo);
        }

        // BUSCAR

        /// <summary>Devuelve la categoría o null si no existe.</summary>
        public Categoria Buscar(string nombre)
        {
            NodoAVLCategoria actual = Raiz;
            while (actual != null)
            {
                int comparacion = Comparar(nombre, actual.Dato.Nombre);
                if (comparacion == 0) return actual.Dato;
                actual = comparacion < 0 ? actual.Izquierdo : actual.Derecho;
            }
            return null;
        }

        // Comparación alfabética sin distinguir mayúsculas/minúsculas,
        // para que "Ficción" y "ficción" se traten como el mismo nombre.
        private int Comparar(string a, string b)
        {
            return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
        }

        // ORDEN ALFABÉTICO

        /// <summary>Todas las categorías en orden alfabético (recorrido inorden).</summary>
        public ListaCategorias ObtenerOrdenadas()
        {
            ListaCategorias resultado = new ListaCategorias();
            Inorden(Raiz, resultado);
            return resultado;
        }

        private void Inorden(NodoAVLCategoria nodo, ListaCategorias destino)
        {
            if (nodo == null) return;
            Inorden(nodo.Izquierdo, destino);
            destino.Agregar(nodo.Dato);
            Inorden(nodo.Derecho, destino);
        }

        //  BALANCEO AVL
        // Idéntico en estructura a ArbolAVLLibros; no se comparte código
        // porque el proyecto no permite genéricos ni templates.

        private int AlturaDe(NodoAVLCategoria nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private void ActualizarAltura(NodoAVLCategoria nodo)
        {
            int hi = AlturaDe(nodo.Izquierdo);
            int hd = AlturaDe(nodo.Derecho);
            nodo.Altura = (hi > hd ? hi : hd) + 1;
        }

        private int FactorBalance(NodoAVLCategoria nodo)
        {
            return nodo == null ? 0 : AlturaDe(nodo.Izquierdo) - AlturaDe(nodo.Derecho);
        }

        private NodoAVLCategoria Balancear(NodoAVLCategoria nodo)
        {
            ActualizarAltura(nodo);
            int fb = FactorBalance(nodo);

            if (fb > 1)
            {
                if (FactorBalance(nodo.Izquierdo) < 0)
                    nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            if (fb < -1)
            {
                if (FactorBalance(nodo.Derecho) > 0)
                    nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        private NodoAVLCategoria RotarDerecha(NodoAVLCategoria y)
        {
            NodoAVLCategoria x = y.Izquierdo;
            NodoAVLCategoria t2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = t2;

            ActualizarAltura(y);
            ActualizarAltura(x);
            return x;
        }

        private NodoAVLCategoria RotarIzquierda(NodoAVLCategoria x)
        {
            NodoAVLCategoria y = x.Derecho;
            NodoAVLCategoria t2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = t2;

            ActualizarAltura(x);
            ActualizarAltura(y);
            return y;
        }
    }
}