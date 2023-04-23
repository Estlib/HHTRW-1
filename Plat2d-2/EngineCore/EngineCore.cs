using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;
using System.Security.Policy;

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
        public static List<Shape2d>[] LevelShapes = new List<Shape2d>[10];
        public static List<Sprite2d>[] LevelSprites = new List<Sprite2d>[10];
        public static bool pausebuttoninput = false;

        public System.Drawing.Color BGColor = System.Drawing.Color.Green;

        public Vector2 CameraZoom = new Vector2(1,1);
        public Vector2 CameraPosition = Vector2.Zero();
        public float CameraAngle = 0f;

        // Define the size of the world. Simulation will still work
        // if bodies reach the end of the world, but it will be slower.
        AABB worldAABB = new AABB
        {
            UpperBound = new Vec2(2000, 2000),
            LowerBound = new Vec2(-2000, -2000)
        };
        // Define the gravity vector.
        Vec2 gravity = new Vec2(0.0f, 560.0f);
        // Do we want to let bodies sleep?
        //bool doSleep = true;
        // Construct a world object, which will hold and simulate the rigid bodies.
        public static World world = null;

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
            world = new World(worldAABB, gravity, pausebuttoninput);

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
            //AllSprites.Add(sprite);
            LevelSprites[DemoGame.currentLevel].Add(sprite);
        }
        public static void UnRegisterSprite(Sprite2d sprite)
        {
            //AllSprites.Remove(sprite);
            LevelSprites[DemoGame.currentLevel].Remove(sprite);
        }
        public static void RemoveAllSprites()
        {
            foreach (var sprite in LevelSprites[DemoGame.currentLevel])
            {
                if (sprite.HasBody())
                {
                    sprite.DestroyStatic(sprite);
                }
            }
            //AllSprites = new List<Sprite2d>();
            //LevelSprites[DemoGame.currentLevel] = new List<Sprite2d>();
            //for (int i = 0; i < AllSprites.Count; i++)
            //{
            //    Sprite2d sprite = AllSprites[i];
            //    AllSprites.Remove(sprite);
            //}

        }
        // Prepare for simulation. Typically we use a time step of 1/60 of a
        // second (60Hz) and 10 iterations. This provides a high quality simulation
        // in most game scenarios.
        float timeStep = 1.0f / 60.0f;
        //float timeStep = 100000.01f;
        int velocityIterations = 16;
        int positionIterations = 16;
        void GameLoop()
        {
            OnLoad();
            while (GameLoopThread.IsAlive)
            {
                try
                {
                    OnDraw();
                    world.Step(timeStep, velocityIterations, positionIterations);
                    OnUpdate();
                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
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
            // Instruct the world to perform a single step of simulation. It is
            // generally best to keep the time step and iterations fixed.
            Graphics g = e.Graphics;
            g.Clear(BGColor);
            //GameLoopThread.Abort();
            g.TranslateTransform(CameraPosition.X, CameraPosition.Y);
            g.RotateTransform(CameraAngle);
            g.ScaleTransform(CameraZoom.X, CameraZoom.Y);
            //try
            //{
            //foreach (Shape2d shape in AllShapes)
            //{
            //    g.FillRectangle(new SolidBrush(System.Drawing.Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
            //}
            foreach (Sprite2d sprite in LevelSprites[DemoGame.currentLevel])
            {
                    if (!sprite.IsReference)
                    {
                        g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
                    }
                }
            //}
            //catch (Exception)
            //{

            //}

        }
        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);

    }
}
