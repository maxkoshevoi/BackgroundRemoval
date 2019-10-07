namespace BackgroundRemoval.Demo
{
    partial class MainForm
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
            this.cbCamera = new System.Windows.Forms.ComboBox();
            this.pbSobel = new System.Windows.Forms.PictureBox();
            this.pbCanny = new System.Windows.Forms.PictureBox();
            this.pbSC = new System.Windows.Forms.PictureBox();
            this.pbMainObject = new System.Windows.Forms.PictureBox();
            this.pbAllObjects = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rbSobel = new System.Windows.Forms.RadioButton();
            this.rbCanny = new System.Windows.Forms.RadioButton();
            this.rbSobelCanny = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pbOriginal = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbSobel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanny)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMainObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAllObjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCamera
            // 
            this.cbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCamera.FormattingEnabled = true;
            this.cbCamera.Location = new System.Drawing.Point(12, 12);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Size = new System.Drawing.Size(697, 21);
            this.cbCamera.TabIndex = 1;
            this.cbCamera.SelectedIndexChanged += new System.EventHandler(this.cbCamera_SelectedIndexChanged);
            // 
            // pbSobel
            // 
            this.pbSobel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSobel.Location = new System.Drawing.Point(12, 39);
            this.pbSobel.Name = "pbSobel";
            this.pbSobel.Size = new System.Drawing.Size(228, 209);
            this.pbSobel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSobel.TabIndex = 12;
            this.pbSobel.TabStop = false;
            // 
            // pbCanny
            // 
            this.pbCanny.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCanny.Location = new System.Drawing.Point(246, 39);
            this.pbCanny.Name = "pbCanny";
            this.pbCanny.Size = new System.Drawing.Size(228, 209);
            this.pbCanny.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCanny.TabIndex = 13;
            this.pbCanny.TabStop = false;
            // 
            // pbSC
            // 
            this.pbSC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSC.Location = new System.Drawing.Point(480, 39);
            this.pbSC.Name = "pbSC";
            this.pbSC.Size = new System.Drawing.Size(228, 209);
            this.pbSC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSC.TabIndex = 14;
            this.pbSC.TabStop = false;
            // 
            // pbMainObject
            // 
            this.pbMainObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMainObject.Location = new System.Drawing.Point(481, 288);
            this.pbMainObject.Name = "pbMainObject";
            this.pbMainObject.Size = new System.Drawing.Size(228, 209);
            this.pbMainObject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbMainObject.TabIndex = 19;
            this.pbMainObject.TabStop = false;
            // 
            // pbAllObjects
            // 
            this.pbAllObjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbAllObjects.Location = new System.Drawing.Point(247, 288);
            this.pbAllObjects.Name = "pbAllObjects";
            this.pbAllObjects.Size = new System.Drawing.Size(228, 209);
            this.pbAllObjects.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAllObjects.TabIndex = 18;
            this.pbAllObjects.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(337, 500);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "All Objects";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(565, 500);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Main Object";
            // 
            // rbSobel
            // 
            this.rbSobel.AutoSize = true;
            this.rbSobel.Location = new System.Drawing.Point(86, 254);
            this.rbSobel.Name = "rbSobel";
            this.rbSobel.Size = new System.Drawing.Size(52, 17);
            this.rbSobel.TabIndex = 26;
            this.rbSobel.Text = "Sobel";
            this.rbSobel.UseVisualStyleBackColor = true;
            this.rbSobel.CheckedChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // rbCanny
            // 
            this.rbCanny.AutoSize = true;
            this.rbCanny.Checked = true;
            this.rbCanny.Location = new System.Drawing.Point(326, 254);
            this.rbCanny.Name = "rbCanny";
            this.rbCanny.Size = new System.Drawing.Size(55, 17);
            this.rbCanny.TabIndex = 27;
            this.rbCanny.TabStop = true;
            this.rbCanny.Text = "Canny";
            this.rbCanny.UseVisualStyleBackColor = true;
            this.rbCanny.CheckedChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // rbSobelCanny
            // 
            this.rbSobelCanny.AutoSize = true;
            this.rbSobelCanny.Location = new System.Drawing.Point(560, 254);
            this.rbSobelCanny.Name = "rbSobelCanny";
            this.rbSobelCanny.Size = new System.Drawing.Size(94, 17);
            this.rbSobelCanny.TabIndex = 28;
            this.rbSobelCanny.Text = "Sobel + Canny";
            this.rbSobelCanny.UseVisualStyleBackColor = true;
            this.rbSobelCanny.CheckedChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Images|*.jpg;*.png|All files|*.*";
            // 
            // pbOriginal
            // 
            this.pbOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbOriginal.Location = new System.Drawing.Point(13, 288);
            this.pbOriginal.Name = "pbOriginal";
            this.pbOriginal.Size = new System.Drawing.Size(228, 209);
            this.pbOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOriginal.TabIndex = 29;
            this.pbOriginal.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 500);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Original";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 536);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbOriginal);
            this.Controls.Add(this.rbSobelCanny);
            this.Controls.Add(this.rbCanny);
            this.Controls.Add(this.rbSobel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pbMainObject);
            this.Controls.Add(this.pbAllObjects);
            this.Controls.Add(this.pbSC);
            this.Controls.Add(this.pbCanny);
            this.Controls.Add(this.pbSobel);
            this.Controls.Add(this.cbCamera);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aforge - Center object detection";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSobel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanny)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMainObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAllObjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCamera;
        private System.Windows.Forms.PictureBox pbSobel;
        private System.Windows.Forms.PictureBox pbCanny;
        private System.Windows.Forms.PictureBox pbSC;
        private System.Windows.Forms.PictureBox pbMainObject;
        private System.Windows.Forms.PictureBox pbAllObjects;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbSobel;
        private System.Windows.Forms.RadioButton rbCanny;
        private System.Windows.Forms.RadioButton rbSobelCanny;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox pbOriginal;
        private System.Windows.Forms.Label label1;
    }
}