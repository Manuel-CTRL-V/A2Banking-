namespace BackOffice.Forms
{
    partial class MainFormMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormMenu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuOpenAccount = new FontAwesome.Sharp.IconMenuItem();
            this.menuManageAccounts = new FontAwesome.Sharp.IconMenuItem();
            this.menuViewAu = new FontAwesome.Sharp.IconMenuItem();
            this.menuViewStatistics = new FontAwesome.Sharp.IconMenuItem();
            this.menuMaintenance = new FontAwesome.Sharp.IconMenuItem();
            this.opUserMaintenance = new FontAwesome.Sharp.IconMenuItem();
            this.opRoleMaintenance = new FontAwesome.Sharp.IconMenuItem();
            this.menuAbout = new FontAwesome.Sharp.IconMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.btnTheme = new FontAwesome.Sharp.IconButton();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(236)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenAccount,
            this.menuManageAccounts,
            this.menuViewAu,
            this.menuViewStatistics,
            this.menuMaintenance,
            this.menuAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1300, 61);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuOpenAccount
            // 
            this.menuOpenAccount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuOpenAccount.IconChar = FontAwesome.Sharp.IconChar.Pen;
            this.menuOpenAccount.IconColor = System.Drawing.Color.Black;
            this.menuOpenAccount.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuOpenAccount.Name = "menuOpenAccount";
            this.menuOpenAccount.Size = new System.Drawing.Size(221, 53);
            this.menuOpenAccount.Text = "Abrir Cuenta Nueva";
            this.menuOpenAccount.Click += new System.EventHandler(this.menuOpenAccount_Click);
            // 
            // menuManageAccounts
            // 
            this.menuManageAccounts.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuManageAccounts.IconChar = FontAwesome.Sharp.IconChar.Gears;
            this.menuManageAccounts.IconColor = System.Drawing.Color.Black;
            this.menuManageAccounts.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuManageAccounts.Name = "menuManageAccounts";
            this.menuManageAccounts.Size = new System.Drawing.Size(212, 53);
            this.menuManageAccounts.Text = "Gestionar Cuentas";
            this.menuManageAccounts.Click += new System.EventHandler(this.menuManageAccounts_Click);
            // 
            // menuViewAu
            // 
            this.menuViewAu.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuViewAu.IconChar = FontAwesome.Sharp.IconChar.Eye;
            this.menuViewAu.IconColor = System.Drawing.Color.Black;
            this.menuViewAu.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuViewAu.Name = "menuViewAu";
            this.menuViewAu.Size = new System.Drawing.Size(163, 53);
            this.menuViewAu.Text = "Ver Auditoría";
            this.menuViewAu.Click += new System.EventHandler(this.menuViewAu_Click);
            // 
            // menuViewStatistics
            // 
            this.menuViewStatistics.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuViewStatistics.IconChar = FontAwesome.Sharp.IconChar.ChartSimple;
            this.menuViewStatistics.IconColor = System.Drawing.Color.Black;
            this.menuViewStatistics.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuViewStatistics.Name = "menuViewStatistics";
            this.menuViewStatistics.Size = new System.Drawing.Size(155, 53);
            this.menuViewStatistics.Text = "Estadísticas";
            this.menuViewStatistics.Click += new System.EventHandler(this.mnuViewStatistics_Click);
            // 
            // menuMaintenance
            // 
            this.menuMaintenance.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opUserMaintenance,
            this.opRoleMaintenance});
            this.menuMaintenance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuMaintenance.IconChar = FontAwesome.Sharp.IconChar.Wrench;
            this.menuMaintenance.IconColor = System.Drawing.Color.Black;
            this.menuMaintenance.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuMaintenance.Name = "menuMaintenance";
            this.menuMaintenance.Size = new System.Drawing.Size(180, 53);
            this.menuMaintenance.Text = "Mantenimiento";
            // 
            // opUserMaintenance
            // 
            this.opUserMaintenance.IconChar = FontAwesome.Sharp.IconChar.User;
            this.opUserMaintenance.IconColor = System.Drawing.Color.Black;
            this.opUserMaintenance.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.opUserMaintenance.Name = "opUserMaintenance";
            this.opUserMaintenance.Size = new System.Drawing.Size(270, 34);
            this.opUserMaintenance.Text = "Usuarios";
            this.opUserMaintenance.Click += new System.EventHandler(this.opUserMaintenance_Click);
            // 
            // opRoleMaintenance
            // 
            this.opRoleMaintenance.IconChar = FontAwesome.Sharp.IconChar.Tag;
            this.opRoleMaintenance.IconColor = System.Drawing.Color.Black;
            this.opRoleMaintenance.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.opRoleMaintenance.Name = "opRoleMaintenance";
            this.opRoleMaintenance.Size = new System.Drawing.Size(270, 34);
            this.opRoleMaintenance.Text = "Roles";
            this.opRoleMaintenance.Click += new System.EventHandler(this.opRoleMaintenance_Click);
            // 
            // menuAbout
            // 
            this.menuAbout.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuAbout.IconChar = FontAwesome.Sharp.IconChar.CircleInfo;
            this.menuAbout.IconColor = System.Drawing.Color.Black;
            this.menuAbout.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(140, 53);
            this.menuAbout.Text = "Acerca De";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblRole);
            this.panel1.Controls.Add(this.lblWelcome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 879);
            this.panel1.TabIndex = 1;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(587, 428);
            this.lblRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(74, 29);
            this.lblRole.TabIndex = 1;
            this.lblRole.Text = "label1";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(587, 356);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(74, 29);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "label1";
            // 
            // iconButton1
            // 
            this.iconButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(236)))));
            this.iconButton1.FlatAppearance.BorderSize = 0;
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.iconButton1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(35)))), ((int)(((byte)(0)))));
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.IconSize = 32;
            this.iconButton1.Location = new System.Drawing.Point(1235, 9);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.iconButton1.Size = new System.Drawing.Size(65, 45);
            this.iconButton1.TabIndex = 20;
            this.iconButton1.UseVisualStyleBackColor = false;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // btnTheme
            // 
            this.btnTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(236)))));
            this.btnTheme.FlatAppearance.BorderSize = 0;
            this.btnTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheme.IconChar = FontAwesome.Sharp.IconChar.Moon;
            this.btnTheme.IconColor = System.Drawing.Color.Black;
            this.btnTheme.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnTheme.IconSize = 24;
            this.btnTheme.Location = new System.Drawing.Point(1165, 9);
            this.btnTheme.Name = "btnTheme";
            this.btnTheme.Size = new System.Drawing.Size(50, 40);
            this.btnTheme.TabIndex = 30;
            this.btnTheme.UseVisualStyleBackColor = false;
            this.btnTheme.Click += new System.EventHandler(this.btnTheme_Click);
            // 
            // MainFormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 940);
            this.Controls.Add(this.btnTheme);
            this.Controls.Add(this.iconButton1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Carlito", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainFormMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainFormMenu";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private FontAwesome.Sharp.IconMenuItem menuOpenAccount;
        private FontAwesome.Sharp.IconMenuItem menuManageAccounts;
        private FontAwesome.Sharp.IconMenuItem menuViewAu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblWelcome;
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconMenuItem menuViewStatistics;
        private FontAwesome.Sharp.IconButton btnTheme;
        private FontAwesome.Sharp.IconMenuItem menuAbout;
        private FontAwesome.Sharp.IconMenuItem menuMaintenance;
        private FontAwesome.Sharp.IconMenuItem opUserMaintenance;
        private FontAwesome.Sharp.IconMenuItem opRoleMaintenance;
    }
}