using System;
using Biblioteca.Dominio;
using Biblioteca.TDAs;
using Biblioteca.Infraestructura;

namespace PruebasTDA
{
    internal static class Program
    {
        private static int fallos = 0;
        private static int totales = 0;

        private static void Verificar(bool condicion, string descripcion)
        {
            totales++;
            Console.WriteLine((condicion ? "  [OK]    " : "  [FALLA] ") + descripcion);
            if (!condicion) fallos++;
        }

        private static Libro Crear(long isbn)
        {
            return new Libro(isbn, "Titulo " + isbn, "Autor " + isbn, null);
        }

        // ================================================================
        // ArbolAVLLibros
        // ================================================================

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

        private static bool EsAVLValido(ArbolAVLLibros arbol) => RevisarAVL(arbol.Raiz) >= 0;

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

        private static void ProbarArbolAVLLibros()
        {
            Console.WriteLine();
            Console.WriteLine("=== ArbolAVLLibros ===");

            ArbolAVLLibros vacio = new ArbolAVLLibros();
            Verificar(vacio.EstaVacio && vacio.Cantidad == 0, "árbol vacío nace vacío");
            Verificar(vacio.Buscar(1) == null, "Buscar en vacío devuelve null");
            Verificar(vacio.Minimo() == null && vacio.Maximo() == null, "Minimo/Maximo en vacío devuelven null");
            Verificar(!vacio.Eliminar(1), "Eliminar en vacío devuelve false");

            ArbolAVLLibros arbol = new ArbolAVLLibros();
            const int N = 10000;
            for (long i = 1; i <= N; i++) arbol.Insertar(Crear(i));
            Verificar(arbol.Cantidad == N, "inserción ascendente: Cantidad = " + N);
            Verificar(EsAVLValido(arbol), "inserción ascendente: AVL válido");
            Verificar(arbol.Altura <= 19, "inserción ascendente: altura <= 19 (obtuvo " + arbol.Altura + ")");
            Verificar(EstaOrdenadoYCompleto(arbol), "inserción ascendente: inorden ordenado y completo");

            Verificar(!arbol.Insertar(Crear(500)), "ISBN repetido se rechaza");
            Verificar(!arbol.Insertar(null), "libro nulo se rechaza");
            Verificar(arbol.Buscar(7777).Isbn == 7777, "Buscar(7777) lo encuentra");
            Verificar(arbol.Buscar(N + 1) == null, "Buscar de ISBN inexistente da null");
            Verificar(arbol.Minimo().Isbn == 1 && arbol.Maximo().Isbn == N, "Minimo=1, Maximo=" + N);

            for (long i = 2; i <= N; i += 2) arbol.Eliminar(i);
            Verificar(arbol.Cantidad == N / 2, "tras eliminar pares: Cantidad = " + arbol.Cantidad);
            Verificar(EsAVLValido(arbol) && EstaOrdenadoYCompleto(arbol), "tras eliminar pares: estructura íntegra");
            Verificar(arbol.Buscar(4) == null && arbol.Buscar(5) != null, "4 eliminado, 5 permanece");
            Verificar(!arbol.Eliminar(4), "eliminar un ISBN ya eliminado da false");

            ArbolAVLLibros aleatorio = new ArbolAVLLibros();
            Random rng = new Random(42);
            for (int i = 0; i < 5000; i++) aleatorio.Insertar(Crear(rng.Next(1, 1000000)));
            Verificar(EsAVLValido(aleatorio) && EstaOrdenadoYCompleto(aleatorio), "aleatorio: estructura íntegra tras insertar");
            rng = new Random(42);
            for (int i = 0; i < 5000; i++) aleatorio.Eliminar(rng.Next(1, 1000000));
            Verificar(aleatorio.EstaVacio, "aleatorio: queda vacío tras eliminar todo lo insertado");

            arbol.Vaciar();
            Verificar(arbol.EstaVacio && arbol.Cantidad == 0, "Vaciar deja el árbol en blanco");
        }

        // ================================================================
        // ArbolAVLCategorias
        // ================================================================

        private static int RevisarAVL(NodoAVLCategoria nodo)
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

        private static bool EsAVLValido(ArbolAVLCategorias arbol) => RevisarAVL(arbol.Raiz) >= 0;

        private static bool EstaOrdenadoAlfabeticamente(ArbolAVLCategorias arbol)
        {
            ListaCategorias lista = arbol.ObtenerOrdenadas();
            if (lista.Cantidad != arbol.Cantidad) return false;
            NodoListaCategoria actual = lista.Primero;
            while (actual != null && actual.Siguiente != null)
            {
                if (string.Compare(actual.Dato.Nombre, actual.Siguiente.Dato.Nombre,
                        StringComparison.OrdinalIgnoreCase) >= 0) return false;
                actual = actual.Siguiente;
            }
            return true;
        }

        private static void ProbarArbolAVLCategorias()
        {
            Console.WriteLine();
            Console.WriteLine("=== ArbolAVLCategorias ===");

            ArbolAVLCategorias vacio = new ArbolAVLCategorias();
            Verificar(vacio.EstaVacio && vacio.Cantidad == 0, "árbol vacío nace vacío");
            Verificar(vacio.Buscar("Ficción") == null, "Buscar en vacío devuelve null");
            Verificar(!vacio.Insertar(null), "Insertar null devuelve false");

            ArbolAVLCategorias arbol = new ArbolAVLCategorias();
            string[] nombresOrdenados = new string[2000];
            for (int i = 0; i < 2000; i++) nombresOrdenados[i] = "Cat" + i.ToString("D5");
            Array.Sort(nombresOrdenados, StringComparer.OrdinalIgnoreCase);
            foreach (string nombre in nombresOrdenados) arbol.Insertar(new Categoria(nombre, null));
            Verificar(arbol.Cantidad == 2000, "inserción alfabética: Cantidad = 2000");
            Verificar(EsAVLValido(arbol), "inserción alfabética: AVL válido");
            Verificar(EstaOrdenadoAlfabeticamente(arbol), "inserción alfabética: orden correcto");

            Verificar(!arbol.Insertar(new Categoria("Cat00500", null)), "nombre repetido exacto se rechaza");
            Verificar(!arbol.Insertar(new Categoria("cAt00500", null)), "repetido con otras mayúsculas se rechaza");
            Verificar(arbol.Buscar("cat01234") != null, "Buscar insensible a mayúsculas/minúsculas");
            Verificar(arbol.Buscar("NoExiste") == null, "Buscar de nombre inexistente da null");

            ArbolAVLCategorias variado = new ArbolAVLCategorias();
            string[] nombres = { "Ficción", "Terror", "Niñez", "Ciencia Ficción", "Álgebra", "biografía" };
            foreach (string n in nombres) variado.Insertar(new Categoria(n, null));
            Verificar(EsAVLValido(variado) && EstaOrdenadoAlfabeticamente(variado), "nombres con tildes/ñ: estructura y orden correctos");
            Verificar(variado.Buscar("BIOGRAFÍA") != null, "insensible a mayúsculas también con tildes");
        }

        // ================================================================
        // ListaMensajes
        // ================================================================

        private static void ProbarListaMensajes()
        {
            Console.WriteLine();
            Console.WriteLine("=== ListaMensajes ===");

            ListaMensajes vacia = new ListaMensajes();
            Verificar(vacia.EstaVacia && vacia.Cantidad == 0, "lista vacía nace vacía");
            Verificar(vacia.ToString() == "", "ToString de vacía es texto vacío");

            ListaMensajes lista = new ListaMensajes();
            lista.Agregar("ISBN 9781234 duplicado, se omitió el registro.");
            lista.Agregar("Categoría 'Fantasía' no existe, no se pudo asignar el libro.");
            lista.Agregar("Se cargaron 42 libros correctamente.");
            Verificar(lista.Cantidad == 3, "Cantidad = 3 tras 3 Agregar");
            NodoMensaje actual = lista.Primero;
            Verificar(actual.Texto.StartsWith("ISBN 9781234"), "primer mensaje respeta orden de inserción");
            Verificar(lista.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Length == 3,
                "ToString concatena una línea por mensaje");

            ListaMensajes muchos = new ListaMensajes();
            for (int i = 0; i < 5000; i++) muchos.Agregar("Mensaje " + i);
            Verificar(muchos.Cantidad == 5000, "agregar 5000 mensajes: Cantidad = 5000");
            Verificar(muchos.Primero.Texto == "Mensaje 0", "el primer mensaje se conserva tras muchos Agregar");
        }

        // ================================================================
        // Categoria
        // ================================================================

        private static void ProbarCategoria()
        {
            Console.WriteLine();
            Console.WriteLine("=== Categoria ===");

            Categoria raiz = new Categoria("Catálogo", null);
            Verificar(raiz.Padre == null, "la raíz no tiene padre");
            Verificar(raiz.Hijos.EstaVacio && raiz.Libros.EstaVacio, "nace sin hijos ni libros");

            Categoria literatura = new Categoria("Literatura", raiz);
            raiz.Hijos.Insertar(literatura);
            Categoria novela = new Categoria("Novela", literatura);
            literatura.Hijos.Insertar(novela);
            Categoria historica = new Categoria("Histórica", novela);
            Categoria romantica = new Categoria("Romántica", novela);
            novela.Hijos.Insertar(historica);
            novela.Hijos.Insertar(romantica);

            Verificar(novela.Padre == literatura, "relación padre-hijo correcta en varios niveles");
            Verificar(novela.Hijos.ObtenerOrdenadas().Primero.Dato.Nombre == "Histórica", "hijas en orden alfabético");

            Libro l1 = new Libro(100, "Guerra y paz", "Tolstói", historica);
            Libro l2 = new Libro(50, "Los novios", "Manzoni", historica);
            historica.Libros.Insertar(l1);
            historica.Libros.Insertar(l2);
            Libro l3 = new Libro(200, "Orgullo y prejuicio", "Austen", romantica);
            romantica.Libros.Insertar(l3);

            Verificar(historica.Libros.Cantidad == 2 && historica.Libros.Minimo().Isbn == 50, "libros propios de Histórica correctos");
            Verificar(novela.Libros.EstaVacio, "categoría intermedia sin libros propios");

            ArbolAVLLibros todosDeNovela = novela.ObtenerLibrosConDescendientes();
            Verificar(todosDeNovela.Cantidad == 3, "ObtenerLibrosConDescendientes junta los 3 libros de las hojas");
            Verificar(todosDeNovela.Minimo().Isbn == 50 && todosDeNovela.Maximo().Isbn == 200, "extremos correctos del combinado");
            Verificar(historica.ObtenerLibrosConDescendientes().Cantidad == 2, "en una hoja, solo trae sus propios libros");
            Verificar(raiz.ObtenerLibrosConDescendientes().Cantidad == 3, "desde la raíz llega a todos los libros");
            Verificar(l1.Categoria == historica && l3.Categoria == romantica, "Libro.Categoria apunta a la categoría dueña");
        }

        // ================================================================
        // Catalogo
        // ================================================================

        private static void ProbarCatalogo()
        {
            Console.WriteLine();
            Console.WriteLine("=== Catalogo ===");

            Catalogo catalogo = new Catalogo();
            Verificar(catalogo.Raiz != null && catalogo.Raiz.Padre == null, "raíz virtual sin padre");
            Verificar(catalogo.IndiceCategorias.EstaVacio && catalogo.IndiceLibros.EstaVacio, "índices nacen vacíos");

            Verificar(catalogo.AgregarCategoria("Literatura", null), "agrega categoría de primer nivel (padre null)");
            Verificar(catalogo.AgregarCategoria("Ciencia", ""), "agrega categoría de primer nivel (padre vacío)");
            Verificar(catalogo.AgregarCategoria("Novela", "Literatura"), "agrega categoría anidada");
            Verificar(catalogo.AgregarCategoria("Histórica", "Novela"), "agrega categoría de tercer nivel");
            Verificar(catalogo.IndiceCategorias.Cantidad == 4, "índice global de categorías = 4");
            Verificar(catalogo.Raiz.Hijos.Cantidad == 2, "raíz tiene 2 hijas directas");

            Categoria novela = catalogo.BuscarCategoria("Novela");
            Categoria historica = catalogo.BuscarCategoria("Histórica");
            Verificar(historica.Padre == novela, "el padre correcto queda enlazado, no la raíz");

            Verificar(!catalogo.AgregarCategoria("Novela", "Literatura"), "nombre repetido se rechaza");
            Verificar(!catalogo.AgregarCategoria("Fantasía", "NoExiste"), "padre inexistente se rechaza");
            Verificar(!catalogo.AgregarCategoria("", "Literatura"), "nombre vacío se rechaza");

            Verificar(catalogo.AgregarLibro(100, "Guerra y paz", "Tolstói", "Histórica"), "agrega libro válido");
            Verificar(catalogo.AgregarLibro(200, "Cien años de soledad", "García Márquez", "Novela"), "agrega segundo libro válido");
            Verificar(!catalogo.AgregarLibro(100, "Otro título", "Otro autor", "Novela"), "ISBN repetido se rechaza");
            Verificar(!catalogo.AgregarLibro(300, "Fantasma", "Autor", "NoExiste"), "categoría inexistente se rechaza");
            Verificar(catalogo.IndiceLibros.Cantidad == 2, "índice global de libros = 2");

            Verificar(catalogo.BuscarLibro(100).Titulo == "Guerra y paz", "BuscarLibro trae el libro correcto");
            Verificar(catalogo.BuscarLibro(999) == null, "BuscarLibro de ISBN inexistente da null");

            Verificar(catalogo.EliminarLibro(100), "elimina libro existente");
            Verificar(catalogo.BuscarLibro(100) == null, "ya no está en el índice global");
            Verificar(historica.Libros.EstaVacio, "tampoco está en el árbol de su propia categoría");
            Verificar(!catalogo.EliminarLibro(100), "eliminar de nuevo el mismo ISBN da false");

            catalogo.Inicializar();
            Verificar(catalogo.IndiceCategorias.EstaVacio && catalogo.IndiceLibros.EstaVacio, "Inicializar limpia ambos índices");
            Verificar(catalogo.BuscarCategoria("Literatura") == null, "las categorías anteriores ya no existen");
            Verificar(catalogo.AgregarCategoria("Nueva", null), "se puede volver a usar después de Inicializar");
        }

        // ================================================================
        // Validador
        // ================================================================

        private static void ProbarValidador()
        {
            Console.WriteLine();
            Console.WriteLine("=== Validador ===");

            Verificar(Validador.EsTextoValido("Novela"), "texto normal es válido");
            Verificar(!Validador.EsTextoValido("") && !Validador.EsTextoValido("   ") && !Validador.EsTextoValido(null),
                "vacío, espacios y null son inválidos");

            Verificar(Validador.EsIsbnValido("9781234567"), "ISBN normal es válido");
            Verificar(!Validador.EsIsbnValido("12A34"), "letras mezcladas son inválidas");
            Verificar(!Validador.EsIsbnValido("-100"), "negativo es inválido");
            Verificar(!Validador.EsIsbnValido("0"), "cero es inválido");
            Verificar(!Validador.EsIsbnValido("99999999999999999999"), "desbordamiento de long es inválido");
            Verificar(Validador.EsIsbnValido("9223372036854775807"), "long.MaxValue exacto es válido");

            Verificar(Validador.ConvertirIsbn("9781234567") == 9781234567L, "ConvertirIsbn convierte correctamente");
            Verificar(Validador.ConvertirIsbn("abc") == 0, "ConvertirIsbn de texto inválido da 0");

            Verificar(Validador.SonElMismoNombre("Ficción", "ficción"), "SonElMismoNombre insensible a mayúsculas");
            Verificar(!Validador.SonElMismoNombre("Ficción", "Terror"), "nombres distintos no son iguales");
            Verificar(Validador.CompararAlfabetico("Ana", "Beto") < 0, "CompararAlfabetico ordena correctamente");
        }

        // ================================================================

        private static int Main()
        {
            Console.WriteLine("PRUEBAS DE REGRESIÓN — RELEASE 1 (TDA y dominio)");

            ProbarArbolAVLLibros();
            ProbarArbolAVLCategorias();
            ProbarListaMensajes();
            ProbarCategoria();
            ProbarCatalogo();
            ProbarValidador();

            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Total de verificaciones: " + totales);
            Console.WriteLine(fallos == 0
                ? "TODAS LAS PRUEBAS PASARON"
                : "PRUEBAS FALLIDAS: " + fallos + " de " + totales);
            return fallos == 0 ? 0 : 1;
        }
    }
}