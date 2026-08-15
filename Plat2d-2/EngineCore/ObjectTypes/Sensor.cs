using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public enum CardinalDirection
    {
        /*
         TL - top left
         TL - top middle
         TL - top right
         TL - middle left
         TL - middle middle
         TL - middle right
         TL - bottom left
         TL - bottom middle
         TL - bottom right
         */
        TL, TT,TR,ML,MM,MR,BL,BB,BR
    }
    public class Sensor
    {
        private static int sensorIncrementor = 0;
        public int sensorID = sensorIncrementor++;
        public int? parentID { get; set; }
        public string parentObjName { get; set; } = "";
        public CardinalDirection ThisSensorDirection { get; set; }
        public Vector2 CoordsOnSprite { get; set; }
        public bool IsTouchingTag { get; set; }

        public Sensor()
        {
            bool isParentSet = CheckParent();
            if (!isParentSet)
            {
                return;
            }
        }

        private bool CheckParent()
        {
            if (parentID == null || parentObjName == "")
            {
                Log.Error("Parent object is not defined, nor its id set. Skipping Sensor creation");
                return false;
            }

            return true;
        }

        public Sensor()
        {
            
        }


    }

}
