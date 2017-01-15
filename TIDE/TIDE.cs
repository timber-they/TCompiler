﻿#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCompiler.Enums;
using TCompiler.Main;
using TCompiler.Settings;
using TIDE.Coloring.color;
using TIDE.Coloring.StringFunctions;
using TIDE.Forms;
using TIDE.Forms.Documentation;
using TIDE.Properties;

#endregion

namespace TIDE
{
    /// <summary>
    /// The main IDE class for the TIDE
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class TIDE : Form
    {
        /// <summary>
        /// The path to save the currently opened document
        /// </summary>
        private string _savePath;
        /// <summary>
        /// The whole text of the current document
        /// </summary>
        private string _wholeText;
        /// <summary>
        /// The documentation window in which the help is shown
        /// </summary>
        private DocumentationWindow _documentationWindow;

        /// <summary>
        /// Initializes a new TIDE
        /// </summary>
        public TIDE()
        {
            _documentationWindow = new DocumentationWindow();

            Intellisensing = false;
            Unsaved = false;
            SavePath = null;
            _wholeText = "";
            InitializeComponent();

            IntelliSensePopUp = new IntelliSensePopUp(GetUpdatedItems(), GetIntelliSensePosition()) { Visible = false };
            IntelliSensePopUp.ItemEntered += (sender, s) => IntelliSense_ItemSelected(s);
            Focus();
        }

        /// <summary>
        /// The current IntelliSensePopUp
        /// </summary>
        private IntelliSensePopUp IntelliSensePopUp { get; }
        /// <summary>
        /// Indicates wether the user didn't save the latest changes
        /// </summary>
        private bool Unsaved { get; set; }
        /// <summary>
        /// Indicates wether the text is changing because of intelliSense actions
        /// </summary>
        private bool Intellisensing { get; set; }

        /// <summary>
        /// The path to save the currently opened document
        /// </summary>
        private string SavePath
        {
            get { return _savePath; }
            set
            {
                var findForm = FindForm();
                if (findForm != null)
                    findForm.Text = value != null ? $@"TIDE - {value.Split('\\', '/').Last()}" : Resources.TIDE;
                _savePath = value;
            }
        }

        /// <summary>
        /// Saves the current dialogue (if necessary or wanted with dialogue)
        /// </summary>
        /// <param name="showDialogue">Indicates wether to use a dialogue</param>
        private void Save(bool showDialogue)
        {
            if ((SavePath == null) || showDialogue)
            {
                var dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    OverwritePrompt = true,
                    Title = Resources.Save,
                    Filter = Resources.Type_Ending,
                    DefaultExt = "tc"
                };
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                SavePath = dialog.FileName;
            }
            Unsaved = false;
            File.WriteAllText(SavePath, editor.Text);
        }

        /// <summary>
        /// Compiles the current document
        /// </summary>
        private async void Run() => await Task.Run(delegate
        {
            Invoke(new Action(() =>
            {
                Main.Initialize(SavePath, "out.asm", "error.txt");
                var ex = Main.CompileFile();
                if (ex != null)
                {
                    if (ex.Line >= 0)
                        ColorSomething.HighlightLine(ex.Line, editor, Color.Red);
                    MessageBox.Show(File.ReadAllText("error.txt"), Resources.Error);
                    if (ex.Line >= 0)
                        ColorSomething.HighlightLine(ex.Line, editor, editor.BackColor);
                    return;
                }
                assemblerTextBox.Text = File.ReadAllText("out.asm");
                ColorAll(assemblerTextBox, true);
                tabControl.SelectTab(assemblerPage);
            }));
        });

        /// <summary>
        /// Opens a new document - always opens a new dialogue
        /// </summary>
        private void Open()
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                Title = Resources.Open,
                Filter = Resources.Type_Ending,
                DefaultExt = "tc"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            SavePath = dialog.FileName;
            editor.TextChanged -= editor_TextChanged;
            editor.Text = File.ReadAllText(SavePath);
            ColorAll(editor);
            _wholeText = new string(editor.Text.ToCharArray());
            editor.TextChanged += editor_TextChanged;
        }

        /// <summary>
        /// Evaluates all the variable names existing in the document
        /// </summary>
        /// <returns>A list of the variable names</returns>
        private IEnumerable<string> GetVariableNames()
        {
            var fin = new List<string>(GlobalProperties.StandardVariables.Select(variable => variable.Name));
            VariableType foo;
            fin.AddRange(
                editor.Lines.Where(s => (s.Trim(' ').Split().Length > 1) && Enum.TryParse(s.Trim(' ').Split()[0], true, out foo))
                    .Select(s => s.Trim(' ').Split()[1]));
            return fin;
        }

        /// <summary>
        /// Evaluates the method names existing in the document
        /// </summary>
        /// <returns>An IEnumerable of the method names</returns>
        private IEnumerable<string> GetMethodNames() => new List<string>(
            editor.Lines.Where(s => s.Trim(' ').Split().Length > 1 && s.Trim(' ').Split().First().Trim(' ') == "method")
                .Select(s => s.Trim(' ').Split()[1].Trim(' ', '[')));

        /// <summary>
        /// Colors the whole document
        /// </summary>
        /// <param name="textBox">The textBox to color</param>
        /// <param name="asm">Indicates wether assembler code is colored</param>
        private async void ColorAll(RichTextBox textBox, bool asm = false)
            => await Task.Run(delegate
            {
                return textBox.Invoke(new Action(() =>
                {
                    BeginUpdate(textBox);
                    foreach (var c in GetCurrent.GetAllChars(textBox))
                        Coloring.Coloring.CharActions(c, textBox);
                    foreach (var word in GetCurrent.GetAllWords(textBox))
                        Coloring.Coloring.WordActions(word, textBox, asm);
                    EndUpdate(textBox);
                }));
            });

        #region IntelliSense

        /// <summary>
        /// Evaluates the position of the IntelliSense window
        /// </summary>
        /// <returns>The position as a point</returns>
        private Point GetIntelliSensePosition()
        {
            var pos = editor.PointToScreen(editor.GetPositionFromCharIndex(editor.SelectionStart));
            return new Point(pos.X, pos.Y + Cursor.Size.Height);
        }

        /// <summary>
        /// Shows the IntelliSense window
        /// </summary>
        private void ShowIntelliSense()
        {
            IntelliSensePopUp.Visible = true;
            IntelliSensePopUp.SelectIndex(0);
            Focus();
        }

        /// <summary>
        /// Hides the IntelliSense window
        /// </summary>
        private void HideIntelliSense() => IntelliSensePopUp.Visible = false;

        /// <summary>
        /// Updates the IntelliSense items
        /// </summary>
        private void UpdatIntelliSense() => IntelliSensePopUp.UpdateList(GetUpdatedItems());

        /// <summary>
        /// Evaluates the updated items for the IntelliSense window
        /// </summary>
        /// <returns>A list of the updated items</returns>
        private IEnumerable<string> GetUpdatedItems()
        {
            var vars = GetVariableNames().ToList();
            var methods = GetMethodNames().ToList();
            var general = PublicStuff.StringColorsTCode.Select(color => color.Thestring).ToList();

            vars.Sort();
            methods.Sort();
            general.Sort();

            var fin = general.Concat(vars).Concat(methods)
                .Where(s =>
                {
                    var current =
                        PublicStuff.Splitters.Any(
                            c =>
                                c ==
                                GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor)?.Value)
                            ? ""
                            : GetCurrent.GetCurrentWord(editor.SelectionStart, editor)?.Value;
                    return string.IsNullOrEmpty(current) || s.StartsWith(current, true, CultureInfo.InvariantCulture);
                }).Distinct().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            return fin;
        }

        #endregion

        #region Eventhandling

        #region ContextMenuHandling

        private void CopyCm(object obj, EventArgs e) => editor.Copy();

        private void CutCm(object obj, EventArgs e) => editor.Cut();

        private void PasteCm(object obj, EventArgs e) => editor.Paste();

        private void UndoCm(object obj, EventArgs e) => editor.Undo();

        private void RedoCm(object obj, EventArgs e) => editor.Redo();

        private void SelectAllCm(object obj, EventArgs e) => editor.SelectAll();

        private void CompileCm(object obj, EventArgs e) => RunButton.PerformClick();

        private void SaveCm(object obj, EventArgs e) => SaveButton.PerformClick();

        private void SaveAsCm(object obj, EventArgs e) => SaveAsButton.PerformClick();

        private void OpenCm(object obj, EventArgs e) => OpenButton.PerformClick();

        private void NewCm(object obj, EventArgs e) => NewButton.PerformClick();

        private void ColorAllCm(object obj, EventArgs e) => ColorAllButton.PerformClick();

        #endregion

        #region ButtonHandling

        /// <summary>
        /// Gets fired when the ColorAllButton got clicked and colors the whole document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void colorAllButton_Click(object sender, EventArgs e) => ColorAll(editor);

        /// <summary>
        /// Gets fired when the help button got clicked and prompts some help
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            _documentationWindow.ShowDialog();
            _documentationWindow = new DocumentationWindow();
        }

        /// <summary>
        /// Gets fired when the new button got pressed and creates a new document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void NewButton_Click(object sender, EventArgs e)
        {
            if (Unsaved)
            {
                var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning,
                    MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Yes:
                        SaveButton.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            editor.Text = "";
            SavePath = null;
        }

        /// <summary>
        /// Gets fired when the Save as Button got pressed and prompts a new save window
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void SaveAsButton_Click(object sender, EventArgs e) => Save(true);

        /// <summary>
        /// Gets fired when the Run button got pressed and compiles the current document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void RunButton_Click(object sender, EventArgs e)
        {
            SaveButton.PerformClick();
            if (SavePath == null)
            {
                MessageBox.Show(Resources.You_have_to_save_first, Resources.Error);
                return;
            }
            Run();
        }

        /// <summary>
        /// Gets fired when the Save button got pressed and saves the current document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void SaveButton_Click(object sender, EventArgs e) => Save(false);

        /// <summary>
        /// Gets fired when the Open button is pressed and opens a new document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (Unsaved)
            {
                var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning,
                    MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                    case DialogResult.Yes:
                        SaveButton.PerformClick();
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            Open();
        }

        #endregion

        /// <summary>
        /// Gets fired when an item from intelliSense is selected and inserts the selected item
        /// </summary>
        /// <param name="item">The selected item</param>
        private void IntelliSense_ItemSelected(string item)
        {
            HideIntelliSense();
            Intellisensing = true;
            var pos = editor.SelectionStart;
            var lw = GetCurrent.GetCurrentWord(pos, editor)?.Value;
            var s = item.Substring(item.Length >= (lw?.Length ?? 0) ? lw?.Length ?? 0 : 0) + " ";
            Focus();
            SendKeys.Send(s); //Because this is hilarious
        }

        /// <summary>
        /// Gets fired when the TextBox changed
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private async void editor_TextChanged(object sender = null, EventArgs e = null)
            => await Task.Run(delegate
            {
                return editor.Invoke(new Action(() =>
                {
                    BeginUpdate(editor);
                    if (editor.Text.Length - _wholeText.Length > 1)
                    {
                        ColorAll(editor);
                        editor_FontChanged(null, null);
                    }
                    else if (StringFunctions.GetRemoved(_wholeText, editor.Text).Contains(';') && editor.Text.Length > 0)
                        Coloring.Coloring.ColorCurrentLine(editor);
                    else
                    {
                        var cChar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
                        if (!string.IsNullOrEmpty(cChar?.Value.ToString()) && (cChar.Value == ';'))
                            Coloring.Coloring.ColorCurrentLine(editor);
                        else
                        {
                            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
                            Coloring.Coloring.WordActions(word, editor);
                            Coloring.Coloring.CharActions(cChar, editor);
                        }
                    }
                    Unsaved = true;
                    _wholeText = new string(editor.Text.ToCharArray());
                    UpdatIntelliSense();
                    EndUpdate(editor);
                    if (!Intellisensing)
                        return;
                    IntelliSensePopUp.Disselect();
                    Intellisensing = false;
                }));
            });

        /// <summary>
        /// Gets fired when the TIDE has loaded
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void TIDE_Load(object sender, EventArgs e)
        {
            IntelliSensePopUp.Show();
            HideIntelliSense();
            editor.Focus();

            editor.ContextMenu = new ContextMenu(new List<MenuItem>
            {
                new MenuItem("Copy",  CopyCm),
                new MenuItem("Cut", CutCm),
                new MenuItem("Paste", PasteCm),
                new MenuItem("Undo", UndoCm),
                new MenuItem("Redo", RedoCm),
                new MenuItem("Select all", SelectAllCm),
                new MenuItem("Compile", CompileCm),
                new MenuItem("Save", SaveCm),
                new MenuItem("Save as", SaveAsCm),
                new MenuItem("Open", OpenCm),
                new MenuItem("New", NewCm),
                new MenuItem("Color all", ColorAllCm)
            }.ToArray());
        }

        /// <summary>
        /// Gets fired when the cursor position has changed
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="eventArgs">Useless</param>
        private void Editor_SelectionChanged(object sender, EventArgs eventArgs)
        {
            var pos = Coloring.Coloring.GetStringofArray(editor.SelectionStart, editor.Lines);
            PositionLabel.Text = string.Format(Resources.Line_Column, pos.Beginning, pos.Ending);
            IntelliSensePopUp.Location = GetIntelliSensePosition();
            UpdatIntelliSense();
        }

        /// <summary>
        /// Gets fired when the TIDE is closing and eventually prompts the user for saving
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void TIDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Unsaved)
                return;
            var res = MessageBox.Show(Resources.Do_you_want_to_save_your_changes, Resources.Warning,
                MessageBoxButtons.YesNoCancel);
            switch (res)
            {
                case DialogResult.Yes:
                    SaveButton.PerformClick();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
            }
        }

        /// <summary>
        /// Gets fired before the user has pressed a key. Is there to prevent tab from doing something else
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Provides information about the pressed key</param>
        private void editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        /// <summary>
        /// Gets fired when the user has pressed a key.
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Provides information about the pressed key</param>
        private void TIDE_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
            e.Handled = false;
            switch (e.KeyCode)
            {
                case Keys.F5:
                    RunButton.PerformClick();
                    break;
                case Keys.Escape:
                    HideIntelliSense();
                    break;
                case Keys.Tab:
                case Keys.Enter:
                    if (!IntelliSensePopUp.Visible)
                    {
                        if (e.KeyCode != Keys.Tab)
                            return;
                        SendKeys.Send(new string(' ', 4));
                        break;
                    }
                    IntelliSense_ItemSelected(IntelliSensePopUp.GetSelected());
                    break;
                case Keys.Down:
                    if (!IntelliSensePopUp.Visible)
                        return;
                    IntelliSensePopUp.ScrollDown();
                    break;
                case Keys.Up:
                    if (!IntelliSensePopUp.Visible)
                        return;
                    IntelliSensePopUp.ScrollUp();
                    break;
                default:
                    if (!e.Control)
                        return;
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            if (e.Shift)
                                SaveAsButton.PerformClick();
                            else
                                SaveButton.PerformClick();
                            break;
                        case Keys.O:
                            OpenButton.PerformClick();
                            break;
                        case Keys.N:
                            NewButton.PerformClick();
                            break;
                        case Keys.Space:
                            ShowIntelliSense();
                            break;
                        default:
                            return;
                    }
                    break;
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// Makes sure that the font can't get changed.
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private async void editor_FontChanged(object sender, EventArgs e)
            => await Task.Run(() => editor.Invoke(new Action(() =>
            {
                BeginUpdate(editor);
                var oldSelection = editor.SelectionStart;
                editor.SelectAll();
                editor.SelectionFont = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                editor.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                editor.Select(oldSelection, 0);
                EndUpdate(editor);
            })));

        /// <summary>
        /// Same as TIDE_KeyDown
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Information about the key</param>
        private void editor_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);
        /// <summary>
        /// Same as TIDE_KeyDown
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Information about the key</param>
        private void ToolBar_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);
        /// <summary>
        /// Same as TIDE_KeyDown
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Information about the key</param>
        private void tabControl_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);

        /// <summary>
        /// Gets fired when the window has resized, because the IntelliSense window has to be moved.
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void TIDE_ResizeEnd(object sender, EventArgs e)
            => IntelliSensePopUp.Location = GetIntelliSensePosition();

        #endregion

        #region Update

        private const int EmSetEventMask = 0x0400 + 69;
        private const int WmSetredraw = 0x0b;
        private IntPtr _oldEventMask;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // ReSharper disable once IdentifierTypo
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void BeginUpdate(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action<RichTextBox>) BeginUpdate, tb);
                return;
            }
            SendMessage(tb.Handle, WmSetredraw, IntPtr.Zero, IntPtr.Zero);
            _oldEventMask = SendMessage(tb.Handle, EmSetEventMask, IntPtr.Zero, IntPtr.Zero);
        }

        private void EndUpdate(RichTextBox tb)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke((Action<RichTextBox>) EndUpdate, tb);
                return;
            }
            SendMessage(tb.Handle, WmSetredraw, (IntPtr) 1, IntPtr.Zero);
            SendMessage(tb.Handle, EmSetEventMask, IntPtr.Zero, _oldEventMask);
        }

        #endregion
    }
}