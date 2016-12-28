using System;
using System.Windows.Forms;

namespace TIDE
{
    partial class TIDE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TIDE));
            this.editor = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.RunButton = new System.Windows.Forms.ToolStripButton();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.OpenButton = new System.Windows.Forms.ToolStripButton();
            this.assemblerPage = new System.Windows.Forms.TabPage();
            this.assemblerTextBox = new System.Windows.Forms.RichTextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.NewButton = new System.Windows.Forms.ToolStripButton();
            this.SaveAsButton = new System.Windows.Forms.ToolStripButton();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.assemblerPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // editor
            // 
            this.editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.editor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.editor.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editor.Location = new System.Drawing.Point(6, 31);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(1451, 704);
            this.editor.TabIndex = 0;
            this.editor.Text = "";
            this.editor.SelectionChanged += new System.EventHandler(this.EditorOnSelectionChanged);
            this.editor.TextChanged += new System.EventHandler(this.editor_TextChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.assemblerPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1465, 783);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.tabPage1.Controls.Add(this.PositionLabel);
            this.tabPage1.Controls.Add(this.ToolBar);
            this.tabPage1.Controls.Add(this.editor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1457, 757);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "editor";
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PositionLabel.Location = new System.Drawing.Point(3, 741);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(115, 13);
            this.PositionLabel.TabIndex = 3;
            this.PositionLabel.Text = "Line: 0; Column: 0";
            // 
            // ToolBar
            // 
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunButton,
            this.SaveButton,
            this.SaveAsButton,
            this.OpenButton,
            this.NewButton});
            this.ToolBar.Location = new System.Drawing.Point(3, 3);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(1451, 25);
            this.ToolBar.TabIndex = 2;
            this.ToolBar.Text = "toolStrip1";
            // 
            // RunButton
            // 
            this.RunButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RunButton.Image = ((System.Drawing.Image)(resources.GetObject("RunButton.Image")));
            this.RunButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(23, 22);
            this.RunButton.Text = "Run";
            this.RunButton.ToolTipText = "Compile the TCode to assembler Code. \r\nYou can then view the code at the assemble" +
    "r tab.";
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(23, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.ToolTipText = "Save the current file";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenButton.Image")));
            this.OpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(23, 22);
            this.OpenButton.Text = "Open";
            this.OpenButton.ToolTipText = "Open existing Project";
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // assemblerPage
            // 
            this.assemblerPage.Controls.Add(this.assemblerTextBox);
            this.assemblerPage.Location = new System.Drawing.Point(4, 22);
            this.assemblerPage.Name = "assemblerPage";
            this.assemblerPage.Padding = new System.Windows.Forms.Padding(3);
            this.assemblerPage.Size = new System.Drawing.Size(1457, 757);
            this.assemblerPage.TabIndex = 1;
            this.assemblerPage.Text = "assembler";
            this.assemblerPage.UseVisualStyleBackColor = true;
            // 
            // assemblerTextBox
            // 
            this.assemblerTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.assemblerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assemblerTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assemblerTextBox.ForeColor = System.Drawing.Color.White;
            this.assemblerTextBox.Location = new System.Drawing.Point(3, 3);
            this.assemblerTextBox.Name = "assemblerTextBox";
            this.assemblerTextBox.ReadOnly = true;
            this.assemblerTextBox.Size = new System.Drawing.Size(1451, 751);
            this.assemblerTextBox.TabIndex = 0;
            this.assemblerTextBox.Text = "";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // NewButton
            // 
            this.NewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewButton.Image = ((System.Drawing.Image)(resources.GetObject("NewButton.Image")));
            this.NewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(23, 22);
            this.NewButton.Text = "New";
            this.NewButton.ToolTipText = "Create new empty file";
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveAsButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveAsButton.Image")));
            this.SaveAsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(23, 22);
            this.SaveAsButton.Text = "Save as";
            this.SaveAsButton.ToolTipText = "Save Document under specific path";
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // TIDE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(1465, 783);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "TIDE";
            this.Text = "TIDE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TIDE_FormClosing);
            this.Load += new System.EventHandler(this.TIDE_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.assemblerPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox editor;
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage assemblerPage;
        private ToolStrip ToolBar;
        private ToolStripButton RunButton;
        private RichTextBox assemblerTextBox;
        private ErrorProvider errorProvider;
        private ToolStripButton SaveButton;
        private global::System.Windows.Forms.ToolStripButton OpenButton;
        private Label PositionLabel;
        private ToolStripButton NewButton;
        private ToolStripButton SaveAsButton;
    }
}

