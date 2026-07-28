using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Cambiar_Color_Imagen_SVG
{
    /// <summary>
    /// Cuadro "Acerca de" con los datos del autor. Se construye por completo en
    /// codigo (sin .Designer.cs) porque es una pantalla pequena y estatica.
    /// </summary>
    public class FormAcerca : Form
    {
        private static readonly Color ColorCabecera = Color.FromArgb(99, 86, 176);
        private static readonly Color ColorAcento = Color.FromArgb(137, 123, 225);
        private static readonly Color ColorCuerpo = Color.FromArgb(245, 244, 255);

        public FormAcerca()
        {
            ConstruirUI();
        }

        private void ConstruirUI()
        {
            this.Text = "Acerca de";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(420, 460);
            this.BackColor = ColorCuerpo;
            this.Font = new Font("Segoe UI", 9F);

            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                // El icono es solo cosmetico: si no esta disponible, se sigue sin el.
            }

            Panel cabecera = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = ColorCabecera
            };

            PictureBox logo = new PictureBox
            {
                Size = new Size(56, 56),
                Location = new Point(24, 27),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            if (this.Icon != null)
            {
                logo.Image = this.Icon.ToBitmap();
            }

            Label titulo = new Label
            {
                Text = "Editor de Colores SVG",
                Font = new Font("Century Gothic", 15F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(92, 28),
                BackColor = Color.Transparent
            };

            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";
            Label subtitulo = new Label
            {
                Text = "Version " + version,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(225, 222, 245),
                AutoSize = true,
                Location = new Point(93, 62),
                BackColor = Color.Transparent
            };

            cabecera.Controls.Add(logo);
            cabecera.Controls.Add(titulo);
            cabecera.Controls.Add(subtitulo);

            Label lblDesarrolladoPor = new Label
            {
                Text = "Desarrollado por",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(110, 100, 140),
                AutoSize = true,
                Location = new Point(24, 128)
            };

            Label lblAutor = new Label
            {
                Text = "Juan Diego Mejía Maestre",
                Font = new Font("Century Gothic", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 38, 80),
                AutoSize = true,
                Location = new Point(24, 148)
            };

            Label lblBio = new Label
            {
                Text = "Desarrollador de software especializado en aplicaciones web, "
                    + "moviles y de escritorio. Represento a Colombia en WorldSkills "
                    + "2022, donde obtuvo medalla de excelencia en desarrollo de "
                    + "software.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(70, 62, 100),
                AutoSize = false,
                Location = new Point(24, 182),
                Size = new Size(372, 66)
            };

            Label lblCanal = new Label
            {
                Text = "Este software se hizo para el canal de Código Limpio.",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(110, 100, 140),
                AutoSize = false,
                Location = new Point(24, 250),
                Size = new Size(372, 20)
            };

            Panel divisor = new Panel
            {
                BackColor = ColorAcento,
                Location = new Point(24, 280),
                Size = new Size(372, 1)
            };

            Label lblEnlaces = new Label
            {
                Text = "Enlaces",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(110, 100, 140),
                AutoSize = true,
                Location = new Point(24, 296)
            };

            int y = 322;
            AgregarEnlace("🌐  Portafolio — juandiegows.com", "https://juandiegows.com/", ref y);
            AgregarEnlace("💻  GitHub — github.com/juandiegows", "https://github.com/juandiegows", ref y);
            AgregarEnlace("🔗  LinkedIn — linkedin.com/in/juandiegows", "https://www.linkedin.com/in/juandiegows/", ref y);
            AgregarEnlace("𝕏  X (Twitter) — @juandiegows", "https://x.com/juandiegows", ref y);

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Size = new Size(96, 34),
                Location = new Point(this.ClientSize.Width - 96 - 24, this.ClientSize.Height - 34 - 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = ColorCabecera,
                ForeColor = Color.White,
                Font = new Font("Century Gothic", 10F),
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Cursor = Cursors.Hand;

            this.Controls.Add(cabecera);
            this.Controls.Add(lblDesarrolladoPor);
            this.Controls.Add(lblAutor);
            this.Controls.Add(lblBio);
            this.Controls.Add(lblCanal);
            this.Controls.Add(divisor);
            this.Controls.Add(lblEnlaces);
            this.Controls.Add(btnCerrar);

            this.AcceptButton = btnCerrar;
            this.CancelButton = btnCerrar;
        }

        /// <summary>
        /// Anade un LinkLabel que abre <paramref name="url"/> en el navegador
        /// predeterminado y avanza <paramref name="y"/> para el siguiente enlace.
        /// </summary>
        private void AgregarEnlace(string texto, string url, ref int y)
        {
            LinkLabel enlace = new LinkLabel
            {
                Text = texto,
                Font = new Font("Segoe UI", 9.5F),
                LinkColor = ColorCabecera,
                ActiveLinkColor = ColorAcento,
                VisitedLinkColor = ColorCabecera,
                LinkBehavior = LinkBehavior.HoverUnderline,
                AutoSize = true,
                Location = new Point(24, y)
            };
            enlace.Click += (sender, e) => AbrirEnlace(url);
            this.Controls.Add(enlace);
            y += 26;
        }

        private static void AbrirEnlace(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
                // Si el sistema no puede abrir el navegador no hay nada mas que hacer aqui.
            }
        }
    }
}
