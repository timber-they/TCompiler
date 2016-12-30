using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TIDE.Forms
{
    public partial class IntelliSensePopUp : Form
    {
        public IntelliSensePopUp(IEnumerable<string> items, Point pos)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = pos;
            Items.Items.AddRange(items.Select(s => s as object).ToArray());
        }
    }
}
