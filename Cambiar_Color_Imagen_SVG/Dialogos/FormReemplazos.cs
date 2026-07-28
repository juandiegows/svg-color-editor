using Cambiar_Color_Imagen_SVG.SVG;
using Cambiar_Color_Imagen_SVG.Tema;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG.Dialogos
{
    /// <summary>
    /// Permite armar varias sustituciones de color y aplicarlas todas juntas.
    /// Antes solo se podia cambiar un color por vez, lo que obligaba a repetir el ciclo
    /// completo (elegir origen, elegir destino, aplicar) por cada tono de la imagen.
    /// </summary>
    public class FormReemplazos : Form
    {
        private readonly Paleta paleta;
        private readonly List<Color> coloresImagen;
        private readonly List<ParColor> pares = new List<ParColor>();
        private readonly List<Color> recientes;

        private FlowLayoutPanel listaPares;
        private FlowLayoutPanel muestrasImagen;
        private NumericUpDown nupTolerancia;

        /// <param name="coloresImagen">Los colores que usa el SVG abierto.</param>
        /// <param name="recientes">Los ultimos colores que eligio el usuario.</param>
        /// <param name="paleta">La paleta del tema activo.</param>
        public FormReemplazos(List<Color> coloresImagen, List<Color> recientes, Paleta paleta)
        {
            this.coloresImagen = coloresImagen ?? new List<Color>();
            this.recientes = recientes ?? new List<Color>();
            this.paleta = paleta ?? Paleta.Clara;

            ConstruirUI();
            LlenarMuestras();
        }

        /// <summary>
        /// Las sustituciones que definio el usuario.
        /// </summary>
        public List<ParColor> Pares
        {
            get { return pares; }
        }

        /// <summary>
        /// Cuanto se permite que un color se aleje del origen y aun asi se reemplace.
        /// </summary>
        public int Tolerancia
        {
            get { return (int)nupTolerancia.Value; }
        }

        private void ConstruirUI()
        {
            this.Text = "Reemplazar varios colores";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(560, 520);
            this.BackColor = paleta.Lienzo;
            this.ForeColor = paleta.Texto;
            this.Font = new Font("Segoe UI", 9F);

            Label lblImagen = new Label
            {
                Text = "Colores de la imagen — haz clic para agregarlos a la lista",
                AutoSize = true,
                ForeColor = paleta.Texto,
                Location = new Point(16, 14)
            };

            muestrasImagen = new FlowLayoutPanel
            {
                Location = new Point(16, 38),
                Size = new Size(528, 110),
                AutoScroll = true,
                BackColor = paleta.Tarjeta
            };

            Label lblPares = new Label
            {
                Text = "Sustituciones",
                AutoSize = true,
                ForeColor = paleta.Texto,
                Location = new Point(16, 160)
            };

            listaPares = new FlowLayoutPanel
            {
                Location = new Point(16, 184),
                Size = new Size(528, 226),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = paleta.Tarjeta
            };

            Label lblTolerancia = new Label
            {
                Text = "Tolerancia",
                AutoSize = true,
                ForeColor = paleta.Texto,
                Location = new Point(16, 428)
            };

            nupTolerancia = new NumericUpDown
            {
                Location = new Point(94, 424),
                Width = 64,
                Minimum = 0,
                Maximum = 128,
                Value = 0
            };

            Label lblAyuda = new Label
            {
                Text = "0 = solo el color exacto. Subela si el SVG repite tonos casi iguales.",
                AutoSize = true,
                ForeColor = paleta.TextoSuave,
                Location = new Point(168, 428)
            };

            Button btnAgregar = CrearBoton("Agregar sustitucion", new Point(16, 462), 160);
            btnAgregar.Click += (s, e) => AgregarPar(Color.Black, Color.White);

            Button btnAceptar = CrearBoton("Aplicar", new Point(346, 462), 96);
            btnAceptar.DialogResult = DialogResult.OK;

            Button btnCancelar = CrearBoton("Cancelar", new Point(450, 462), 96);
            btnCancelar.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblImagen);
            this.Controls.Add(muestrasImagen);
            this.Controls.Add(lblPares);
            this.Controls.Add(listaPares);
            this.Controls.Add(lblTolerancia);
            this.Controls.Add(nupTolerancia);
            this.Controls.Add(lblAyuda);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnAceptar);
            this.Controls.Add(btnCancelar);

            this.AcceptButton = btnAceptar;
            this.CancelButton = btnCancelar;
        }

        private Button CrearBoton(string texto, Point posicion, int ancho)
        {
            Button boton = new Button
            {
                Text = texto,
                Location = posicion,
                Size = new Size(ancho, 32),
                FlatStyle = FlatStyle.Flat,
                BackColor = paleta.Barra,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            boton.FlatAppearance.BorderSize = 0;
            return boton;
        }

        /// <summary>
        /// Muestra la paleta real del SVG para que el usuario no tenga que acertarle al
        /// color exacto con el cuentagotas.
        /// </summary>
        private void LlenarMuestras()
        {
            if (coloresImagen.Count == 0)
            {
                muestrasImagen.Controls.Add(new Label
                {
                    Text = "No se detectaron colores solidos en esta imagen.",
                    AutoSize = true,
                    ForeColor = paleta.Texto,
                    Margin = new Padding(6)
                });

                return;
            }

            foreach (Color color in coloresImagen)
            {
                Color propio = color;

                Panel muestra = new Panel
                {
                    Size = new Size(38, 38),
                    Margin = new Padding(5),
                    BackColor = propio,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand
                };

                new ToolTip().SetToolTip(muestra, AHex(propio));
                muestra.Click += (s, e) => AgregarPar(propio, propio);

                muestrasImagen.Controls.Add(muestra);
            }
        }

        /// <summary>
        /// Anade una fila de sustitucion a la lista.
        /// </summary>
        private void AgregarPar(Color origen, Color destino)
        {
            ParColor par = new ParColor(origen, destino);
            pares.Add(par);

            Panel fila = new Panel
            {
                Size = new Size(496, 44),
                Margin = new Padding(4),
                BackColor = paleta.Lienzo
            };

            Panel muestraOrigen = CrearMuestraEditable(origen, nuevo => par.Origen = nuevo);
            muestraOrigen.Location = new Point(8, 6);

            Label flecha = new Label
            {
                Text = "→",
                AutoSize = true,
                ForeColor = paleta.Texto,
                Location = new Point(126, 12)
            };

            Panel muestraDestino = CrearMuestraEditable(destino, nuevo => par.Destino = nuevo);
            muestraDestino.Location = new Point(158, 6);

            Button btnQuitar = new Button
            {
                Text = "Quitar",
                Size = new Size(72, 30),
                Location = new Point(400, 6),
                FlatStyle = FlatStyle.Flat,
                BackColor = paleta.Barra,
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };

            btnQuitar.FlatAppearance.BorderSize = 0;
            btnQuitar.Click += (s, e) =>
            {
                pares.Remove(par);
                listaPares.Controls.Remove(fila);
                fila.Dispose();
            };

            fila.Controls.Add(muestraOrigen);
            fila.Controls.Add(flecha);
            fila.Controls.Add(muestraDestino);
            fila.Controls.Add(btnQuitar);

            listaPares.Controls.Add(fila);
        }

        /// <summary>
        /// Crea un recuadro de color que abre el selector al hacerle clic.
        /// </summary>
        /// <param name="inicial">El color con el que arranca.</param>
        /// <param name="alCambiar">Que hacer cuando el usuario elige otro.</param>
        private Panel CrearMuestraEditable(Color inicial, Action<Color> alCambiar)
        {
            Panel muestra = new Panel
            {
                Size = new Size(110, 32),
                BackColor = inicial,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            Label texto = new Label
            {
                Dock = DockStyle.Fill,
                Text = AHex(inicial),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ColorLegible(inicial),
                Cursor = Cursors.Hand
            };

            muestra.Controls.Add(texto);

            void Elegir(object remitente, EventArgs argumentos)
            {
                using (ColorDialog selector = new ColorDialog { Color = muestra.BackColor, FullOpen = true })
                {
                    selector.CustomColors = ColoresPersonalizados();

                    if (selector.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    muestra.BackColor = selector.Color;
                    texto.Text = AHex(selector.Color);
                    texto.ForeColor = ColorLegible(selector.Color);
                    alCambiar(selector.Color);
                }
            }

            muestra.Click += Elegir;
            texto.Click += Elegir;

            return muestra;
        }

        /// <summary>
        /// Precarga el selector con los colores de la imagen y los usados hace poco.
        /// </summary>
        private int[] ColoresPersonalizados()
        {
            List<int> valores = new List<int>();

            foreach (Color color in recientes)
            {
                valores.Add(AOleColor(color));
            }

            foreach (Color color in coloresImagen)
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

        private static string AHex(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        /// <summary>
        /// Elige blanco o negro segun cual se lea mejor sobre el color de fondo.
        /// </summary>
        private static Color ColorLegible(Color fondo)
        {
            double luz = (0.299 * fondo.R) + (0.587 * fondo.G) + (0.114 * fondo.B);
            return luz > 140 ? Color.Black : Color.White;
        }
    }
}
