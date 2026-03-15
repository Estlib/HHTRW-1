using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public class Area
    {
        //the data an area has:

        //list of existing level-type items
        public static List<Level> Levels { get; set; }

        //an array of area numbers to compose a "clear map"
        //an array of area numbers player needs to clear to consider world complete - can be used to exclude bonus levels.
        public static List<int> ClearMap = GetIDsFromLevels(Levels);
        //areanumber
        public static int AreaNumber { get; set; }
        //areaname
        public static string AreaName { get; set; }
        //areamap
        /*same as clearmap*/
        public static bool isAreaClear { get; set; }
        public static bool isRequiredToBeCleared { get; set; }
        public static bool canPassOnMap { get; set; } = false;




        //NOTE, DUE TO SIMILARITIES TO WORLD, IT MIGHT BE USEFUL TO USE AN INHERITANCE INSTEAD OF A BASE WA-TYPE
        public Area(List<Level> levels, /*List<int> clearMap,*/ int areaNumber, string areaName)
        {
            Levels = levels;
            AreaNumber = areaNumber;
            AreaName = areaName;
            //ClearMap = clearMap;
        }
        private static List<int> GetIDsFromLevels(List<Level> levels)
        {
            List<int> ids = new List<int>();
            foreach (var level in levels)
            {
                if (level.isClearingRequired == true)
                {
                    ids.Add(levels.IndexOf(level));
                }
            }
            return ids;
        }

    }
}
