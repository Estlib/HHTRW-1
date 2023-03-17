using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

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
        private string Title = "HHTRW-engine1";
        private Canvas Window = null;
        private Thread GameLoopThread = null;

        public EngineCore(Vector2 ScreenSize, string Title)
        {
            this.ScreenSize = ScreenSize;
            this.Title = Title;

            Window = new Canvas();
            Window.Size = new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y);
            Window.Text = this.Title;
            Window.Paint += Renderer;
            GameLoopThread = new Thread(GameLoop);
            GameLoopThread.Start();
            Application.Run(Window);
        }
        void GameLoop()
        {
            OnLoad();
            while (GameLoopThread.IsAlive)
            {
                try
                {
                    OnDraw();
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    OnUpdate();
                    Thread.Sleep(1);
                }
                catch (Exception)
                {
                    Console.WriteLine("Game is loading...");
                }
            }
        }

        private void Renderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GameLoopThread.Abort();
        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
    }
}
