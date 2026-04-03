using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectTypes
{
    enum HudElementType
    {
        Digit, Bar, Icon, Name
    }
    enum BarElementFillAmount
    {
       Full, Half, None
    }
    internal class HUDObject
    {
        //shared properties
        public Sprite2d Display { get; set; }
        public List<Bitmap> DisplayElements { get; set; }

        //digit item properties
        public HudElementType ElementDigitType { get; set; } = HudElementType.Digit;
        public int DisplayedDataInt { get; set; }

        //bar item properties
        public HudElementType ElementBarType { get; set; } = HudElementType.Bar;
        public BarElementFillAmount ElementFillAmount { get; set; } = BarElementFillAmount.None;


    }
}
