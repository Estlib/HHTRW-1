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
        public DemoGame() : base(new Vector2(615, 615),"HHTRW-engine1 demo")
        {

        }




        public override void OnLoad()
        {
            Console.WriteLine("OnLoad works.");
            BGColor = Color.Black;
            Shape2d player = new Shape2d(new Vector2(10, 10), new Vector2(10, 10), "Test");
        }
        int frame = 0;
        public override void OnDraw()
        {

        }
        public override void OnUpdate()
        {
            Console.WriteLine($"Framecount: {frame}.");
            frame++;
        }
    }
}
