#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCompiler.Enums;
using TCompiler.Main;
using TCompiler.Settings;
using TIDE.Coloring.StringFunctions;
using TIDE.Coloring.Types;
using TIDE.Forms.Documentation;
using TIDE.Properties;
using ThreadState = System.Threading.ThreadState;

#endregion

namespace TIDE.Forms
{
    /// <summary>
    ///     The main IDE class for the TIDE
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class TIDE_MainWindow : Form
    {
        /// <summary>
        ///     The documentation window in which the help is shown
        /// </summary>
        private readonly DocumentationWindow _documentationWindow;

        /// <summary>
        ///     The path to save the currently opened document
        /// </summary>
        private string _savePath;

        /// <summary>
        ///     The whole text of the current document
        /// </summary>
        private string _wholeText;

        private bool _newKey;

        /// <summary>
        ///     Initializes a new TIDE
        /// </summary>
        public TIDE_MainWindow()
        {
            _documentationWindow = new DocumentationWindow();

            StopIntelliSenseUpdateThread();

            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Intellisensing = false;
            Unsaved = false;
            SavePath = null;
            _wholeText = "";

            IntelliSensePopUp = new IntelliSensePopUp(new Point(0, 0)) { Visible = false };
            IntelliSensePopUp.ItemEntered += (sender, e) => IntelliSense_ItemSelected((string) sender);
            InitializeComponent();
            Focus();
        }

        private void StopIntelliSenseUpdateThread()
        {
            if (_intelliSenseUpdateThread != null && _intelliSenseUpdateThread.IsAlive)
                _intelliSenseUpdateThread.Abort();

            while (_intelliSenseUpdateThread != null && _intelliSenseUpdateThread?.IsAlive == true &&
                   ((_intelliSenseUpdateThread?.ThreadState & ThreadState.AbortRequested) == ThreadState.AbortRequested ||
                    (_intelliSenseUpdateThread?.ThreadState & ThreadState.Unstarted) != ThreadState.Unstarted))
            {
            }

            _intelliSenseUpdateThread = new Thread(() =>
            {
                IntelliSensePopUp.UpdateList(GetUpdatedItems());
            })
            {
                Name = "UpdateIntelliSenseThread",
                Priority = ThreadPriority.Lowest,
                IsBackground = true,
            };
        }

        /// <summary>
        ///     The current IntelliSensePopUp
        /// </summary>
        private IntelliSensePopUp IntelliSensePopUp { get; }

        /// <summary>
        ///     Indicates wether the user didn't save the latest changes
        /// </summary>
        private bool Unsaved { get; set; }

        /// <summary>
        ///     Indicates wether the text is changing because of intelliSense actions
        /// </summary>
        private bool Intellisensing { get; set; }

        private Thread _intelliSenseUpdateThread;

        /// <summary>
        ///     The path to save the currently opened document
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
        ///     Saves the current dialogue (if necessary or wanted with dialogue)
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
        ///     Compiles the current document
        /// </summary>
        private async Task<string> Compile() => await Task.Run(() =>
        {
            Main.Initialize(SavePath, "out.asm", "error.txt");
            var ex = Main.CompileFile();
            var error = File.ReadAllText("error.txt");
            var output = File.ReadAllText("out.asm");
            if (ex != null)
            {
                if (ex.Line >= 0)
                    editor.HighlightLine(ex.Line, Color.Red);
                MessageBox.Show(error, Resources.Error);
                if (ex.Line >= 0)
                    editor.HighlightLine(ex.Line, editor.BackColor);
                return "";
            }

            Invoke(new Action(() =>
            {
                tabControl.SelectTab(assemblerPage);
                assemblerTextBox.Text = output;
                assemblerTextBox.ColorAll(true);
            }));
            return output;
        });

        /// <summary>
        ///     Opens a new document - always opens a new dialogue
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
            editor.ColorAll();
            _wholeText = new string(editor.Text.ToCharArray());
            editor.TextChanged += editor_TextChanged;
        }

        /// <summary>
        ///     Evaluates all the variable names existing in the document
        /// </summary>
        /// <returns>A list of the variable names</returns>
        private IEnumerable<string> GetVariableNames()
        {
            var fin = new List<string>(GlobalProperties.StandardVariables.Select(variable => variable.Name));
            VariableType foo;
            var lines = (string[]) editor.Invoke(new Func<string[]>(() => editor.Lines));
            fin.AddRange(
                lines.Where(
                        s => (s.Trim(' ').Split().Length > 1) && Enum.TryParse(s.Trim(' ').Split()[0], true, out foo))
                    .Select(s => s.Trim(' ').Split()[1]));
            return fin;
        }

        /// <summary>
        ///     Evaluates the method names existing in the document
        /// </summary>
        /// <returns>An IEnumerable of the method names</returns>
        private IEnumerable<string> GetMethodNames()
        {
            var lines = (string[]) editor.Invoke(new Func<string[]>(() => editor.Lines));
            return new List<string>(
                lines.Where(
                        s => (s.Trim(' ').Split().Length > 1) && (s.Trim(' ').Split().First().Trim(' ') == "method"))
                    .Select(s => s.Trim(' ').Split()[1].Trim(' ', '[')));
        }

        #region IntelliSense

        /// <summary>
        ///     Evaluates the position of the IntelliSense window
        /// </summary>
        /// <returns>The position as a point</returns>
        private Point GetIntelliSensePosition()
        {
            if (editor.InvokeRequired)
                return (Point) editor.Invoke(new Func<Point>(GetIntelliSensePosition));
            var pos = editor.PointToScreen(editor.GetPositionFromCharIndex(editor.SelectionStart));
            return new Point(pos.X, pos.Y + Cursor.Size.Height);
        }

        /// <summary>
        ///     Shows the IntelliSense window
        /// </summary>
        private void ShowIntelliSense()
        {
            IntelliSensePopUp.Visible = true;
            IntelliSensePopUp.SelectIndex(0);
            UpdateIntelliSense();
            Focus();
        }

        /// <summary>
        ///     Hides the IntelliSense window
        /// </summary>
        private void HideIntelliSense() => IntelliSensePopUp.Visible = false;

        ///// <summary>
        /////     Updates the IntelliSense items
        ///// </summary>
        //private void UpdatIntelliSense()
        //{
        //    var t = 
        //    t.Start();
        //}

        /// <summary>
        ///     Evaluates the updated items for the IntelliSense window
        /// </summary>
        /// <returns>A list of the updated items</returns>
        private List<string> GetUpdatedItems()
        {
            var vars = GetVariableNames().ToArray();
            var methods = GetMethodNames().ToArray();
            var general =
                (string[])
                editor.Invoke(
                    new Func<string[]>(
                        () => PublicStuff.StringColorsTCode.Select(color => color.Thestring).ToArray()));

            editor.Invoke(new Action(() =>
            {
                Array.Sort(vars);
                Array.Sort(methods);
                Array.Sort(general);
            }));

            var character =
                (char?)
                editor.Invoke(
                    new Func<char?>(() => GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor)?.Value));
            var word =
                (string)
                editor.Invoke(new Func<string>(() => GetCurrent.GetCurrentWord(editor.SelectionStart, editor)?.Value));

            var fin = general.Concat(vars).Concat(methods)
                .Where(s =>
                {
                    var current =
                        PublicStuff.Splitters.Any(
                            c =>
                                    c == character)
                            ? ""
                            : word;
                    return string.IsNullOrEmpty(current) ||
                           s.StartsWith(current, true, CultureInfo.InvariantCulture);
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
        ///     Gets fired when the ColorAllButton got clicked and colors the whole document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void colorAllButton_Click(object sender, EventArgs e) => editor.ColorAll();

        /// <summary>
        ///     Gets fired when the help button got clicked and prompts some help
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void HelpButton_Click(object sender, EventArgs e)
        {
            _documentationWindow.ShowDialog();
            //_documentationWindow = new DocumentationWindow();
        }

        /// <summary>
        ///     Gets fired when the new button got pressed and creates a new document
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
        ///     Gets fired when the Save as Button got pressed and prompts a new save window
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void SaveAsButton_Click(object sender, EventArgs e) => Save(true);

        /// <summary>
        ///     Gets fired when the Run button got pressed and compiles the current document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private async void RunButton_Click(object sender, EventArgs e)
        {
            SaveButton.PerformClick();
            if (SavePath == null)
            {
                MessageBox.Show(Resources.You_have_to_save_first, Resources.Error);
                return;
            }

            var processName = "8051SimulatorAsm.jar";

            var compiled = await Compile();
            if (string.IsNullOrEmpty(compiled))
                return;
            Clipboard.SetText(compiled);
            if (!File.Exists(processName))
            {
                MessageBox.Show(Resources.LostTheSimulatorFileInfoText, Resources.Error);
                return;
            }
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(processName),
            };
            process.Start();
        }

        /// <summary>
        ///     Gets fired when the Save button got pressed and saves the current document
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void SaveButton_Click(object sender, EventArgs e) => Save(false);

        /// <summary>
        ///     Gets fired when the Open button is pressed and opens a new document
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

        private async void ParseToAssemblerButton_Click(object sender, EventArgs e)
        {
            SaveButton.PerformClick();
            if (SavePath == null)
            {
                MessageBox.Show(Resources.You_have_to_save_first, Resources.Error);
                return;
            }
            await Compile();
        }

        #endregion

        /// <summary>
        ///     Gets fired when an item from intelliSense is selected and inserts the selected item
        /// </summary>
        /// <param name="item">The selected item</param>
        private void IntelliSense_ItemSelected(string item)
        {
            HideIntelliSense();
            Intellisensing = true;
            var res = GetCurrent.GetCurrentWord(editor.SelectionStart, editor)?.Value;
            var s = item.Substring(item.Length >= (res?.Length ?? 0) ? res?.Length ?? 0 : 0) + " ";
            Focus();
            SendKeys.Send(s); //Because this is hilarious;
        }

        /// <summary>
        ///     Gets fired when the TextBox changed
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void editor_TextChanged(object sender = null, EventArgs e = null)
        {
            _newKey = false;
            if (editor.Text.Length - _wholeText.Length == 0)
                return;
            if (editor.Text.Length - _wholeText.Length > 1)
            {
                editor.ColorAll();
                editor_FontChanged();
            }
            else if (StringFunctions.GetRemoved(_wholeText, editor.Text).Contains(';') && (editor.Text.Length > 0))
                editor.ColorCurrentLine();
            else
            {
                var cChar = GetCurrent.GetCurrentCharacter(editor.SelectionStart, editor);
                if (!string.IsNullOrEmpty(cChar?.Value.ToString()) && (cChar.Value == ';'))
                    editor.ColorCurrentLine();
                else
                {
                    editor.BeginUpdate();
                    var word = (Word) editor.Invoke(new Func<Word>(() => GetCurrent.GetCurrentWord(editor.SelectionStart, editor)));
                    Coloring.Coloring.WordActions(word, editor);
                    Coloring.Coloring.CharActions(cChar, editor);
                    editor.EndUpdate();
                }
            }
            Unsaved = true;
            _wholeText = new string(editor.Text.ToCharArray());
            if (Intellisensing)
            {
                IntelliSensePopUp.Disselect();
                Intellisensing = false;
            }

            if (_newKey)
                return;
            UpdateIntelliSense();
        }

        private void UpdateIntelliSense()
        {
            StopIntelliSenseUpdateThread();
            try
            {
                _intelliSenseUpdateThread?.Start();
            }
            catch (Exception)
            {
                try
                {
                    _intelliSenseUpdateThread?.Start();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            Editor_SelectionChanged(null, null);
        }

        /// <summary>
        ///     Gets fired when the TIDE has loaded
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
                new MenuItem("Copy", CopyCm),
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
        ///     Gets fired when the cursor position has changed
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="eventArgs">Useless</param>
        private async void Editor_SelectionChanged(object sender, EventArgs eventArgs)
            => await Task.Run(() =>
            {
                if (!IntelliSensePopUp.Visible)
                    return;
                editor.Invoke(new Action(() =>
                {
                    PositionLabel.Text = string.Format(Resources.Line_Column,
                        editor.GetLineFromCharIndex(editor.SelectionStart),
                        editor.SelectionStart - editor.GetFirstCharIndexOfCurrentLine());
                    IntelliSensePopUp.Location = GetIntelliSensePosition();
                }));
            });

        /// <summary>
        ///     Gets fired when the TIDE is closing and eventually prompts the user for saving
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
        ///     Gets fired before the user has pressed a key. Is there to prevent tab from doing something else
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Provides information about the pressed key</param>
        private void editor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            _newKey = true;
            if (e.KeyCode == Keys.Tab)
                e.IsInputKey = true;
        }

        /// <summary>
        ///     Gets fired when the user has pressed a key.
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Provides information about the pressed key</param>
        private void TIDE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
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
                    case Keys.F5:
                        ParseToAssemblerButton.PerformClick();
                        break;
                    default:
                        return;
                }
            else
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        RunButton.PerformClick();
                        break;
                    case Keys.Escape:
                        HideIntelliSense();
                        break;
                    case Keys.Tab:
                        if (!IntelliSensePopUp.Visible)
                        {
                            RemoveSpaces();
                            editor.BeginUpdate();
                            SendKeys.Send(new string(' ', 4));
                            editor.EndUpdate();
                            break;
                        }
                        IntelliSense_ItemSelected(IntelliSensePopUp.GetSelected());
                        break;
                    case Keys.Enter:
                        if (!IntelliSensePopUp.Visible)
                        {
                            RemoveSpaces();
                            var lineIndex = editor.GetLineFromCharIndex(editor.SelectionStart);
                            var line = editor.Lines.Length > lineIndex ? editor.Lines[lineIndex] : null;
                            if (line == null)
                                return;
                            editor.BeginUpdate();
                            SendKeys.Send(new string(' ', line.TakeWhile(c => c == ' ').Count()));
                            editor.EndUpdate();
                            return;
                        }
                        IntelliSense_ItemSelected(IntelliSensePopUp.GetSelected());
                        break;
                    case Keys.Down:
                        if (!IntelliSensePopUp.Visible || e.Shift)
                            return;
                        IntelliSensePopUp.ScrollDown();
                        break;
                    case Keys.Up:
                        if (!IntelliSensePopUp.Visible || e.Shift)
                            return;
                        IntelliSensePopUp.ScrollUp();
                        break;
                    case Keys.Space:
                        RemoveSpaces();
                        return;
                    default:
                        return;
                }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void RemoveSpaces()
        {
            var word = GetCurrent.GetCurrentWord(editor.SelectionStart, editor);
            var beginningIndex = editor.GetFirstCharIndexOfCurrentLine();
            if (
                PublicStuff.EndCommands.All(
                    s => !string.Equals(s, word.Value, StringComparison.CurrentCultureIgnoreCase)) ||
                !editor.Text.Substring(beginningIndex).StartsWith(new string(' ', 4)))
                return;
            editor.BeginUpdate();
            var os = editor.SelectionStart;
            editor.Select(beginningIndex, 4);
            editor.SelectedText = "";
            editor.SelectionStart = os - 4;
            editor.EndUpdate();
        }

        /// <summary>
        ///     Makes sure that the font can't get changed.
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void editor_FontChanged(object sender = null, EventArgs e = null)
        {
            editor.BeginUpdate();
            var oldSelection = editor.SelectionStart;
            editor.SelectAll();
            editor.SelectionFont = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            editor.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            editor.Select(oldSelection, 0);
            editor.EndUpdate();
        }

        /// <summary>
        ///     Same as TIDE_KeyDown
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Information about the key</param>
        private void editor_KeyDown(object sender, KeyEventArgs e)
        {
            //TIDE_KeyDown(sender, e);
        }

        /// <summary>
        ///     Same as TIDE_KeyDown
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Information about the key</param>
        private void ToolBar_KeyDown(object sender, KeyEventArgs e)
        {
            //TIDE_KeyDown(sender, e);
        }

        /// <summary>
        ///     Same as TIDE_KeyDown
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Information about the key</param>
        private void tabControl_KeyDown(object sender, KeyEventArgs e) => TIDE_KeyDown(sender, e);

        /// <summary>
        ///     Gets fired when the window has resized, because the IntelliSense window has to be moved.
        /// </summary>
        /// <param name="sender">Useless</param>
        /// <param name="e">Useless</param>
        private void TIDE_ResizeEnd(object sender, EventArgs e)
            => IntelliSensePopUp.Location = GetIntelliSensePosition();

        #endregion
    }
}