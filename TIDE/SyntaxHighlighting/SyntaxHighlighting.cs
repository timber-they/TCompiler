using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using TIDE.Types;

namespace TIDE.SyntaxHighlighting
{
    public static class SyntaxHighlighting
    {
        public static void ColourRanges(List<Range> ranges, RichTextBox rtb)
        {
            var f = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            f.ClearAllProperties();

            var n = rtb.Document.ContentStart;

            ranges = ranges.Where(range => range.End+3 < f.Text.Length).ToList();

            var tRanges = ranges.Select(
                range =>
                    new TextRange(n.GetPositionAtOffset(range.Start + 2, LogicalDirection.Forward),
                        n.GetPositionAtOffset(range.End + 3, LogicalDirection.Backward))).ToList();

            for (var i = 0; i < tRanges.Count; i++)
                tRanges[i].ApplyPropertyValue(TextElement.ForegroundProperty, ranges[i].Color);
        }
    }
}