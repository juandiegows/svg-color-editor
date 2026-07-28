using System;
using System.IO;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// <param name="args">
        /// Opcionalmente, la ruta de un SVG para abrirlo al arrancar. Es lo que pasa
        /// Windows cuando se usa "Abrir con" o se arrastra un archivo sobre el
        /// ejecutable.
        /// </param>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormCambiarSVG(RutaInicial(args)));
        }

        /// <summary>
        /// Devuelve el primer argumento que sea un SVG existente, o null.
        /// </summary>
        private static string RutaInicial(string[] args)
        {
            if (args == null)
            {
                return null;
            }

            foreach (string argumento in args)
            {
                if (".svg".Equals(Path.GetExtension(argumento), StringComparison.OrdinalIgnoreCase)
                    && File.Exists(argumento))
                {
                    return argumento;
                }
            }

            return null;
        }
    }
}
