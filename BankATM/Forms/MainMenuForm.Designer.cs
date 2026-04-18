namespace BankATM.Forms
{
    partial class MainMenuForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.timerIdle = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.btnMiniStatement = new FontAwesome.Sharp.IconButton();
            this.btnSettings = new FontAwesome.Sharp.IconButton();
            this.btnTheme = new FontAwesome.Sharp.IconButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExit2 = new FontAwesome.Sharp.IconButton();
            this.btnExit = new Krypton.Toolkit.KryptonButton();
            this.btnChangePin = new Krypton.Toolkit.KryptonButton();
            this.btnTransfer = new Krypton.Toolkit.KryptonButton();
            this.btnWithdraw = new Krypton.Toolkit.KryptonButton();
            this.btnBalance = new Krypton.Toolkit.KryptonButton();
            this.btnDeposit = new Krypton.Toolkit.KryptonButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timerClock
            // 
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // timerIdle
            // 
            this.timerIdle.Tick += new System.EventHandler(this.timerIdle_Tick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(252)))), ((int)(((byte)(206)))));
            this.label1.Location = new System.Drawing.Point(852, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 10);
            this.label1.TabIndex = 21;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnMiniStatement);
            this.kryptonPanel1.Controls.Add(this.btnSettings);
            this.kryptonPanel1.Controls.Add(this.btnTheme);
            this.kryptonPanel1.Controls.Add(this.pictureBox1);
            this.kryptonPanel1.Controls.Add(this.btnExit2);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Padding = new System.Windows.Forms.Padding(4);
            this.kryptonPanel1.Size = new System.Drawing.Size(1788, 148);
            this.kryptonPanel1.StateCommon.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(236)))));
            this.kryptonPanel1.StateCommon.Color2 = System.Drawing.Color.White;
            this.kryptonPanel1.StateCommon.ColorAngle = 1F;
            this.kryptonPanel1.TabIndex = 18;
            // 
            // btnMiniStatement
            // 
            this.btnMiniStatement.BackColor = System.Drawing.Color.Transparent;
            this.btnMiniStatement.FlatAppearance.BorderSize = 0;
            this.btnMiniStatement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMiniStatement.IconChar = FontAwesome.Sharp.IconChar.List;
            this.btnMiniStatement.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.btnMiniStatement.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnMiniStatement.IconSize = 24;
            this.btnMiniStatement.Location = new System.Drawing.Point(7, 18);
            this.btnMiniStatement.Name = "btnMiniStatement";
            this.btnMiniStatement.Size = new System.Drawing.Size(65, 52);
            this.btnMiniStatement.TabIndex = 27;
            this.btnMiniStatement.UseVisualStyleBackColor = false;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.IconChar = FontAwesome.Sharp.IconChar.Cog;
            this.btnSettings.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.btnSettings.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSettings.IconSize = 24;
            this.btnSettings.Location = new System.Drawing.Point(78, 24);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(50, 40);
            this.btnSettings.TabIndex = 26;
            this.btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnTheme
            // 
            this.btnTheme.BackColor = System.Drawing.Color.Transparent;
            this.btnTheme.FlatAppearance.BorderSize = 0;
            this.btnTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheme.IconChar = FontAwesome.Sharp.IconChar.Moon;
            this.btnTheme.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.btnTheme.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnTheme.IconSize = 24;
            this.btnTheme.Location = new System.Drawing.Point(1675, 24);
            this.btnTheme.Name = "btnTheme";
            this.btnTheme.Size = new System.Drawing.Size(50, 40);
            this.btnTheme.TabIndex = 25;
            this.btnTheme.UseVisualStyleBackColor = false;
            this.btnTheme.Click += new System.EventHandler(this.btnTheme_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(712, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(383, 148);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // btnExit2
            // 
            this.btnExit2.BackColor = System.Drawing.Color.Transparent;
            this.btnExit2.FlatAppearance.BorderSize = 0;
            this.btnExit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit2.IconChar = FontAwesome.Sharp.IconChar.Remove;
            this.btnExit2.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(35)))), ((int)(((byte)(0)))));
            this.btnExit2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnExit2.IconSize = 24;
            this.btnExit2.Location = new System.Drawing.Point(1731, 24);
            this.btnExit2.Name = "btnExit2";
            this.btnExit2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnExit2.Size = new System.Drawing.Size(50, 40);
            this.btnExit2.TabIndex = 11;
            this.btnExit2.UseVisualStyleBackColor = false;
            this.btnExit2.Click += new System.EventHandler(this.btnExit2_Click);
            // 
            // btnExit
            // 
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Location = new System.Drawing.Point(929, 763);
            this.btnExit.Name = "btnExit";
            this.btnExit.OverrideDefault.Back.Color1 = System.Drawing.Color.White;
            this.btnExit.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnExit.OverrideDefault.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnExit.Size = new System.Drawing.Size(726, 189);
            this.btnExit.StateCommon.Back.Color1 = System.Drawing.Color.Salmon;
            this.btnExit.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnExit.StateCommon.Back.ColorAngle = 10F;
            this.btnExit.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnExit.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnExit.StateCommon.Border.Rounding = 15F;
            this.btnExit.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.TabIndex = 16;
            this.btnExit.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnExit.Values.Text = "SALIR";
            // 
            // btnChangePin
            // 
            this.btnChangePin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePin.Location = new System.Drawing.Point(153, 763);
            this.btnChangePin.Name = "btnChangePin";
            this.btnChangePin.OverrideDefault.Back.Color1 = System.Drawing.Color.White;
            this.btnChangePin.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnChangePin.OverrideDefault.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnChangePin.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnChangePin.Size = new System.Drawing.Size(726, 189);
            this.btnChangePin.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnChangePin.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnChangePin.StateCommon.Back.ColorAngle = 2F;
            this.btnChangePin.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnChangePin.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnChangePin.StateCommon.Border.Rounding = 15F;
            this.btnChangePin.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePin.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePin.TabIndex = 15;
            this.btnChangePin.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnChangePin.Values.Text = "CAMBIAR PIN";
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(929, 518);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.OverrideDefault.Back.Color1 = System.Drawing.Color.White;
            this.btnTransfer.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnTransfer.OverrideDefault.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnTransfer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnTransfer.Size = new System.Drawing.Size(726, 189);
            this.btnTransfer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnTransfer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnTransfer.StateCommon.Back.ColorAngle = 2F;
            this.btnTransfer.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnTransfer.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnTransfer.StateCommon.Border.Rounding = 15F;
            this.btnTransfer.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfer.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransfer.TabIndex = 14;
            this.btnTransfer.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnTransfer.Values.Text = "TRANSFERENCIA";
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWithdraw.Location = new System.Drawing.Point(153, 518);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.OverrideDefault.Back.Color1 = System.Drawing.Color.White;
            this.btnWithdraw.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnWithdraw.OverrideDefault.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnWithdraw.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnWithdraw.Size = new System.Drawing.Size(726, 189);
            this.btnWithdraw.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnWithdraw.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnWithdraw.StateCommon.Back.ColorAngle = 2F;
            this.btnWithdraw.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnWithdraw.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnWithdraw.StateCommon.Border.Rounding = 15F;
            this.btnWithdraw.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWithdraw.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWithdraw.TabIndex = 13;
            this.btnWithdraw.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnWithdraw.Values.Text = "RETIRO";
            // 
            // btnBalance
            // 
            this.btnBalance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBalance.Location = new System.Drawing.Point(929, 292);
            this.btnBalance.Name = "btnBalance";
            this.btnBalance.OverrideDefault.Back.Color1 = System.Drawing.Color.White;
            this.btnBalance.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnBalance.OverrideDefault.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnBalance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnBalance.Size = new System.Drawing.Size(726, 189);
            this.btnBalance.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnBalance.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnBalance.StateCommon.Back.ColorAngle = 2F;
            this.btnBalance.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnBalance.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnBalance.StateCommon.Border.Rounding = 15F;
            this.btnBalance.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBalance.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBalance.StateTracking.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnBalance.StateTracking.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnBalance.TabIndex = 12;
            this.btnBalance.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnBalance.Values.Text = "CONSULTAR SALDO";
            // 
            // btnDeposit
            // 
            this.btnDeposit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeposit.Location = new System.Drawing.Point(153, 292);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.OverrideDefault.Back.Color1 = System.Drawing.Color.White;
            this.btnDeposit.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnDeposit.OverrideDefault.Content.Padding = new System.Windows.Forms.Padding(0);
            this.btnDeposit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDeposit.Size = new System.Drawing.Size(726, 189);
            this.btnDeposit.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnDeposit.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnDeposit.StateCommon.Back.ColorAngle = 2F;
            this.btnDeposit.StateCommon.Border.Color1 = System.Drawing.Color.Transparent;
            this.btnDeposit.StateCommon.Border.Color2 = System.Drawing.Color.Transparent;
            this.btnDeposit.StateCommon.Border.Rounding = 15F;
            this.btnDeposit.StateCommon.Content.LongText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeposit.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeposit.TabIndex = 11;
            this.btnDeposit.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnDeposit.Values.Text = "DEPÓSITO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Carlito", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.label2.Location = new System.Drawing.Point(740, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(355, 24);
            this.label2.TabIndex = 22;
            this.label2.Text = "S E L E C C C I O N E    U N    S E R V I C I O";
            // 
            // lblGreeting
            // 
            this.lblGreeting.AutoSize = true;
            this.lblGreeting.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreeting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.lblGreeting.Location = new System.Drawing.Point(12, 177);
            this.lblGreeting.Name = "lblGreeting";
            this.lblGreeting.Size = new System.Drawing.Size(17, 24);
            this.lblGreeting.TabIndex = 23;
            this.lblGreeting.Text = "-";
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.Font = new System.Drawing.Font("Carlito", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.lblDateTime.Location = new System.Drawing.Point(1585, 177);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(16, 24);
            this.lblDateTime.TabIndex = 24;
            this.lblDateTime.Text = "-";
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1788, 1045);
            this.Controls.Add(this.lblDateTime);
            this.Controls.Add(this.lblGreeting);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnChangePin);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.btnWithdraw);
            this.Controls.Add(this.btnBalance);
            this.Controls.Add(this.btnDeposit);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainMenuForm";
            this.Load += new System.EventHandler(this.MainMenuForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.Timer timerIdle;
        private System.Windows.Forms.Label label1;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private FontAwesome.Sharp.IconButton btnExit2;
        private Krypton.Toolkit.KryptonButton btnExit;
        private Krypton.Toolkit.KryptonButton btnChangePin;
        private Krypton.Toolkit.KryptonButton btnTransfer;
        private Krypton.Toolkit.KryptonButton btnWithdraw;
        private Krypton.Toolkit.KryptonButton btnBalance;
        private Krypton.Toolkit.KryptonButton btnDeposit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton btnTheme;
        private FontAwesome.Sharp.IconButton btnSettings;
        private FontAwesome.Sharp.IconButton btnMiniStatement;
    }
}