using Plat2d_2.EngineCore.ObjectControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public class WorldStructure
    {
        //this is the data for a single world, it needs:

        //List of existing List-area-type items
        public static List<Area> Areas { get; set; }
        //an array of area numbers to compose a "clear map"
        //an array of area numbers player needs to clear to consider world complete - can be used to exclude bonus levels.
        public static List<int> ClearAreas { get; set; }

        //worldname
        public static string WorldName { get; set; }
        //worldorderinteger
        public static int WorldOrderInteger { get; set; }
        //world map - single level item, will be rendered using a non-platformer renderer, can be a singular platform level for now.
        public static Level WorldMap { get; set; }
        public static bool isWorldClear { get; set; } = false;

        //NOTE, DUE TO SIMILARITIES TO AREA, IT MIGHT BE USEFUL TO USE AN INHERITANCE INSTEAD OF A BASE WA-TYPE
        public WorldStructure(
            List<Area> areas, 
            List<int> clearAreas, 
            string worldName, 
            int worldOrderInteger,
            Level worldMap
            )
        {
            Areas = areas;
            ClearAreas = clearAreas;
            WorldName = worldName;
            WorldOrderInteger = worldOrderInteger;
            WorldMap = worldMap;
        }

        /// <summary>
        /// Returns world 1 data
        /// </summary>
        /// <returns>Harenimus data as a WorldStructure </returns>
        public static WorldStructure GetHarenimus()
        {
            WorldStructure harenimus = new WorldStructure(

                Areas = new List<Area> 
                { 
                    LevelController.GetAreaW1_1(),
                    LevelController.GetAreaW1_2(),
                    LevelController.GetAreaW1_3(),
                    LevelController.GetAreaW1_4(),
                    LevelController.GetAreaW1_5(),
                    LevelController.GetAreaW1_6(),
                },
                ClearAreas = new List<int> { 1, 2, 3, 4, 5, 6 },
                WorldName = "The Kingdom of Harenimus",
                WorldOrderInteger = 1,
                WorldMap = LevelController.GetWM_1()
            );

            return harenimus;
        }
    }

}
