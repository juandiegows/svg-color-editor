using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG.Controles
{
    /// <summary>
    /// Area de previsualizacion con desplazamiento, zoom y fondo de cuadros.
    ///
    /// No crea su propio PictureBox: adopta el que ya existe en el disenador. Asi los
    /// eventos que ya tenia conectados (el cuentagotas del MouseDown, sobre todo) siguen
    /// funcionando sin volver a cablearlos.
    /// </summary>
    public class LienzoImagen : Panel
    {
        private const float ZoomMinimo = 0.1f;
        private const float ZoomMaximo = 8f;
        private const int LadoCuadro = 8;

        private readonly PictureBox lienzo;

        private Bitmap cuadros;
        private float zoom = 1f;
        private bool mostrarCuadros = true;

        /// <summary>
        /// Se dispara cuando cambia el zoom, para que la interfaz muestre el porcentaje.
        /// </summary>
        public event EventHandler ZoomCambiado;

        /// <param name="existente">El PictureBox del disenador que va a mostrar la imagen.</param>
        public LienzoImagen(PictureBox existente)
        {
            lienzo = existente ?? throw new ArgumentNullException(nameof(existente));

            this.AutoScroll = true;

            // Con Dock.Fill el PictureBox nunca podria ser mas grande que el area visible,
            // y sin eso no hay desplazamiento ni zoom posible.
            lienzo.Dock = DockStyle.None;
            lienzo.SizeMode = PictureBoxSizeMode.Zoom;
            lienzo.BackgroundImageLayout = ImageLayout.Tile;

            if (lienzo.Parent != null)
            {
                lienzo.Parent.Controls.Remove(lienzo);
            }

            this.Controls.Add(lienzo);

            lienzo.MouseWheel += Lienzo_MouseWheel;
            this.MouseWheel += Lienzo_MouseWheel;
        }

        /// <summary>
        /// El PictureBox que muestra la imagen.
        /// </summary>
        public PictureBox Vista
        {
            get { return lienzo; }
        }

        /// <summary>
        /// El factor de ampliacion de la vista. No cambia el tamano del documento SVG:
        /// solo el de la previsualizacion.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Zoom
        {
            get { return zoom; }
            set
            {
                float nuevo = Math.Min(ZoomMaximo, Math.Max(ZoomMinimo, value));

                if (Math.Abs(nuevo - zoom) < 0.001f)
                {
                    return;
                }

                zoom = nuevo;
                Reacomodar();
                ZoomCambiado?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// True para pintar el tablero de cuadros que representa la transparencia.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MostrarCuadros
        {
            get { return mostrarCuadros; }
            set
            {
                mostrarCuadros = value;
                AplicarFondo();
            }
        }

        /// <summary>
        /// Coloca la imagen y recalcula el tamano de la vista.
        /// </summary>
        /// <param name="imagen">La imagen ya dibujada, o null para vaciar el lienzo.</param>
        public void MostrarImagen(Image imagen)
        {
            lienzo.Image = imagen;
            Reacomodar();
        }

        /// <summary>
        /// Ajusta el zoom para que la imagen entre completa en el area visible.
        /// </summary>
        public void AjustarAVentana()
        {
            if (lienzo.Image == null)
            {
                return;
            }

            float porAncho = (float)this.ClientSize.Width / lienzo.Image.Width;
            float porAlto = (float)this.ClientSize.Height / lienzo.Image.Height;

            // Nunca se amplia de mas al ajustar: una imagen pequena se deja a su tamano.
            Zoom = Math.Min(1f, Math.Min(porAncho, porAlto));
        }

        /// <summary>
        /// El area util para dibujar, sin contar las barras de desplazamiento.
        /// Es la medida que debe usar "Ajustar" para calcular el tamano del documento.
        /// </summary>
        public Size AreaVisible
        {
            get { return this.ClientSize; }
        }

        /// <summary>
        /// Recalcula el tamano y la posicion de la vista dentro del area con scroll.
        /// </summary>
        private void Reacomodar()
        {
            if (lienzo.Image == null)
            {
                lienzo.Size = Size.Empty;
                return;
            }

            int ancho = Math.Max(1, (int)Math.Round(lienzo.Image.Width * zoom));
            int alto = Math.Max(1, (int)Math.Round(lienzo.Image.Height * zoom));

            lienzo.Size = new Size(ancho, alto);
            Centrar();
            AplicarFondo();
        }

        /// <summary>
        /// Deja la imagen centrada mientras quepa, y pegada al scroll cuando no cabe.
        /// </summary>
        private void Centrar()
        {
            int x = Math.Max(0, (this.ClientSize.Width - lienzo.Width) / 2);
            int y = Math.Max(0, (this.ClientSize.Height - lienzo.Height) / 2);

            lienzo.Location = new Point(x + this.AutoScrollPosition.X, y + this.AutoScrollPosition.Y);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Centrar();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Centrar();
        }

        /// <summary>
        /// Ctrl + rueda hace zoom; la rueda sola desplaza, como en cualquier visor.
        /// </summary>
        private void Lienzo_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != Keys.Control)
            {
                return;
            }

            Zoom = e.Delta > 0 ? zoom * 1.15f : zoom / 1.15f;
        }

        private void AplicarFondo()
        {
            if (!mostrarCuadros)
            {
                lienzo.BackgroundImage = null;
                return;
            }

            if (cuadros == null)
            {
                cuadros = CrearCuadros();
            }

            lienzo.BackgroundImage = cuadros;
        }

        /// <summary>
        /// Genera el mosaico de dos tonos que se repite detras de la imagen.
        /// </summary>
        private static Bitmap CrearCuadros()
        {
            Bitmap mosaico = new Bitmap(LadoCuadro * 2, LadoCuadro * 2);

            using (Graphics dibujo = Graphics.FromImage(mosaico))
            using (SolidBrush claro = new SolidBrush(Color.FromArgb(255, 255, 255)))
            using (SolidBrush oscuro = new SolidBrush(Color.FromArgb(214, 214, 214)))
            {
                dibujo.SmoothingMode = SmoothingMode.None;
                dibujo.FillRectangle(claro, 0, 0, mosaico.Width, mosaico.Height);
                dibujo.FillRectangle(oscuro, 0, 0, LadoCuadro, LadoCuadro);
                dibujo.FillRectangle(oscuro, LadoCuadro, LadoCuadro, LadoCuadro, LadoCuadro);
            }

            return mosaico;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && cuadros != null)
            {
                lienzo.BackgroundImage = null;
                cuadros.Dispose();
                cuadros = null;
            }

            base.Dispose(disposing);
        }
    }
}
