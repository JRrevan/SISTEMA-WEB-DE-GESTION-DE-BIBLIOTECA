 using Biblioteca.Dominio;

namespace Biblioteca.TDAs
{
    /// Árbol AVL de libros ordenado por ISBN (clave única).
    /// Búsqueda, inserción y eliminación en O(log n).
  
    public class ArbolAVLLibros
    {
        public NodoAVLLibro? Raiz { get; private set; }
        public int Cantidad { get; private set; }
        public bool EstaVacio => Raiz == null;
        public int Altura => AlturaDe(Raiz);

        // INSERTAR

        /// Devuelve false si el ISBN ya existe (o el libro es nulo).
        public bool Insertar(Libro libro)
        {
            if (libro == null) return false;
            if (Buscar(libro.Isbn) != null) return false;

            Raiz = Insertar(Raiz, libro);
            Cantidad++;
            return true;
        }

        private NodoAVLLibro Insertar(NodoAVLLibro? nodo, Libro libro)
        {
            if (nodo == null) return new NodoAVLLibro(libro);

            if (libro.Isbn < nodo.Dato.Isbn)
                nodo.Izquierdo = Insertar(nodo.Izquierdo, libro);
            else
                nodo.Derecho = Insertar(nodo.Derecho, libro);

            return Balancear(nodo);
        }

        // BUSCAR

        /// Devuelve el libro o null si no existe.
        public Libro? Buscar(long isbn)
        {
            NodoAVLLibro actual = Raiz;
            while (actual != null)
            {
                if (isbn == actual.Dato.Isbn) return actual.Dato;
                actual = isbn < actual.Dato.Isbn ? actual.Izquierdo : actual.Derecho;
            }
            return null;
        }

        //  ELIMINAR

        /// Devuelve false si el ISBN no existía.
        public bool Eliminar(long isbn)
        {
            if (Buscar(isbn) == null) return false;

            Raiz = Eliminar(Raiz, isbn);
            Cantidad--;
            return true;
        }

        private NodoAVLLibro? Eliminar(NodoAVLLibro? nodo, long isbn)
        {
            if (nodo == null) return null;

            if (isbn < nodo.Dato.Isbn)
            {
                nodo.Izquierdo = Eliminar(nodo.Izquierdo, isbn);
            }
            else if (isbn > nodo.Dato.Isbn)
            {
                nodo.Derecho = Eliminar(nodo.Derecho, isbn);
            }
            else
            {
                // Caso 1 y 2: sin hijos o con un solo hijo.
                if (nodo.Izquierdo == null) return nodo.Derecho;
                if (nodo.Derecho == null) return nodo.Izquierdo;

                // Caso 3: dos hijos. Se reemplaza por su sucesor inorden
                // (el menor del subárbol derecho) y se borra ese sucesor.
                NodoAVLLibro sucesor = NodoMinimo(nodo.Derecho);
                nodo.Dato = sucesor.Dato;
                nodo.Derecho = Eliminar(nodo.Derecho, sucesor.Dato.Isbn);
            }

            return Balancear(nodo);
        }

        // MENOR / MAYOR

        ///Libro con el ISBN más pequeño, o null si está vacío.
        public Libro? Minimo()
        {
            return Raiz == null ? null : NodoMinimo(Raiz).Dato;
        }

        ///Libro con el ISBN más grande, o null si está vacío.
        public Libro? Maximo()
        {
            if (Raiz == null) return null;
            NodoAVLLibro actual = Raiz;
            while (actual.Derecho != null) actual = actual.Derecho;
            return actual.Dato;
        }

        private NodoAVLLibro NodoMinimo(NodoAVLLibro nodo)
        {
            while (nodo.Izquierdo != null) nodo = nodo.Izquierdo;
            return nodo;
        }

        //ORDEN ASCENDENTE

        ///Todos los libros por ISBN ascendente (recorrido inorden).
        public ListaLibros ObtenerAscendente()
        {
            ListaLibros resultado = new ListaLibros();
            Inorden(Raiz, resultado);
            return resultado;
        }

        private void Inorden(NodoAVLLibro? nodo, ListaLibros destino)
        {
            if (nodo == null) return;
            Inorden(nodo.Izquierdo, destino);
            destino.Agregar(nodo.Dato);
            Inorden(nodo.Derecho, destino);
        }

        ///Deja el árbol vacío (usado por Inicialización).
        public void Vaciar()
        {
            Raiz = null;
            Cantidad = 0;
        }

        // BALANCEO AVL

        private int AlturaDe(NodoAVLLibro? nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        private void ActualizarAltura(NodoAVLLibro nodo)
        {
            int hi = AlturaDe(nodo.Izquierdo);
            int hd = AlturaDe(nodo.Derecho);
            nodo.Altura = (hi > hd ? hi : hd) + 1;
        }

        // Positivo = pesa más la izquierda; negativo = pesa más la derecha.
        private int FactorBalance(NodoAVLLibro? nodo)
        {
            return nodo == null ? 0 : AlturaDe(nodo.Izquierdo) - AlturaDe(nodo.Derecho);
        }

        private NodoAVLLibro Balancear(NodoAVLLibro nodo)
        {
            ActualizarAltura(nodo);
            int fb = FactorBalance(nodo);

            if (fb > 1)
            {
                // Caso izquierda-derecha: primero se rota el hijo izquierdo.
                if (FactorBalance(nodo.Izquierdo) < 0)
                    nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            if (fb < -1)
            {
                // Caso derecha-izquierda: primero se rota el hijo derecho.
                if (FactorBalance(nodo.Derecho) > 0)
                    nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        private NodoAVLLibro RotarDerecha(NodoAVLLibro y)
        {
            NodoAVLLibro x = y.Izquierdo;
            NodoAVLLibro t2 = x.Derecho;

            x.Derecho = y;
            y.Izquierdo = t2;

            ActualizarAltura(y);
            ActualizarAltura(x);
            return x;
        }

        private NodoAVLLibro RotarIzquierda(NodoAVLLibro x)
        {
            NodoAVLLibro y = x.Derecho;
            NodoAVLLibro t2 = y.Izquierdo;

            y.Izquierdo = x;
            x.Derecho = t2;

            ActualizarAltura(x);
            ActualizarAltura(y);
            return y;
        }
    }
}