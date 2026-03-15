using Plat2d_2.EngineCore.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectControllers
{
    public class WorldController
    {
        
        public WorldController() { }

        public static World GetHarenimus()
        {
            World harenimus = new World();
            harenimus.WorldName = "The Kingdom of Harenimus";
            harenimus.WorldOrderInteger = 1;
            harenimus.WorldMap = LevelController.GetWM_1();
        }
    }
}
