namespace Sah_Ai
{
    partial class InitialForm
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
            this.Network = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Network
            // 
            this.Network.Location = new System.Drawing.Point(351, 172);
            this.Network.Name = "Network";
            this.Network.Size = new System.Drawing.Size(121, 56);
            this.Network.TabIndex = 0;
            this.Network.Text = "Network";
            this.Network.UseVisualStyleBackColor = true;
            // 
            // InitialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Network);
            this.Name = "InitialForm";
            this.Text = "InitialForm";
            this.Load += new System.EventHandler(this.InitialForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Network;
    }
}