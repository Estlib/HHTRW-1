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
        Shape2d player;
        public DemoGame() : base(new Vector2(615, 615),"HHTRW-engine1 demo")
        {

        }




        public override void OnLoad()
        {
            Console.WriteLine("OnLoad works.");
            BGColor = Color.Black;
            player = new Shape2d(new Vector2(8, 8), new Vector2(32, 32), "Test");
        }
        public override void OnDraw()
        {

        }
        int timeframe = 0;
        public override void OnUpdate()
        {
            if (timeframe>400)
            {
                if (player != null)
                {
                    player.DestroySelf();
                    player = null;
                }
            }
            timeframe++;
            //Console.WriteLine($"Framecount: {frame}.");
            //frame++;
        }
    }
}
