using Cambiar_Color_Imagen_SVG.SVG;
using Cambiar_Color_Imagen_SVG.Tema;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG.Galeria
{
    /// <summary>
    /// Panel lateral con las miniaturas de los SVG de ejemplo que se distribuyen junto
    /// a la aplicacion. Al hacer clic en una miniatura se avisa con SvgSeleccionado.
    /// </summary>
    public class PanelGaleria : Panel
    {
        /// <summary>
        /// Carpeta de ejemplos. El csproj la copia al lado del ejecutable.
        /// </summary>
        private const string CarpetaEjemplos = "Personajes Ejemplo";

        private static readonly Size TamanoMiniatura = new Size(80, 80);

        private static readonly Color ColorPanel = Color.FromArgb(137, 123, 225);
        private static readonly Color ColorMiniatura = Color.FromArgb(180, 168, 251);
        private static readonly Color ColorSeleccion = Color.White;

        private readonly FlowLayoutPanel contenedor;
        private readonly ToolTip globo = new ToolTip();
        private readonly Label titulo;

        private PictureBox seleccionada;

        private Color colorMiniatura = ColorMiniatura;
        private Color colorSeleccion = ColorSeleccion;

        /// <summary>
        /// Se dispara con la ruta completa del SVG que eligio el usuario.
        /// </summary>
        public event Action<string> SvgSeleccionado;

        public PanelGaleria()
        {
            this.Dock = DockStyle.Right;
            this.Width = 200;
            this.BackColor = ColorPanel;

            contenedor = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(6, 6, 0, 6),
                BackColor = ColorPanel
            };

            titulo = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Text = "Ejemplos",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Century Gothic", 12F)
            };

            // El titulo se agrega de ultimo para que quede arriba y el contenedor ocupe
            // el resto: el acoplado se resuelve del final de Controls hacia el principio.
            this.Controls.Add(contenedor);
            this.Controls.Add(titulo);

            CargarMiniaturas();
        }

        /// <summary>
        /// Crea un PictureBox vacio por cada SVG de la carpeta de ejemplos. Las imagenes
        /// se dibujan despues, en DibujarEnSegundoPlano.
        /// </summary>
        private void CargarMiniaturas()
        {
            string carpeta = Path.Combine(AppContext.BaseDirectory, CarpetaEjemplos);

            if (!Directory.Exists(carpeta))
            {
                MostrarAviso("No se encontro la carpeta de ejemplos");
                return;
            }

            string[] rutas = Directory.GetFiles(carpeta, "*.svg").OrderBy(ruta => ruta).ToArray();

            if (rutas.Length == 0)
            {
                MostrarAviso("No hay ejemplos disponibles");
                return;
            }

            foreach (string ruta in rutas)
            {
                contenedor.Controls.Add(CrearMiniatura(ruta));
            }
        }

        /// <summary>
        /// Arma el PictureBox que representa un SVG, todavia sin imagen.
        /// </summary>
        /// <param name="ruta">La ruta completa del SVG.</param>
        /// <returns>El control listo para recibir la miniatura.</returns>
        private PictureBox CrearMiniatura(string ruta)
        {
            PictureBox miniatura = new PictureBox
            {
                Size = TamanoMiniatura,
                Margin = new Padding(4),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = colorMiniatura,
                Cursor = Cursors.Hand,
                Tag = ruta
            };

            miniatura.Click += Miniatura_Click;
            globo.SetToolTip(miniatura, Path.GetFileNameWithoutExtension(ruta));

            return miniatura;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Dibujar los 12 ejemplos cuesta un par de segundos porque hay que parsear
            // cada SVG entero. Si se hiciera aqui mismo, la ventana tardaria eso en
            // aparecer, asi que se dibujan aparte y van entrando de a una.
            Task.Run(() => DibujarEnSegundoPlano());
        }

        /// <summary>
        /// Dibuja las miniaturas fuera del hilo de la interfaz y las va colocando.
        /// </summary>
        private void DibujarEnSegundoPlano()
        {
            foreach (Control control in contenedor.Controls)
            {
                PictureBox miniatura = control as PictureBox;

                if (miniatura == null)
                {
                    continue;
                }

                Bitmap imagen;

                try
                {
                    imagen = SVGParser.GetMiniatura((string)miniatura.Tag, TamanoMiniatura);
                }
                catch (Exception)
                {
                    // Un SVG danado se salta: no debe impedir que se vea el resto.
                    continue;
                }

                if (!ColocarMiniatura(miniatura, imagen))
                {
                    // Cerraron la ventana mientras se dibujaba.
                    imagen.Dispose();
                    return;
                }
            }
        }

        /// <summary>
        /// Pasa la imagen ya dibujada al hilo de la interfaz.
        /// </summary>
        /// <param name="miniatura">El control que la va a mostrar.</param>
        /// <param name="imagen">La imagen recien dibujada.</param>
        /// <returns>False si la galeria ya se cerro y no se pudo colocar.</returns>
        private bool ColocarMiniatura(PictureBox miniatura, Bitmap imagen)
        {
            try
            {
                if (this.IsDisposed || !this.IsHandleCreated)
                {
                    return false;
                }

                this.BeginInvoke(new Action(() => miniatura.Image = imagen));
                return true;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        private void Miniatura_Click(object sender, EventArgs e)
        {
            PictureBox miniatura = (PictureBox)sender;

            Resaltar(miniatura);

            if (SvgSeleccionado != null)
            {
                SvgSeleccionado((string)miniatura.Tag);
            }
        }

        /// <summary>
        /// Marca cual miniatura esta en uso y devuelve la anterior a su color normal.
        /// </summary>
        /// <param name="miniatura">La miniatura recien elegida.</param>
        private void Resaltar(PictureBox miniatura)
        {
            if (seleccionada != null)
            {
                seleccionada.BackColor = colorMiniatura;
            }

            seleccionada = miniatura;
            miniatura.BackColor = colorSeleccion;
        }

        /// <summary>
        /// Repinta la galeria con los colores del tema activo.
        /// </summary>
        /// <param name="paleta">La paleta del tema.</param>
        public void AplicarTema(Paleta paleta)
        {
            if (paleta == null)
            {
                return;
            }

            colorMiniatura = paleta.Tarjeta;
            colorSeleccion = paleta.Seleccion;

            this.BackColor = paleta.Lateral;
            contenedor.BackColor = paleta.Lateral;
            titulo.ForeColor = paleta.Texto;

            foreach (Control control in contenedor.Controls)
            {
                if (control is PictureBox miniatura)
                {
                    miniatura.BackColor = ReferenceEquals(miniatura, seleccionada)
                        ? colorSeleccion
                        : colorMiniatura;
                }
                else if (control is Label aviso)
                {
                    aviso.ForeColor = paleta.Texto;
                }
            }
        }

        private void MostrarAviso(string texto)
        {
            contenedor.Controls.Add(new Label
            {
                Text = texto,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(170, 60),
                Margin = new Padding(4)
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && contenedor != null)
            {
                // PictureBox no libera su Image al hacer Dispose, hay que hacerlo aqui.
                foreach (Control control in contenedor.Controls)
                {
                    PictureBox miniatura = control as PictureBox;

                    if (miniatura != null && miniatura.Image != null)
                    {
                        miniatura.Image.Dispose();
                    }
                }

                globo.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
