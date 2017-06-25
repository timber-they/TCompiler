using System;
using System.Drawing;
using System.Windows.Forms;

namespace TIDE.Forms
{
    partial class IntelliSensePopUp
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
            this.Items = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Items
            // 
            this.Items.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Items.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Items.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Items.ForeColor = System.Drawing.Color.White;
            this.Items.FormattingEnabled = true;
            this.Items.Location = new System.Drawing.Point(0, 0);
            this.Items.Margin = new System.Windows.Forms.Padding(0);
            this.Items.Name = "Items";
            this.Items.Size = new System.Drawing.Size(152, 80);
            this.Items.TabIndex = 0;
            this.Items.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Items_MouseDoubleClick);
            this.Items.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Items_PreviewKeyDown);
            // 
            // IntelliSensePopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F); 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(152, 80);
//            this.ControlBox = false;
            this.Controls.Add(this.Items);
            this.DoubleBuffered = true;
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
//            this.MaximizeBox = false;
//            this.MinimizeBox = false;
            this.Name = "IntelliSensePopUp";
//            this.ShowIcon = false;
//            this.ShowInTaskbar = false;
//            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private ListBox Items;
    }
}