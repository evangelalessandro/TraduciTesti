namespace TraslatePdf.Forms
{
    partial class GoogleOrMicrosoftTranslate
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoogleOrMicrosoftTranslate));
			this.labelGoogleTranslate = new System.Windows.Forms.Label();
			this.comboBoxFrom = new System.Windows.Forms.ComboBox();
			this.buttonTranslate = new System.Windows.Forms.Button();
			this.labelTo = new System.Windows.Forms.Label();
			this.comboBoxTo = new System.Windows.Forms.ComboBox();
			this.labelFrom = new System.Windows.Forms.Label();
			this.txtSourceText = new System.Windows.Forms.TextBox();
			this.labelSourceText = new System.Windows.Forms.Label();
			this.btnCopiaGoogle = new System.Windows.Forms.Button();
			this.txtGoogle = new System.Windows.Forms.TextBox();
			this.btnFormatSource = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelGoogleTranslate
			// 
			this.labelGoogleTranslate.AutoSize = true;
			this.labelGoogleTranslate.Location = new System.Drawing.Point(14, 259);
			this.labelGoogleTranslate.Name = "labelGoogleTranslate";
			this.labelGoogleTranslate.Size = new System.Drawing.Size(84, 13);
			this.labelGoogleTranslate.TabIndex = 20;
			this.labelGoogleTranslate.Text = "Google translate";
			// 
			// comboBoxFrom
			// 
			this.comboBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxFrom.FormattingEnabled = true;
			this.comboBoxFrom.Location = new System.Drawing.Point(61, 29);
			this.comboBoxFrom.Name = "comboBoxFrom";
			this.comboBoxFrom.Size = new System.Drawing.Size(176, 21);
			this.comboBoxFrom.TabIndex = 0;
			// 
			// buttonTranslate
			// 
			this.buttonTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonTranslate.Location = new System.Drawing.Point(453, 12);
			this.buttonTranslate.Name = "buttonTranslate";
			this.buttonTranslate.Size = new System.Drawing.Size(204, 53);
			this.buttonTranslate.TabIndex = 2;
			this.buttonTranslate.Text = "Translate";
			this.buttonTranslate.UseVisualStyleBackColor = true;
			this.buttonTranslate.Click += new System.EventHandler(this.buttonTranslate_Click);
			// 
			// labelTo
			// 
			this.labelTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTo.AutoSize = true;
			this.labelTo.Location = new System.Drawing.Point(253, 32);
			this.labelTo.Name = "labelTo";
			this.labelTo.Size = new System.Drawing.Size(23, 13);
			this.labelTo.TabIndex = 25;
			this.labelTo.Text = "To:";
			// 
			// comboBoxTo
			// 
			this.comboBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBoxTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTo.FormattingEnabled = true;
			this.comboBoxTo.Location = new System.Drawing.Point(282, 29);
			this.comboBoxTo.Name = "comboBoxTo";
			this.comboBoxTo.Size = new System.Drawing.Size(156, 21);
			this.comboBoxTo.TabIndex = 1;
			// 
			// labelFrom
			// 
			this.labelFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelFrom.AutoSize = true;
			this.labelFrom.Location = new System.Drawing.Point(22, 32);
			this.labelFrom.Name = "labelFrom";
			this.labelFrom.Size = new System.Drawing.Size(33, 13);
			this.labelFrom.TabIndex = 23;
			this.labelFrom.Text = "From:";
			// 
			// txtSourceText
			// 
			this.txtSourceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSourceText.Location = new System.Drawing.Point(17, 71);
			this.txtSourceText.MaxLength = 0;
			this.txtSourceText.Multiline = true;
			this.txtSourceText.Name = "txtSourceText";
			this.txtSourceText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtSourceText.Size = new System.Drawing.Size(889, 185);
			this.txtSourceText.TabIndex = 3;
			this.txtSourceText.Text = resources.GetString("txtSourceText.Text");
			// 
			// labelSourceText
			// 
			this.labelSourceText.AutoSize = true;
			this.labelSourceText.Location = new System.Drawing.Point(14, 55);
			this.labelSourceText.Name = "labelSourceText";
			this.labelSourceText.Size = new System.Drawing.Size(61, 13);
			this.labelSourceText.TabIndex = 28;
			this.labelSourceText.Text = "Source text";
			// 
			// btnCopiaGoogle
			// 
			this.btnCopiaGoogle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopiaGoogle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCopiaGoogle.Location = new System.Drawing.Point(702, 447);
			this.btnCopiaGoogle.Name = "btnCopiaGoogle";
			this.btnCopiaGoogle.Size = new System.Drawing.Size(204, 32);
			this.btnCopiaGoogle.TabIndex = 29;
			this.btnCopiaGoogle.Text = "Copia";
			this.btnCopiaGoogle.UseVisualStyleBackColor = true;
			this.btnCopiaGoogle.Click += new System.EventHandler(this.btnCopiaGoogle_Click);
			// 
			// txtGoogle
			// 
			this.txtGoogle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGoogle.Location = new System.Drawing.Point(17, 275);
			this.txtGoogle.MaxLength = 0;
			this.txtGoogle.Multiline = true;
			this.txtGoogle.Name = "txtGoogle";
			this.txtGoogle.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtGoogle.Size = new System.Drawing.Size(889, 166);
			this.txtGoogle.TabIndex = 31;
			// 
			// btnFormatSource
			// 
			this.btnFormatSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFormatSource.Location = new System.Drawing.Point(673, 12);
			this.btnFormatSource.Name = "btnFormatSource";
			this.btnFormatSource.Size = new System.Drawing.Size(204, 53);
			this.btnFormatSource.TabIndex = 32;
			this.btnFormatSource.Text = "Formatta Source";
			this.btnFormatSource.UseVisualStyleBackColor = true;
			this.btnFormatSource.Click += new System.EventHandler(this.btnFormatSource_Click);
			// 
			// GoogleOrMicrosoftTranslate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(918, 491);
			this.Controls.Add(this.btnFormatSource);
			this.Controls.Add(this.txtGoogle);
			this.Controls.Add(this.btnCopiaGoogle);
			this.Controls.Add(this.labelSourceText);
			this.Controls.Add(this.txtSourceText);
			this.Controls.Add(this.comboBoxFrom);
			this.Controls.Add(this.buttonTranslate);
			this.Controls.Add(this.labelTo);
			this.Controls.Add(this.comboBoxTo);
			this.Controls.Add(this.labelFrom);
			this.Controls.Add(this.labelGoogleTranslate);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GoogleOrMicrosoftTranslate";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "GoogleOrMicrosoftTranslate";
			this.Shown += new System.EventHandler(this.GoogleOrMicrosoftTranslate_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelGoogleTranslate;
        private System.Windows.Forms.ComboBox comboBoxFrom;
        private System.Windows.Forms.Button buttonTranslate;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.ComboBox comboBoxTo;
        private System.Windows.Forms.Label labelFrom;
        private System.Windows.Forms.TextBox txtSourceText;
        private System.Windows.Forms.Label labelSourceText;
    private System.Windows.Forms.Button btnCopiaGoogle;
    private System.Windows.Forms.TextBox txtGoogle;
		private System.Windows.Forms.Button btnFormatSource;
	}
}