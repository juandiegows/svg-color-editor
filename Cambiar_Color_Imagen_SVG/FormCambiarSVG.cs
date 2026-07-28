using Cambiar_Color_Imagen_SVG.Controles;
using Cambiar_Color_Imagen_SVG.Dialogos;
using Cambiar_Color_Imagen_SVG.Edicion;
using Cambiar_Color_Imagen_SVG.Galeria;
using Cambiar_Color_Imagen_SVG.Preferencias;
using Cambiar_Color_Imagen_SVG.SVG;
using Cambiar_Color_Imagen_SVG.Tema;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG
{
    public partial class FormCambiarSVG : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        private readonly Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        /// <summary>
        /// Lee el color del pixel que hay bajo un punto de la pantalla.
        /// </summary>
        public Color GetColor(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        /// <summary>
        /// El lado maximo al que se deja dibujar el documento. Un SVG puede pedir
        /// cualquier tamano, pero el mapa de bits intermedio crece al cuadrado y pasado
        /// este punto se agota la memoria en vez de dibujar nada util.
        /// </summary>
        private const int LadoMaximo = 4096;

        /// <summary>
        /// Cuanto se aleja como maximo un color leido con el cuentagotas del color real
        /// del SVG para darlos por el mismo. Al ampliar la vista el pixel que ve el
        /// usuario esta interpolado, asi que casi nunca coincide exacto.
        /// </summary>
        private const int DistanciaAjuste = 48;

        //Declaración de variables
        private string selectedPath;
        private Svg.SvgDocument svgDocument;
        private PanelGaleria galeria;

        private readonly HistorialEdicion historial = new HistorialEdicion();
        private readonly PreferenciasApp preferencias = PreferenciasApp.Cargar();
        private readonly Dictionary<Control, ColoresOriginales> coloresOriginales = new Dictionary<Control, ColoresOriginales>();

        private LienzoImagen lienzo;
        private List<Color> paletaImagen = new List<Color>();
        private bool actualizandoTamano;
        private string rutaInicial;

        private Button btnDeshacer;
        private Button btnRehacer;
        private Button btnCuadros;
        private Button btnTema;
        private Label lblZoom;

        private const int WS_SIZEBOX = 0x00040000;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int atributo, ref int valor, int tamano);

        private const int DWMWA_BORDER_COLOR = 34;
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_COLOR_NONE = unchecked((int)0xFFFFFFFE);

        /// <summary>
        /// Anade el borde de redimensionado de Windows al formulario.
        /// El formulario usa FormBorderStyle.None, asi que sin esto lo unico que se
        /// puede arrastrar es la franja de 1 pixel del Padding, y solo por la derecha
        /// y por abajo. Con WS_SIZEBOX es Windows quien gestiona los 8 lados, con sus
        /// cursores correctos, y ademas se habilita Aero Snap.
        ///
        /// Aqui NO se pone WS_EX_COMPOSITED, que seria lo mas efectivo contra el
        /// parpadeo: combinado con WS_SIZEBOX deja la ventana sin responder al arrastre
        /// de los bordes (se comprobo, fallaban los 8 lados). El parpadeo se mitiga con
        /// ActivarDobleBuffer.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_SIZEBOX;
                return cp;
            }
        }

        /// <summary>
        /// Disimula el marco que Windows pinta por culpa de WS_SIZEBOX.
        /// En un formulario sin barra de titulo ese marco se ve como una franja clara
        /// arriba, asi que se le pone el mismo color que la barra propia de la aplicacion
        /// y se quita la linea del contorno. Son APIs de Windows 11; en versiones
        /// anteriores la llamada devuelve error y no pasa nada.
        /// </summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            PintarMarcoDeVentana();
        }

        private void PintarMarcoDeVentana()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            int sinBorde = DWMWA_COLOR_NONE;
            DwmSetWindowAttribute(this.Handle, DWMWA_BORDER_COLOR, ref sinBorde, sizeof(int));

            // COLORREF es 0x00BBGGRR, al reves que Color.ToArgb().
            Color barra = panelSuperior.BackColor;
            int colorBarra = barra.R | (barra.G << 8) | (barra.B << 16);
            DwmSetWindowAttribute(this.Handle, DWMWA_CAPTION_COLOR, ref colorBarra, sizeof(int));
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        private const int WM_GETMINMAXINFO = 0x0024;
        private const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        /// <summary>
        /// Responde a WM_GETMINMAXINFO para calcular a mano el tamano y la posicion
        /// de maximizado.
        /// Con FormBorderStyle.None, el WindowState.Maximized por defecto de WinForms
        /// usa el monitor primario en vez del monitor donde esta la ventana, asi que en
        /// monitores secundarios no llegaba al maximo (o se pasaba al monitor vecino).
        /// La correccion es dar aqui, a mano, el area de trabajo del monitor real
        /// (MonitorFromWindow) expresada relativa a ese mismo monitor. No hace falta
        /// compensar ningun borde: esta ventana no tiene marco no-cliente (WS_SIZEBOX
        /// sin WS_CAPTION), asi que sumarle relleno solo desalineaba la ventana (dejaba
        /// hueco a la izquierda y se salia por la derecha al monitor siguiente).
        ///
        /// IMPORTANTE: no se puede dejar que esto siga a base.WndProc. Form tiene su
        /// propio manejo de WM_GETMINMAXINFO (el mismo que usa el monitor primario a
        /// secas) y, si se lo deja correr despues, vuelve a escribir el struct encima
        /// del nuestro. Eso se notaba como un margen de ~2px sobrante y, peor, como
        /// ptMaxTrackSize/ptMinTrackSize quedando mal calculados: el usuario no podia
        /// redimensionar la ventana en Normal, y al restaurar desde Maximizado quedaba
        /// con el mismo tamano maximizado en vez de volver al tamano anterior.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GETMINMAXINFO)
            {
                AjustarLimitesDeMaximizado(m);
                return;
            }

            base.WndProc(ref m);
        }

        private void AjustarLimitesDeMaximizado(Message m)
        {
            IntPtr monitor = MonitorFromWindow(this.Handle, MONITOR_DEFAULTTONEAREST);
            if (monitor == IntPtr.Zero)
            {
                return;
            }

            MONITORINFO info = new MONITORINFO();
            info.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            if (!GetMonitorInfo(monitor, ref info))
            {
                return;
            }

            MINMAXINFO mmi = (MINMAXINFO)m.GetLParam(typeof(MINMAXINFO));

            mmi.ptMaxPosition.X = info.rcWork.Left - info.rcMonitor.Left;
            mmi.ptMaxPosition.Y = info.rcWork.Top - info.rcMonitor.Top;
            mmi.ptMaxSize.X = info.rcWork.Right - info.rcWork.Left;
            mmi.ptMaxSize.Y = info.rcWork.Bottom - info.rcWork.Top;
            mmi.ptMaxTrackSize = mmi.ptMaxSize;

            Marshal.StructureToPtr(mmi, m.LParam, true);
        }

        public FormCambiarSVG() : this(null)
        {
        }

        /// <param name="rutaInicial">
        /// Un SVG para abrir en cuanto la ventana este lista, o null para arrancar vacia.
        /// </param>
        public FormCambiarSVG(string rutaInicial)
        {
            InitializeComponent();

            GuardarColoresOriginales(this);

            AgregarGaleria();
            MontarLienzo();
            MontarHerramientas();

            AplicarCursorDeMano(this);
            ActivarDobleBuffer(this);

            ConfigurarLimitesDeTamano();
            ConfigurarArrastreDeArchivos();

            this.KeyPreview = true;
            this.FormClosing += FormCambiarSVG_FormClosing;

            AplicarTema();
            ActualizarBotonesHistorial();

            this.rutaInicial = rutaInicial;
        }

        /// <summary>
        /// El SVG que se abre solo al arrancar se carga aqui y no en el constructor: el
        /// tamano con el que se dibuja sale del area visible del lienzo, y esa medida no
        /// es real hasta que la ventana esta en pantalla.
        /// </summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (!string.IsNullOrEmpty(rutaInicial))
            {
                string ruta = rutaInicial;
                rutaInicial = null;
                CargarSvg(ruta);
            }
        }

        /// <summary>
        /// El maximo que traia el disenador (99999) permite pedir un lienzo de miles de
        /// millones de pixeles, que revienta al dibujar. Se baja a algo que si se puede
        /// rasterizar.
        /// </summary>
        private void ConfigurarLimitesDeTamano()
        {
            nupAncho.Minimum = 1;
            nupAlto.Minimum = 1;
            nupAncho.Maximum = LadoMaximo;
            nupAlto.Maximum = LadoMaximo;
        }

        /// <summary>
        /// Monta la galeria de ejemplos como columna a la derecha del formulario.
        /// </summary>
        private void AgregarGaleria()
        {
            galeria = new PanelGaleria();
            galeria.SvgSeleccionado += CargarSvg;

            // El acoplado se resuelve del final de Controls hacia el principio, asi que
            // la barra de titulo se manda al final para que siga ocupando todo el ancho
            // y la galeria quede por debajo de ella.
            this.Controls.Add(galeria);
            this.panelSuperior.SendToBack();
        }

        /// <summary>
        /// Envuelve el PictureBox de la vista previa en un lienzo con zoom y scroll.
        /// </summary>
        private void MontarLienzo()
        {
            lienzo = new LienzoImagen(pickImagen)
            {
                Dock = DockStyle.Fill,
                MostrarCuadros = preferencias.FondoCuadros,
                BackColor = panelCentral.BackColor
            };

            lienzo.ZoomCambiado += (s, e) => ActualizarEtiquetaZoom();

            panelCentral.Controls.Add(lienzo);

            // El lienzo nace despues de la primera captura, asi que se registra aparte:
            // sin esto el tema no sabria a que color devolverlo y quedaria gris.
            GuardarColoresOriginales(lienzo);

            // El relleno se coloca de ultimo en el acoplado, es decir de primero en la
            // coleccion, para que la barra de herramientas de arriba conserve su franja.
            lienzo.BringToFront();
        }

        /// <summary>
        /// Anade al panel izquierdo la seccion de herramientas nuevas.
        /// El panel ya estaba lleno de arriba abajo, asi que se le activa el
        /// desplazamiento y se le da algo mas de ancho para que la barra no tape las
        /// tarjetas que ya habia.
        /// </summary>
        private void MontarHerramientas()
        {
            panelIzquierdo.Width = 244;
            panelIzquierdo.AutoScroll = true;

            // Sin este cambio las tarjetas seguirian al borde derecho del panel y la
            // barra de desplazamiento las recortaria.
            panelColor.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            panelTamaño.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // Con el panel desplazable, un boton acoplado abajo quedaria flotando sobre
            // el contenido; pasa a ser un control mas de la columna.
            btnAcerca.Dock = DockStyle.None;

            Label titulo = new Label
            {
                Text = "Herramientas",
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Century Gothic", 12F),
                Location = new Point(27, 463)
            };

            Panel caja = new Panel
            {
                Location = new Point(4, 490),
                Size = new Size(210, 250),
                BackColor = Color.FromArgb(120, 106, 210)
            };

            btnDeshacer = BotonHerramienta("Deshacer", new Point(8, 10), 95);
            btnDeshacer.Click += (s, e) => Deshacer();

            btnRehacer = BotonHerramienta("Rehacer", new Point(107, 10), 95);
            btnRehacer.Click += (s, e) => Rehacer();

            Button btnReemplazos = BotonHerramienta("Reemplazar varios...", new Point(8, 48), 194);
            btnReemplazos.Click += BtnReemplazos_Click;

            Button btnGuardarSvg = BotonHerramienta("Guardar SVG...", new Point(8, 88), 194);
            btnGuardarSvg.Click += BtnGuardarSvg_Click;

            Button btnExportar = BotonHerramienta("Exportar tamanos...", new Point(8, 128), 194);
            btnExportar.Click += BtnExportar_Click;

            lblZoom = new Label
            {
                Text = "Zoom 100%",
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(10, 176)
            };

            Button btnZoomMenos = BotonHerramienta("-", new Point(96, 170), 32);
            btnZoomMenos.Click += (s, e) => lienzo.Zoom = lienzo.Zoom / 1.2f;

            Button btnZoomMas = BotonHerramienta("+", new Point(132, 170), 32);
            btnZoomMas.Click += (s, e) => lienzo.Zoom = lienzo.Zoom * 1.2f;

            Button btnZoomReset = BotonHerramienta("1:1", new Point(168, 170), 34);
            btnZoomReset.Click += (s, e) => lienzo.Zoom = 1f;

            btnCuadros = BotonHerramienta("Cuadros", new Point(8, 208), 95);
            btnCuadros.Click += (s, e) => AlternarCuadros();

            btnTema = BotonHerramienta("Tema", new Point(107, 208), 95);
            btnTema.Click += (s, e) => AlternarTema();

            caja.Controls.Add(btnDeshacer);
            caja.Controls.Add(btnRehacer);
            caja.Controls.Add(btnReemplazos);
            caja.Controls.Add(btnGuardarSvg);
            caja.Controls.Add(btnExportar);
            caja.Controls.Add(lblZoom);
            caja.Controls.Add(btnZoomMenos);
            caja.Controls.Add(btnZoomMas);
            caja.Controls.Add(btnZoomReset);
            caja.Controls.Add(btnCuadros);
            caja.Controls.Add(btnTema);

            btnAcerca.Location = new Point(4, 752);
            btnAcerca.Size = new Size(210, 45);

            panelIzquierdo.Controls.Add(titulo);
            panelIzquierdo.Controls.Add(caja);

            GuardarColoresOriginales(titulo);
            GuardarColoresOriginales(caja);
        }

        private Button BotonHerramienta(string texto, Point posicion, int ancho)
        {
            Button boton = new Button
            {
                Text = texto,
                Location = posicion,
                Size = new Size(ancho, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(99, 86, 176),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };

            boton.FlatAppearance.BorderSize = 0;
            return boton;
        }

        /// <summary>
        /// Permite soltar un archivo SVG sobre la ventana para abrirlo.
        /// </summary>
        private void ConfigurarArrastreDeArchivos()
        {
            this.AllowDrop = true;
            this.DragEnter += Ventana_DragEnter;
            this.DragDrop += Ventana_DragDrop;

            lienzo.AllowDrop = true;
            lienzo.DragEnter += Ventana_DragEnter;
            lienzo.DragDrop += Ventana_DragDrop;

            pickImagen.AllowDrop = true;
            pickImagen.DragEnter += Ventana_DragEnter;
            pickImagen.DragDrop += Ventana_DragDrop;
        }

        private void Ventana_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = RutaSvgArrastrada(e) != null ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Ventana_DragDrop(object sender, DragEventArgs e)
        {
            string ruta = RutaSvgArrastrada(e);

            if (ruta != null)
            {
                CargarSvg(ruta);
            }
        }

        /// <summary>
        /// Devuelve el primer SVG del arrastre, o null si no viene ninguno.
        /// </summary>
        private static string RutaSvgArrastrada(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return null;
            }

            string[] rutas = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (rutas == null)
            {
                return null;
            }

            foreach (string ruta in rutas)
            {
                if (".svg".Equals(Path.GetExtension(ruta), StringComparison.OrdinalIgnoreCase))
                {
                    return ruta;
                }
            }

            return null;
        }

        /// <summary>
        /// Atajos de teclado. KeyPreview esta activo, asi que llegan aunque el foco este
        /// en un control hijo.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.O:
                    btnBuscar_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.S:
                    BtnGuardarSvg_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.E:
                    BtnExportar_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.Z:
                    Deshacer();
                    return true;

                case Keys.Control | Keys.Y:
                case Keys.Control | Keys.Shift | Keys.Z:
                    Rehacer();
                    return true;

                case Keys.Control | Keys.Oemplus:
                case Keys.Control | Keys.Add:
                    lienzo.Zoom = lienzo.Zoom * 1.2f;
                    return true;

                case Keys.Control | Keys.OemMinus:
                case Keys.Control | Keys.Subtract:
                    lienzo.Zoom = lienzo.Zoom / 1.2f;
                    return true;

                case Keys.Control | Keys.D0:
                    lienzo.Zoom = 1f;
                    return true;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        /// <summary>
        /// Activa el doble buffer en el formulario y en sus paneles.
        /// Al redimensionar, cada panel borra su fondo y lo vuelve a pintar, y eso es lo
        /// que se percibe como parpadeo. DoubleBuffered es una propiedad protegida de
        /// Control, asi que a los hijos solo se llega por reflexion.
        /// </summary>
        /// <param name="contenedor">El control desde el que se empieza a recorrer.</param>
        private static void ActivarDobleBuffer(Control contenedor)
        {
            PropertyInfo propiedad = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (propiedad == null)
            {
                return;
            }

            propiedad.SetValue(contenedor, true, null);

            foreach (Control hijo in contenedor.Controls)
            {
                // Solo los contenedores: son los que pintan grandes areas de fondo.
                if (hijo is Panel || hijo is Guna.UI2.WinForms.Guna2Panel)
                {
                    ActivarDobleBuffer(hijo);
                }
            }
        }

        /// <summary>
        /// Pone el cursor de mano sobre todos los botones del formulario.
        /// Guna2Button no lo trae por defecto y las flechas de ampliar/reducir son
        /// PictureBox, asi que hay que asignarlo a mano.
        /// </summary>
        /// <param name="contenedor">El control cuyos hijos se van a recorrer.</param>
        private void AplicarCursorDeMano(Control contenedor)
        {
            foreach (Control control in contenedor.Controls)
            {
                // pickImagen se excluye: su cursor cambia a cruz al elegir un color.
                bool esBoton = control != pickImagen
                    && (control is Guna.UI2.WinForms.Guna2Button
                        || control is Button
                        || control is PictureBox);

                if (esBoton)
                {
                    control.Cursor = Cursors.Hand;
                }

                if (control.HasChildren)
                {
                    AplicarCursorDeMano(control);
                }
            }
        }

        private void panelSuperior_MouseDown(object sender, MouseEventArgs e)
        {
            ArrastrarVentana();
        }

        private void lblLogo_MouseDown(object sender, MouseEventArgs e)
        {
            ArrastrarVentana();
        }

        /// <summary>
        /// Le pide a Windows que mueva la ventana como si se arrastrara por su barra de
        /// titulo, que es lo que este formulario no tiene.
        /// </summary>
        private void ArrastrarVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximizarNormal_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Normal)
            {
                this.Padding =  new Padding(0);
                WindowState = FormWindowState.Maximized;
                btnMaximizarNormal.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.Icono_Restaurar;
            }
            else
            {
                // Sin borde arriba: esa franja la cubre el marco de la ventana.
                this.Padding = new Padding(1, 0, 1, 1);
                WindowState = FormWindowState.Normal;
                btnMaximizarNormal.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.Maximizar;
            }
        }

        private void FormCambiarSVG_FormClosing(object sender, FormClosingEventArgs e)
        {
            preferencias.Guardar();
        }

        private void BtnAmpliar_MouseEnter(object sender, EventArgs e)
        {
            btnAmpliar.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.subir_hover;
        }

        private void BtnAmpliar_MouseLeave(object sender, EventArgs e)
        {
            btnAmpliar.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.subir;
        }

        private void btnReducir_MouseEnter(object sender, EventArgs e)
        {
            btnReducir.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.bajar_hover;
        }

        private void btnReducir_MouseLeave(object sender, EventArgs e)
        {
            btnReducir.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.bajar;
        }

        private void btnAmpliar_Click(object sender, EventArgs e)
        {
            EscalarDocumento(1.1f);
        }

        private void BtnReducir_Click(object sender, EventArgs e)
        {
            EscalarDocumento(1f / 1.1f);
        }

        /// <summary>
        /// Amplia o reduce el documento manteniendo su proporcion.
        ///
        /// La version anterior sumaba o restaba 10 pixeles a cada lado y solo lo hacia si
        /// el resultado cabia dentro del PictureBox. Eso daba la sensacion de que los
        /// botones funcionaban "a veces": en cuanto la imagen alcanzaba el area visible
        /// dejaban de responder sin avisar, y como esa area cambia al redimensionar o
        /// maximizar la ventana, el limite se movia solo. Ademas, sumar lo mismo a los
        /// dos lados deformaba cualquier imagen que no fuera cuadrada.
        /// Ahora el limite es el del rasterizador, no el del area visible, que para eso
        /// esta el zoom de la vista.
        /// </summary>
        /// <param name="factor">Cuanto se multiplica el tamano actual.</param>
        private void EscalarDocumento(float factor)
        {
            if (!ValidarForm())
            {
                return;
            }

            int anchoActual = (int)nupAncho.Value;
            int altoActual = (int)nupAlto.Value;

            int ancho = (int)Math.Round(anchoActual * factor);
            int alto = (int)Math.Round(altoActual * factor);

            // En imagenes pequenas el redondeo puede dejar el mismo numero y el boton
            // pareceria muerto; se fuerza al menos un pixel de diferencia.
            if (ancho == anchoActual)
            {
                ancho += factor > 1f ? 1 : -1;
            }

            if (alto == altoActual)
            {
                alto += factor > 1f ? 1 : -1;
            }

            if (ancho < 1 || alto < 1 || ancho > LadoMaximo || alto > LadoMaximo)
            {
                return;
            }

            AplicarTamano(ancho, alto, true);
        }

        /// <summary>
        /// Lleva un tamano al documento, a los contadores y a la vista, de una sola vez.
        /// </summary>
        /// <param name="ancho">El ancho en pixeles.</param>
        /// <param name="alto">El alto en pixeles.</param>
        /// <param name="registrar">True para anotar el cambio en el historial.</param>
        private void AplicarTamano(int ancho, int alto, bool registrar)
        {
            ancho = Math.Max(1, Math.Min(LadoMaximo, ancho));
            alto = Math.Max(1, Math.Min(LadoMaximo, alto));

            // Escribir en los contadores dispara sus ValueChanged, que a su vez redibujan.
            // Sin esta bandera cada clic provocaba tres dibujados y un fotograma
            // intermedio deformado, con el ancho nuevo y el alto todavia viejo.
            actualizandoTamano = true;

            try
            {
                nupAncho.Value = ancho;
                nupAlto.Value = alto;
            }
            finally
            {
                actualizandoTamano = false;
            }

            svgDocument.Width = ancho;
            svgDocument.Height = alto;

            Redibujar();

            if (registrar)
            {
                RegistrarCambio();
            }
        }

        private void nupAncho_ValueChanged(object sender, EventArgs e)
        {
            TamanoEscritoAMano();
        }

        private void nupAlto_ValueChanged(object sender, EventArgs e)
        {
            TamanoEscritoAMano();
        }

        /// <summary>
        /// Aplica el tamano que el usuario escribio en los contadores.
        /// </summary>
        private void TamanoEscritoAMano()
        {
            if (actualizandoTamano || svgDocument == null || pickImagen.Image == null)
            {
                return;
            }

            svgDocument.Width = (int)nupAncho.Value;
            svgDocument.Height = (int)nupAlto.Value;

            Redibujar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(preferencias.UltimaCarpeta) && Directory.Exists(preferencias.UltimaCarpeta))
            {
                filePicker.InitialDirectory = preferencias.UltimaCarpeta;
            }

            if (filePicker.ShowDialog() == DialogResult.OK)
            {
                CargarSvg(filePicker.FileName);
            }
        }

        /// <summary>
        /// Abre un SVG y lo deja listo para editar: es el camino que comparten el boton
        /// Buscar, la galeria de ejemplos y el arrastrar y soltar.
        /// </summary>
        /// <param name="ruta">La ruta completa del archivo SVG.</param>
        private void CargarSvg(string ruta)
        {
            try
            {
                SVGParser.SizeInicio = TamanoDeReferencia();
                svgDocument = SVGParser.GetSvgDocument(ruta);
            }
            catch (Exception error)
            {
                MostrarError("No se pudo abrir el SVG", ruta, error);
                return;
            }

            selectedPath = ruta;
            txtBuscar.Text = ruta;
            Guardar.FileName = ruta;
            preferencias.UltimaCarpeta = Path.GetDirectoryName(ruta);

            actualizandoTamano = true;

            try
            {
                nupAncho.Value = Math.Max(1, Math.Min(LadoMaximo, (int)svgDocument.Width.Value));
                nupAlto.Value = Math.Max(1, Math.Min(LadoMaximo, (int)svgDocument.Height.Value));
            }
            finally
            {
                actualizandoTamano = false;
            }

            Redibujar();

            historial.Reiniciar(svgDocument);
            ActualizarBotonesHistorial();
        }

        /// <summary>
        /// El tamano con el que se abre una imagen nueva: el area visible del lienzo, o
        /// una medida razonable si todavia no esta creado.
        /// </summary>
        private Size TamanoDeReferencia()
        {
            if (lienzo == null || lienzo.AreaVisible.Width < 16 || lienzo.AreaVisible.Height < 16)
            {
                return new Size(512, 512);
            }

            return lienzo.AreaVisible;
        }

        /// <summary>
        /// Vuelve a dibujar el documento y refresca la paleta detectada.
        /// </summary>
        private void Redibujar()
        {
            if (svgDocument == null)
            {
                return;
            }

            Image anterior = pickImagen.Image;

            try
            {
                lienzo.MostrarImagen(svgDocument.Draw());
            }
            catch (Exception error)
            {
                MostrarError("No se pudo dibujar la imagen", selectedPath, error);
                return;
            }

            // PictureBox no libera la imagen que reemplaza, y aqui se reemplaza en cada
            // cambio de color o de tamano.
            if (anterior != null && !ReferenceEquals(anterior, pickImagen.Image))
            {
                anterior.Dispose();
            }

            paletaImagen = ColorSvg.ObtenerPaleta(svgDocument);
            ActualizarEtiquetaZoom();
        }

        private void ActualizarEtiquetaZoom()
        {
            if (lblZoom != null)
            {
                lblZoom.Text = "Zoom " + (int)Math.Round(lienzo.Zoom * 100) + "%";
            }
        }

        private void BtnColorFondo_Click(object sender, EventArgs e)
        {
            if (ElegirColor(btnColorFondo.BackColor, out Color elegido))
            {
                btnColorFondo.BackColor = elegido;
                pickImagen.BackColor = elegido;

                // Un fondo solido y el tablero de transparencia no tienen sentido juntos.
                lienzo.MostrarCuadros = false;
                preferencias.FondoCuadros = false;
                ActualizarBotonCuadros();
            }
        }

        private void btnColorOrigen_Click(object sender, EventArgs e)
        {
            if (ElegirColor(btnColorOrigen.BackColor, out Color elegido))
            {
                btnColorOrigen.BackColor = elegido;
            }
        }

        private void btnColorDestino_Click(object sender, EventArgs e)
        {
            if (ElegirColor(btnColorDestino.BackColor, out Color elegido))
            {
                btnColorDestino.BackColor = elegido;
            }
        }

        /// <summary>
        /// Abre el selector de color precargado con los colores recientes y los de la
        /// imagen, y recuerda el que se elija.
        /// </summary>
        /// <param name="actual">El color desde el que se parte.</param>
        /// <param name="elegido">El color que eligio el usuario.</param>
        /// <returns>False si cerro el selector sin elegir.</returns>
        private bool ElegirColor(Color actual, out Color elegido)
        {
            elegido = actual;

            colorPicker.Color = actual;
            colorPicker.FullOpen = true;
            colorPicker.CustomColors = ColoresPersonalizados();

            if (colorPicker.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }

            elegido = colorPicker.Color;
            preferencias.AgregarReciente(elegido);

            return true;
        }

        /// <summary>
        /// Los cuadros de color del selector: primero lo que uso hace poco, y luego los
        /// colores que de verdad tiene la imagen abierta.
        /// </summary>
        private int[] ColoresPersonalizados()
        {
            List<int> valores = new List<int>();

            foreach (Color color in preferencias.ObtenerRecientes())
            {
                valores.Add(AOleColor(color));
            }

            foreach (Color color in paletaImagen)
            {
                if (valores.Count >= 16)
                {
                    break;
                }

                valores.Add(AOleColor(color));
            }

            return valores.ToArray();
        }

        /// <summary>
        /// ColorDialog.CustomColors usa BGR, al reves que Color.ToArgb().
        /// </summary>
        private static int AOleColor(Color color)
        {
            return color.R | (color.G << 8) | (color.B << 16);
        }

        private void btnElegirOrigen_CheckedChanged(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                if (btnElegirOrigen.Checked)
                {
                    btnElegirDestino.Checked = false;
                    btnElegirOrigen.FillColor = Color.LightPink;
                    pickImagen.Cursor = Cursors.Cross;
                }
                else
                {
                    btnElegirOrigen.FillColor = Color.FromArgb(94, 148, 255);
                    pickImagen.Cursor = Cursors.Default;
                }
            }
        }

        private void BtnElegirDestino_CheckedChanged(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                if (btnElegirDestino.Checked)
                {
                    btnElegirOrigen.Checked = false;
                    btnElegirDestino.FillColor = Color.LightPink;
                    pickImagen.Cursor = Cursors.Cross;
                }
                else
                {
                    btnElegirDestino.FillColor = Color.FromArgb(94, 148, 255);
                    pickImagen.Cursor = Cursors.Default;
                }
            }
        }

        private void pickImagen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if (!btnElegirOrigen.Checked && !btnElegirDestino.Checked)
            {
                return;
            }

            if (!ValidarForm())
            {
                return;
            }

            Color leido = AjustarAPaleta(GetColor(Cursor.Position));

            if (btnElegirOrigen.Checked)
            {
                btnColorOrigen.BackColor = leido;
                btnElegirOrigen.Checked = false;
            }
            else
            {
                btnColorDestino.BackColor = leido;
                btnElegirDestino.Checked = false;
            }

            preferencias.AgregarReciente(leido);
        }

        /// <summary>
        /// Lleva el color leido de la pantalla al color mas parecido de los que usa el
        /// SVG, si hay alguno lo bastante cerca.
        ///
        /// El cuentagotas lee el pixel ya dibujado, y ese pixel pasa por el suavizado del
        /// rasterizador y por la interpolacion del zoom. En los bordes casi nunca
        /// coincide con el color exacto del archivo, asi que sin este ajuste el usuario
        /// eligia un color que no existia en el SVG y "Cambiar" no hacia nada.
        /// </summary>
        /// <param name="leido">El color que se leyo de la pantalla.</param>
        private Color AjustarAPaleta(Color leido)
        {
            Color mejor = leido;
            int menorDistancia = int.MaxValue;

            foreach (Color color in paletaImagen)
            {
                int distancia = Math.Abs(color.R - leido.R)
                    + Math.Abs(color.G - leido.G)
                    + Math.Abs(color.B - leido.B);

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    mejor = color;
                }
            }

            return menorDistancia <= DistanciaAjuste ? mejor : leido;
        }

        private bool ValidarForm()
        {
            if (svgDocument == null || pickImagen.Image == null)
            {
                MessageBox.Show(this, "Por favor, selecciona una imagen SVG.", "Editor de Colores SVG",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void BtnAjustar_Click_3(object sender, EventArgs e)
        {
            if (!ValidarForm())
            {
                return;
            }

            Size area = TamanoDeReferencia();

            AplicarTamano(area.Width, area.Height, true);
            lienzo.Zoom = 1f;
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            if (!ValidarForm())
            {
                return;
            }

            AplicarReemplazos(
                new List<ParColor> { new ParColor(btnColorOrigen.BackColor, btnColorDestino.BackColor) },
                0);
        }

        private void BtnReemplazos_Click(object sender, EventArgs e)
        {
            if (!ValidarForm())
            {
                return;
            }

            using (FormReemplazos dialogo = new FormReemplazos(paletaImagen, preferencias.ObtenerRecientes(), PaletaActiva()))
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK || dialogo.Pares.Count == 0)
                {
                    return;
                }

                AplicarReemplazos(dialogo.Pares, dialogo.Tolerancia);
            }
        }

        /// <summary>
        /// Aplica las sustituciones al documento y avisa del resultado.
        /// Antes, si ningun color coincidia, la imagen simplemente no cambiaba y no habia
        /// forma de saber si el problema era el color elegido o el archivo.
        /// </summary>
        /// <param name="pares">Las parejas origen/destino.</param>
        /// <param name="tolerancia">La tolerancia por canal.</param>
        private void AplicarReemplazos(List<ParColor> pares, int tolerancia)
        {
            int cambios;

            try
            {
                cambios = ColorSvg.Reemplazar(svgDocument, pares, tolerancia);
            }
            catch (Exception error)
            {
                MostrarError("No se pudieron cambiar los colores", selectedPath, error);
                return;
            }

            if (cambios == 0)
            {
                MessageBox.Show(
                    this,
                    "Ninguna figura usaba ese color, asi que no hubo cambios.\n\n"
                        + "Prueba con el cuentagotas sobre la imagen, o abre "
                        + "\"Reemplazar varios...\" para elegir entre los colores que "
                        + "de verdad tiene el archivo.",
                    "Editor de Colores SVG",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            Redibujar();
            RegistrarCambio();
        }

        private void RegistrarCambio()
        {
            historial.Registrar(svgDocument);
            ActualizarBotonesHistorial();
        }

        private void Deshacer()
        {
            SvgDocument anterior = historial.Deshacer();

            if (anterior == null)
            {
                return;
            }

            RestaurarDocumento(anterior);
        }

        private void Rehacer()
        {
            SvgDocument siguiente = historial.Rehacer();

            if (siguiente == null)
            {
                return;
            }

            RestaurarDocumento(siguiente);
        }

        /// <summary>
        /// Deja en pantalla un documento recuperado del historial.
        /// </summary>
        private void RestaurarDocumento(SvgDocument documento)
        {
            svgDocument = documento;

            actualizandoTamano = true;

            try
            {
                nupAncho.Value = Math.Max(1, Math.Min(LadoMaximo, (int)svgDocument.Width.Value));
                nupAlto.Value = Math.Max(1, Math.Min(LadoMaximo, (int)svgDocument.Height.Value));
            }
            finally
            {
                actualizandoTamano = false;
            }

            Redibujar();
            ActualizarBotonesHistorial();
        }

        private void ActualizarBotonesHistorial()
        {
            if (btnDeshacer != null)
            {
                btnDeshacer.Enabled = historial.PuedeDeshacer;
            }

            if (btnRehacer != null)
            {
                btnRehacer.Enabled = historial.PuedeRehacer;
            }
        }

        /// <summary>
        /// Recarga el archivo desde el disco y descarta lo editado.
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedPath))
            {
                MessageBox.Show(this, "Primero abre una imagen SVG.", "Editor de Colores SVG",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CargarSvg(selectedPath);
        }

        /// <summary>
        /// Guarda un mapa de bits del tamano que se ve ahora.
        /// </summary>
        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (!ValidarForm())
            {
                return;
            }

            Guardar.Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|BMP (*.bmp)|*.bmp";
            Guardar.FileName = Path.GetFileNameWithoutExtension(selectedPath ?? "imagen") + ".png";

            if (!string.IsNullOrEmpty(preferencias.UltimaCarpeta) && Directory.Exists(preferencias.UltimaCarpeta))
            {
                Guardar.InitialDirectory = preferencias.UltimaCarpeta;
            }

            // El codigo anterior ignoraba el resultado del dialogo y guardaba igual, asi
            // que cancelar escribia un archivo con el nombre que hubiera quedado.
            if (Guardar.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                FormatoExportacion formato = FormatoPorExtension(Guardar.FileName);

                // El fondo del lienzo es una ayuda visual, no parte del dibujo: un PNG
                // se guarda con su transparencia intacta aunque en pantalla se este
                // viendo sobre un color. JPG y BMP no admiten alfa, y solo ahi se aplana
                // contra el color que eligio el usuario.
                Color fondo = formato == FormatoExportacion.Png
                    ? Color.Transparent
                    : pickImagen.BackColor;

                ExportadorSvg.GuardarMapaDeBits(
                    svgDocument,
                    Guardar.FileName,
                    formato,
                    (int)nupAncho.Value,
                    (int)nupAlto.Value,
                    fondo);

                preferencias.UltimaCarpeta = Path.GetDirectoryName(Guardar.FileName);
            }
            catch (Exception error)
            {
                MostrarError("No se pudo guardar la imagen", Guardar.FileName, error);
            }
        }

        /// <summary>
        /// Guarda el SVG editado, que es el resultado que de verdad busca esta
        /// aplicacion: el vector recoloreado y todavia escalable.
        /// </summary>
        private void BtnGuardarSvg_Click(object sender, EventArgs e)
        {
            if (!ValidarForm())
            {
                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog
            {
                Filter = "SVG (*.svg)|*.svg",
                FileName = Path.GetFileNameWithoutExtension(selectedPath ?? "imagen") + ".svg"
            })
            {
                if (!string.IsNullOrEmpty(preferencias.UltimaCarpeta) && Directory.Exists(preferencias.UltimaCarpeta))
                {
                    dialogo.InitialDirectory = preferencias.UltimaCarpeta;
                }

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    ExportadorSvg.GuardarSvg(svgDocument, dialogo.FileName);
                    preferencias.UltimaCarpeta = Path.GetDirectoryName(dialogo.FileName);
                }
                catch (Exception error)
                {
                    MostrarError("No se pudo guardar el SVG", dialogo.FileName, error);
                }
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            if (!ValidarForm())
            {
                return;
            }

            string nombre = Path.GetFileNameWithoutExtension(selectedPath ?? "imagen");
            string carpeta = preferencias.UltimaCarpeta;

            if (string.IsNullOrEmpty(carpeta) || !Directory.Exists(carpeta))
            {
                carpeta = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }

            using (FormExportar dialogo = new FormExportar(nombre, carpeta, pickImagen.BackColor, PaletaActiva()))
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    List<string> generados = ExportadorSvg.ExportarLote(
                        svgDocument,
                        dialogo.Carpeta,
                        dialogo.NombreBase,
                        dialogo.Formato,
                        dialogo.Tamanos,
                        dialogo.Fondo);

                    preferencias.UltimaCarpeta = dialogo.Carpeta;

                    MessageBox.Show(
                        this,
                        "Se generaron " + generados.Count + " archivos en:\n" + dialogo.Carpeta,
                        "Exportar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception error)
                {
                    MostrarError("No se pudo exportar", dialogo.Carpeta, error);
                }
            }
        }

        private static FormatoExportacion FormatoPorExtension(string ruta)
        {
            string extension = Path.GetExtension(ruta) ?? string.Empty;

            if (".jpg".Equals(extension, StringComparison.OrdinalIgnoreCase)
                || ".jpeg".Equals(extension, StringComparison.OrdinalIgnoreCase))
            {
                return FormatoExportacion.Jpg;
            }

            if (".bmp".Equals(extension, StringComparison.OrdinalIgnoreCase))
            {
                return FormatoExportacion.Bmp;
            }

            return FormatoExportacion.Png;
        }

        /// <summary>
        /// Muestra un fallo con su causa concreta.
        /// La version anterior se tragaba las excepciones en silencio, asi que un SVG con
        /// una estructura rara se veia igual que uno sin colores que cambiar.
        /// </summary>
        private void MostrarError(string resumen, string ruta, Exception error)
        {
            string detalle = resumen;

            if (!string.IsNullOrEmpty(ruta))
            {
                detalle += "\n\nArchivo: " + ruta;
            }

            detalle += "\n\nDetalle: " + error.Message;

            MessageBox.Show(this, detalle, "Editor de Colores SVG", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void AlternarCuadros()
        {
            lienzo.MostrarCuadros = !lienzo.MostrarCuadros;
            preferencias.FondoCuadros = lienzo.MostrarCuadros;
            ActualizarBotonCuadros();
        }

        private void ActualizarBotonCuadros()
        {
            if (btnCuadros != null)
            {
                btnCuadros.Text = lienzo.MostrarCuadros ? "Cuadros" : "Fondo";
            }
        }

        private void AlternarTema()
        {
            preferencias.TemaOscuro = !preferencias.TemaOscuro;
            AplicarTema();
        }

        private Paleta PaletaActiva()
        {
            return Paleta.De(preferencias.TemaOscuro);
        }

        /// <summary>
        /// Los colores con los que nacio un control, para poder volver al tema claro sin
        /// tener que replicar a mano todos los tonos del disenador.
        /// </summary>
        private class ColoresOriginales
        {
            public Color Fondo;
            public Color Texto;
        }

        private void GuardarColoresOriginales(Control contenedor)
        {
            if (!coloresOriginales.ContainsKey(contenedor))
            {
                coloresOriginales[contenedor] = new ColoresOriginales
                {
                    Fondo = contenedor.BackColor,
                    Texto = contenedor.ForeColor
                };
            }

            foreach (Control hijo in contenedor.Controls)
            {
                GuardarColoresOriginales(hijo);
            }
        }

        /// <summary>
        /// Pinta la interfaz con el tema activo.
        /// En claro se restauran los colores exactos del disenador; en oscuro se traduce
        /// cada tono de la paleta original a su equivalente hundido. Los cuadros que
        /// representan un color elegido por el usuario se dejan fuera: ahi el color es
        /// informacion, no decoracion.
        /// </summary>
        private void AplicarTema()
        {
            Paleta destino = PaletaActiva();
            Dictionary<int, Color> equivalencias = new Dictionary<int, Color>
            {
                { Paleta.Clara.Barra.ToArgb(), destino.Barra },
                { Paleta.Clara.Lateral.ToArgb(), destino.Lateral },
                { Paleta.Clara.Lienzo.ToArgb(), destino.Lienzo },
                { Color.FromArgb(120, 106, 210).ToArgb(), preferencias.TemaOscuro ? destino.Tarjeta : Color.FromArgb(120, 106, 210) }
            };

            AplicarTema(this, equivalencias, destino);

            if (galeria != null)
            {
                galeria.AplicarTema(destino);
            }

            if (btnTema != null)
            {
                btnTema.Text = preferencias.TemaOscuro ? "Claro" : "Oscuro";
            }

            ActualizarBotonCuadros();
            PintarMarcoDeVentana();
        }

        private void AplicarTema(Control contenedor, Dictionary<int, Color> equivalencias, Paleta destino)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (!EsDecoracion(control))
                {
                    continue;
                }

                if (coloresOriginales.TryGetValue(control, out ColoresOriginales original))
                {
                    control.BackColor = equivalencias.TryGetValue(original.Fondo.ToArgb(), out Color fondo)
                        ? fondo
                        : original.Fondo;

                    // El unico texto oscuro del diseno queda ilegible sobre el fondo
                    // hundido del tema oscuro.
                    control.ForeColor = preferencias.TemaOscuro && EsCasiNegro(original.Texto)
                        ? destino.Texto
                        : original.Texto;
                }

                if (control.HasChildren)
                {
                    AplicarTema(control, equivalencias, destino);
                }
            }
        }

        /// <summary>
        /// Los cuadros de color y la vista previa muestran datos, no la piel de la app.
        /// </summary>
        private bool EsDecoracion(Control control)
        {
            return control != btnColorOrigen
                && control != btnColorDestino
                && control != btnColorFondo
                && control != pickImagen;
        }

        private static bool EsCasiNegro(Color color)
        {
            return color.R < 96 && color.G < 96 && color.B < 96;
        }

        private void BtnAcerca_Click(object sender, EventArgs e)
        {
            using (FormAcerca formAcerca = new FormAcerca())
            {
                formAcerca.ShowDialog(this);
            }
        }
    }
}
