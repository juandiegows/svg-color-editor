using Svg;
using System.Collections.Generic;

namespace Cambiar_Color_Imagen_SVG.Edicion
{
    /// <summary>
    /// Historial de deshacer y rehacer del documento que se esta editando.
    ///
    /// Guarda instantaneas del SVG serializado a texto en vez de ir anotando cada accion
    /// por separado. Es mas simple y no se puede desincronizar: cualquier cambio (color,
    /// tamano, o los que se agreguen despues) queda cubierto sin tener que escribir su
    /// operacion inversa. El costo es el texto de cada instantanea, que para un SVG es
    /// pequeno y ademas queda acotado por PasosMaximos.
    /// </summary>
    public class HistorialEdicion
    {
        private const int PasosMaximos = 40;

        private readonly List<string> pasado = new List<string>();
        private readonly List<string> futuro = new List<string>();

        private string actual;

        public bool PuedeDeshacer
        {
            get { return pasado.Count > 0; }
        }

        public bool PuedeRehacer
        {
            get { return futuro.Count > 0; }
        }

        /// <summary>
        /// Empieza de cero con el documento indicado como estado inicial.
        /// Se llama al abrir un SVG: el historial del archivo anterior ya no aplica.
        /// </summary>
        /// <param name="documento">El documento recien abierto.</param>
        public void Reiniciar(SvgDocument documento)
        {
            pasado.Clear();
            futuro.Clear();
            actual = Serializar(documento);
        }

        /// <summary>
        /// Anota que el documento acaba de cambiar.
        /// </summary>
        /// <param name="documento">El documento ya modificado.</param>
        public void Registrar(SvgDocument documento)
        {
            string instantanea = Serializar(documento);

            if (instantanea == null || instantanea == actual)
            {
                // Sin cambios reales: no se ensucia el historial con pasos vacios.
                return;
            }

            if (actual != null)
            {
                pasado.Add(actual);

                if (pasado.Count > PasosMaximos)
                {
                    pasado.RemoveAt(0);
                }
            }

            actual = instantanea;

            // Al abrir una rama nueva, lo que se habia deshecho deja de ser alcanzable.
            futuro.Clear();
        }

        /// <summary>
        /// Retrocede un paso.
        /// </summary>
        /// <returns>El documento anterior, o null si no habia nada que deshacer.</returns>
        public SvgDocument Deshacer()
        {
            if (pasado.Count == 0)
            {
                return null;
            }

            futuro.Add(actual);
            actual = Quitar(pasado);

            return Deserializar(actual);
        }

        /// <summary>
        /// Avanza un paso de los que se habian deshecho.
        /// </summary>
        /// <returns>El documento siguiente, o null si no habia nada que rehacer.</returns>
        public SvgDocument Rehacer()
        {
            if (futuro.Count == 0)
            {
                return null;
            }

            pasado.Add(actual);
            actual = Quitar(futuro);

            return Deserializar(actual);
        }

        private static string Quitar(List<string> lista)
        {
            string ultimo = lista[lista.Count - 1];
            lista.RemoveAt(lista.Count - 1);
            return ultimo;
        }

        private static string Serializar(SvgDocument documento)
        {
            if (documento == null)
            {
                return null;
            }

            return documento.GetXML();
        }

        private static SvgDocument Deserializar(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            return SvgDocument.FromSvg<SvgDocument>(xml);
        }
    }
}
