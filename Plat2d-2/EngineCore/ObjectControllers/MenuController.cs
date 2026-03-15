using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore.ObjectControllers
{
    public class MenuController
    {
        public MenuController() { }

        /// <summary>
        /// Returns all menu object sprites.
        /// </summary>
        /// <returns>A list of sprites for the menu objects. main, and pause </returns>
        public static List<Bitmap> GetMenuObjectSprites()
        {
            Log.Warning("Function does not contain any sprites assigned, not implemented.");
            return new List<Bitmap>();
        }
    }

}
