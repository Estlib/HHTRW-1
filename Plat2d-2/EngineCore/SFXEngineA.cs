using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class SFXEngineA : SoundPlayer
    {
        public List<SFX> effects;

        public void Main(String[] args)
        {

        }
        public static void SFXRunner(string filepath)
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = filepath;
            player.Play();

        }
    }
}
