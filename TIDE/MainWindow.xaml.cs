using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TIDE.Types;

namespace TIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox.TextChanged -= TextBox_OnTextChanged;

            SyntaxHighlighting.SyntaxHighlighting.ColourRanges(new List<Range> {new Range(Brushes.Cyan, 1,3), new Range(Brushes.Red, 5,7)}, TextBox );

            TextBox.TextChanged += TextBox_OnTextChanged;
        }
    }
}
