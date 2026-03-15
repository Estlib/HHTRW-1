using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectControllers
{
    public class LevelController
    {
        public LevelController() { }

        internal static Level GetWM_1()
        {
            Level harenimusWorldMap = new Level()
            {
                14,
                "screens",
                ArtData.TitleMenuMapRefsTags(),

            };

        }
    }
}
