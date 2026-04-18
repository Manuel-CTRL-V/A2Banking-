namespace BankATM.Forms
{
    partial class TransferForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferForm));
            this.lblCommissionNote = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblCommissionEstimate = new System.Windows.Forms.Label();
            this.lblAmountDisplay = new System.Windows.Forms.Label();
            this.btnConfirm = new Krypton.Toolkit.KryptonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnBack = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtToAccount = new Krypton.Toolkit.KryptonTextBox();
            this.txtAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblTitular = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCommissionNote
            // 
            this.lblCommissionNote.AutoSize = true;
            this.lblCommissionNote.Font = new System.Drawing.Font("Carlito", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommissionNote.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(138)))));
            this.lblCommissionNote.Location = new System.Drawing.Point(199, 361);
            this.lblCommissionNote.Name = "lblCommissionNote";
            this.lblCommissionNote.Size = new System.Drawing.Size(393, 22);
            this.lblCommissionNote.TabIndex = 2;
            this.lblCommissionNote.Text = "Se aplicará comisión del 0.15% si el titular es distinto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Carlito", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(103)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(132, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "Cuenta a transferir";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Carlito", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(103)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(406, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "Cantidad";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.lblError.Location = new System.Drawing.Point(12, 572);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(15, 24);
            this.lblError.TabIndex = 7;
            this.lblError.Text = ".";
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.lblBalance.Location = new System.Drawing.Point(579, 535);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(15, 24);
            this.lblBalance.TabIndex = 8;
            this.lblBalance.Text = ".";
            // 
            // lblCommissionEstimate
            // 
            this.lblCommissionEstimate.AutoSize = true;
            this.lblCommissionEstimate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(79)))), ((int)(((byte)(45)))));
            this.lblCommissionEstimate.Location = new System.Drawing.Point(645, 243);
            this.lblCommissionEstimate.Name = "lblCommissionEstimate";
            this.lblCommissionEstimate.Size = new System.Drawing.Size(13, 20);
            this.lblCommissionEstimate.TabIndex = 9;
            this.lblCommissionEstimate.Text = ".";
            // 
            // lblAmountDisplay
            // 
            this.lblAmountDisplay.AutoSize = true;
            this.lblAmountDisplay.Location = new System.Drawing.Point(645, 243);
            this.lblAmountDisplay.Name = "lblAmountDisplay";
            this.lblAmountDisplay.Size = new System.Drawing.Size(0, 20);
            this.lblAmountDisplay.TabIndex = 10;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(249, 401);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(285, 60);
            this.btnConfirm.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(243)))), ((int)(((byte)(232)))));
            this.btnConfirm.StateCommon.Back.Color2 = System.Drawing.Color.White;
            this.btnConfirm.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))));
            this.btnConfirm.StateCommon.Border.Color2 = System.Drawing.Color.White;
            this.btnConfirm.StateCommon.Border.Rounding = 15F;
            this.btnConfirm.StateCommon.Border.Width = 2;
            this.btnConfirm.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(128)))), ((int)(((byte)(90)))));
            this.btnConfirm.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Carlito", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.TabIndex = 11;
            this.btnConfirm.Values.DropDownArrowColor = System.Drawing.Color.Empty;
            this.btnConfirm.Values.Text = "CONFIRMAR";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(246, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(288, 165);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
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
            this.btnBack.TabIndex = 30;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(147)))), ((int)(((byte)(138)))));
            this.label1.Location = new System.Drawing.Point(166, 475);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(453, 18);
            this.label1.TabIndex = 31;
            this.label1.Text = "(Apoye el dedo en el biométrico luego de presionar \"Confirmar\")";
            // 
            // txtToAccount
            // 
            this.txtToAccount.Location = new System.Drawing.Point(136, 224);
            this.txtToAccount.Name = "txtToAccount";
            this.txtToAccount.Size = new System.Drawing.Size(209, 57);
            this.txtToAccount.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtToAccount.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtToAccount.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtToAccount.StateCommon.Border.Rounding = 16F;
            this.txtToAccount.StateCommon.Content.Color1 = System.Drawing.Color.Gray;
            this.txtToAccount.StateCommon.Content.Font = new System.Drawing.Font("Carlito", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToAccount.TabIndex = 32;
            this.txtToAccount.ToolTipValues.Description = "Ingrese su usuario";
            this.txtToAccount.TextChanged += new System.EventHandler(this.txtToAccount_TextChanged_1);
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(410, 224);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(209, 57);
            this.txtAmount.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtAmount.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtAmount.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(244)))), ((int)(((byte)(242)))));
            this.txtAmount.StateCommon.Border.Rounding = 16F;
            this.txtAmount.StateCommon.Content.Color1 = System.Drawing.Color.Gray;
            this.txtAmount.StateCommon.Content.Font = new System.Drawing.Font("Carlito", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.TabIndex = 33;
            this.txtAmount.ToolTipValues.Description = "Ingrese su usuario";
            // 
            // lblTitular
            // 
            this.lblTitular.AutoSize = true;
            this.lblTitular.Font = new System.Drawing.Font("Carlito", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitular.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(103)))), ((int)(((byte)(67)))));
            this.lblTitular.Location = new System.Drawing.Point(132, 305);
            this.lblTitular.Name = "lblTitular";
            this.lblTitular.Size = new System.Drawing.Size(185, 22);
            this.lblTitular.TabIndex = 34;
            this.lblTitular.Text = "Titular: no seleccionado";
            // 
            // TransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(233)))), ((int)(((byte)(225)))));
            this.ClientSize = new System.Drawing.Size(838, 621);
            this.Controls.Add(this.lblTitular);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.txtToAccount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblAmountDisplay);
            this.Controls.Add(this.lblCommissionEstimate);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCommissionNote);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransferForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TransferForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblCommissionNote;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblCommissionEstimate;
        private System.Windows.Forms.Label lblAmountDisplay;
        private Krypton.Toolkit.KryptonButton btnConfirm;
        private System.Windows.Forms.PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton btnBack;
        private System.Windows.Forms.Label label1;
        private Krypton.Toolkit.KryptonTextBox txtToAccount;
        private Krypton.Toolkit.KryptonTextBox txtAmount;
        private System.Windows.Forms.Label lblTitular;
    }
}