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
        TL,TT,TR,ML,MM,MR,BL,BB,BR
    }
    public class Sensor
    {
        private static int sensorIncrementor = 0;
        public int SensorID = sensorIncrementor++;
        public int? ParentID { get; set; }
        public string ParentObjName { get; set; } = "";
        public CardinalDirection ThisSensorDirection { get; set; }
        public Vector2 CoordsOnSprite { get; set; }
        public bool? IsTouchingTag { get; set; } = false;

        public Sensor()
        {
            bool isParentSet = CheckParent();
            if (!isParentSet)
            {
                return;
            }
        }

        public Sensor(string parentObjName, CardinalDirection direction, Vector2 coords, bool isTouchingTag = false, int parentID = -1) : base()
        {
            //solve parent
            ParentID = parentID;
            if (string.IsNullOrEmpty(parentObjName))
            {
                ParentObjName = ParentID.ToString();
            }
            else
            {
                ParentObjName = parentObjName;
            }

            ThisSensorDirection = direction;
            CoordsOnSprite = coords;
            IsTouchingTag = isTouchingTag;

        }
        private bool CheckParent()
        {
            if (ParentID == null || ParentObjName == "")
            {
                Log.Error("Parent object is not defined, nor its id set. Skipping Sensor creation");
                return false;
            }

            return true;
        }


    }

}
