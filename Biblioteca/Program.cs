using System;
using Biblioteca.Dominio;
using Biblioteca.TDAs;

namespace PruebasTDA
{
    internal static class Program
    {
        private static int fallos = 0;

        private static void Verificar(bool condicion, string descripcion)
        {
            Console.WriteLine((condicion ? "  [OK]    " : "  [FALLA] ") + descripcion);
            if (!condicion) fallos++;
        }

        private static Libro Crear(long isbn)
        {
            return new Libro(isbn, "Titulo " + isbn, "Autor " + isbn, null);
        }

        // Devuelve la altura real del subárbol, o -1 si algún nodo rompe el AVL
        // (altura guardada incorrecta o factor de balance fuera de -1..1).
        private static int RevisarAVL(NodoAVLLibro nodo)
        {
            if (nodo == null) return 0;

            int hi = RevisarAVL(nodo.Izquierdo);
            int hd = RevisarAVL(nodo.Derecho);
            if (hi < 0 || hd < 0) return -1;

            int diferencia = hi - hd;
            if (diferencia > 1 || diferencia < -1) return -1;

            int altura = (hi > hd ? hi : hd) + 1;
            if (nodo.Altura != altura) return -1;
            return altura;
        }

        private static bool EsAVLValido(ArbolAVLLibros arbol)
        {
            return RevisarAVL(arbol.Raiz) >= 0;
        }

        private static bool EstaOrdenadoYCompleto(ArbolAVLLibros arbol)
        {
            ListaLibros lista = arbol.ObtenerAscendente();
            if (lista.Cantidad != arbol.Cantidad) return false;

            NodoListaLibro actual = lista.Primero;
            while (actual != null && actual.Siguiente != null)
            {
                if (actual.Dato.Isbn >= actual.Siguiente.Dato.Isbn) return false;
                actual = actual.Siguiente;
            }
            return true;
        }

        private static int Main()
        {
            Console.WriteLine("1) Árbol vacío");
            ArbolAVLLibros vacio = new ArbolAVLLibros();
            Verificar(vacio.EstaVacio && vacio.Cantidad == 0, "nace vacío");
            Verificar(vacio.Buscar(1) == null, "Buscar en vacío devuelve null");
            Verificar(vacio.Minimo() == null && vacio.Maximo() == null, "Minimo/Maximo devuelven null");
            Verificar(!vacio.Eliminar(1), "Eliminar en vacío devuelve false");
            Verificar(vacio.ObtenerAscendente().Cantidad == 0, "lista ascendente vacía");

            Console.WriteLine("2) Inserción ascendente (peor caso de un ABB simple)");
            ArbolAVLLibros arbol = new ArbolAVLLibros();
            const int N = 10000;
            for (long i = 1; i <= N; i++) arbol.Insertar(Crear(i));
            Verificar(arbol.Cantidad == N, "Cantidad = " + arbol.Cantidad);
            Verificar(EsAVLValido(arbol), "alturas y balance correctos en todos los nodos");
            Console.WriteLine("        altura obtenida: " + arbol.Altura + " (un ABB simple tendría " + N + ")");
            Verificar(arbol.Altura <= 19, "altura <= 19 con 10,000 nodos");
            Verificar(EstaOrdenadoYCompleto(arbol), "inorden ascendente y completo");

            Console.WriteLine("3) Duplicados, búsqueda y extremos");
            Verificar(!arbol.Insertar(Crear(500)), "ISBN repetido se rechaza");
            Verificar(!arbol.Insertar(null), "libro nulo se rechaza");
            Verificar(arbol.Cantidad == N, "la cantidad no cambió");
            Verificar(arbol.Buscar(7777) != null && arbol.Buscar(7777).Isbn == 7777, "Buscar(7777) lo encuentra");
            Verificar(arbol.Buscar(N + 1) == null, "Buscar de un ISBN inexistente da null");
            Verificar(arbol.Minimo().Isbn == 1, "Minimo = 1");
            Verificar(arbol.Maximo().Isbn == N, "Maximo = " + N);

            Console.WriteLine("4) Eliminación de los ISBN pares");
            for (long i = 2; i <= N; i += 2)
            {
                if (!arbol.Eliminar(i)) { Verificar(false, "no pudo eliminar " + i); break; }
            }
            Verificar(arbol.Cantidad == N / 2, "Cantidad = " + arbol.Cantidad);
            Verificar(EsAVLValido(arbol), "sigue siendo AVL válido");
            Verificar(EstaOrdenadoYCompleto(arbol), "sigue ordenado y completo");
            Verificar(arbol.Buscar(4) == null && arbol.Buscar(5) != null, "4 ya no está, 5 sí");
            Verificar(!arbol.Eliminar(4), "eliminar de nuevo un ISBN ya eliminado da false");
            Verificar(arbol.Minimo().Isbn == 1 && arbol.Maximo().Isbn == N - 1, "extremos = 1 y " + (N - 1));

            Console.WriteLine("5) Eliminar menor, mayor y raíz");
            long raiz = arbol.Raiz.Dato.Isbn;
            arbol.Eliminar(arbol.Minimo().Isbn);
            arbol.Eliminar(arbol.Maximo().Isbn);
            arbol.Eliminar(raiz);
            Verificar(arbol.Buscar(raiz) == null, "la raíz eliminada ya no se encuentra");
            Verificar(EsAVLValido(arbol) && EstaOrdenadoYCompleto(arbol), "estructura íntegra");

            Console.WriteLine("6) Aleatorio: 5,000 inserciones y luego todas las eliminaciones");
            ArbolAVLLibros aleatorio = new ArbolAVLLibros();
            Random rng = new Random(42);
            for (int i = 0; i < 5000; i++) aleatorio.Insertar(Crear(rng.Next(1, 1000000)));
            int insertados = aleatorio.Cantidad;
            Verificar(EsAVLValido(aleatorio) && EstaOrdenadoYCompleto(aleatorio),
                      insertados + " libros únicos, estructura íntegra");

            rng = new Random(42);   // misma secuencia: borra lo mismo que insertó
            bool todoBien = true;
            for (int i = 0; i < 5000; i++)
            {
                aleatorio.Eliminar(rng.Next(1, 1000000));
                if (i % 500 == 0 && !EsAVLValido(aleatorio)) todoBien = false;
            }
            Verificar(todoBien, "AVL válido durante las eliminaciones");
            Verificar(aleatorio.EstaVacio && aleatorio.Cantidad == 0, "termina vacío");

            Console.WriteLine("7) Vaciar");
            arbol.Vaciar();
            Verificar(arbol.EstaVacio && arbol.Cantidad == 0 && arbol.Buscar(5) == null, "Vaciar deja el árbol en blanco");

            Console.WriteLine();
            Console.WriteLine(fallos == 0 ? "TODAS LAS PRUEBAS PASARON" : "PRUEBAS FALLIDAS: " + fallos);
            return fallos == 0 ? 0 : 1;
        }
    }
}