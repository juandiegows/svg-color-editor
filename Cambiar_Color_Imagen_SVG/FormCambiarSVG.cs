using Cambiar_Color_Imagen_SVG.SVG;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG
{
    public partial class FormCambiarSVG : Form
    {

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);


        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColor(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        //Declaración de variables
        private string selectedPath;
        private Svg.SvgDocument svgDocument;

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

            int sinBorde = DWMWA_COLOR_NONE;
            DwmSetWindowAttribute(this.Handle, DWMWA_BORDER_COLOR, ref sinBorde, sizeof(int));

            // COLORREF es 0x00BBGGRR, al reves que Color.ToArgb().
            Color barra = panelSuperior.BackColor;
            int colorBarra = barra.R | (barra.G << 8) | (barra.B << 16);
            DwmSetWindowAttribute(this.Handle, DWMWA_CAPTION_COLOR, ref colorBarra, sizeof(int));
        }

        public FormCambiarSVG()
        {
            InitializeComponent();
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            AplicarCursorDeMano(this);
            ActivarDobleBuffer(this);
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
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void lblLogo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
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

        private void BtnReducir_Click(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                int W = (int)nupAncho.Value;
                int H = (int)nupAlto.Value;

                if ((W - 10) > 0 && (H - 10) > 0)
                {
                    W -= 10;
                    nupAncho.Value = W;

                    H -= 10;
                    nupAlto.Value = H;

                    svgDocument.Width = W;
                    svgDocument.Height = H;

                    pickImagen.Image = svgDocument.Draw();
                }
            }
        }

      
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DialogResult selectResult = filePicker.ShowDialog();
            if (selectResult == System.Windows.Forms.DialogResult.OK)
            {
                
                SVGParser.SizeInicio = new Size(pickImagen.Width, pickImagen.Height);
                selectedPath = filePicker.FileName;
                txtBuscar.Text = selectedPath;
                svgDocument = SVGParser.GetSvgDocument(selectedPath);
                nupAncho.Value = (int) svgDocument.Width.Value;
                nupAlto.Value = (int)  svgDocument.Height.Value;
                pickImagen.Image = SVGParser.GetBitmapFromSVG(selectedPath);
                Guardar.FileName = filePicker.FileName;
            }
        }

        private void BtnColorFondo_Click(object sender, EventArgs e)
        {
            DialogResult result = colorPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                btnColorFondo.BackColor = colorPicker.Color;
                pickImagen.BackColor = colorPicker.Color;
            }
        }

        private void btnColorOrigen_Click(object sender, EventArgs e)
        {
            DialogResult result = colorPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                btnColorOrigen.BackColor = colorPicker.Color;
            }
        }

        private void btnColorDestino_Click(object sender, EventArgs e)
        {
            DialogResult result = colorPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                btnColorDestino.BackColor = colorPicker.Color;
            }
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

        private void btnAmpliar_Click(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                int W = (int)nupAncho.Value;
                int H = (int)nupAlto.Value;

                if ((W + 10) < pickImagen.Width && (H + 10) < pickImagen.Height)
                {
                    W += 10;
                    nupAncho.Value = W;

                    H += 10;
                    nupAlto.Value = H;

                    svgDocument.Width = W;
                    svgDocument.Height = H;

                    pickImagen.Image = svgDocument.Draw();
                }
            }
        }
        private bool ValidarForm()
        {
            if (svgDocument == null || pickImagen.Image == null)
            {
                MessageBox.Show("Por favor, Seleccione una imagen SVG");
                return false;
            }
            return true;
        }

        private void BtnAjustar_Click_3(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                svgDocument.Width = pickImagen.Width;
                svgDocument.Height = pickImagen.Height;
                nupAncho.Value = pickImagen.Width;
                nupAlto.Value = pickImagen.Height;
                pickImagen.Image = svgDocument.Draw();
            }
          
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                foreach (Svg.SvgElement item in svgDocument.Children)
                {
                    CambiarColor(item, btnColorOrigen.BackColor, btnColorDestino.BackColor);
                }
                pickImagen.Image = svgDocument.Draw();
            }
         
        }

        private void CambiarColor(SvgElement element, Color sourceColor, Color replaceColor)
        {
            try
            {
                if (element is SvgPath)
                {

                    if (((element as SvgPath).Fill as SvgColourServer).Colour.ToArgb() == sourceColor.ToArgb())
                    {
                        (element as SvgPath).Fill = new SvgColourServer(replaceColor);
                    }
                }
            }
            catch (Exception)
            {


            }

            if (element.Children.Count > 0)
            {
                foreach (var item in element.Children)
                {
                    CambiarColor(item, sourceColor, replaceColor);
                }
            }

        }

        private void pickImagen_MouseDown(object sender, MouseEventArgs e)
        {
            if (btnElegirOrigen.Checked)
            {
                if (ValidarForm())
                {


                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {

                        btnColorOrigen.BackColor = GetColor(Cursor.Position);
                        btnElegirOrigen.Checked = false;
                    }
                }
            }

            if (btnElegirDestino.Checked)
            {
                if (ValidarForm())
                {


                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {

                        btnColorDestino.BackColor = GetColor(Cursor.Position);
                        btnElegirDestino.Checked = false;
                    }
                }
            }
        }

        private void nupAncho_ValueChanged(object sender, EventArgs e)
        {
         
                int W = (int)nupAncho.Value;
                int H = (int)nupAlto.Value;

                if (W != 0 && H != 0 && pickImagen.Image != null)
                {

                    if (ValidarForm())
                    {
                        svgDocument.Width = W;
                        svgDocument.Height = H;

                        pickImagen.Image = svgDocument.Draw();
                    }

                }
            
           
        }

        private void nupAlto_ValueChanged(object sender, EventArgs e)
        {

           
                int W = (int)nupAncho.Value;
                int H = (int)nupAlto.Value;

                if (W != 0 && H != 0 && pickImagen.Image != null)
                {
                    if (ValidarForm())
                    {
                        svgDocument.Width = W;
                        svgDocument.Height = H;

                        pickImagen.Image = svgDocument.Draw();
                    }

                }
            
           
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                Guardar.FileName = Guardar.FileName.ToString().Replace(".SVG", ".PNG").Replace(".svg", ".png");
                Image Imagen = pickImagen.Image;
                Guardar.ShowDialog();
                Imagen.Save(Guardar.FileName);
            }
           
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarForm())
            {
                SVGParser.SizeInicio = new Size(pickImagen.Width, pickImagen.Height);
                svgDocument = SVGParser.GetSvgDocument(selectedPath);
                nupAncho.Value = (int)svgDocument.Width.Value;
                nupAlto.Value = (int)svgDocument.Height.Value;
                pickImagen.Image = SVGParser.GetBitmapFromSVG(selectedPath);
            }
             
        }

        private void BtnAcerca_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Este software fue hecho por Juan Diego\npara el canal de código Limpio");
        }
    }
}
