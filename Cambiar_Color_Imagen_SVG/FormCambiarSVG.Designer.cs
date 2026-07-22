namespace Cambiar_Color_Imagen_SVG
{
    partial class FormCambiarSVG
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCambiarSVG));
            this.ResizeForm = new Guna.UI2.WinForms.Guna2ResizeForm(this.components);
            this.panelSuperior = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.btnTamaño = new Guna.UI2.WinForms.Guna2Button();
            this.panelTamaño = new Guna.UI2.WinForms.Guna2Panel();
            this.nupAlto = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.lblAlto = new System.Windows.Forms.Label();
            this.nupAncho = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.lblAncho = new System.Windows.Forms.Label();
            this.btnColor = new Guna.UI2.WinForms.Guna2Button();
            this.panelColor = new Guna.UI2.WinForms.Guna2Panel();
            this.btnElegirDestino = new Guna.UI2.WinForms.Guna2Button();
            this.btnCambiar = new Guna.UI2.WinForms.Guna2Button();
            this.lblColorxRemplazar = new System.Windows.Forms.Label();
            this.btnColorDestino = new System.Windows.Forms.Button();
            this.btnElegirOrigen = new Guna.UI2.WinForms.Guna2Button();
            this.btnColorOrigen = new System.Windows.Forms.Button();
            this.lblColorRemplazar = new System.Windows.Forms.Label();
            this.btnAcerca = new Guna.UI2.WinForms.Guna2Button();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.panelAjustar = new System.Windows.Forms.Panel();
            this.btnAjustar = new Guna.UI2.WinForms.Guna2Button();
            this.lblColorFondo = new System.Windows.Forms.Label();
            this.btnColorFondo = new System.Windows.Forms.Button();
            this.btnGuardar = new Guna.UI2.WinForms.Guna2Button();
            this.btnDescargar = new Guna.UI2.WinForms.Guna2Button();
            this.panelBuscador = new System.Windows.Forms.Panel();
            this.btnBuscar = new Guna.UI2.WinForms.Guna2Button();
            this.txtBuscar = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblImagenSVG = new System.Windows.Forms.Label();
            this.filePicker = new System.Windows.Forms.OpenFileDialog();
            this.colorPicker = new System.Windows.Forms.ColorDialog();
            this.Guardar = new System.Windows.Forms.SaveFileDialog();
            this.pickImagen = new System.Windows.Forms.PictureBox();
            this.btnReducir = new System.Windows.Forms.PictureBox();
            this.btnAmpliar = new System.Windows.Forms.PictureBox();
            this.btnLogo = new Guna.UI2.WinForms.Guna2Button();
            this.btnMinimizar = new Guna.UI2.WinForms.Guna2Button();
            this.btnMaximizarNormal = new Guna.UI2.WinForms.Guna2Button();
            this.btnCerrar = new Guna.UI2.WinForms.Guna2Button();
            this.panelSuperior.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.panelTamaño.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupAlto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupAncho)).BeginInit();
            this.panelColor.SuspendLayout();
            this.panelCentral.SuspendLayout();
            this.panelAjustar.SuspendLayout();
            this.panelBuscador.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pickImagen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReducir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAmpliar)).BeginInit();
            this.SuspendLayout();
            // 
            // ResizeForm
            // 
            this.ResizeForm.TargetForm = this;
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(86)))), ((int)(((byte)(176)))));
            this.panelSuperior.Controls.Add(this.lblLogo);
            this.panelSuperior.Controls.Add(this.btnLogo);
            this.panelSuperior.Controls.Add(this.btnMinimizar);
            this.panelSuperior.Controls.Add(this.btnMaximizarNormal);
            this.panelSuperior.Controls.Add(this.btnCerrar);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelSuperior.Location = new System.Drawing.Point(1, 1);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(798, 34);
            this.panelSuperior.TabIndex = 0;
            this.panelSuperior.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelSuperior_MouseDown);
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(41, 7);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(117, 21);
            this.lblLogo.TabIndex = 5;
            this.lblLogo.Text = "Código limpio";
            this.lblLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogo_MouseDown);
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(123)))), ((int)(((byte)(225)))));
            this.panelIzquierdo.Controls.Add(this.btnTamaño);
            this.panelIzquierdo.Controls.Add(this.panelTamaño);
            this.panelIzquierdo.Controls.Add(this.btnColor);
            this.panelIzquierdo.Controls.Add(this.panelColor);
            this.panelIzquierdo.Controls.Add(this.btnAcerca);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(1, 35);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(220, 504);
            this.panelIzquierdo.TabIndex = 1;
            // 
            // btnTamaño
            // 
            this.btnTamaño.AutoRoundedCorners = true;
            this.btnTamaño.BorderColor = System.Drawing.Color.White;
            this.btnTamaño.BorderRadius = 11;
            this.btnTamaño.BorderThickness = 2;
            this.btnTamaño.CheckedState.Parent = this.btnTamaño;
            this.btnTamaño.CustomImages.Parent = this.btnTamaño;
            this.btnTamaño.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTamaño.ForeColor = System.Drawing.Color.White;
            this.btnTamaño.HoverState.Parent = this.btnTamaño;
            this.btnTamaño.Location = new System.Drawing.Point(27, 271);
            this.btnTamaño.Name = "btnTamaño";
            this.btnTamaño.ShadowDecoration.Parent = this.btnTamaño;
            this.btnTamaño.Size = new System.Drawing.Size(123, 25);
            this.btnTamaño.TabIndex = 3;
            this.btnTamaño.Text = "Tamaño";
            // 
            // panelTamaño
            // 
            this.panelTamaño.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTamaño.BorderColor = System.Drawing.Color.White;
            this.panelTamaño.BorderRadius = 20;
            this.panelTamaño.BorderThickness = 2;
            this.panelTamaño.Controls.Add(this.btnReducir);
            this.panelTamaño.Controls.Add(this.btnAmpliar);
            this.panelTamaño.Controls.Add(this.nupAlto);
            this.panelTamaño.Controls.Add(this.lblAlto);
            this.panelTamaño.Controls.Add(this.nupAncho);
            this.panelTamaño.Controls.Add(this.lblAncho);
            this.panelTamaño.Location = new System.Drawing.Point(4, 281);
            this.panelTamaño.Name = "panelTamaño";
            this.panelTamaño.ShadowDecoration.Parent = this.panelTamaño;
            this.panelTamaño.Size = new System.Drawing.Size(210, 172);
            this.panelTamaño.TabIndex = 4;
            // 
            // nupAlto
            // 
            this.nupAlto.BackColor = System.Drawing.Color.Transparent;
            this.nupAlto.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nupAlto.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.nupAlto.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.nupAlto.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.nupAlto.DisabledState.Parent = this.nupAlto;
            this.nupAlto.DisabledState.UpDownButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(177)))), ((int)(((byte)(177)))));
            this.nupAlto.DisabledState.UpDownButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.nupAlto.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.nupAlto.FocusedState.Parent = this.nupAlto;
            this.nupAlto.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nupAlto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.nupAlto.Location = new System.Drawing.Point(96, 69);
            this.nupAlto.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nupAlto.Name = "nupAlto";
            this.nupAlto.ShadowDecoration.Parent = this.nupAlto;
            this.nupAlto.Size = new System.Drawing.Size(100, 36);
            this.nupAlto.TabIndex = 16;
            this.nupAlto.ValueChanged += new System.EventHandler(this.nupAlto_ValueChanged);
            // 
            // lblAlto
            // 
            this.lblAlto.AutoSize = true;
            this.lblAlto.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlto.ForeColor = System.Drawing.Color.White;
            this.lblAlto.Location = new System.Drawing.Point(21, 80);
            this.lblAlto.Name = "lblAlto";
            this.lblAlto.Size = new System.Drawing.Size(43, 21);
            this.lblAlto.TabIndex = 15;
            this.lblAlto.Text = "Alto";
            // 
            // nupAncho
            // 
            this.nupAncho.BackColor = System.Drawing.Color.Transparent;
            this.nupAncho.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.nupAncho.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.nupAncho.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.nupAncho.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.nupAncho.DisabledState.Parent = this.nupAncho;
            this.nupAncho.DisabledState.UpDownButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(177)))), ((int)(((byte)(177)))));
            this.nupAncho.DisabledState.UpDownButtonForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.nupAncho.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.nupAncho.FocusedState.Parent = this.nupAncho;
            this.nupAncho.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.nupAncho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.nupAncho.Location = new System.Drawing.Point(96, 21);
            this.nupAncho.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nupAncho.Name = "nupAncho";
            this.nupAncho.ShadowDecoration.Parent = this.nupAncho;
            this.nupAncho.Size = new System.Drawing.Size(100, 36);
            this.nupAncho.TabIndex = 14;
            this.nupAncho.ValueChanged += new System.EventHandler(this.nupAncho_ValueChanged);
            // 
            // lblAncho
            // 
            this.lblAncho.AutoSize = true;
            this.lblAncho.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAncho.ForeColor = System.Drawing.Color.White;
            this.lblAncho.Location = new System.Drawing.Point(21, 28);
            this.lblAncho.Name = "lblAncho";
            this.lblAncho.Size = new System.Drawing.Size(63, 21);
            this.lblAncho.TabIndex = 13;
            this.lblAncho.Text = "Ancho";
            // 
            // btnColor
            // 
            this.btnColor.AutoRoundedCorners = true;
            this.btnColor.BorderColor = System.Drawing.Color.White;
            this.btnColor.BorderRadius = 11;
            this.btnColor.BorderThickness = 2;
            this.btnColor.CheckedState.Parent = this.btnColor;
            this.btnColor.CustomImages.Parent = this.btnColor;
            this.btnColor.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColor.ForeColor = System.Drawing.Color.White;
            this.btnColor.HoverState.Parent = this.btnColor;
            this.btnColor.Location = new System.Drawing.Point(27, 25);
            this.btnColor.Name = "btnColor";
            this.btnColor.ShadowDecoration.Parent = this.btnColor;
            this.btnColor.Size = new System.Drawing.Size(123, 25);
            this.btnColor.TabIndex = 1;
            this.btnColor.Text = "Color";
            // 
            // panelColor
            // 
            this.panelColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelColor.BorderColor = System.Drawing.Color.White;
            this.panelColor.BorderRadius = 20;
            this.panelColor.BorderThickness = 2;
            this.panelColor.Controls.Add(this.btnElegirDestino);
            this.panelColor.Controls.Add(this.btnCambiar);
            this.panelColor.Controls.Add(this.lblColorxRemplazar);
            this.panelColor.Controls.Add(this.btnColorDestino);
            this.panelColor.Controls.Add(this.btnElegirOrigen);
            this.panelColor.Controls.Add(this.btnColorOrigen);
            this.panelColor.Controls.Add(this.lblColorRemplazar);
            this.panelColor.Font = new System.Drawing.Font("Raleway", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelColor.Location = new System.Drawing.Point(4, 38);
            this.panelColor.Name = "panelColor";
            this.panelColor.ShadowDecoration.Parent = this.panelColor;
            this.panelColor.Size = new System.Drawing.Size(210, 227);
            this.panelColor.TabIndex = 2;
            // 
            // btnElegirDestino
            // 
            this.btnElegirDestino.AutoRoundedCorners = true;
            this.btnElegirDestino.BorderColor = System.Drawing.Color.White;
            this.btnElegirDestino.BorderRadius = 13;
            this.btnElegirDestino.BorderThickness = 2;
            this.btnElegirDestino.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.btnElegirDestino.CheckedState.Parent = this.btnElegirDestino;
            this.btnElegirDestino.CustomImages.Parent = this.btnElegirDestino;
            this.btnElegirDestino.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnElegirDestino.ForeColor = System.Drawing.Color.White;
            this.btnElegirDestino.HoverState.Parent = this.btnElegirDestino;
            this.btnElegirDestino.Location = new System.Drawing.Point(98, 125);
            this.btnElegirDestino.Name = "btnElegirDestino";
            this.btnElegirDestino.ShadowDecoration.Parent = this.btnElegirDestino;
            this.btnElegirDestino.Size = new System.Drawing.Size(58, 28);
            this.btnElegirDestino.TabIndex = 13;
            this.btnElegirDestino.Text = "Elegir";
            this.btnElegirDestino.CheckedChanged += new System.EventHandler(this.BtnElegirDestino_CheckedChanged);
            // 
            // btnCambiar
            // 
            this.btnCambiar.AutoRoundedCorners = true;
            this.btnCambiar.BorderColor = System.Drawing.Color.White;
            this.btnCambiar.BorderRadius = 19;
            this.btnCambiar.BorderThickness = 2;
            this.btnCambiar.CheckedState.Parent = this.btnCambiar;
            this.btnCambiar.CustomImages.Parent = this.btnCambiar;
            this.btnCambiar.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.btnCambiar.ForeColor = System.Drawing.Color.White;
            this.btnCambiar.HoverState.Parent = this.btnCambiar;
            this.btnCambiar.Location = new System.Drawing.Point(29, 174);
            this.btnCambiar.Name = "btnCambiar";
            this.btnCambiar.ShadowDecoration.Parent = this.btnCambiar;
            this.btnCambiar.Size = new System.Drawing.Size(148, 41);
            this.btnCambiar.TabIndex = 12;
            this.btnCambiar.Text = "Cambiar";
            this.btnCambiar.Click += new System.EventHandler(this.btnCambiar_Click);
            // 
            // lblColorxRemplazar
            // 
            this.lblColorxRemplazar.AutoSize = true;
            this.lblColorxRemplazar.ForeColor = System.Drawing.Color.White;
            this.lblColorxRemplazar.Location = new System.Drawing.Point(19, 95);
            this.lblColorxRemplazar.Name = "lblColorxRemplazar";
            this.lblColorxRemplazar.Size = new System.Drawing.Size(170, 22);
            this.lblColorxRemplazar.TabIndex = 11;
            this.lblColorxRemplazar.Text = "Color por reemplazar";
            // 
            // btnColorDestino
            // 
            this.btnColorDestino.BackColor = System.Drawing.Color.Blue;
            this.btnColorDestino.Location = new System.Drawing.Point(23, 120);
            this.btnColorDestino.Name = "btnColorDestino";
            this.btnColorDestino.Size = new System.Drawing.Size(69, 41);
            this.btnColorDestino.TabIndex = 9;
            this.btnColorDestino.UseVisualStyleBackColor = false;
            this.btnColorDestino.Click += new System.EventHandler(this.btnColorDestino_Click);
            // 
            // btnElegirOrigen
            // 
            this.btnElegirOrigen.AutoRoundedCorners = true;
            this.btnElegirOrigen.BorderColor = System.Drawing.Color.White;
            this.btnElegirOrigen.BorderRadius = 13;
            this.btnElegirOrigen.BorderThickness = 2;
            this.btnElegirOrigen.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.ToogleButton;
            this.btnElegirOrigen.CheckedState.Parent = this.btnElegirOrigen;
            this.btnElegirOrigen.CustomImages.Parent = this.btnElegirOrigen;
            this.btnElegirOrigen.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnElegirOrigen.ForeColor = System.Drawing.Color.White;
            this.btnElegirOrigen.HoverState.Parent = this.btnElegirOrigen;
            this.btnElegirOrigen.Location = new System.Drawing.Point(96, 58);
            this.btnElegirOrigen.Name = "btnElegirOrigen";
            this.btnElegirOrigen.ShadowDecoration.Parent = this.btnElegirOrigen;
            this.btnElegirOrigen.Size = new System.Drawing.Size(58, 28);
            this.btnElegirOrigen.TabIndex = 8;
            this.btnElegirOrigen.Text = "Elegir";
            this.btnElegirOrigen.CheckedChanged += new System.EventHandler(this.btnElegirOrigen_CheckedChanged);
            // 
            // btnColorOrigen
            // 
            this.btnColorOrigen.BackColor = System.Drawing.Color.Red;
            this.btnColorOrigen.Location = new System.Drawing.Point(21, 51);
            this.btnColorOrigen.Name = "btnColorOrigen";
            this.btnColorOrigen.Size = new System.Drawing.Size(69, 41);
            this.btnColorOrigen.TabIndex = 7;
            this.btnColorOrigen.UseVisualStyleBackColor = false;
            this.btnColorOrigen.Click += new System.EventHandler(this.btnColorOrigen_Click);
            // 
            // lblColorRemplazar
            // 
            this.lblColorRemplazar.AutoSize = true;
            this.lblColorRemplazar.ForeColor = System.Drawing.Color.White;
            this.lblColorRemplazar.Location = new System.Drawing.Point(19, 20);
            this.lblColorRemplazar.Name = "lblColorRemplazar";
            this.lblColorRemplazar.Size = new System.Drawing.Size(153, 22);
            this.lblColorRemplazar.TabIndex = 6;
            this.lblColorRemplazar.Text = "Color a reemplazar";
            // 
            // btnAcerca
            // 
            this.btnAcerca.CheckedState.Parent = this.btnAcerca;
            this.btnAcerca.CustomImages.Parent = this.btnAcerca;
            this.btnAcerca.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAcerca.FillColor = System.Drawing.Color.Gray;
            this.btnAcerca.Font = new System.Drawing.Font("Raleway", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcerca.ForeColor = System.Drawing.Color.White;
            this.btnAcerca.HoverState.Parent = this.btnAcerca;
            this.btnAcerca.Location = new System.Drawing.Point(0, 459);
            this.btnAcerca.Name = "btnAcerca";
            this.btnAcerca.ShadowDecoration.Parent = this.btnAcerca;
            this.btnAcerca.Size = new System.Drawing.Size(220, 45);
            this.btnAcerca.TabIndex = 0;
            this.btnAcerca.Text = "Acerca de";
            this.btnAcerca.Click += new System.EventHandler(this.BtnAcerca_Click);
            // 
            // panelCentral
            // 
            this.panelCentral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(168)))), ((int)(((byte)(251)))));
            this.panelCentral.Controls.Add(this.pickImagen);
            this.panelCentral.Controls.Add(this.panelAjustar);
            this.panelCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentral.Location = new System.Drawing.Point(221, 35);
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Size = new System.Drawing.Size(578, 504);
            this.panelCentral.TabIndex = 2;
            // 
            // panelAjustar
            // 
            this.panelAjustar.Controls.Add(this.btnAjustar);
            this.panelAjustar.Controls.Add(this.lblColorFondo);
            this.panelAjustar.Controls.Add(this.btnColorFondo);
            this.panelAjustar.Controls.Add(this.btnGuardar);
            this.panelAjustar.Controls.Add(this.btnDescargar);
            this.panelAjustar.Controls.Add(this.panelBuscador);
            this.panelAjustar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAjustar.Location = new System.Drawing.Point(0, 0);
            this.panelAjustar.Name = "panelAjustar";
            this.panelAjustar.Size = new System.Drawing.Size(578, 101);
            this.panelAjustar.TabIndex = 0;
            // 
            // btnAjustar
            // 
            this.btnAjustar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAjustar.AutoRoundedCorners = true;
            this.btnAjustar.BorderColor = System.Drawing.Color.White;
            this.btnAjustar.BorderRadius = 16;
            this.btnAjustar.BorderThickness = 2;
            this.btnAjustar.CheckedState.Parent = this.btnAjustar;
            this.btnAjustar.CustomImages.Parent = this.btnAjustar;
            this.btnAjustar.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.btnAjustar.ForeColor = System.Drawing.Color.White;
            this.btnAjustar.HoverState.Parent = this.btnAjustar;
            this.btnAjustar.Location = new System.Drawing.Point(222, 57);
            this.btnAjustar.Name = "btnAjustar";
            this.btnAjustar.ShadowDecoration.Parent = this.btnAjustar;
            this.btnAjustar.Size = new System.Drawing.Size(115, 35);
            this.btnAjustar.TabIndex = 17;
            this.btnAjustar.Text = "Ajustar";
            this.btnAjustar.Click += new System.EventHandler(this.BtnAjustar_Click_3);
            // 
            // lblColorFondo
            // 
            this.lblColorFondo.AutoSize = true;
            this.lblColorFondo.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColorFondo.ForeColor = System.Drawing.Color.Black;
            this.lblColorFondo.Location = new System.Drawing.Point(82, 67);
            this.lblColorFondo.Name = "lblColorFondo";
            this.lblColorFondo.Size = new System.Drawing.Size(128, 19);
            this.lblColorFondo.TabIndex = 14;
            this.lblColorFondo.Text = "Color del fondo";
            // 
            // btnColorFondo
            // 
            this.btnColorFondo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(168)))), ((int)(((byte)(251)))));
            this.btnColorFondo.Location = new System.Drawing.Point(7, 55);
            this.btnColorFondo.Name = "btnColorFondo";
            this.btnColorFondo.Size = new System.Drawing.Size(69, 41);
            this.btnColorFondo.TabIndex = 14;
            this.btnColorFondo.UseVisualStyleBackColor = false;
            this.btnColorFondo.Click += new System.EventHandler(this.BtnColorFondo_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardar.AutoRoundedCorners = true;
            this.btnGuardar.BorderColor = System.Drawing.Color.White;
            this.btnGuardar.BorderRadius = 16;
            this.btnGuardar.BorderThickness = 2;
            this.btnGuardar.CheckedState.Parent = this.btnGuardar;
            this.btnGuardar.CustomImages.Parent = this.btnGuardar;
            this.btnGuardar.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.HoverState.Parent = this.btnGuardar;
            this.btnGuardar.Location = new System.Drawing.Point(472, 58);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.ShadowDecoration.Parent = this.btnGuardar;
            this.btnGuardar.Size = new System.Drawing.Size(102, 35);
            this.btnGuardar.TabIndex = 16;
            this.btnGuardar.Text = "Refrescar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnDescargar
            // 
            this.btnDescargar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDescargar.AutoRoundedCorners = true;
            this.btnDescargar.BorderColor = System.Drawing.Color.White;
            this.btnDescargar.BorderRadius = 16;
            this.btnDescargar.BorderThickness = 2;
            this.btnDescargar.CheckedState.Parent = this.btnDescargar;
            this.btnDescargar.CustomImages.Parent = this.btnDescargar;
            this.btnDescargar.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.btnDescargar.ForeColor = System.Drawing.Color.White;
            this.btnDescargar.HoverState.Parent = this.btnDescargar;
            this.btnDescargar.Location = new System.Drawing.Point(343, 58);
            this.btnDescargar.Name = "btnDescargar";
            this.btnDescargar.ShadowDecoration.Parent = this.btnDescargar;
            this.btnDescargar.Size = new System.Drawing.Size(115, 35);
            this.btnDescargar.TabIndex = 15;
            this.btnDescargar.Text = "Descargar";
            this.btnDescargar.Click += new System.EventHandler(this.btnDescargar_Click);
            // 
            // panelBuscador
            // 
            this.panelBuscador.Controls.Add(this.btnBuscar);
            this.panelBuscador.Controls.Add(this.txtBuscar);
            this.panelBuscador.Controls.Add(this.lblImagenSVG);
            this.panelBuscador.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBuscador.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelBuscador.Location = new System.Drawing.Point(0, 0);
            this.panelBuscador.Name = "panelBuscador";
            this.panelBuscador.Size = new System.Drawing.Size(578, 50);
            this.panelBuscador.TabIndex = 0;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.AutoRoundedCorners = true;
            this.btnBuscar.BorderColor = System.Drawing.Color.White;
            this.btnBuscar.BorderRadius = 16;
            this.btnBuscar.BorderThickness = 2;
            this.btnBuscar.CheckedState.Parent = this.btnBuscar;
            this.btnBuscar.CustomImages.Parent = this.btnBuscar;
            this.btnBuscar.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.HoverState.Parent = this.btnBuscar;
            this.btnBuscar.Location = new System.Drawing.Point(472, 6);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.ShadowDecoration.Parent = this.btnBuscar;
            this.btnBuscar.Size = new System.Drawing.Size(102, 35);
            this.btnBuscar.TabIndex = 14;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtBuscar
            // 
            this.txtBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuscar.AutoRoundedCorners = true;
            this.txtBuscar.BorderRadius = 17;
            this.txtBuscar.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBuscar.DefaultText = "";
            this.txtBuscar.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBuscar.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBuscar.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBuscar.DisabledState.Parent = this.txtBuscar;
            this.txtBuscar.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBuscar.Enabled = false;
            this.txtBuscar.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBuscar.FocusedState.Parent = this.txtBuscar;
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBuscar.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBuscar.HoverState.Parent = this.txtBuscar;
            this.txtBuscar.Location = new System.Drawing.Point(122, 6);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.PasswordChar = '\0';
            this.txtBuscar.PlaceholderText = "";
            this.txtBuscar.SelectedText = "";
            this.txtBuscar.ShadowDecoration.Parent = this.txtBuscar;
            this.txtBuscar.Size = new System.Drawing.Size(344, 36);
            this.txtBuscar.TabIndex = 8;
            // 
            // lblImagenSVG
            // 
            this.lblImagenSVG.AutoSize = true;
            this.lblImagenSVG.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImagenSVG.ForeColor = System.Drawing.Color.White;
            this.lblImagenSVG.Location = new System.Drawing.Point(6, 14);
            this.lblImagenSVG.Name = "lblImagenSVG";
            this.lblImagenSVG.Size = new System.Drawing.Size(107, 19);
            this.lblImagenSVG.TabIndex = 7;
            this.lblImagenSVG.Text = "Imagen SVG";
            // 
            // filePicker
            // 
            this.filePicker.FileName = "openFileDialog1";
            this.filePicker.Filter = "SVG(*.SVG)|*.SVG";
            // 
            // Guardar
            // 
            this.Guardar.Filter = "PNG(*.PNG)|*:PNG|JPEG(*.JPG)|*.JPG|BMP(*.BMP)|*.BMP";
            // 
            // pickImagen
            // 
            this.pickImagen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pickImagen.Location = new System.Drawing.Point(0, 101);
            this.pickImagen.Name = "pickImagen";
            this.pickImagen.Size = new System.Drawing.Size(578, 403);
            this.pickImagen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pickImagen.TabIndex = 1;
            this.pickImagen.TabStop = false;
            this.pickImagen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pickImagen_MouseDown);
            // 
            // btnReducir
            // 
            this.btnReducir.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.bajar;
            this.btnReducir.Location = new System.Drawing.Point(114, 121);
            this.btnReducir.Name = "btnReducir";
            this.btnReducir.Size = new System.Drawing.Size(40, 40);
            this.btnReducir.TabIndex = 18;
            this.btnReducir.TabStop = false;
            this.btnReducir.Click += new System.EventHandler(this.BtnReducir_Click);
            this.btnReducir.MouseEnter += new System.EventHandler(this.btnReducir_MouseEnter);
            this.btnReducir.MouseLeave += new System.EventHandler(this.btnReducir_MouseLeave);
            // 
            // btnAmpliar
            // 
            this.btnAmpliar.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.subir;
            this.btnAmpliar.Location = new System.Drawing.Point(53, 120);
            this.btnAmpliar.Name = "btnAmpliar";
            this.btnAmpliar.Size = new System.Drawing.Size(40, 40);
            this.btnAmpliar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnAmpliar.TabIndex = 17;
            this.btnAmpliar.TabStop = false;
            this.btnAmpliar.Click += new System.EventHandler(this.btnAmpliar_Click);
            this.btnAmpliar.MouseEnter += new System.EventHandler(this.BtnAmpliar_MouseEnter);
            this.btnAmpliar.MouseLeave += new System.EventHandler(this.BtnAmpliar_MouseLeave);
            // 
            // btnLogo
            // 
            this.btnLogo.CheckedState.Parent = this.btnLogo;
            this.btnLogo.CustomImages.Parent = this.btnLogo;
            this.btnLogo.FillColor = System.Drawing.Color.Transparent;
            this.btnLogo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLogo.ForeColor = System.Drawing.Color.White;
            this.btnLogo.HoverState.Parent = this.btnLogo;
            this.btnLogo.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.Cambiar_color_Imagen_SVG;
            this.btnLogo.Location = new System.Drawing.Point(4, 3);
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.ShadowDecoration.Parent = this.btnLogo;
            this.btnLogo.Size = new System.Drawing.Size(30, 30);
            this.btnLogo.TabIndex = 4;
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.CheckedState.Parent = this.btnMinimizar;
            this.btnMinimizar.CustomImages.Parent = this.btnMinimizar;
            this.btnMinimizar.FillColor = System.Drawing.Color.Transparent;
            this.btnMinimizar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.HoverState.Parent = this.btnMinimizar;
            this.btnMinimizar.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.minimizar_2;
            this.btnMinimizar.Location = new System.Drawing.Point(692, 0);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.ShadowDecoration.Parent = this.btnMinimizar;
            this.btnMinimizar.Size = new System.Drawing.Size(30, 30);
            this.btnMinimizar.TabIndex = 3;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            // 
            // btnMaximizarNormal
            // 
            this.btnMaximizarNormal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizarNormal.CheckedState.Parent = this.btnMaximizarNormal;
            this.btnMaximizarNormal.CustomImages.Parent = this.btnMaximizarNormal;
            this.btnMaximizarNormal.FillColor = System.Drawing.Color.Transparent;
            this.btnMaximizarNormal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnMaximizarNormal.ForeColor = System.Drawing.Color.White;
            this.btnMaximizarNormal.HoverState.Parent = this.btnMaximizarNormal;
            this.btnMaximizarNormal.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.Maximizar;
            this.btnMaximizarNormal.Location = new System.Drawing.Point(728, 0);
            this.btnMaximizarNormal.Name = "btnMaximizarNormal";
            this.btnMaximizarNormal.ShadowDecoration.Parent = this.btnMaximizarNormal;
            this.btnMaximizarNormal.Size = new System.Drawing.Size(30, 30);
            this.btnMaximizarNormal.TabIndex = 2;
            this.btnMaximizarNormal.Click += new System.EventHandler(this.btnMaximizarNormal_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.CheckedState.Parent = this.btnCerrar;
            this.btnCerrar.CustomImages.Parent = this.btnCerrar;
            this.btnCerrar.FillColor = System.Drawing.Color.Transparent;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.HoverState.Parent = this.btnCerrar;
            this.btnCerrar.Image = global::Cambiar_Color_Imagen_SVG.Properties.Resources.close;
            this.btnCerrar.Location = new System.Drawing.Point(764, 0);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.ShadowDecoration.Parent = this.btnCerrar;
            this.btnCerrar.Size = new System.Drawing.Size(30, 30);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // FormCambiarSVG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 540);
            this.Controls.Add(this.panelCentral);
            this.Controls.Add(this.panelIzquierdo);
            this.Controls.Add(this.panelSuperior);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1020, 540);
            this.Name = "FormCambiarSVG";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor de Colores SVG";
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelIzquierdo.ResumeLayout(false);
            this.panelTamaño.ResumeLayout(false);
            this.panelTamaño.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupAlto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupAncho)).EndInit();
            this.panelColor.ResumeLayout(false);
            this.panelColor.PerformLayout();
            this.panelCentral.ResumeLayout(false);
            this.panelAjustar.ResumeLayout(false);
            this.panelAjustar.PerformLayout();
            this.panelBuscador.ResumeLayout(false);
            this.panelBuscador.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pickImagen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReducir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAmpliar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2ResizeForm ResizeForm;
        private System.Windows.Forms.Panel panelSuperior;
        private Guna.UI2.WinForms.Guna2Button btnCerrar;
        private Guna.UI2.WinForms.Guna2Button btnMinimizar;
        private Guna.UI2.WinForms.Guna2Button btnMaximizarNormal;
        private Guna.UI2.WinForms.Guna2Button btnLogo;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.Panel panelIzquierdo;
        private Guna.UI2.WinForms.Guna2Button btnAcerca;
        private Guna.UI2.WinForms.Guna2Button btnColor;
        private Guna.UI2.WinForms.Guna2Panel panelColor;
        private Guna.UI2.WinForms.Guna2Button btnTamaño;
        private Guna.UI2.WinForms.Guna2Panel panelTamaño;
        private System.Windows.Forms.Label lblColorRemplazar;
        private System.Windows.Forms.Button btnColorOrigen;
        private Guna.UI2.WinForms.Guna2Button btnCambiar;
        private System.Windows.Forms.Label lblColorxRemplazar;
        private System.Windows.Forms.Button btnColorDestino;
        private Guna.UI2.WinForms.Guna2Button btnElegirOrigen;
        private Guna.UI2.WinForms.Guna2NumericUpDown nupAlto;
        private System.Windows.Forms.Label lblAlto;
        private Guna.UI2.WinForms.Guna2NumericUpDown nupAncho;
        private System.Windows.Forms.Label lblAncho;
        private System.Windows.Forms.PictureBox btnAmpliar;
        private System.Windows.Forms.PictureBox btnReducir;
        private System.Windows.Forms.PictureBox pickImagen;
        private System.Windows.Forms.Panel panelAjustar;
        private System.Windows.Forms.Panel panelBuscador;
        private Guna.UI2.WinForms.Guna2Button btnBuscar;
        private Guna.UI2.WinForms.Guna2TextBox txtBuscar;
        private System.Windows.Forms.Label lblImagenSVG;
        private Guna.UI2.WinForms.Guna2Button btnGuardar;
        private Guna.UI2.WinForms.Guna2Button btnDescargar;
        private System.Windows.Forms.OpenFileDialog filePicker;
        private System.Windows.Forms.Label lblColorFondo;
        private System.Windows.Forms.Button btnColorFondo;
        private System.Windows.Forms.ColorDialog colorPicker;
        private Guna.UI2.WinForms.Guna2Button btnElegirDestino;
        private Guna.UI2.WinForms.Guna2Button btnAjustar;
        private System.Windows.Forms.SaveFileDialog Guardar;
    }
}

