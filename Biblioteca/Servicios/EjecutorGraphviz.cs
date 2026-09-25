using System;
using System.Diagnostics;
using System.IO;

namespace Biblioteca.Servicios
{
    public class EjecutorGraphviz
    {
        public string GenerarImagenSvg(string dotContent, string fileName)
        {
            string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "graficos");
            if (!Directory.Exists(wwwrootPath))
            {
                Directory.CreateDirectory(wwwrootPath);
            }

            string dotFilePath = Path.Combine(wwwrootPath, $"{fileName}.dot");
            string svgFilePath = Path.Combine(wwwrootPath, $"{fileName}.svg");

            File.WriteAllText(dotFilePath, dotContent);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "dot",
                Arguments = $"-Tsvg \"{dotFilePath}\" -o \"{svgFilePath}\"",
                RedirectStandardOutput = false,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Graphviz no pudo generar el gráfico: {error}");
                }
            }

            return $"/graficos/{fileName}.svg";
        }
    }
}