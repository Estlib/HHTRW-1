using System;
using System.Collections.Generic;
using System.Drawing;
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

        /// <summary>
        /// Returns reference strings for tiles in assets folder.
        /// Artgroup - NoArt
        /// </summary>
        /// <returns>Array of Sprite2d references</returns>
        public static Sprite2d[] GetArtRefs(string type = null)
        {
            Sprite2d[] artrefs = new Sprite2d[64];
            switch (type)
            {
                case "PlainsArtRefs":
                    artrefs[0] = new Sprite2d("tiles/plains/00");
                    artrefs[1] = new Sprite2d("tiles/plains/01");
                    artrefs[2] = new Sprite2d("tiles/plains/02");
                    artrefs[3] = new Sprite2d("tiles/plains/03");
                    artrefs[4] = new Sprite2d("tiles/plains/04");
                    artrefs[5] = new Sprite2d("tiles/plains/05");
                    artrefs[6] = new Sprite2d("tiles/plains/06");
                    artrefs[7] = new Sprite2d("tiles/plains/07");
                    artrefs[8] = new Sprite2d("tiles/plains/08");
                    artrefs[9] = new Sprite2d("tiles/plains/09");
                    artrefs[10] = new Sprite2d("tiles/plains/10");
                    artrefs[11] = new Sprite2d("tiles/plains/11");
                    artrefs[12] = new Sprite2d("tiles/plains/12");
                    artrefs[13] = new Sprite2d("tiles/plains/13");
                    artrefs[14] = new Sprite2d("tiles/plains/14");
                    artrefs[15] = new Sprite2d("tiles/plains/15");
                    artrefs[16] = new Sprite2d("tiles/plains/16");
                    artrefs[17] = new Sprite2d("tiles/plains/17");
                    artrefs[18] = new Sprite2d("tiles/plains/18");
                    artrefs[19] = new Sprite2d("tiles/plains/19");
                    artrefs[20] = new Sprite2d("tiles/plains/20");
                    artrefs[21] = new Sprite2d("tiles/plains/21");
                    artrefs[22] = new Sprite2d("tiles/plains/22");
                    artrefs[23] = new Sprite2d("tiles/plains/23");
                    artrefs[24] = new Sprite2d("tiles/plains/24");
                    artrefs[25] = new Sprite2d("tiles/plains/25");
                    artrefs[26] = new Sprite2d("tiles/plains/26");
                    artrefs[27] = new Sprite2d("tiles/plains/27");
                    artrefs[28] = new Sprite2d("tiles/plains/28");
                    artrefs[29] = new Sprite2d("tiles/plains/29");
                    artrefs[30] = new Sprite2d("tiles/plains/30");
                    artrefs[31] = new Sprite2d("tiles/plains/31");
                    artrefs[32] = new Sprite2d("tiles/plains/32");
                    artrefs[33] = new Sprite2d("tiles/plains/33");
                    artrefs[34] = new Sprite2d("tiles/plains/34");
                    artrefs[35] = new Sprite2d("tiles/plains/35");
                    artrefs[36] = new Sprite2d("tiles/plains/36");
                    artrefs[37] = new Sprite2d("tiles/plains/37");
                    artrefs[38] = new Sprite2d("tiles/plains/38");
                    artrefs[39] = new Sprite2d("tiles/plains/39");
                    artrefs[40] = new Sprite2d("tiles/plains/40");
                    artrefs[41] = new Sprite2d("tiles/plains/41");
                    artrefs[42] = new Sprite2d("tiles/plains/42");
                    artrefs[43] = new Sprite2d("tiles/plains/43");
                    artrefs[44] = new Sprite2d("tiles/plains/44");
                    artrefs[45] = new Sprite2d("tiles/plains/45");
                    artrefs[46] = new Sprite2d("tiles/plains/46");
                    artrefs[47] = new Sprite2d("tiles/plains/47");
                    artrefs[48] = new Sprite2d("tiles/plains/48");
                    artrefs[49] = new Sprite2d("tiles/plains/49");
                    artrefs[50] = new Sprite2d("tiles/plains/50");
                    artrefs[51] = new Sprite2d("tiles/plains/51");
                    artrefs[52] = new Sprite2d("tiles/plains/52");
                    artrefs[53] = new Sprite2d("tiles/plains/53");
                    artrefs[54] = new Sprite2d("tiles/plains/54");
                    artrefs[55] = new Sprite2d("tiles/plains/55");
                    artrefs[56] = new Sprite2d("tiles/plains/56");
                    artrefs[57] = new Sprite2d("tiles/plains/57");
                    artrefs[58] = new Sprite2d("tiles/plains/58");
                    artrefs[59] = new Sprite2d("tiles/plains/59");
                    artrefs[60] = new Sprite2d("tiles/plains/60");
                    artrefs[61] = new Sprite2d("tiles/plains/61");
                    artrefs[62] = new Sprite2d("tiles/plains/62");
                    artrefs[63] = new Sprite2d("tiles/plains/63");
                    break;
                case "TitleMenuMapRefs":
                    artrefs = new Sprite2d[89];
                    artrefs[0] = new Sprite2d("tiles/titlemapmenu/00");
                    artrefs[1] = new Sprite2d("tiles/titlemapmenu/01");
                    artrefs[2] = new Sprite2d("tiles/titlemapmenu/02");
                    artrefs[3] = new Sprite2d("tiles/titlemapmenu/03");
                    artrefs[4] = new Sprite2d("tiles/titlemapmenu/04");
                    artrefs[5] = new Sprite2d("tiles/titlemapmenu/05");
                    artrefs[6] = new Sprite2d("tiles/titlemapmenu/06");
                    artrefs[7] = new Sprite2d("tiles/titlemapmenu/07");
                    artrefs[8] = new Sprite2d("tiles/titlemapmenu/08");
                    artrefs[9] = new Sprite2d("tiles/titlemapmenu/09");
                    artrefs[10] = new Sprite2d("tiles/titlemapmenu/10");
                    artrefs[11] = new Sprite2d("tiles/titlemapmenu/11");
                    artrefs[12] = new Sprite2d("tiles/titlemapmenu/12");
                    artrefs[13] = new Sprite2d("tiles/titlemapmenu/13");
                    artrefs[14] = new Sprite2d("tiles/titlemapmenu/14");
                    artrefs[15] = new Sprite2d("tiles/titlemapmenu/15");
                    artrefs[16] = new Sprite2d("tiles/titlemapmenu/16");
                    artrefs[17] = new Sprite2d("tiles/titlemapmenu/17");
                    artrefs[18] = new Sprite2d("tiles/titlemapmenu/18");
                    artrefs[19] = new Sprite2d("tiles/titlemapmenu/19");
                    artrefs[20] = new Sprite2d("tiles/titlemapmenu/20");
                    artrefs[21] = new Sprite2d("tiles/titlemapmenu/21");
                    artrefs[22] = new Sprite2d("tiles/titlemapmenu/22");
                    artrefs[23] = new Sprite2d("tiles/titlemapmenu/23");
                    artrefs[24] = new Sprite2d("tiles/titlemapmenu/24");
                    artrefs[25] = new Sprite2d("tiles/titlemapmenu/25");
                    artrefs[26] = new Sprite2d("tiles/titlemapmenu/26");
                    artrefs[27] = new Sprite2d("tiles/titlemapmenu/27");
                    artrefs[28] = new Sprite2d("tiles/titlemapmenu/28");
                    artrefs[29] = new Sprite2d("tiles/titlemapmenu/29");
                    artrefs[30] = new Sprite2d("tiles/titlemapmenu/30");
                    artrefs[31] = new Sprite2d("tiles/titlemapmenu/31");
                    artrefs[32] = new Sprite2d("tiles/titlemapmenu/32");
                    artrefs[33] = new Sprite2d("tiles/titlemapmenu/33");
                    artrefs[34] = new Sprite2d("tiles/titlemapmenu/34");
                    artrefs[35] = new Sprite2d("tiles/titlemapmenu/35");
                    artrefs[36] = new Sprite2d("tiles/titlemapmenu/36");
                    artrefs[37] = new Sprite2d("tiles/titlemapmenu/37");
                    artrefs[38] = new Sprite2d("tiles/titlemapmenu/38");
                    artrefs[39] = new Sprite2d("tiles/titlemapmenu/39");
                    artrefs[40] = new Sprite2d("tiles/titlemapmenu/40");
                    artrefs[41] = new Sprite2d("tiles/titlemapmenu/41");
                    artrefs[42] = new Sprite2d("tiles/titlemapmenu/42");
                    artrefs[43] = new Sprite2d("tiles/titlemapmenu/43");
                    artrefs[44] = new Sprite2d("tiles/titlemapmenu/44");
                    artrefs[45] = new Sprite2d("tiles/titlemapmenu/45");
                    artrefs[46] = new Sprite2d("tiles/titlemapmenu/46");
                    artrefs[47] = new Sprite2d("tiles/titlemapmenu/47");
                    artrefs[48] = new Sprite2d("tiles/titlemapmenu/48");
                    artrefs[49] = new Sprite2d("tiles/titlemapmenu/49");
                    artrefs[50] = new Sprite2d("tiles/titlemapmenu/50");
                    artrefs[51] = new Sprite2d("tiles/titlemapmenu/51");
                    artrefs[52] = new Sprite2d("tiles/titlemapmenu/52");
                    artrefs[53] = new Sprite2d("tiles/titlemapmenu/53");
                    artrefs[54] = new Sprite2d("tiles/titlemapmenu/54");
                    artrefs[55] = new Sprite2d("tiles/titlemapmenu/55");
                    artrefs[56] = new Sprite2d("tiles/titlemapmenu/56");
                    artrefs[57] = new Sprite2d("tiles/titlemapmenu/57");
                    artrefs[58] = new Sprite2d("tiles/titlemapmenu/58");
                    artrefs[59] = new Sprite2d("tiles/titlemapmenu/59");
                    artrefs[60] = new Sprite2d("tiles/titlemapmenu/60");
                    artrefs[61] = new Sprite2d("tiles/titlemapmenu/61");
                    artrefs[62] = new Sprite2d("tiles/titlemapmenu/62");
                    artrefs[63] = new Sprite2d("tiles/titlemapmenu/63");
                    artrefs[64] = new Sprite2d("tiles/titlemapmenu/64");
                    artrefs[65] = new Sprite2d("tiles/titlemapmenu/65");
                    artrefs[66] = new Sprite2d("tiles/titlemapmenu/66");
                    artrefs[67] = new Sprite2d("tiles/titlemapmenu/67");
                    artrefs[68] = new Sprite2d("tiles/titlemapmenu/68");
                    artrefs[69] = new Sprite2d("tiles/titlemapmenu/69");
                    artrefs[70] = new Sprite2d("tiles/titlemapmenu/70");
                    artrefs[71] = new Sprite2d("tiles/titlemapmenu/71");
                    artrefs[72] = new Sprite2d("tiles/titlemapmenu/72");
                    artrefs[73] = new Sprite2d("tiles/titlemapmenu/73");
                    artrefs[74] = new Sprite2d("tiles/titlemapmenu/74");
                    artrefs[75] = new Sprite2d("tiles/titlemapmenu/75");
                    artrefs[76] = new Sprite2d("tiles/titlemapmenu/76");
                    artrefs[77] = new Sprite2d("tiles/titlemapmenu/77");
                    artrefs[78] = new Sprite2d("tiles/titlemapmenu/78");
                    artrefs[79] = new Sprite2d("tiles/titlemapmenu/79");
                    artrefs[80] = new Sprite2d("tiles/titlemapmenu/80");
                    artrefs[81] = new Sprite2d("tiles/titlemapmenu/81");
                    artrefs[82] = new Sprite2d("tiles/titlemapmenu/82");
                    artrefs[83] = new Sprite2d("tiles/titlemapmenu/83");
                    artrefs[84] = new Sprite2d("tiles/titlemapmenu/84");
                    artrefs[85] = new Sprite2d("tiles/titlemapmenu/85");
                    artrefs[86] = new Sprite2d("tiles/titlemapmenu/86");
                    artrefs[87] = new Sprite2d("tiles/titlemapmenu/87");
                    artrefs[88] = new Sprite2d("tiles/titlemapmenu/88");
                    break;
                case "UndergroundArtRefs":
                    artrefs[0] = new Sprite2d("tiles/underground/00");
                    artrefs[1] = new Sprite2d("tiles/underground/01");
                    artrefs[2] = new Sprite2d("tiles/underground/02");
                    artrefs[3] = new Sprite2d("tiles/underground/03");
                    artrefs[4] = new Sprite2d("tiles/underground/04");
                    artrefs[5] = new Sprite2d("tiles/underground/05");
                    artrefs[6] = new Sprite2d("tiles/underground/06");
                    artrefs[7] = new Sprite2d("tiles/underground/07");
                    artrefs[8] = new Sprite2d("tiles/underground/08");
                    artrefs[9] = new Sprite2d("tiles/underground/09");
                    artrefs[10] = new Sprite2d("tiles/underground/10");
                    artrefs[11] = new Sprite2d("tiles/underground/11");
                    artrefs[12] = new Sprite2d("tiles/underground/12");
                    artrefs[13] = new Sprite2d("tiles/underground/13");
                    artrefs[14] = new Sprite2d("tiles/underground/14");
                    artrefs[15] = new Sprite2d("tiles/underground/15");
                    artrefs[16] = new Sprite2d("tiles/underground/16");
                    artrefs[17] = new Sprite2d("tiles/underground/17");
                    artrefs[18] = new Sprite2d("tiles/underground/18");
                    artrefs[19] = new Sprite2d("tiles/underground/19");
                    artrefs[20] = new Sprite2d("tiles/underground/20");
                    artrefs[21] = new Sprite2d("tiles/underground/21");
                    artrefs[22] = new Sprite2d("tiles/underground/22");
                    artrefs[23] = new Sprite2d("tiles/underground/23");
                    artrefs[24] = new Sprite2d("tiles/underground/24");
                    artrefs[25] = new Sprite2d("tiles/underground/25");
                    artrefs[26] = new Sprite2d("tiles/underground/26");
                    artrefs[27] = new Sprite2d("tiles/underground/27");
                    artrefs[28] = new Sprite2d("tiles/underground/28");
                    artrefs[29] = new Sprite2d("tiles/underground/29");
                    artrefs[30] = new Sprite2d("tiles/underground/30");
                    artrefs[31] = new Sprite2d("tiles/underground/31");
                    artrefs[32] = new Sprite2d("tiles/underground/32");
                    artrefs[33] = new Sprite2d("tiles/underground/33");
                    artrefs[34] = new Sprite2d("tiles/underground/34");
                    artrefs[35] = new Sprite2d("tiles/underground/35");
                    artrefs[36] = new Sprite2d("tiles/underground/36");
                    artrefs[37] = new Sprite2d("tiles/underground/37");
                    artrefs[38] = new Sprite2d("tiles/underground/38");
                    artrefs[39] = new Sprite2d("tiles/underground/39");
                    artrefs[40] = new Sprite2d("tiles/underground/40");
                    artrefs[41] = new Sprite2d("tiles/underground/41");
                    artrefs[42] = new Sprite2d("tiles/underground/42");
                    artrefs[43] = new Sprite2d("tiles/underground/43");
                    artrefs[44] = new Sprite2d("tiles/underground/44");
                    artrefs[45] = new Sprite2d("tiles/underground/45");
                    artrefs[46] = new Sprite2d("tiles/underground/46");
                    artrefs[47] = new Sprite2d("tiles/underground/47");
                    artrefs[48] = new Sprite2d("tiles/underground/48");
                    artrefs[49] = new Sprite2d("tiles/underground/49");
                    artrefs[50] = new Sprite2d("tiles/underground/50");
                    artrefs[51] = new Sprite2d("tiles/underground/51");
                    artrefs[52] = new Sprite2d("tiles/underground/52");
                    artrefs[53] = new Sprite2d("tiles/underground/53");
                    artrefs[54] = new Sprite2d("tiles/underground/54");
                    artrefs[55] = new Sprite2d("tiles/underground/55");
                    artrefs[56] = new Sprite2d("tiles/underground/56");
                    artrefs[57] = new Sprite2d("tiles/underground/57");
                    artrefs[58] = new Sprite2d("tiles/underground/58");
                    artrefs[59] = new Sprite2d("tiles/underground/59");
                    artrefs[60] = new Sprite2d("tiles/underground/60");
                    artrefs[61] = new Sprite2d("tiles/underground/61");
                    artrefs[62] = new Sprite2d("tiles/underground/62");
                    artrefs[63] = new Sprite2d("tiles/underground/63");
                    break;
                case "DesertArtRefs":
                    artrefs[0] = new Sprite2d("tiles/desert/00");
                    artrefs[1] = new Sprite2d("tiles/desert/01");
                    artrefs[2] = new Sprite2d("tiles/desert/02");
                    artrefs[3] = new Sprite2d("tiles/desert/03");
                    artrefs[4] = new Sprite2d("tiles/desert/04");
                    artrefs[5] = new Sprite2d("tiles/desert/05");
                    artrefs[6] = new Sprite2d("tiles/desert/06");
                    artrefs[7] = new Sprite2d("tiles/desert/07");
                    artrefs[8] = new Sprite2d("tiles/desert/08");
                    artrefs[9] = new Sprite2d("tiles/desert/09");
                    artrefs[10] = new Sprite2d("tiles/desert/10");
                    artrefs[11] = new Sprite2d("tiles/desert/11");
                    artrefs[12] = new Sprite2d("tiles/desert/12");
                    artrefs[13] = new Sprite2d("tiles/desert/13");
                    artrefs[14] = new Sprite2d("tiles/desert/14");
                    artrefs[15] = new Sprite2d("tiles/desert/15");
                    artrefs[16] = new Sprite2d("tiles/desert/16");
                    artrefs[17] = new Sprite2d("tiles/desert/17");
                    artrefs[18] = new Sprite2d("tiles/desert/18");
                    artrefs[19] = new Sprite2d("tiles/desert/19");
                    artrefs[20] = new Sprite2d("tiles/desert/20");
                    artrefs[21] = new Sprite2d("tiles/desert/21");
                    artrefs[22] = new Sprite2d("tiles/desert/22");
                    artrefs[23] = new Sprite2d("tiles/desert/23");
                    artrefs[24] = new Sprite2d("tiles/desert/24");
                    artrefs[25] = new Sprite2d("tiles/desert/25");
                    artrefs[26] = new Sprite2d("tiles/desert/26");
                    artrefs[27] = new Sprite2d("tiles/desert/27");
                    artrefs[28] = new Sprite2d("tiles/desert/28");
                    artrefs[29] = new Sprite2d("tiles/desert/29");
                    artrefs[30] = new Sprite2d("tiles/desert/30");
                    artrefs[31] = new Sprite2d("tiles/desert/31");
                    artrefs[32] = new Sprite2d("tiles/desert/32");
                    artrefs[33] = new Sprite2d("tiles/desert/33");
                    artrefs[34] = new Sprite2d("tiles/desert/34");
                    artrefs[35] = new Sprite2d("tiles/desert/35");
                    artrefs[36] = new Sprite2d("tiles/desert/36");
                    artrefs[37] = new Sprite2d("tiles/desert/37");
                    artrefs[38] = new Sprite2d("tiles/desert/38");
                    artrefs[39] = new Sprite2d("tiles/desert/39");
                    artrefs[40] = new Sprite2d("tiles/desert/40");
                    artrefs[41] = new Sprite2d("tiles/desert/41");
                    artrefs[42] = new Sprite2d("tiles/desert/42");
                    artrefs[43] = new Sprite2d("tiles/desert/43");
                    artrefs[44] = new Sprite2d("tiles/desert/44");
                    artrefs[45] = new Sprite2d("tiles/desert/45");
                    artrefs[46] = new Sprite2d("tiles/desert/46");
                    artrefs[47] = new Sprite2d("tiles/desert/47");
                    artrefs[48] = new Sprite2d("tiles/desert/48");
                    artrefs[49] = new Sprite2d("tiles/desert/49");
                    artrefs[50] = new Sprite2d("tiles/desert/50");
                    artrefs[51] = new Sprite2d("tiles/desert/51");
                    artrefs[52] = new Sprite2d("tiles/desert/52");
                    artrefs[53] = new Sprite2d("tiles/desert/53");
                    artrefs[54] = new Sprite2d("tiles/desert/54");
                    artrefs[55] = new Sprite2d("tiles/desert/55");
                    artrefs[56] = new Sprite2d("tiles/desert/56");
                    artrefs[57] = new Sprite2d("tiles/desert/57");
                    artrefs[58] = new Sprite2d("tiles/desert/58");
                    artrefs[59] = new Sprite2d("tiles/desert/59");
                    artrefs[60] = new Sprite2d("tiles/desert/60");
                    artrefs[61] = new Sprite2d("tiles/desert/61");
                    artrefs[62] = new Sprite2d("tiles/desert/62");
                    artrefs[63] = new Sprite2d("tiles/desert/63");
                    break;
                case "ForestArtRefs":
                    artrefs[0] = new Sprite2d("tiles/forest/00");
                    artrefs[1] = new Sprite2d("tiles/forest/01");
                    artrefs[2] = new Sprite2d("tiles/forest/02");
                    artrefs[3] = new Sprite2d("tiles/forest/03");
                    artrefs[4] = new Sprite2d("tiles/forest/04");
                    artrefs[5] = new Sprite2d("tiles/forest/05");
                    artrefs[6] = new Sprite2d("tiles/forest/06");
                    artrefs[7] = new Sprite2d("tiles/forest/07");
                    artrefs[8] = new Sprite2d("tiles/forest/08");
                    artrefs[9] = new Sprite2d("tiles/forest/09");
                    artrefs[10] = new Sprite2d("tiles/forest/10");
                    artrefs[11] = new Sprite2d("tiles/forest/11");
                    artrefs[12] = new Sprite2d("tiles/forest/12");
                    artrefs[13] = new Sprite2d("tiles/forest/13");
                    artrefs[14] = new Sprite2d("tiles/forest/14");
                    artrefs[15] = new Sprite2d("tiles/forest/15");
                    artrefs[16] = new Sprite2d("tiles/forest/16");
                    artrefs[17] = new Sprite2d("tiles/forest/17");
                    artrefs[18] = new Sprite2d("tiles/forest/18");
                    artrefs[19] = new Sprite2d("tiles/forest/19");
                    artrefs[20] = new Sprite2d("tiles/forest/20");
                    artrefs[21] = new Sprite2d("tiles/forest/21");
                    artrefs[22] = new Sprite2d("tiles/forest/22");
                    artrefs[23] = new Sprite2d("tiles/forest/23");
                    artrefs[24] = new Sprite2d("tiles/forest/24");
                    artrefs[25] = new Sprite2d("tiles/forest/25");
                    artrefs[26] = new Sprite2d("tiles/forest/26");
                    artrefs[27] = new Sprite2d("tiles/forest/27");
                    artrefs[28] = new Sprite2d("tiles/forest/28");
                    artrefs[29] = new Sprite2d("tiles/forest/29");
                    artrefs[30] = new Sprite2d("tiles/forest/30");
                    artrefs[31] = new Sprite2d("tiles/forest/31");
                    artrefs[32] = new Sprite2d("tiles/forest/32");
                    artrefs[33] = new Sprite2d("tiles/forest/33");
                    artrefs[34] = new Sprite2d("tiles/forest/34");
                    artrefs[35] = new Sprite2d("tiles/forest/35");
                    artrefs[36] = new Sprite2d("tiles/forest/36");
                    artrefs[37] = new Sprite2d("tiles/forest/37");
                    artrefs[38] = new Sprite2d("tiles/forest/38");
                    artrefs[39] = new Sprite2d("tiles/forest/39");
                    artrefs[40] = new Sprite2d("tiles/forest/40");
                    artrefs[41] = new Sprite2d("tiles/forest/41");
                    artrefs[42] = new Sprite2d("tiles/forest/42");
                    artrefs[43] = new Sprite2d("tiles/forest/43");
                    artrefs[44] = new Sprite2d("tiles/forest/44");
                    artrefs[45] = new Sprite2d("tiles/forest/45");
                    artrefs[46] = new Sprite2d("tiles/forest/46");
                    artrefs[47] = new Sprite2d("tiles/forest/47");
                    artrefs[48] = new Sprite2d("tiles/forest/48");
                    artrefs[49] = new Sprite2d("tiles/forest/49");
                    artrefs[50] = new Sprite2d("tiles/forest/50");
                    artrefs[51] = new Sprite2d("tiles/forest/51");
                    artrefs[52] = new Sprite2d("tiles/forest/52");
                    artrefs[53] = new Sprite2d("tiles/forest/53");
                    artrefs[54] = new Sprite2d("tiles/forest/54");
                    artrefs[55] = new Sprite2d("tiles/forest/55");
                    artrefs[56] = new Sprite2d("tiles/forest/56");
                    artrefs[57] = new Sprite2d("tiles/forest/57");
                    artrefs[58] = new Sprite2d("tiles/forest/58");
                    artrefs[59] = new Sprite2d("tiles/forest/59");
                    artrefs[60] = new Sprite2d("tiles/forest/60");
                    artrefs[61] = new Sprite2d("tiles/forest/61");
                    artrefs[62] = new Sprite2d("tiles/forest/62");
                    artrefs[63] = new Sprite2d("tiles/forest/63");
                    break;
                case "CastleArtRefs":
                    artrefs[0] = new Sprite2d("tiles/castle/00");
                    artrefs[1] = new Sprite2d("tiles/castle/01");
                    artrefs[2] = new Sprite2d("tiles/castle/02");
                    artrefs[3] = new Sprite2d("tiles/castle/03");
                    artrefs[4] = new Sprite2d("tiles/castle/04");
                    artrefs[5] = new Sprite2d("tiles/castle/05");
                    artrefs[6] = new Sprite2d("tiles/castle/06");
                    artrefs[7] = new Sprite2d("tiles/castle/07");
                    artrefs[8] = new Sprite2d("tiles/castle/08");
                    artrefs[9] = new Sprite2d("tiles/castle/09");
                    artrefs[10] = new Sprite2d("tiles/castle/10");
                    artrefs[11] = new Sprite2d("tiles/castle/11");
                    artrefs[12] = new Sprite2d("tiles/castle/12");
                    artrefs[13] = new Sprite2d("tiles/castle/13");
                    artrefs[14] = new Sprite2d("tiles/castle/14");
                    artrefs[15] = new Sprite2d("tiles/castle/15");
                    artrefs[16] = new Sprite2d("tiles/castle/16");
                    artrefs[17] = new Sprite2d("tiles/castle/17");
                    artrefs[18] = new Sprite2d("tiles/castle/18");
                    artrefs[19] = new Sprite2d("tiles/castle/19");
                    artrefs[20] = new Sprite2d("tiles/castle/20");
                    artrefs[21] = new Sprite2d("tiles/castle/21");
                    artrefs[22] = new Sprite2d("tiles/castle/22");
                    artrefs[23] = new Sprite2d("tiles/castle/23");
                    artrefs[24] = new Sprite2d("tiles/castle/24");
                    artrefs[25] = new Sprite2d("tiles/castle/25");
                    artrefs[26] = new Sprite2d("tiles/castle/26");
                    artrefs[27] = new Sprite2d("tiles/castle/27");
                    artrefs[28] = new Sprite2d("tiles/castle/28");
                    artrefs[29] = new Sprite2d("tiles/castle/29");
                    artrefs[30] = new Sprite2d("tiles/castle/30");
                    artrefs[31] = new Sprite2d("tiles/castle/31");
                    artrefs[32] = new Sprite2d("tiles/castle/32");
                    artrefs[33] = new Sprite2d("tiles/castle/33");
                    artrefs[34] = new Sprite2d("tiles/castle/34");
                    artrefs[35] = new Sprite2d("tiles/castle/35");
                    artrefs[36] = new Sprite2d("tiles/castle/36");
                    artrefs[37] = new Sprite2d("tiles/castle/37");
                    artrefs[38] = new Sprite2d("tiles/castle/38");
                    artrefs[39] = new Sprite2d("tiles/castle/39");
                    artrefs[40] = new Sprite2d("tiles/castle/40");
                    artrefs[41] = new Sprite2d("tiles/castle/41");
                    artrefs[42] = new Sprite2d("tiles/castle/42");
                    artrefs[43] = new Sprite2d("tiles/castle/43");
                    artrefs[44] = new Sprite2d("tiles/castle/44");
                    artrefs[45] = new Sprite2d("tiles/castle/45");
                    artrefs[46] = new Sprite2d("tiles/castle/46");
                    artrefs[47] = new Sprite2d("tiles/castle/47");
                    artrefs[48] = new Sprite2d("tiles/castle/48");
                    artrefs[49] = new Sprite2d("tiles/castle/49");
                    artrefs[50] = new Sprite2d("tiles/castle/50");
                    artrefs[51] = new Sprite2d("tiles/castle/51");
                    artrefs[52] = new Sprite2d("tiles/castle/52");
                    artrefs[53] = new Sprite2d("tiles/castle/53");
                    artrefs[54] = new Sprite2d("tiles/castle/54");
                    artrefs[55] = new Sprite2d("tiles/castle/55");
                    artrefs[56] = new Sprite2d("tiles/castle/56");
                    artrefs[57] = new Sprite2d("tiles/castle/57");
                    artrefs[58] = new Sprite2d("tiles/castle/58");
                    artrefs[59] = new Sprite2d("tiles/castle/59");
                    artrefs[60] = new Sprite2d("tiles/castle/60");
                    artrefs[61] = new Sprite2d("tiles/castle/61");
                    artrefs[62] = new Sprite2d("tiles/castle/62");
                    artrefs[63] = new Sprite2d("tiles/castle/63");
                    break;
                case "NoArtRefs":
                default:
                        if (type == null)
                        {
                            Log.Warning("Type is null, using NoArtRefs instead.");
                        }
                    artrefs[0] = new Sprite2d("tiles/noart/testblock1");
                    artrefs[1] = new Sprite2d("tiles/noart/testblock5");
                    artrefs[2] = new Sprite2d("tiles/noart/testobject2");
                    artrefs[3] = new Sprite2d("tiles/noart/testobject3");
                    artrefs[4] = new Sprite2d("tiles/noart/testblock6");
                    artrefs[5] = new Sprite2d("tiles/noart/ts1");
                    artrefs[6] = new Sprite2d("tiles/noart/ts2");
                    artrefs[7] = new Sprite2d("tiles/noart/ts3");
                    artrefs[8] = new Sprite2d("tiles/noart/ts4");
                    artrefs[9] = new Sprite2d("tiles/noart/ts5");
                    artrefs[10] = new Sprite2d("tiles/noart/ts6");
                    artrefs[11] = new Sprite2d("tiles/noart/ts7");
                    artrefs[12] = new Sprite2d("tiles/noart/ts8");
                    artrefs[13] = new Sprite2d("tiles/noart/ts9");
                    artrefs[14] = new Sprite2d("tiles/noart/lss1");
                    artrefs[15] = new Sprite2d("tiles/noart/lss2");
                    artrefs[16] = new Sprite2d("tiles/noart/lss3");
                    artrefs[17] = new Sprite2d("tiles/noart/lss4");
                    artrefs[18] = new Sprite2d("tiles/noart/lss5");
                    artrefs[19] = new Sprite2d("tiles/noart/lss6");
                    artrefs[20] = new Sprite2d("tiles/noart/numbertile01");
                    artrefs[21] = new Sprite2d("tiles/noart/numbertile02");
                    artrefs[22] = new Sprite2d("tiles/noart/numbertile03");
                    artrefs[23] = new Sprite2d("tiles/noart/numbertile04");
                    artrefs[24] = new Sprite2d("tiles/noart/numbertile05");
                    artrefs[25] = new Sprite2d("tiles/noart/numbertile06");
                    artrefs[26] = new Sprite2d("tiles/noart/numbertile07");
                    artrefs[27] = new Sprite2d("tiles/noart/numbertile08");
                    artrefs[28] = new Sprite2d("tiles/noart/numbertile09");
                    artrefs[29] = new Sprite2d("tiles/noart/numbertile0A");
                    break;
                }
            return artrefs;
        }

        internal static List<Bitmap> PlayerSprites()
        {
            return new List<Bitmap>()
            {
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand1.png")),           //0 - standing --------------*
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run1.png")),             //1 - running 1/6
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run2.png")),             //2 - running 2/6
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run3.png")),             //3 - running 3/6
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run4.png")),             //4 - running 4/6
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run5.png")),             //5 - running 5/6
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run6.png")),             //6 - running 6/6
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand2.png")),           //7 - inbetween for standing and crouching
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3duck.png")),       //8 - crouching
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3jump.png")),       //9 - jumping
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/ALTfall.png")),          //10 - falling
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand1flip.png")),       //11 - standing flipped --------------*
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run1flip.png")),         //12 - running 1/6 flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run2flip.png")),         //13 - running 2/6 flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run3flip.png")),         //14 - running 3/6 flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run4flip.png")),         //15 - running 4/6 flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run5flip.png")),         //16 - running 5/6 flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run6flip.png")),         //17 - running 6/6 flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand2flip.png")),       //18 - inbetween for standing and crouching flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3duckflip.png")),   //19 - crouching flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3jumpflip.png")),   //20 - jumping flipped
                new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/ALTfallFLIP.png"))       //21 - falling flipped
            };
        }

        internal static List<Bitmap> MenuSprites()
        {
            return new List<Bitmap>()
            {
                new Bitmap(Image.FromFile($"assets/sprites/menu/00.png")),  //0 - unselected menu level 0 template
                new Bitmap(Image.FromFile($"assets/sprites/menu/01.png")),  //1 - item 1/4 selected, level 0 (start game)
                new Bitmap(Image.FromFile($"assets/sprites/menu/02.png")),  //2 - item 2/4 selected, level 0 (save load)
                new Bitmap(Image.FromFile($"assets/sprites/menu/03.png")),  //3 - item 3/4 selected, level 0 (options)-(submenu)
                new Bitmap(Image.FromFile($"assets/sprites/menu/04.png")),  //4 - item 4/4 selected, level 0 (exit)
                new Bitmap(Image.FromFile($"assets/sprites/menu/05.png")),  //5 - item 1/3 selected, level 1 options (info)-(subscreen)
                new Bitmap(Image.FromFile($"assets/sprites/menu/06.png")),  //6 - item 2/3 selected, level 1 options (sound test)-(subscreen)
                new Bitmap(Image.FromFile($"assets/sprites/menu/07.png")),  //7 - item 3/3 selected, level 1 options (fullscreen)-(function toggle)
            };                                                            
        }


    }
}
