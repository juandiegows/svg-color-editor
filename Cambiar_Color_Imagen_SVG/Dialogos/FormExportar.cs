using Cambiar_Color_Imagen_SVG.SVG;
using Cambiar_Color_Imagen_SVG.Tema;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG.Dialogos
{
    /// <summary>
    /// Exporta la imagen en varios tamanos de una sola pasada.
    /// Pensado para el caso mas comun de la aplicacion: sacar el mismo icono ya
    /// recoloreado en todas las medidas que pide una tienda o un proyecto.
    /// </summary>
    public class FormExportar : Form
    {
        private readonly Paleta paleta;
        private readonly List<CheckBox> casillas = new List<CheckBox>();

        private ComboBox cmbFormato;
        private TextBox txtCarpeta;
        private TextBox txtNombre;
        private CheckBox chkTransparente;
        private NumericUpDown nupPersonalizado;
        private CheckBox chkPersonalizado;

        private Color fondo;

        /// <param name="nombreSugerido">El nombre de archivo, sin extension.</param>
        /// <param name="carpetaSugerida">La carpeta donde proponer el guardado.</param>
        /// <param name="fondoActual">El color de fondo que se ve en el lienzo.</param>
        /// <param name="paleta">La paleta del tema activo.</param>
        public FormExportar(string nombreSugerido, string carpetaSugerida, Color fondoActual, Paleta paleta)
        {
            this.paleta = paleta ?? Paleta.Clara;
            this.fondo = fondoActual;

            ConstruirUI(nombreSugerido, carpetaSugerida);
        }

        public FormatoExportacion Formato
        {
            get { return (FormatoExportacion)cmbFormato.SelectedIndex; }
        }

        public string Carpeta
        {
            get { return txtCarpeta.Text; }
        }

        public string NombreBase
        {
            get { return txtNombre.Text; }
        }

        /// <summary>
        /// El fondo a aplicar: transparente si el usuario lo pidio.
        /// </summary>
        public Color Fondo
        {
            get { return chkTransparente.Checked ? Color.Transparent : fondo; }
        }

        /// <summary>
        /// Los tamanos marcados, de menor a mayor y sin repetir.
        /// </summary>
        public List<int> Tamanos
        {
            get
            {
                List<int> elegidos = new List<int>();

                foreach (CheckBox casilla in casillas)
                {
                    if (casilla.Checked)
                    {
                        elegidos.Add((int)casilla.Tag);
                    }
                }

                if (chkPersonalizado.Checked && !elegidos.Contains((int)nupPersonalizado.Value))
                {
                    elegidos.Add((int)nupPersonalizado.Value);
                }

                elegidos.Sort();
                return elegidos;
            }
        }

        private void ConstruirUI(string nombreSugerido, string carpetaSugerida)
        {
            this.Text = "Exportar";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(480, 424);
            this.BackColor = paleta.Lienzo;
            this.ForeColor = paleta.Texto;
            this.Font = new Font("Segoe UI", 9F);

            this.Controls.Add(Etiqueta("Formato", new Point(16, 18)));

            cmbFormato = new ComboBox
            {
                Location = new Point(96, 14),
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbFormato.Items.AddRange(new object[] { "SVG (vector)", "PNG", "JPG", "BMP" });
            cmbFormato.SelectedIndex = 1;
            cmbFormato.SelectedIndexChanged += (s, e) => ActualizarDisponibilidad();

            this.Controls.Add(cmbFormato);
            this.Controls.Add(Etiqueta("Tamanos", new Point(16, 58)));

            FlowLayoutPanel panelTamanos = new FlowLayoutPanel
            {
                Location = new Point(16, 82),
                Size = new Size(448, 96),
                BackColor = paleta.Tarjeta
            };

            foreach (int lado in ExportadorSvg.TamanosSugeridos)
            {
                CheckBox casilla = new CheckBox
                {
                    Text = lado + " px",
                    Tag = lado,
                    AutoSize = true,
                    Margin = new Padding(10, 8, 10, 8),
                    ForeColor = paleta.Texto,
                    Checked = lado == 256,
                    Cursor = Cursors.Hand
                };

                casillas.Add(casilla);
                panelTamanos.Controls.Add(casilla);
            }

            this.Controls.Add(panelTamanos);

            chkPersonalizado = new CheckBox
            {
                Text = "Otro tamano",
                AutoSize = true,
                Location = new Point(16, 190),
                ForeColor = paleta.Texto,
                Cursor = Cursors.Hand
            };

            nupPersonalizado = new NumericUpDown
            {
                Location = new Point(130, 186),
                Width = 88,
                Minimum = 1,
                Maximum = 8192,
                Value = 640
            };

            this.Controls.Add(chkPersonalizado);
            this.Controls.Add(nupPersonalizado);

            chkTransparente = new CheckBox
            {
                Text = "Fondo transparente",
                AutoSize = true,
                Location = new Point(16, 226),
                Checked = true,
                ForeColor = paleta.Texto,
                Cursor = Cursors.Hand
            };

            chkTransparente.CheckedChanged += (s, e) => ActualizarDisponibilidad();
            this.Controls.Add(chkTransparente);

            Button btnFondo = Boton("Color de fondo...", new Point(196, 222), 140);
            btnFondo.Click += ElegirFondo;
            this.Controls.Add(btnFondo);

            this.Controls.Add(Etiqueta("Carpeta", new Point(16, 268)));

            txtCarpeta = new TextBox
            {
                Location = new Point(96, 264),
                Width = 268,
                Text = carpetaSugerida ?? string.Empty
            };

            Button btnCarpeta = Boton("Elegir...", new Point(374, 262), 90);
            btnCarpeta.Click += ElegirCarpeta;

            this.Controls.Add(txtCarpeta);
            this.Controls.Add(btnCarpeta);
            this.Controls.Add(Etiqueta("Nombre", new Point(16, 308)));

            txtNombre = new TextBox
            {
                Location = new Point(96, 304),
                Width = 268,
                Text = nombreSugerido ?? "imagen"
            };

            this.Controls.Add(txtNombre);

            Label ayuda = new Label
            {
                Text = "Cada archivo se guarda como nombre_256x256.png",
                AutoSize = true,
                ForeColor = paleta.TextoSuave,
                Location = new Point(96, 332)
            };

            this.Controls.Add(ayuda);

            Button btnExportar = Boton("Exportar", new Point(266, 376), 96);
            btnExportar.Click += Validar;

            Button btnCancelar = Boton("Cancelar", new Point(370, 376), 94);
            btnCancelar.DialogResult = DialogResult.Cancel;

            this.Controls.Add(btnExportar);
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnExportar;
            this.CancelButton = btnCancelar;

            ActualizarDisponibilidad();
        }

        private Label Etiqueta(string texto, Point posicion)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                ForeColor = paleta.Texto,
                Location = posicion
            };
        }

        private Button Boton(string texto, Point posicion, int ancho)
        {
            Button boton = new Button
            {
                Text = texto,
                Location = posicion,
                Size = new Size(ancho, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = paleta.Barra,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            boton.FlatAppearance.BorderSize = 0;
            return boton;
        }

        /// <summary>
        /// JPG y BMP no guardan transparencia, y el SVG no necesita color de fondo:
        /// las opciones que no aplican se desactivan en vez de fallar despues.
        /// </summary>
        private void ActualizarDisponibilidad()
        {
            bool esVector = Formato == FormatoExportacion.Svg;
            bool admiteAlfa = Formato == FormatoExportacion.Png || esVector;

            chkTransparente.Enabled = admiteAlfa;

            if (!admiteAlfa)
            {
                chkTransparente.Checked = false;
            }
        }

        private void ElegirFondo(object sender, EventArgs e)
        {
            using (ColorDialog selector = new ColorDialog { Color = fondo, FullOpen = true })
            {
                if (selector.ShowDialog(this) == DialogResult.OK)
                {
                    fondo = selector.Color;
                    chkTransparente.Checked = false;
                }
            }
        }

        private void ElegirCarpeta(object sender, EventArgs e)
        {
            using (FolderBrowserDialog selector = new FolderBrowserDialog())
            {
                if (Directory.Exists(txtCarpeta.Text))
                {
                    selector.SelectedPath = txtCarpeta.Text;
                }

                if (selector.ShowDialog(this) == DialogResult.OK)
                {
                    txtCarpeta.Text = selector.SelectedPath;
                }
            }
        }

        /// <summary>
        /// Comprueba lo minimo antes de cerrar, para no dejar que el exportador falle
        /// con una carpeta vacia o sin ningun tamano marcado.
        /// </summary>
        private void Validar(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCarpeta.Text))
            {
                Avisar("Elige la carpeta donde guardar.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                Avisar("Escribe un nombre para los archivos.");
                return;
            }

            if (Tamanos.Count == 0)
            {
                Avisar("Marca al menos un tamano.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Avisar(string mensaje)
        {
            MessageBox.Show(this, mensaje, "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
