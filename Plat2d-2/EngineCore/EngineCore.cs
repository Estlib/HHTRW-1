using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plat2d_2.EngineCore
{
    class Canvas : Form
    {
        public Canvas()
        {
            this.DoubleBuffered = true;
        }
    }
    public abstract class EngineCore
    {
        private Vector2 ScreenSize = new Vector2(512, 512);
        private string Title;
        private Canvas Windows = null;

        public EngineCore(Vector2 ScreenSize, string Title)
        {
            this.ScreenSize = ScreenSize;
            this.Title = Title;


        }
    }
}
