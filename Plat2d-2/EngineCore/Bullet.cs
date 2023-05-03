using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class Bullet
    {
        public Sprite2d sprite2d;
        public bool isfacingleft;

        public Bullet(Sprite2d sprite2d, bool isfacingleft)
        {
            this.sprite2d = sprite2d;
            this.isfacingleft = isfacingleft;
        }
    }
}
