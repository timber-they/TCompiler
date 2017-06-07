using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TCompiler.Enums;
using TCompiler.Settings;
using TIDE.Coloring.StringFunctions;
using TIDE.Forms;

namespace TIDE.IntelliSense
{
    public class IntelliSenseManager
    {
        private Thread _intelliSenseUpdateThread;
        private readonly TIDE_MainWindow _mainWindow;

        public IntelliSenseManager(TIDE_MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        ///     Updates the intelliSense popup
        /// </summary>
        public void UpdateIntelliSense()
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
            _mainWindow.Editor_SelectionChanged(null, null);
        }

        /// <summary>
        ///     Aborts the intelliSenseUpdate thread and tries to recreate it
        /// </summary>
        public void StopIntelliSenseUpdateThread()
        {
            if (_intelliSenseUpdateThread != null && _intelliSenseUpdateThread.IsAlive)
                _intelliSenseUpdateThread.Abort();

            while (_intelliSenseUpdateThread != null && _intelliSenseUpdateThread?.IsAlive == true &&
                   ((_intelliSenseUpdateThread?.ThreadState & ThreadState.AbortRequested) ==
                    ThreadState.AbortRequested ||
                    (_intelliSenseUpdateThread?.ThreadState & ThreadState.Unstarted) != ThreadState.Unstarted))
            {
            }

            _intelliSenseUpdateThread = new Thread(() =>
            {
                _mainWindow.IntelliSensePopUp.UpdateList(GetUpdatedItems());
            })
            {
                Name = "UpdateIntelliSenseThread",
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
        }

        /// <summary>
        ///     Evaluates the updated items for the IntelliSense window
        /// </summary>
        /// <returns>A list of the updated items</returns>
        private List<string> GetUpdatedItems()
        {
            var line = (int)_mainWindow.Editor.Invoke(new Func<int>(() =>
               _mainWindow.Editor.GetLineFromCharIndex(_mainWindow.Editor.SelectionStart)));
            var vars = GetVariables()
                .Where(variable => variable.VisibilityRangeLines.Item1 <= line &&
                                   variable.VisibilityRangeLines.Item2 >= line).Select(variable => variable.Name)
                .ToArray();

            var methods = GetMethodNames().ToArray();
            var general =
                (string[])
                _mainWindow.Editor.Invoke(
                    new Func<string[]>(
                        () => PublicStuff.StringColorsTCode.Select(color =>
                            color.Thestring).ToArray()));

            _mainWindow.Editor.Invoke(new Action(() =>
            {
                Array.Sort(vars);
                Array.Sort(methods);
                Array.Sort(general);
            }));

            var character =
                (char?)
                _mainWindow.Editor.Invoke(
                    new Func<char?>(() =>
                        GetCurrent.GetCurrentCharacter(_mainWindow.Editor.SelectionStart,
                            _mainWindow.Editor)?.Value));
            var word =
                (string)
                _mainWindow.Editor.Invoke(new Func<string>(() =>
                    GetCurrent.GetCurrentWord(_mainWindow.Editor.SelectionStart,
                        _mainWindow.Editor)?.Value));

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

        /// <summary>
        ///     Evaluates all the variables existing in the document
        /// </summary>
        /// <returns>A list of the variables, containing the visibility range</returns>
        private IEnumerable<Variable> GetVariables()
        {
            var internalText = (string[])_mainWindow.Editor.Invoke(new Func<string[]>(() =>
               _mainWindow.Editor.Lines.Select(s => s.Split(';').FirstOrDefault()).ToArray()));

            var fin = new List<Variable>(
                GlobalProperties.StandardVariables.Select(
                    variable => new Variable(variable.Name, (0, internalText.Length - 1), 0)));

            var currentBlockVariables = new List<Variable>();
            var currentLayer = 0;

            var isBeginningBlockRegex =
                new Regex($"{string.Join("|", PublicStuff.BeginningCommands.Select(s => $"\\b{s}\\b"))}");
            var isEndingBlockRegex =
                new Regex($"{string.Join("|", PublicStuff.EndCommands.Select(s => $"\\b{s}\\b"))}");
            var getVariableNameRegex = new Regex(
                $"({string.Join("|", Enum.GetNames(typeof(VariableType)).Select(s => $"\\b{s} "))})(\\w+)",
                RegexOptions.IgnoreCase);

            for (var line = 0; line < internalText.Length; line++)
            {
                if (isBeginningBlockRegex.IsMatch(internalText[line]))
                    currentLayer++;
                else if (isEndingBlockRegex.IsMatch(internalText[line]))
                {
                    currentLayer--;
                    foreach (var blockVariable in new List<Variable>(currentBlockVariables))
                    {
                        if (blockVariable.Layer <= currentLayer)
                            continue;
                        currentBlockVariables.Remove(blockVariable);
                        blockVariable.VisibilityRangeLines = (blockVariable.VisibilityRangeLines.Item1, line - 1);
                        fin.Add(blockVariable);
                    }
                }
                else
                {
                    var match = getVariableNameRegex.Match(internalText[line]);
                    if (match.Success)
                        currentBlockVariables.Add(new Variable(match.Groups.Cast<Group>().Last().Value, line,
                            currentLayer));
                }
            }


            var externalText = _mainWindow.ExternalFiles.Select(content => content.Content.Split('\n')).SelectMany(s => s)
                .ToList();

            foreach (var line in externalText)
            {
                if (isBeginningBlockRegex.IsMatch(line))
                    currentLayer++;
                else if (isEndingBlockRegex.IsMatch(line))
                    currentLayer--;
                else if (currentLayer == 0)
                {
                    var match = getVariableNameRegex.Match(line);
                    if (match.Success)
                        fin.Add(new Variable(match.Groups.Cast<Group>().Last().Value,
                            (0, internalText.Length - 1),
                            0));
                }
            }
            return fin;
        }

        /// <summary>
        ///     Evaluates the method names existing in the document
        /// </summary>
        /// <returns>An IEnumerable of the method names</returns>
        private IEnumerable<string> GetMethodNames()
        {
            var lines = ((string[])_mainWindow.Editor.Invoke(new Func<string[]>(() =>
            _mainWindow.Editor.Lines))).ToList();

            foreach (var file in _mainWindow.ExternalFiles)
                lines.AddRange(file.Content.Split('\n'));

            return new List<string>(
                lines.Where(
                        s => s.Trim(' ').Split().Length > 1 && s.Trim(' ').Split().First().Trim(' ') == "method")
                    .Select(s => s.Trim(' ').Split()[1].Trim(' ', '[')));
        }

        /// <summary>
        ///     Evaluates the position of the IntelliSense window
        /// </summary>
        /// <returns>The position as a point</returns>
        public Point GetIntelliSensePosition()
        {
            if (_mainWindow.Editor.InvokeRequired)
                return (Point)_mainWindow.Editor.Invoke(new Func<Point>(GetIntelliSensePosition));
            var pos = _mainWindow.Editor.PointToScreen(
                _mainWindow.Editor.GetPositionFromCharIndex(_mainWindow.Editor.SelectionStart));
            return new Point(pos.X, pos.Y + _mainWindow.Cursor.Size.Height);
        }

        /// <summary>
        ///     Shows the IntelliSense window
        /// </summary>
        public void ShowIntelliSense()
        {
            _mainWindow.IntelliSensePopUp.Visible = true;
            _mainWindow.IntelliSenseCancelled = false;
            _mainWindow.IntelliSensePopUp.SelectIndex(0);
            UpdateIntelliSense();
            _mainWindow.Focus();
        }

        /// <summary>
        ///     Hides the IntelliSense window
        /// </summary>
        public void HideIntelliSense() => _mainWindow.IntelliSensePopUp.Visible = false;
    }
}