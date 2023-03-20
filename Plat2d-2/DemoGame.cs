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

        bool left;
        bool right;
        bool up;
        bool down;

        string[,] Map =
        {
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
            {".",".",".",".","G","G",".",".",".",".","G","G",".",".",".",".",".","G","G","." },
            {".",".",".","G","G","G",".",".",".","G","G","G",".",".",".","G","G","G","G","." },
        };
        public DemoGame() : base(new Vector2(615, 615),"HHTRW-engine1 demo")
        {

        }

        public override void OnLoad()
        {
            Console.WriteLine("OnLoad works.");
            BGColor = Color.Black;
            //player = new Shape2d(new Vector2(8, 8), new Vector2(32, 32), "Test");
            //player = new Sprite2d(new Vector2(8, 8), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j,i] == "G")
                    {
                        new Sprite2d(new Vector2(i*16, j*16), new Vector2(16, 16), "tiles/noart/testblock1", "Ground");
                    }
                    if (Map[j, i] == ".")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), "tiles/noart/testblock5", "Air");
                    }
                }
            }
            player = new Sprite2d(new Vector2(8, 8), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
        }
        public override void OnDraw()
        {

        }
        //int timeframe = 0;
        float x = 1;
        public override void OnUpdate()
        {
            if (up)
            {
                player.Position.Y -= 1;
            }
            if (down)
            {
                player.Position.Y += 1;
            }
            if (left)
            {
                player.Position.X -= 1;
            }
            if (right)
            {
                player.Position.X += 1;
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
