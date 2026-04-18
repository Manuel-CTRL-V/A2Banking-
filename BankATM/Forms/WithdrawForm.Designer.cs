namespace BankATM.Forms
{
    partial class WithdrawForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WithdrawForm));
            this.lblBalance = new System.Windows.Forms.Label();
            this.tblQuickAmounts = new System.Windows.Forms.TableLayoutPanel();
            this.btnQuick5000 = new FontAwesome.Sharp.IconButton();
            this.btnQuick2000 = new FontAwesome.Sharp.IconButton();
            this.btnQuick1000 = new FontAwesome.Sharp.IconButton();
            this.btnQuick500 = new FontAwesome.Sharp.IconButton();
            this.btnBack = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new Krypton.Toolkit.KryptonButton();
            this.lblAmountDisplay = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtAmount = new Krypton.Toolkit.KryptonTextBox();
            this.tblQuickAmounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(562, 697);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(0, 20);
            this.lblBalance.TabIndex = 8;
            // 
            // tblQuickAmounts
            // 
            this.tblQuickAmounts.ColumnCount = 2;
            this.tblQuickAmounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblQuickAmounts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblQuickAmounts.Controls.Add(this.btnQuick5000, 1, 1);
            this.tblQuickAmounts.Controls.Add(this.btnQuick2000, 0, 1);
            this.tblQuickAmounts.Controls.Add(this.btnQuick1000, 1, 0);
            this.tblQuickAmounts.Controls.Add(this.btnQuick500, 0, 0);
            this.tblQuickAmounts.Location = new System.Drawing.Point(88, 250);
            this.tblQuickAmounts.Name = "tblQuickAmounts";
            this.tblQuickAmounts.RowCount = 2;
            this.tblQuickAmounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblQuickAmounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblQuickAmounts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblQuickAmounts.Size = new System.Drawing.Size(710, 262);
            this.tblQuickAmounts.TabIndex = 16;
            // 
            // btnQuick5000
            // 
            this.btnQuick5000.AutoSize = true;
            this.btnQuick5000.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.btnQuick5000.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuick5000.FlatAppearance.BorderSize = 0;
            this.btnQuick5000.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuick5000.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuick5000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.btnQuick5000.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnQuick5000.IconColor = System.Drawing.Color.Black;
            this.btnQuick5000.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnQuick5000.Location = new System.Drawing.Point(358, 134);
            this.btnQuick5000.Name = "btnQuick5000";
            this.btnQuick5000.Size = new System.Drawing.Size(349, 125);
            this.btnQuick5000.TabIndex = 5;
            this.btnQuick5000.Text = "5000 RD$";
            this.btnQuick5000.UseVisualStyleBackColor = false;
            this.btnQuick5000.Click += new System.EventHandler(this.btnQuick5000_Click);
            // 
            // btnQuick2000
            // 
            this.btnQuick2000.AutoSize = true;
            this.btnQuick2000.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(103)))), ((int)(((byte)(67)))));
            this.btnQuick2000.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuick2000.FlatAppearance.BorderSize = 0;
            this.btnQuick2000.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuick2000.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuick2000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.btnQuick2000.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnQuick2000.IconColor = System.Drawing.Color.Black;
            this.btnQuick2000.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnQuick2000.Location = new System.Drawing.Point(3, 134);
            this.btnQuick2000.Name = "btnQuick2000";
            this.btnQuick2000.Size = new System.Drawing.Size(349, 125);
            this.btnQuick2000.TabIndex = 4;
            this.btnQuick2000.Text = "2000 RD$";
            this.btnQuick2000.UseVisualStyleBackColor = false;
            this.btnQuick2000.Click += new System.EventHandler(this.btnQuick2000_Click);
            // 
            // btnQuick1000
            // 
            this.btnQuick1000.AutoSize = true;
            this.btnQuick1000.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))));
            this.btnQuick1000.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuick1000.FlatAppearance.BorderSize = 0;
            this.btnQuick1000.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuick1000.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuick1000.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.btnQuick1000.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnQuick1000.IconColor = System.Drawing.Color.Black;
            this.btnQuick1000.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnQuick1000.Location = new System.Drawing.Point(358, 3);
            this.btnQuick1000.Name = "btnQuick1000";
            this.btnQuick1000.Size = new System.Drawing.Size(349, 125);
            this.btnQuick1000.TabIndex = 3;
            this.btnQuick1000.Text = "1000 RD$";
            this.btnQuick1000.UseVisualStyleBackColor = false;
            this.btnQuick1000.Click += new System.EventHandler(this.btnQuick1000_Click);
            // 
            // btnQuick500
            // 
            this.btnQuick500.AutoSize = true;
            this.btnQuick500.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(155)))), ((int)(((byte)(114)))));
            this.btnQuick500.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuick500.FlatAppearance.BorderSize = 0;
            this.btnQuick500.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQuick500.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuick500.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.btnQuick500.IconChar = FontAwesome.Sharp.IconChar.None;
            this.btnQuick500.IconColor = System.Drawing.Color.Black;
            this.btnQuick500.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnQuick500.Location = new System.Drawing.Point(3, 3);
            this.btnQuick500.Name = "btnQuick500";
            this.btnQuick500.Size = new System.Drawing.Size(349, 125);
            this.btnQuick500.TabIndex = 2;
            this.btnQuick500.Text = "500 RD$";
            this.btnQuick500.UseVisualStyleBackColor = false;
            this.btnQuick500.Click += new System.EventHandler(this.btnQuick500_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this.btnBack.FlatAppearance.BorderSize = 3;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBack.IconChar = FontAwesome.Sharp.IconChar.ArrowLeft;
            this.btnBack.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(35)))), ((int)(((byte)(0)))));
            this.btnBack.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBack.Location = new System.Drawing.Point(12, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(106, 72);
            this.btnBack.TabIndex = 18;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(138)))));
            this.label1.Location = new System.Drawing.Point(228, 631);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(453, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "(Apoye el dedo en el biométrico luego de presionar \"Confirmar\")";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(636, 537);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(281, 82);
            this.btnConfirm.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.btnConfirm.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.btnConfirm.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))));
            this.btnConfirm.StateCommon.Border.Rounding = 15F;
            this.btnConfirm.StateCommon.Border.Width = 2;
            this.btnConfirm.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))));
            this.btnConfirm.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.TabIndex = 19;
            this.btnConfirm.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnConfirm.Values.Text = "CONFIRMAR";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click_1);
            // 
            // lblAmountDisplay
            // 
            this.lblAmountDisplay.AutoSize = true;
            this.lblAmountDisplay.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountDisplay.Location = new System.Drawing.Point(373, 201);
            this.lblAmountDisplay.Name = "lblAmountDisplay";
            this.lblAmountDisplay.Size = new System.Drawing.Size(0, 23);
            this.lblAmountDisplay.TabIndex = 22;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(45, 697);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 20);
            this.lblError.TabIndex = 23;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(285, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(311, 149);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(308, 553);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 25);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 27;
            this.pictureBox2.TabStop = false;
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(334, 537);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(209, 71);
            this.txtAmount.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.txtAmount.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtAmount.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtAmount.StateCommon.Border.Rounding = 16F;
            this.txtAmount.StateCommon.Content.Color1 = System.Drawing.Color.Gray;
            this.txtAmount.StateCommon.Content.Font = new System.Drawing.Font("Carlito", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.TabIndex = 34;
            this.txtAmount.ToolTipValues.Description = "Elija o ingrese la cantidad para retirar";
            this.txtAmount.ToolTipValues.EnableToolTips = true;
            this.txtAmount.ToolTipValues.Heading = "Cantidad";
            // 
            // WithdrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(929, 752);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblAmountDisplay);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tblQuickAmounts);
            this.Controls.Add(this.lblBalance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WithdrawForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WithdrawForm";
            this.tblQuickAmounts.ResumeLayout(false);
            this.tblQuickAmounts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.TableLayoutPanel tblQuickAmounts;
        private FontAwesome.Sharp.IconButton btnQuick5000;
        private FontAwesome.Sharp.IconButton btnQuick2000;
        private FontAwesome.Sharp.IconButton btnQuick1000;
        private FontAwesome.Sharp.IconButton btnQuick500;
        private FontAwesome.Sharp.IconButton btnBack;
        private System.Windows.Forms.Label label1;
        private Krypton.Toolkit.KryptonButton btnConfirm;
        private System.Windows.Forms.Label lblAmountDisplay;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Krypton.Toolkit.KryptonTextBox txtAmount;
    }
}