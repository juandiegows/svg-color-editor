using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Cambiar_Color_Imagen_SVG.SVG
{
    /// <summary>
    /// Un color de origen con el color por el que se va a reemplazar.
    /// </summary>
    public class ParColor
    {
        public ParColor(Color origen, Color destino)
        {
            Origen = origen;
            Destino = destino;
        }

        public Color Origen { get; set; }

        public Color Destino { get; set; }
    }

    /// <summary>
    /// Lee y reemplaza los colores solidos de un documento SVG.
    ///
    /// La version anterior solo miraba el Fill de los SvgPath, asi que en la mayoria de
    /// los SVG reales no cambiaba casi nada: se le escapaban los contornos (Stroke), las
    /// figuras basicas (rect, circle, ellipse, line, polygon) y los degradados. Aqui se
    /// recorre el arbol completo y se atienden las tres formas en las que un SVG declara
    /// un color solido: fill, stroke y las paradas de degradado (stop-color).
    /// </summary>
    public static class ColorSvg
    {
        /// <summary>
        /// Reemplaza en todo el documento cada color de origen por el destino que le
        /// corresponde.
        /// </summary>
        /// <param name="documento">El documento a modificar.</param>
        /// <param name="pares">Las parejas origen/destino a aplicar.</param>
        /// <param name="tolerancia">
        /// Cuanto se permite que un color se aleje del origen y aun asi cuente como el
        /// mismo, por canal (0 = coincidencia exacta). Ayuda con los SVG exportados de
        /// editores, que suelen repetir un color con variaciones minimas.
        /// </param>
        /// <returns>Cuantos colores se cambiaron.</returns>
        public static int Reemplazar(SvgDocument documento, IList<ParColor> pares, int tolerancia)
        {
            if (documento == null || pares == null || pares.Count == 0)
            {
                return 0;
            }

            int cambios = 0;

            RecorrerElementos(documento, elemento =>
            {
                cambios += ReemplazarEnElemento(elemento, pares, tolerancia);
            });

            return cambios;
        }

        /// <summary>
        /// Devuelve los colores solidos que usa el documento, sin repetir y ordenados por
        /// cuantas veces aparecen. Sirve para ofrecerle al usuario la paleta real de la
        /// imagen en vez de obligarlo a adivinar el color exacto con el cuentagotas.
        /// </summary>
        /// <param name="documento">El documento a inspeccionar.</param>
        /// <returns>Los colores encontrados, del mas usado al menos usado.</returns>
        public static List<Color> ObtenerPaleta(SvgDocument documento)
        {
            List<Color> paleta = new List<Color>();

            if (documento == null)
            {
                return paleta;
            }

            Dictionary<int, int> apariciones = new Dictionary<int, int>();

            RecorrerElementos(documento, elemento =>
            {
                foreach (Color color in ColoresDe(elemento))
                {
                    int clave = color.ToArgb();
                    apariciones.TryGetValue(clave, out int veces);
                    apariciones[clave] = veces + 1;
                }
            });

            List<KeyValuePair<int, int>> ordenados = new List<KeyValuePair<int, int>>(apariciones);
            ordenados.Sort((a, b) => b.Value.CompareTo(a.Value));

            foreach (KeyValuePair<int, int> entrada in ordenados)
            {
                paleta.Add(Color.FromArgb(entrada.Key));
            }

            return paleta;
        }

        /// <summary>
        /// Recorre el documento y todos sus descendientes.
        /// El propio SvgDocument tambien entra: puede declarar un fill que heredan sus
        /// hijos, y Descendants() no lo incluye.
        /// </summary>
        private static void RecorrerElementos(SvgDocument documento, Action<SvgElement> accion)
        {
            accion(documento);

            foreach (SvgElement descendiente in documento.Descendants())
            {
                accion(descendiente);
            }
        }

        private static int ReemplazarEnElemento(SvgElement elemento, IList<ParColor> pares, int tolerancia)
        {
            int cambios = 0;

            if (TryReemplazar(elemento.Fill, pares, tolerancia, out SvgColourServer nuevoFill))
            {
                elemento.Fill = nuevoFill;
                cambios++;
            }

            if (TryReemplazar(elemento.Stroke, pares, tolerancia, out SvgColourServer nuevoStroke))
            {
                elemento.Stroke = nuevoStroke;
                cambios++;
            }

            if (elemento is SvgGradientStop parada
                && TryReemplazar(parada.StopColor, pares, tolerancia, out SvgColourServer nuevoStop))
            {
                parada.StopColor = nuevoStop;
                cambios++;
            }

            return cambios;
        }

        /// <summary>
        /// Calcula el reemplazo de una pintura, si es que le toca alguno.
        /// </summary>
        /// <param name="pintura">La pintura actual (fill, stroke o stop-color).</param>
        /// <param name="pares">Las parejas origen/destino a aplicar.</param>
        /// <param name="tolerancia">La tolerancia por canal.</param>
        /// <param name="reemplazo">La pintura nueva, si hubo coincidencia.</param>
        /// <returns>True si hay que reemplazarla.</returns>
        private static bool TryReemplazar(
            SvgPaintServer pintura,
            IList<ParColor> pares,
            int tolerancia,
            out SvgColourServer reemplazo)
        {
            reemplazo = null;

            if (!EsColorReal(pintura, out Color actual))
            {
                return false;
            }

            foreach (ParColor par in pares)
            {
                if (SonElMismoColor(actual, par.Origen, tolerancia))
                {
                    // Se conserva el alfa original: el usuario elige el tono en un
                    // ColorDialog, que siempre devuelve opaco, y si se copiara tal cual
                    // se perderia la transparencia que traiga la figura.
                    reemplazo = new SvgColourServer(Color.FromArgb(actual.A, par.Destino));
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<Color> ColoresDe(SvgElement elemento)
        {
            if (EsColorReal(elemento.Fill, out Color relleno))
            {
                yield return relleno;
            }

            if (EsColorReal(elemento.Stroke, out Color contorno))
            {
                yield return contorno;
            }

            if (elemento is SvgGradientStop parada && EsColorReal(parada.StopColor, out Color stop))
            {
                yield return stop;
            }
        }

        /// <summary>
        /// Dice si una pintura es un color solido de verdad.
        ///
        /// Hay que descartar SvgPaintServer.None, Inherit y NotSet: son instancias
        /// centinela y, aunque son SvgColourServer, su Colour es negro. Si no se filtran,
        /// reemplazar el negro le asignaria un color a cada elemento que en realidad no
        /// pintaba nada o heredaba del padre, y la imagen se llenaria de manchas.
        /// La comparacion es por referencia a proposito: es lo unico que las distingue de
        /// un negro escrito por el autor del SVG.
        /// </summary>
        private static bool EsColorReal(SvgPaintServer pintura, out Color color)
        {
            color = Color.Empty;

            if (pintura == null
                || ReferenceEquals(pintura, SvgPaintServer.None)
                || ReferenceEquals(pintura, SvgPaintServer.Inherit)
                || ReferenceEquals(pintura, SvgPaintServer.NotSet))
            {
                return false;
            }

            SvgColourServer solido = pintura as SvgColourServer;

            if (solido == null)
            {
                // Degradados y patrones: sus colores se atienden en cada SvgGradientStop.
                return false;
            }

            color = solido.Colour;
            return true;
        }

        private static bool SonElMismoColor(Color uno, Color otro, int tolerancia)
        {
            if (tolerancia <= 0)
            {
                return uno.R == otro.R && uno.G == otro.G && uno.B == otro.B;
            }

            return Math.Abs(uno.R - otro.R) <= tolerancia
                && Math.Abs(uno.G - otro.G) <= tolerancia
                && Math.Abs(uno.B - otro.B) <= tolerancia;
        }
    }
}
