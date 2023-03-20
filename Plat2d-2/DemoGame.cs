using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Plat2d_2.EngineCore;

namespace Plat2d_2
{
    class DemoGame : EngineCore.EngineCore
    {
        //Sprite2d player;

        string[,] Map =
        {
            {".",".",".",".",".",".","." },
            {".",".",".",".",".","G","." },
            {".",".",".",".","G","G","." },
            {".",".",".","G","G","G","." },
            {"G","G","G","G","G","G","G" },
            {".",".",".",".",".",".","." },
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
                }
            }
        }
        public override void OnDraw()
        {

        }
        //int timeframe = 0;
        float x = 1;
        public override void OnUpdate()
        {
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
    }
}
