using System.Drawing;

namespace Cambiar_Color_Imagen_SVG.Tema
{
    /// <summary>
    /// Los colores de la interfaz. Se agrupan por el papel que cumplen y no por su tono,
    /// para que cambiar de tema sea cambiar de paleta y no ir tocando control por control.
    /// </summary>
    public class Paleta
    {
        public Color Barra { get; private set; }

        public Color Lateral { get; private set; }

        public Color Lienzo { get; private set; }

        public Color Tarjeta { get; private set; }

        public Color Texto { get; private set; }

        public Color TextoSuave { get; private set; }

        public Color Acento { get; private set; }

        public Color Seleccion { get; private set; }

        /// <summary>
        /// La paleta original de la aplicacion, en morados claros.
        /// </summary>
        public static readonly Paleta Clara = new Paleta
        {
            Barra = Color.FromArgb(99, 86, 176),
            Lateral = Color.FromArgb(137, 123, 225),
            Lienzo = Color.FromArgb(180, 168, 251),
            Tarjeta = Color.FromArgb(180, 168, 251),
            Texto = Color.White,
            TextoSuave = Color.FromArgb(235, 232, 250),
            Acento = Color.FromArgb(94, 148, 255),
            Seleccion = Color.White
        };

        /// <summary>
        /// La version oscura, con los mismos morados pero desaturados y hundidos, para
        /// que la aplicacion siga siendo reconocible.
        /// </summary>
        public static readonly Paleta Oscura = new Paleta
        {
            Barra = Color.FromArgb(38, 34, 60),
            Lateral = Color.FromArgb(52, 46, 82),
            Lienzo = Color.FromArgb(30, 27, 45),
            Tarjeta = Color.FromArgb(62, 55, 95),
            Texto = Color.White,
            TextoSuave = Color.FromArgb(190, 184, 215),
            Acento = Color.FromArgb(126, 152, 255),
            Seleccion = Color.FromArgb(126, 152, 255)
        };

        public static Paleta De(bool oscuro)
        {
            return oscuro ? Oscura : Clara;
        }
    }
}
