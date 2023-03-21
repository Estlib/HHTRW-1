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
        private Vector2 ScreenSize = new Vector2(320, 240);
        private string Title = "HHTRW-engine1";
        private Canvas Window = null;
        private Thread GameLoopThread = null;

        public static List<Shape2d> AllShapes = new List<Shape2d>();
        public static List<Sprite2d> AllSprites = new List<Sprite2d>();

        public Color BGColor = Color.Green;

        public Vector2 CameraZoom = new Vector2(1,1);
        public Vector2 CameraPosition = Vector2.Zero();
        public float CameraAngle = 0f;
        public EngineCore(Vector2 ScreenSize, string Title)
        {
            Log.Info("Game is starting");
            this.ScreenSize = ScreenSize;
            this.Title = Title;

            Window = new Canvas();
            Window.Size = new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y);
            Window.Text = this.Title;
            Window.Paint += Renderer;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp+= Window_KeyUp;
            Window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Window.FormClosing += Window_FormClosing;
            GameLoopThread = new Thread(GameLoop);
            GameLoopThread.Start();
            Application.Run(Window);
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameLoopThread.Abort();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            GetKeyUp(e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            GetKeyDown(e);
        }

        public static void RegisterShape(Shape2d shape)
        {
            AllShapes.Add(shape);
        }
        public static void UnRegisterShape(Shape2d shape)
        {
            AllShapes.Remove(shape);
        }
        public static void RegisterSprite(Sprite2d sprite)
        {
            AllSprites.Add(sprite);
        }
        public static void UnRegisterSprite(Sprite2d sprite)
        {
            AllSprites.Remove(sprite);
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
                    Thread.Sleep(2);
                }
                catch (Exception)
                {
                    Log.Error("Game has not been found.");
                }
            }
        }

        private void Renderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BGColor);
            //GameLoopThread.Abort();
            g.TranslateTransform(CameraPosition.X, CameraPosition.Y);
            g.RotateTransform(CameraAngle);
            g.ScaleTransform(CameraZoom.X, CameraZoom.Y);
            try
            {
                foreach (Shape2d shape in AllShapes)
                {
                    g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
                }
                foreach (Sprite2d sprite in AllSprites)
                {
                    if (!sprite.IsReference)
                    {
                        g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);
    }
}
