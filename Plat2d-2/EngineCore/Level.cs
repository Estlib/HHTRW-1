using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class Level
    {
        public int? musicNumber = null;
        public string artSetFolder = null;
        public string[,] firstMostLayer = null;
        public string[,] objectRenderLayer = null;
        public string[,] lastMostLayer = null;
        public string[,] hudLayer = null;

        //public int? musicNumber {  get; set; }
        //public string artSetFolder { get; set; }
        //public string[,] GetFirstMostLayer 
        //{ 
        //    get { return Level1(firstMostLayer); }
        //    set { firstMostLayer = this.firstMostLayer; }
        //}
        //public string[,] objectRenderLayer { get; set; }
        //public string[,] lastMostLayer { get; set; }
        //public string[,] hudLayer { get; set; }
        //public Level(string artSetFolder)
        //{
        //    this.musicNumber = 0;
        //    this.artSetFolder = artSetFolder;
        //    this.firstMostLayer = null;
        //    this.objectRenderLayer = null;
        //    this.lastMostLayer = null;
        //    this.hudLayer = null;
        //}
        public Level(string artSetFolder)
        {
            this.musicNumber = 0;
            this.artSetFolder = artSetFolder;
            this.firstMostLayer = Level1(firstMostLayer);
            this.objectRenderLayer = null;
            this.lastMostLayer = null;
            this.hudLayer = null;
        }
        public Level(string artSetFolder, string[,] firstMostLayer)
        {
            this.musicNumber = 0;
            this.artSetFolder = artSetFolder;
            this.firstMostLayer = Level1(firstMostLayer);
            this.objectRenderLayer = null;
            this.lastMostLayer = null;
            this.hudLayer = null;
        }
        public string[,] Level1(string[,] firstMostLayer)
        {
            string[,] Map =
            {
                {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
                {"G",".",".",".",".",".","C",".","C",".",".",".",".",".",".",".",".",".",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","C",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
                {"G",".",".","G","G","G",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
                {"G",".","P",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
                {"G",".",".",".",".",".",".","G","G","G",".",".",".",".",".",".","G",".",".","G" },
                {"G",".","C",".",".",".",".",".",".",".",".",".",".",".",".",".","G","C",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".","C",".","C",".","G",".",".","G" },
                {"G",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","G",".",".","G" },
                {"G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G","G" },
            };
            return Map;
        }
    }
    //integration is needed for OnLoad to load the level item and cycle through the art and make the level
}
