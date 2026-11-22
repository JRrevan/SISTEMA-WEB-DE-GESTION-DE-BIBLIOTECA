# 📚 Sistema Web de Gestión de Biblioteca

Aplicación web desarrollada en **ASP.NET Core MVC (C# / .NET 10)** para administrar un catálogo de libros organizado en categorías y subcategorías jerárquicas. Toda la información se mantiene en memoria usando **TDAs no lineales implementados desde cero** (árboles AVL y listas enlazadas), sin colecciones genéricas de .NET.

> Proyecto 2 · Introducción a la Programación y Computación 2 · Universidad de San Carlos de Guatemala

---

## ✨ Funcionalidades

- **Inicialización del sistema:** borra todo el catálogo y lo deja vacío.
- **Carga incremental desde XML:** agrega categorías y libros sin perder lo ya cargado, con un reporte de éxitos y errores por registro.
- **Gestión de categorías:**
  - Agregar categorías y subcategorías (con categoría padre opcional).
  - Visualizar la estructura jerárquica en HTML, partiendo de la raíz o de cualquier subcategoría.
- **Gestión de libros:**
  - Registrar y eliminar libros.
  - Buscar por ISBN.
  - Mostrar el menor y el mayor ISBN del catálogo.
  - Listar libros ordenados por ISBN, filtrando por categoría o subcategoría (incluye descendientes).
- **Gráficos con Graphviz (SVG):**
  - Estructura jerárquica de categorías.
  - Libros de una categoría en orden ascendente por ISBN.
  - Árbol AVL global de libros.
- **Ayuda:** datos del estudiante y acceso a la documentación (PDF).

---

## 🧱 Estructuras de datos

| TDA | Clave / orden | Uso |
|---|---|---|
| `ArbolAVLLibros` | ISBN (único) | Libros de cada categoría e índice global. Insertar, buscar, eliminar, mínimo, máximo, inorden |
| `ArbolAVLCategorias` | Nombre (sin distinguir mayúsculas) | Hijas de cada categoría e índice global de categorías |
| `ListaLibros` | Ascendente por ISBN | Resultado de recorridos inorden |
| `ListaCategorias` | Alfabético | Resultado de recorridos inorden |
| `ListaMensajes` | Orden de llegada | Reporte de carga del XML |

Todas las inserciones y búsquedas son **O(log n)**. Las listas enlazadas mantienen puntero al último nodo para agregar en **O(1)**.

---

## 🏗️ Arquitectura

El proyecto está organizado en 5 capas:

```
Biblioteca/
├── Presentacion/        Capa 1 · Controllers, Views y Layout
├── Servicios/           Capa 2 · CatalogoService (singleton), CargaXmlService, GraphvizService
├── Dominio/             Capa 3 · Catalogo, Categoria, Libro, ResultadoCarga
├── TDAs/                Capa 4 · Árboles AVL, listas enlazadas y sus nodos
├── Infraetructura/      Capa 5 · LectorXmlEntrada, Validador
├── Docs/                Documentación del proyecto (ensayo en PDF)
└── wwwroot/graficos/    SVG generados por Graphviz en tiempo de ejecución
```

El detalle completo de la arquitectura está en [`Biblioteca/Docs/Arbol de capas.txt`](Biblioteca/Docs/Arbol%20de%20capas.txt).

---

## ⚙️ Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Graphviz](https://graphviz.org/download/) instalado y con el ejecutable `dot` disponible en el `PATH` (necesario para generar los gráficos)

Para verificar la instalación de Graphviz:

```bash
dot -V
```

---

## 🚀 Ejecución

```bash
git clone https://github.com/<tu-usuario>/IPC2_Proy02_202602_202400128.git
cd IPC2_Proy02_202602_202400128/Biblioteca
dotnet run
```

Luego abre en el navegador la URL que indique la consola (por ejemplo `http://localhost:5000`).

---

## 📄 Formato del XML de entrada

```xml
<config>
  <listaCategorias>
    <categoria>Ciencia</categoria>
    <categoria padre="Ciencia">Física</categoria>
    <categoria padre="Ciencia">Química</categoria>
  </listaCategorias>

  <listaLibros>
    <libro>
      <ISBN>9780000001</ISBN>
      <titulo>Fundamentos de Física</titulo>
      <autor>Autor Ejemplo</autor>
      <categoria>Física</categoria>
    </libro>
  </listaLibros>
</config>
```

**Reglas de validación:**

- La etiqueta raíz debe ser `<config>`.
- Los nombres de categoría son únicos y se comparan sin distinguir mayúsculas/minúsculas.
- Una categoría con `padre` solo se registra si el padre existe (el orden en el XML no importa: se resuelve de forma diferida).
- El ISBN debe ser numérico y positivo, y no puede repetirse.
- Un libro solo se agrega si su categoría existe.
- Un error en un registro **no detiene** el procesamiento del resto; todos los resultados se muestran en el reporte de carga.

---

## 👤 Autor

**Jonathan Lizardo Rivera Luna** · Carné 202400128
Facultad de Ingeniería · Escuela de Ciencias y Sistemas
Universidad de San Carlos de Guatemala
