using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Cambiar_Color_Imagen_SVG.SVG
{
    /// <summary>
    /// Los formatos en los que la aplicacion puede exportar.
    /// </summary>
    public enum FormatoExportacion
    {
        Svg,
        Png,
        Jpg,
        Bmp
    }

    /// <summary>
    /// Guarda el documento editado en disco, como vector o como mapa de bits.
    /// </summary>
    public static class ExportadorSvg
    {
        /// <summary>
        /// Los tamanos que se ofrecen al exportar en lote. Son las medidas tipicas de
        /// iconos de aplicacion y de imagenes para redes.
        /// </summary>
        public static readonly int[] TamanosSugeridos = { 16, 32, 48, 64, 128, 256, 512, 1024 };

        public static string ExtensionDe(FormatoExportacion formato)
        {
            switch (formato)
            {
                case FormatoExportacion.Svg:
                    return ".svg";
                case FormatoExportacion.Jpg:
                    return ".jpg";
                case FormatoExportacion.Bmp:
                    return ".bmp";
                default:
                    return ".png";
            }
        }

        /// <summary>
        /// Guarda el SVG tal como quedo despues de editarlo, conservando el vector.
        /// Es lo que faltaba: hasta ahora solo se podia sacar un PNG, asi que el
        /// resultado de recolorear un SVG dejaba de ser escalable.
        /// </summary>
        /// <param name="documento">El documento a guardar.</param>
        /// <param name="ruta">La ruta destino.</param>
        public static void GuardarSvg(SvgDocument documento, string ruta)
        {
            if (documento == null)
            {
                throw new ArgumentNullException(nameof(documento));
            }

            documento.Write(ruta, false);
        }

        /// <summary>
        /// Dibuja el documento en un tamano concreto y lo guarda como mapa de bits.
        /// </summary>
        /// <param name="documento">El documento a dibujar.</param>
        /// <param name="ruta">La ruta destino.</param>
        /// <param name="formato">El formato de salida.</param>
        /// <param name="ancho">El ancho en pixeles.</param>
        /// <param name="alto">El alto en pixeles.</param>
        /// <param name="fondo">
        /// El color de fondo, o Color.Transparent para conservar la transparencia.
        /// </param>
        public static void GuardarMapaDeBits(
            SvgDocument documento,
            string ruta,
            FormatoExportacion formato,
            int ancho,
            int alto,
            Color fondo)
        {
            if (documento == null)
            {
                throw new ArgumentNullException(nameof(documento));
            }

            using (Bitmap dibujo = documento.Draw(ancho, alto))
            using (Bitmap salida = AplicarFondo(dibujo, formato, fondo))
            {
                salida.Save(ruta, FormatoImagenDe(formato));
            }
        }

        /// <summary>
        /// Exporta el documento a varios tamanos de una sola vez, dentro de una carpeta.
        /// </summary>
        /// <param name="documento">El documento a dibujar.</param>
        /// <param name="carpeta">La carpeta destino.</param>
        /// <param name="nombreBase">El nombre de archivo, sin extension ni tamano.</param>
        /// <param name="formato">El formato de salida.</param>
        /// <param name="tamanos">Los lados en pixeles a generar.</param>
        /// <param name="fondo">El color de fondo, o Color.Transparent.</param>
        /// <returns>Las rutas de los archivos generados.</returns>
        public static List<string> ExportarLote(
            SvgDocument documento,
            string carpeta,
            string nombreBase,
            FormatoExportacion formato,
            IEnumerable<int> tamanos,
            Color fondo)
        {
            List<string> generados = new List<string>();

            if (documento == null || tamanos == null)
            {
                return generados;
            }

            Directory.CreateDirectory(carpeta);

            foreach (int lado in tamanos)
            {
                if (lado <= 0)
                {
                    continue;
                }

                string nombre = nombreBase + "_" + lado + "x" + lado + ExtensionDe(formato);
                string ruta = Path.Combine(carpeta, nombre);

                if (formato == FormatoExportacion.Svg)
                {
                    // El vector no depende del tamano de salida, pero se respeta el lado
                    // pedido escribiendo width/height, que es lo que leen los
                    // navegadores cuando lo insertan sin estilos.
                    GuardarSvgConTamano(documento, ruta, lado);
                }
                else
                {
                    GuardarMapaDeBits(documento, ruta, formato, lado, lado, fondo);
                }

                generados.Add(ruta);
            }

            return generados;
        }

        /// <summary>
        /// Escribe el SVG fijandole un tamano, sin alterar el documento que se esta
        /// editando en pantalla.
        /// </summary>
        private static void GuardarSvgConTamano(SvgDocument documento, string ruta, int lado)
        {
            SvgUnit anchoPrevio = documento.Width;
            SvgUnit altoPrevio = documento.Height;

            try
            {
                documento.Width = lado;
                documento.Height = lado;
                documento.Write(ruta, false);
            }
            finally
            {
                documento.Width = anchoPrevio;
                documento.Height = altoPrevio;
            }
        }

        /// <summary>
        /// Pone el dibujo sobre el color de fondo pedido.
        /// JPG y BMP no guardan transparencia: si se guardaran tal cual, las zonas
        /// transparentes saldrian negras, asi que en esos formatos siempre se aplana
        /// contra un fondo (blanco si el usuario eligio transparente).
        /// </summary>
        private static Bitmap AplicarFondo(Bitmap dibujo, FormatoExportacion formato, Color fondo)
        {
            bool guardaAlfa = formato == FormatoExportacion.Png;

            if (guardaAlfa && fondo.A == 0)
            {
                return new Bitmap(dibujo);
            }

            Color relleno = fondo.A == 0 ? Color.White : fondo;

            Bitmap plano = new Bitmap(dibujo.Width, dibujo.Height, PixelFormat.Format32bppArgb);

            using (Graphics lienzo = Graphics.FromImage(plano))
            {
                lienzo.Clear(relleno);
                lienzo.DrawImageUnscaled(dibujo, 0, 0);
            }

            return plano;
        }

        private static ImageFormat FormatoImagenDe(FormatoExportacion formato)
        {
            switch (formato)
            {
                case FormatoExportacion.Jpg:
                    return ImageFormat.Jpeg;
                case FormatoExportacion.Bmp:
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Png;
            }
        }
    }
}
