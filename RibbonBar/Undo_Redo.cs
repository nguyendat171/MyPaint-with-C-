using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RibbonBar
{
    class Undo_Redo
    {
        public static List<Shape> Shapes = new List<Shape>();
        public static List<int> Type_Shapes = new List<int>();
        public static int n = 0;
    }
}
