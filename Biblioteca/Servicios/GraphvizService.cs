using Biblioteca.Dominio;
using Biblioteca.TDAs;
using System;

namespace Biblioteca.Servicios
{
    public class GraphvizService
    {
        private readonly GeneradorDot _generadorDot;
        private readonly EjecutorGraphviz _ejecutorGraphviz;

        public GraphvizService()
        {
            _generadorDot = new GeneradorDot();
            _ejecutorGraphviz = new EjecutorGraphviz();
        }

        public string GenerarGraficoCategorias(Categoria raiz)
        {
            string dotContent = _generadorDot.GenerarEstructuraCategorias(raiz);
            string fileName = $"cat_{Guid.NewGuid()}";
            return _ejecutorGraphviz.GenerarImagenSvg(dotContent, fileName);
        }

        public string GenerarGraficoLibrosCategoria(Categoria categoria)
        {
            string dotContent = _generadorDot.GenerarLibrosAscendente(categoria.ObtenerLibrosConDescendientes(), categoria.Nombre);
            string fileName = $"libros_{Guid.NewGuid()}";
            return _ejecutorGraphviz.GenerarImagenSvg(dotContent, fileName);
        }

        public string GenerarGraficoArbolLibros(ArbolAVLLibros arbol)
        {
            string dotContent = _generadorDot.GenerarArbolLibros(arbol);
            string fileName = $"avl_libros_{Guid.NewGuid()}";
            return _ejecutorGraphviz.GenerarImagenSvg(dotContent, fileName);
        }
    }
}