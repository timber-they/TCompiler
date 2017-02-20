using System.Windows.Forms;

namespace TIDE.Forms
{
    partial class TIDE_MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TIDE_MainWindow));
            this.editor = new TIDE.Forms.TideTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.ParseToAssemblerButton = new System.Windows.Forms.ToolStripButton();
            this.RunButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveButton = new System.Windows.Forms.ToolStripButton();
            this.SaveAsButton = new System.Windows.Forms.ToolStripButton();
            this.OpenButton = new System.Windows.Forms.ToolStripButton();
            this.NewButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.HelpButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ColorAllButton = new System.Windows.Forms.ToolStripButton();
            this.assemblerPage = new System.Windows.Forms.TabPage();
            this.assemblerTextBox = new TIDE.Forms.TideTextBox();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.assemblerPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // editor
            // 
            this.editor.AcceptsTab = true;
            this.editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.editor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.editor.BulletIndent = 10;
            this.editor.DetectUrls = false;
            this.editor.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editor.ForeColor = System.Drawing.Color.White;
            this.editor.Location = new System.Drawing.Point(6, 31);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(1451, 704);
            this.editor.TabIndex = 0;
            this.editor.TabStop = false;
            this.editor.Text = "";
            this.editor.SelectionChanged += new System.EventHandler(this.Editor_SelectionChanged);
            this.editor.FontChanged += new System.EventHandler(this.editor_FontChanged);
            this.editor.TextChanged += new System.EventHandler(this.editor_TextChanged);
            this.editor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.editor_KeyDown);
            this.editor.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editor_PreviewKeyDown);
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
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyDown);
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
            this.ParseToAssemblerButton,
            this.RunButton,
            this.toolStripSeparator1,
            this.SaveButton,
            this.SaveAsButton,
            this.OpenButton,
            this.NewButton,
            this.toolStripSeparator2,
            this.HelpButton,
            this.toolStripSeparator3,
            this.ColorAllButton});
            this.ToolBar.Location = new System.Drawing.Point(3, 3);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(1451, 25);
            this.ToolBar.TabIndex = 2;
            this.ToolBar.Text = "Toolbar";
            this.ToolBar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ToolBar_KeyDown);
            // 
            // ParseToAssemblerButton
            // 
            this.ParseToAssemblerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ParseToAssemblerButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ParseToAssemblerButton.ForeColor = System.Drawing.Color.Black;
            this.ParseToAssemblerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ParseToAssemblerButton.Name = "ParseToAssemblerButton";
            this.ParseToAssemblerButton.Size = new System.Drawing.Size(23, 22);
            this.ParseToAssemblerButton.Text = "A";
            this.ParseToAssemblerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.ParseToAssemblerButton.ToolTipText = "Compiles the document and views the equivalent assembler code.";
            this.ParseToAssemblerButton.Click += new System.EventHandler(this.ParseToAssemblerButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RunButton.ForeColor = System.Drawing.Color.Transparent;
            this.RunButton.Image = ((System.Drawing.Image)(resources.GetObject("RunButton.Image")));
            this.RunButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(23, 22);
            this.RunButton.Text = "Run";
            this.RunButton.ToolTipText = "Compile the TCode to assembler Code, copy it and run the simulator from Ronald Ho" +
    "lzer.\r\nYou can then paste the assembler code into the simulator.\r\nSimulator: hol" +
    "zers-familie.de";
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // SaveButton
            // 
            this.SaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveButton.ForeColor = System.Drawing.Color.Transparent;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(23, 22);
            this.SaveButton.Text = "Save";
            this.SaveButton.ToolTipText = "Save the current file";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveAsButton.ForeColor = System.Drawing.Color.Transparent;
            this.SaveAsButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveAsButton.Image")));
            this.SaveAsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(23, 22);
            this.SaveAsButton.Text = "Save as";
            this.SaveAsButton.ToolTipText = "Save Document under specific path";
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenButton.ForeColor = System.Drawing.Color.Transparent;
            this.OpenButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenButton.Image")));
            this.OpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(23, 22);
            this.OpenButton.Text = "Open";
            this.OpenButton.ToolTipText = "Open existing Project";
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // NewButton
            // 
            this.NewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewButton.ForeColor = System.Drawing.Color.Transparent;
            this.NewButton.Image = ((System.Drawing.Image)(resources.GetObject("NewButton.Image")));
            this.NewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(23, 22);
            this.NewButton.Text = "New";
            this.NewButton.ToolTipText = "Create new empty file";
            this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // HelpButton
            // 
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HelpButton.ForeColor = System.Drawing.Color.Transparent;
            this.HelpButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpButton.Image")));
            this.HelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(23, 22);
            this.HelpButton.Text = "Help";
            this.HelpButton.ToolTipText = "View me some help\r\nDocumentation, instructions,...";
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ColorAllButton
            // 
            this.ColorAllButton.BackColor = System.Drawing.Color.Transparent;
            this.ColorAllButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ColorAllButton.ForeColor = System.Drawing.Color.Black;
            this.ColorAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ColorAllButton.Name = "ColorAllButton";
            this.ColorAllButton.Size = new System.Drawing.Size(23, 22);
            this.ColorAllButton.Text = "C";
            this.ColorAllButton.ToolTipText = "colors the whole document. ";
            this.ColorAllButton.Click += new System.EventHandler(this.colorAllButton_Click);
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
            // TIDE_MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(1465, 783);
            this.Controls.Add(this.tabControl);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TIDE_MainWindow";
            this.Text = "TIDE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TIDE_FormClosing);
            this.Load += new System.EventHandler(this.TIDE_Load);
            this.ResizeEnd += new System.EventHandler(this.TIDE_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TIDE_KeyDown);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.assemblerPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TideTextBox editor;
        private TabControl tabControl;
        private TabPage tabPage1;
        private TabPage assemblerPage;
        private ToolStrip ToolBar;
        private ToolStripButton RunButton;
        private TideTextBox assemblerTextBox;
        private ToolStripButton SaveButton;
        private global::System.Windows.Forms.ToolStripButton OpenButton;
        private Label PositionLabel;
        private ToolStripButton NewButton;
        private ToolStripButton SaveAsButton;
        private new ToolStripButton HelpButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ColorAllButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton ParseToAssemblerButton;
    }
}

