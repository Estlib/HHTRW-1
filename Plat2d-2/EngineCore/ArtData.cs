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
        public static string[] TitleMenuMapRefsTags()
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

        /// <summary>
        /// Returns solidity reference tags for artset: PlainsArt
        /// </summary>
        /// <returns>A string array with reference tags</returns>
        public static string[] PlainsArtRefsTags()
        {
            return new string[] {
            "Air","Coin","Ground","Ground","Ground","Ground","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Ground","Ground",
            "Finish","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Ground","Ground","Ground"};
        }

        /// <summary>
        /// Returns solidity reference tags for artset: UndergroundArt
        /// </summary>
        /// <returns>A string array with reference tags</returns>
        public static string[] UndergroundArtRefsTags()
        {
            return new string[] {
            "Air","Coin","Ground","Ground","Ground","Ground","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Ground","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Ground",
            "Finish","Air","Air","Air","Ground","Air","Air","Ground",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Ground","Ground","Air"};
        }

        /// <summary>
        /// Returns solidity reference tags for artset: ForestArt
        /// </summary>
        /// <returns>A string array with reference tags</returns>
        public static string[] ForestArtRefsTags()
        {
            return new string[] {
            "Air","Coin","Ground","Ground","Ground","Ground","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Ground","Ground",
            "Finish","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air"};
        }

        /// <summary>
        /// Returns solidity reference tags for artset: DesertArt
        /// </summary>
        /// <returns>A string array with reference tags</returns>
        public static string[] DesertArtRefsTags()
        {
            return new string[] {
            "Air","Coin","Ground","Ground","Ground","Ground","Ground","Air",
            "Air","Ground","Air","Air","Air","Ground","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Ground","Ground",
            "Finish","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air"};
        }

        /// <summary>
        /// Returns solidity reference tags for artset: CastleArt
        /// </summary>
        /// <returns>A string array with reference tags</returns>
        public static string[] CastleArtRefsTags()
        {
            return new string[] {
            "Air","Coin","Ground","Ground","Ground","Ground","Ground","Ground",
            "Ground","Ground","Ground","Ground","Ground","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Ground","Ground",
            "Finish","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air",
            "Air","Air","Air","Air","Air","Air","Air","Air"};
        }
    }
}
