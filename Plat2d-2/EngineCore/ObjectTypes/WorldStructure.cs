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
        public List<Area> Areas { get; set; }
        public List<Level> Levels { get; set; } //used for utility levels
        //an array of area numbers to compose a "clear map"
        //an array of area numbers player needs to clear to consider world complete - can be used to exclude bonus levels.
        public List<int> ClearAreas { get; set; }
        public List<bool> AreAreasClear { get; set; }

        //worldname
        public string WorldName { get; set; }
        //worldorderinteger
        public int WorldOrderInteger { get; set; }
        //world map - single level item, will be rendered using a non-platformer renderer, can be a singular platform level for now.
        public Level WorldMap { get; set; }
        public bool isWorldClear { get; set; } = false;

        //NOTE, DUE TO SIMILARITIES TO AREA, IT MIGHT BE USEFUL TO USE AN INHERITANCE INSTEAD OF A BASE WA-TYPE
        public WorldStructure(
            List<Area> areas, 
            List<int> clearAreas, 
            List<bool> areAreasClear,
            string worldName, 
            int worldOrderInteger,
            Level worldMap
            )
        {
            Areas = areas;
            ClearAreas = clearAreas;
            AreAreasClear = areAreasClear;
            WorldName = worldName;
            WorldOrderInteger = worldOrderInteger;
            WorldMap = worldMap;
        }
        public WorldStructure(
            List<Level> levels, 
            string worldName, 
            int worldOrderInteger
            )
        {
            Levels = levels;
            WorldName = worldName;
            WorldOrderInteger = worldOrderInteger;
        }

        /// <summary>
        /// Returns world 1 data
        /// </summary>
        /// <returns>Harenimus data as a WorldStructure </returns>
        public static WorldStructure GetHarenimus()
        {
            WorldStructure harenimus = new WorldStructure(

                new List<Area> 
                { 
                    LevelController.GetAreaW1_1(),
                    LevelController.GetAreaW1_2(),
                    LevelController.GetAreaW1_3(),
                    LevelController.GetAreaW1_4(),
                    LevelController.GetAreaW1_5(),
                    LevelController.GetAreaW1_6(),
                },
                new List<int> { 1, 2, 3, 4, 5, 6 },
                new List<bool> { false, false, false, false, false, false },
                "The Kingdom of Harenimus",
                1,
                LevelController.GetWM_1()
            );

            return harenimus;
        }
        /// <summary>
        /// Returns games utility screens that are levels
        /// </summary>
        /// <returns>Utility data as a WorldStructure </returns>
        public static WorldStructure GetScreens()
        {
            WorldStructure screens = new WorldStructure(

                new List<Area>
                {
                    new Area
                    {
                        Levels = new List<Level>{ LevelController.GetTitleScreen() },
                        AreaNumber = 1,
                        AreaName = "Title Screen"
                    },
                    new Area
                    {
                        Levels = new List<Level>{ LevelController.GetTitleScreen() },
                        AreaNumber = 2,
                        AreaName = "Game Over"
                    },
                },
                null,
                null,
                "Utility",
                0,
                null
            );
            screens.isWorldClear = true;

            return screens;
        }
    }

}
