using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class ArtData
    {
        public ArtData() { }

        /// <summary>
        /// Returns solidity reference tags for artset: TitleMenuMap
        /// </summary>
        /// <returns>A string array with reference tags</returns>
        public static string[] TitleMenuMapRefsTag()
        {
            return new string[] {
                "Air","Finish","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Ground","Ground","Ground","Ground","Ground","Ground",
            "Ground","Ground","Ground","Ground","Ground","Air","Air","Air",
            "Air","Level1","Level2","Level3","Level4","Level5","Level6","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Ground","Ground",
            "Air"};//,"Air","Air","Air","Air","Air","Air","Air" 
        }
    }
}
