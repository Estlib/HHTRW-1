using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Plat2d_2.EngineCore;
using System.Windows.Forms;

namespace Plat2d_2
{
    class DemoGame : EngineCore.EngineCore
    {
        Sprite2d player;
        //Sprite2d player2;

        bool left;
        bool right;
        bool up;
        bool down;

        Vector2 lastPos = Vector2.Zero();

        string[,] Map =
        {
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            {"G",".","G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".","G",".",".",".","C",".","C",".",".",".",".",".",".",".",".",".",".","G" },
            {"G",".","G",".",".",".",".",".",".",".",".",".",".","G",".",".",".","C",".","G" },
            {"G",".","G",".",".",".",".",".",".",".",".",".",".",".","G",".",".",".",".","G" },
            {"G",".",".","G","G","G","G","G","G","G",".",".",".",".",".","G",".",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G","P",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G",".",".",".",".","G","G","G","G","G",".",".",".",".",".",".","G",".",".","G" },
            {"G",".","C",".",".","G",".",".",".",".",".",".",".",".",".",".","G","C",".","G" },
            {"G",".",".",".",".","G",".",".",".",".",".",".","C",".","C",".","G",".",".","G" },
            {"G",".",".",".",".","G",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
            {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
        };
        public DemoGame() : base(new Vector2(615, 615),"HHTRW-engine1 demo")
        {

        }

        public override void OnLoad()
        {
            Console.WriteLine("OnLoad works.");
            BGColor = Color.Black;
            //CameraZoom = new Vector2(.1f,.1f);
            Sprite2d groundRef = new Sprite2d( "tiles/noart/testblock1");
            Sprite2d airRef = new Sprite2d( "tiles/noart/testblock5");
            Sprite2d coinRef = new Sprite2d( "tiles/noart/testobject2");
            //player = new Shape2d(new Vector2(8, 8), new Vector2(32, 32), "Test");
            //player = new Sprite2d(new Vector2(8, 8), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j,i] == "G")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), groundRef, "Ground");
                    }
                    if (Map[j, i] == ".")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), airRef, "Air");
                    }
                    if (Map[j, i] == "C")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), coinRef, "Coin");
                    }
                }
            }
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "P")
                    {
                        player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
                        player.CreateDynamic();
                    }
                }
            }
            //player = new Sprite2d(new Vector2(64, 96), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
            //player2 = new Sprite2d(new Vector2(128, 192), new Vector2(32, 32), "player/wipspriteset/stand1", "Player2");
        }
        public override void OnDraw()
        {

        }
        //int timeframe = 0;
        //float x = 1;
        int times = 0;
        public override void OnUpdate()
        {
            if (player == null)
            {
                return;
            }
            times++;
            player.UpdatePosition();
            if (up)
            {
                //player.Position.Y -= 1;
            }
            if (down)
            {
                //player.Position.Y += 1;
            }
            if (left)
            {
                //player.Position.X -= 1;
            }
            if (right)
            {
                //player.Position.X += 1;
            }
            Sprite2d coin = player.IsColliding("Coin");
            if (coin != null)
            {
                Log.Info("Coin is being touched");
                coin.DestroySelf();
            }
            if (player.IsColliding("Ground") != null)
            {
                //Log.Info($"Collision is happening. {times}");
                //times ++;
                //player.Position.X = lastPos.X;
                //player.Position.Y = lastPos.Y;
            }
            else
            {
                //lastPos.X = player.Position.X;
                //lastPos.Y = player.Position.Y;
            }
            //CameraPosition.X++;
            //CameraAngle += .1f;
            //player.Position.X += x;          
            //if (timeframe>400)
            //{
            //    if (player != null)
            //    {
            //        player.DestroySelf();
            //        player = null;
            //    }
            //}
            //timeframe++;
            //Console.WriteLine($"Framecount: {frame}.");
            //frame++;
        }

        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = true; }
            if (e.KeyCode == Keys.A) { left = true; }
            if (e.KeyCode == Keys.S) { down = true; }
            if (e.KeyCode == Keys.D) { right = true; }
        }

        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = false; }
            if (e.KeyCode == Keys.A) { left = false; }
            if (e.KeyCode == Keys.S) { down = false; }
            if (e.KeyCode == Keys.D) { right = false; }
        }
    }
}
