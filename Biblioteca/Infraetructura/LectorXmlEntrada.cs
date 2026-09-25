using System;
using System.IO;
using System.Xml.Linq;
using Biblioteca.Dominio;

namespace Biblioteca.Servicios
{
    public class LectorXmlEntrada
    {
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

                // 1. Procesar Categorías (Carga incremental)
                XElement listaCategorias = config.Element("listaCategorias");
                if (listaCategorias != null)
                {
                    foreach (XElement catNode in listaCategorias.Elements("categoria"))
                    {
                        string nombreCategoria = catNode.Value.Trim();
                        XAttribute attrPadre = catNode.Attribute("padre");
                        
                        // El atributo padre es opcional
                        string nombrePadre = attrPadre != null ? attrPadre.Value.Trim() : "";

                        bool exito = catalogo.AgregarCategoria(nombreCategoria, nombrePadre);
                        
                        if (!exito)
                        {
                            resultado.AgregarMensaje($"Error: No se pudo agregar la categoría '{nombreCategoria}'. Posible duplicado o padre '{nombrePadre}' inexistente.");
                        }
                        else
                        {
                            resultado.AgregarMensaje($"Éxito: Categoría '{nombreCategoria}' registrada correctamente.");
                        }
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

                        if (!long.TryParse(valorIsbn, out long isbn))
                        {
                            resultado.AgregarMensaje("Error: Un libro tiene un formato de ISBN inválido (debe ser numérico).");
                            continue;
                        }

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