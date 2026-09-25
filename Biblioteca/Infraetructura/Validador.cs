namespace Biblioteca.Infraestructura
{
    /// Reglas de validación reutilizadas por el lector de XML  y por los formularios de la web , para que un mismo dato
    /// nunca se acepte en un lado y se rechace en el otro.
    public static class Validador
    {
        ///Un texto es válido si no es null y tiene al menos un carácter distinto de espacio (nombres de categoría, títulos, autores).
        public static bool EsTextoValido(string texto)
        {
            return !string.IsNullOrWhiteSpace(texto);
        }

        /// Un ISBN es válido si el texto representa un enteropositivo. Se usa antes de convertir a long, porque el XML
        /// siempre trae el ISBN como texto.
        public static bool EsIsbnValido(string textoIsbn)
        {
            if (string.IsNullOrWhiteSpace(textoIsbn)) return false;

            string t = textoIsbn.Trim();
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] < '0' || t[i] > '9') return false;
            }

            // Todo dígitos, pero puede desbordar long (más de 19 dígitos).
            return long.TryParse(t, out long valor) && valor > 0;
        }

        ///Convierte un ISBN ya validado con EsIsbnValido a long. Si el texto no es válido, devuelve 0.
        public static long ConvertirIsbn(string textoIsbn)
        {
            if (!EsIsbnValido(textoIsbn)) return 0;
            return long.Parse(textoIsbn.Trim());
        }

        /// <summary>Comparación alfabética de dos nombres, sin distinguir
        /// mayúsculas/minúsculas. La misma regla que usan los árboles de
        /// categorías, expuesta aquí para quien necesite comparar nombres
        /// sin pasar por un árbol (por ejemplo, al validar el XML).</summary>
        public static int CompararAlfabetico(string a, string b)
        {
            return string.Compare(a, b, System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Dos nombres son el mismo nombre de categoría si
        /// coinciden ignorando mayúsculas/minúsculas.</summary>
        public static bool SonElMismoNombre(string a, string b)
        {
            return CompararAlfabetico(a, b) == 0;
        }
    }
}