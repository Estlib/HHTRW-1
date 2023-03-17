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

        private static List<Shape2d> AllShapes = new List<Shape2d>();

        public Color BGColor = Color.Green;

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

        public static void RegisterShape(Shape2d shape)
        {
            AllShapes.Add(shape);
        }
        public static void UnRegisterShape(Shape2d shape)
        {
            AllShapes.Remove(shape);
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
            g.Clear(BGColor);
            //GameLoopThread.Abort();
            foreach (Shape2d shape in AllShapes)
            {
                g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X,shape.Position.Y, shape.Scale.X, shape.Scale.Y);
            }
        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
    }
}
