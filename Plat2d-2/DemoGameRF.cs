using Box2DX.Dynamics;
using Plat2d_2.EngineCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Plat2d_2.EngineCore.ObjectTypes;

namespace Plat2d_2
{
    public class DemoGameRF : EngineCore.EngineCore
    {
        // Fields that this engine has:

        /* framerate stuff */
        // win api methods
        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint uMilliseconds);
        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint uMilliseconds);

        public static int MaxFPS = 50; // target fps, pal ftw
        public double MaxFT = (1000d / MaxFPS); // frametime, per frame in 1s
        public Stopwatch stopwatch = new Stopwatch();
        int fpsCounter = 0; //how many fps this frame
        Stopwatch fpsTimer = Stopwatch.StartNew(); //timerthingy for the frametime ms counting
        public double lastFrameTime = 0; //how long last frame took

        /* hud stuff */
        //random
        string fontplace = "assets/fonts/arcade-legacy.ttf"; // where font
        string levelclearingsforlabel = $"______  _  _"; // string that is displayed in window to show level clear tags for world 1
        // prototype hud labels */
        Label CrystalLabel;
        Label ScoreLabel;
        Label HealthLabel;
        Label LivesLabel;
        Label AmmoLabel;
        Label LevelLabel;
        Label SelectedWeaponLabel;

        /* Runtime stuff in-game */
        //player
        Sprite2d player; //variable to hold players sprite with body
        List<Bitmap> playerSpritesBitmap = new List<Bitmap>(); //holds player sprite bitmaps
        //enemies
        public static List<EnemyV2> enemiesv2 = new List<EnemyV2>(); //enemies that exist
        List<Bitmap> walkingEnemySpritesBitmap = EnemyV2.EnemySprites("walking"); //holds walking enemy type object sprite bitmaps
        //loggers
        public static bool logThisEnemy = false;
        public static int loggedEnemyArrayID = -1;
        public static bool exitTool = false;
        //animation
        int runRate = 3; //how many frames pass, before sprite is animated again
        int animationClock = 0; //where the animation clock currently is
        //level
        public static int currentLevelEndSize = 0; // how long the level is
        public static bool reloadtrigger = false; //level reloading trigger

        /* menu stuff*/
        List<Bitmap> menuSpritesBitmap = new List<Bitmap>(); //holds menu object sprite bitmaps

        /* Gameplay stuff */
        //World 1
        WorldStructure harenimus = WorldStructure.GetHarenimus();
    }
}
