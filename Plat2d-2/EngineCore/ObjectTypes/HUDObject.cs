using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    public enum HudElementType
    {
        Digit, Bar, Icon, Name
    }
    public enum BarElementFillAmount
    {
       Full, Half, None
    }
    public class HUDObject
    {
        //shared properties
        public Sprite2d Display { get; set; }
        public List<Bitmap> DisplayElements { get; set; }

        //digit item properties
        public HudElementType ElementDigitType { get; set; } = HudElementType.Digit;
        public int DisplayedDataInt { get; set; } = 0;
        /*
         * Legend:
         * Digit - 0,1,2,3,4,5,6,7,8,9 all numbers
         */

        //bar item properties
        public HudElementType ElementBarType { get; set; } = HudElementType.Bar;
        public BarElementFillAmount ElementFillAmount { get; set; } = BarElementFillAmount.None;

        public HUDObject(Sprite2d currentSprite, List<Bitmap> allElementSprites, int displayThisInt)
        {
            Display = currentSprite;
            DisplayElements = allElementSprites;
            DisplayedDataInt = displayThisInt;
        }
    }
}
