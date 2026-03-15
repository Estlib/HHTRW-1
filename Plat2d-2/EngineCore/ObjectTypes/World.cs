using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public class World
    {
        //this is the data for a single world, it needs:

        //List of existing List-area-type items
        public List<Area> Areas { get; set; }
        //an array of area numbers to compose a "clear map"
        //an array of area numbers player needs to clear to consider world complete - can be used to exclude bonus levels.
        public List<int> ClearAreas { get; set; }

        //worldname
        public string WorldName { get; set; }
        //worldorderinteger
        public int WorldOrderInteger { get; set; }
        //world map - single level item, will be rendered using a non-platformer renderer, can be a singular platform level for now.
        public Level WorldMap { get; set; }

        //NOTE, DUE TO SIMILARITIES TO AREA, IT MIGHT BE USEFUL TO USE AN INHERITANCE INSTEAD OF A BASE WA-TYPE
        public World(
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
    }

}
