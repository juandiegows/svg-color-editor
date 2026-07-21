using Svg;
using Svg.FilterEffects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cambiar_Color_Imagen_SVG.SVG
{
    static class SVGParser
    {

      
        /// <summary>
        /// El tamaño de inicio
        /// </summary>
        public static Size SizeInicio { get; set; }

        /// <summary>
        /// Convierte un archivo SVG en una imagen Bitmap.
        /// </summary>
        /// <param name="filePath">La ruta completa de la imagen SVG.</param>
        /// <returns>Devuelve la imagen Bitmap convertida.</returns>
        public static Bitmap GetBitmapFromSVG(string filePath)
        {

            Size ancho = SizeInicio;
            SvgDocument svgDocument = SVGParser.GetSvgDocument(filePath);
            Bitmap bitmap = svgDocument.Draw();
            return bitmap;
        }

        /// <summary>
        /// Obtiene un SvgDocument para manipularlo usando la ruta proporcionada.
        /// </summary>
        /// <param name="filePath">La ruta de la imagen Bitmap</param>
        /// <returns>Returns el archivo SVG.</returns>
        public static SvgDocument GetSvgDocument(string filePath)
        {
            SvgDocument document = SvgDocument.Open(filePath);
            QuitarFiltrosNoSoportados(document);
            return AdjustSize(document);
        }

        /// <summary>
        /// Elimina las primitivas feColorMatrix de los filtros del documento.
        /// La libreria Svg no las compone correctamente: descarta el contenido del
        /// grupo filtrado y solo dibuja la sombra, por lo que la imagen se veia como
        /// una silueta gris sin colores. Las demas primitivas (feGaussianBlur,
        /// feOffset, feComposite, feBlend) si funcionan, asi que la sombra se conserva.
        /// Es habitual en los SVG exportados desde Figma, que usan feColorMatrix
        /// dentro del filtro de sombra.
        /// </summary>
        /// <param name="document">El documento SVG a limpiar.</param>
        private static void QuitarFiltrosNoSoportados(SvgDocument document)
        {
            foreach (SvgColourMatrix matriz in document.Descendants().OfType<SvgColourMatrix>().ToList())
            {
                if (matriz.Parent != null)
                {
                    matriz.Parent.Children.Remove(matriz);
                }
            }
        }

        /// <summary>
        /// Makes sure that the image does not exceed the maximum size, while preserving aspect ratio.
        /// </summary>
        /// <param name="document">The SVG document to resize.</param>
        /// <returns>Returns a resized or the original document depending on the document.</returns>
        private static SvgDocument AdjustSize(SvgDocument document)
        {
                document.Width = SizeInicio.Width;
                document.Height = SizeInicio.Height;                

            return document;
        }

    }
}
