using MetaTextBox;


namespace TestApplication
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.metaTextBox1 = new MetaTextBox.MetaTextBox();
            this.SuspendLayout();
            // 
            // metaTextBox1
            // 
            this.metaTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.metaTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metaTextBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metaTextBox1.ForeColor = System.Drawing.Color.White;
            this.metaTextBox1.Location = new System.Drawing.Point(0, 0);
            this.metaTextBox1.Name = "metaTextBox1";
            this.metaTextBox1.Size = new System.Drawing.Size(474, 383);
            this.metaTextBox1.TabIndex = 0;
            this.metaTextBox1.Text = new ColoredString(metaTextBox1.ForeColor, metaTextBox1.BackColor, resources.GetString("metaTextBox1.Text"));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 383);
            this.Controls.Add(this.metaTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private MetaTextBox.MetaTextBox metaTextBox1;
    }
}