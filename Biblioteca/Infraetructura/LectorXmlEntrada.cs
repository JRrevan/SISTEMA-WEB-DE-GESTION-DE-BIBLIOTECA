using System;
using System.IO;
using System.Xml.Linq;
using Biblioteca.Dominio;
using Biblioteca.Infraestructura;

namespace Biblioteca.Servicios
{
    public class LectorXmlEntrada
    {
        private class NodoCategoriaPendiente
        {
            public string Nombre;
            public string NombrePadre;
            public NodoCategoriaPendiente Siguiente;
        }

        public ResultadoCarga Procesar(Stream xmlStream, Catalogo catalogo)
        {
            ResultadoCarga resultado = new ResultadoCarga();
            
            try
            {
                XDocument doc = XDocument.Load(xmlStream);
                XElement config = doc.Element("config");

                if (config == null)
                {
                    resultado.AgregarMensaje("Error Crítico: El archivo XML no tiene la etiqueta raíz <config>.");
                    return resultado;
                }

                // 1. Procesar Categorías con linking diferido
                XElement listaCategorias = config.Element("listaCategorias");
                if (listaCategorias != null)
                {
                    NodoCategoriaPendiente pendientes = null;
                    NodoCategoriaPendiente ultimo = null;
                    foreach (XElement catNode in listaCategorias.Elements("categoria"))
                    {
                        string nombreCategoria = catNode.Value.Trim();
                        XAttribute attrPadre = catNode.Attribute("padre");
                        string nombrePadre = attrPadre != null ? attrPadre.Value.Trim() : "";

                        NodoCategoriaPendiente nueva = new NodoCategoriaPendiente
                        {
                            Nombre = nombreCategoria,
                            NombrePadre = nombrePadre
                        };

                        if (pendientes == null) pendientes = nueva;
                        else ultimo.Siguiente = nueva;
                        ultimo = nueva;
                    }

                    bool huboCambios;
                    do
                    {
                        huboCambios = false;
                        NodoCategoriaPendiente anterior = null;
                        NodoCategoriaPendiente actual = pendientes;

                        while (actual != null)
                        {
                            NodoCategoriaPendiente siguiente = actual.Siguiente;
                            bool nombreDuplicado = catalogo.BuscarCategoria(actual.Nombre) != null;
                            bool padrePendiente = !string.IsNullOrWhiteSpace(actual.NombrePadre)
                                && catalogo.BuscarCategoria(actual.NombrePadre) == null;

                            if (nombreDuplicado || !padrePendiente && catalogo.AgregarCategoria(actual.Nombre, actual.NombrePadre))
                            {
                                if (nombreDuplicado)
                                {
                                    resultado.AgregarMensaje($"Error: No se pudo agregar la categoría '{actual.Nombre}'. Posible duplicado o padre '{actual.NombrePadre}' inexistente.");
                                }
                                else
                                {
                                    resultado.AgregarMensaje($"Éxito: Categoría '{actual.Nombre}' registrada correctamente.");
                                }

                                if (anterior == null) pendientes = siguiente;
                                else anterior.Siguiente = siguiente;
                                huboCambios = true;
                            }
                            else
                            {
                                anterior = actual;
                            }

                            actual = siguiente;
                        }
                    } while (huboCambios);

                    while (pendientes != null)
                    {
                        resultado.AgregarMensaje($"Error: No se pudo agregar la categoría '{pendientes.Nombre}'. Posible duplicado o padre '{pendientes.NombrePadre}' inexistente.");
                        pendientes = pendientes.Siguiente;
                    }
                }

                // 2. Procesar Libros (Carga incremental)
                XElement listaLibros = config.Element("listaLibros");
                if (listaLibros != null)
                {
                    foreach (XElement libNode in listaLibros.Elements("libro"))
                    {
                        string valorIsbn = libNode.Element("ISBN")?.Value.Trim() ?? "";
                        string titulo = libNode.Element("titulo")?.Value.Trim() ?? "Sin Título";
                        string autor = libNode.Element("autor")?.Value.Trim() ?? "Desconocido";
                        string nombreCategoria = libNode.Element("categoria")?.Value.Trim() ?? "";

                        if (!Validador.EsIsbnValido(valorIsbn))
                        {
                            resultado.AgregarMensaje("Error: Un libro tiene un formato de ISBN inválido (debe ser numérico).");
                            continue;
                        }

                        long isbn = Validador.ConvertirIsbn(valorIsbn);

                        bool exito = catalogo.AgregarLibro(isbn, titulo, autor, nombreCategoria);

                        if (!exito)
                        {
                            resultado.AgregarMensaje($"Error: No se pudo agregar el libro ISBN {isbn}. Categoría '{nombreCategoria}' desconocida o ISBN duplicado.");
                        }
                        else
                        {
                            resultado.AgregarMensaje($"Éxito: Libro ISBN {isbn} ('{titulo}') agregado correctamente.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado.AgregarMensaje($"Error al leer el archivo XML: {ex.Message}");
            }

            return resultado;
        }
    }
}