using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class SFXEngine : SoundPlayer
    {
        public static SoundPlayer Coin;
        public bool isCoinPlaying { get; set; }

        public static SoundPlayer Bullet;
        public bool isBulletPlaying { get; set; }


    }


}
