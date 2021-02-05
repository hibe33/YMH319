using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RA_LANG
{
    class OUTPUT_PANEL
    {
        static TextBox box = Form1.output_area;
        public static void write(string text)
        {
            box.AppendText("\r\n"+ "> " +text);
        }
    }
}
