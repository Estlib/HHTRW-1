using Box2DX.Common;
using Box2DX.Dynamics;
using Plat2d_2.EngineCore;
using Plat2d_2.EngineCore.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        bool loadfontfromfileandnotos = false;
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
        int facedirection; //holds value for which direction player last faced
        bool left; // key left bool
        bool right; // key right bool
        bool up; // key up bool
        bool down; // key down bool
        bool fire; // key fire bool
        bool jump; // key jump bool
        bool respawntester; // respawns player at spawn location
        bool jumpmode; // can player currently jump
        bool nokey; // is any key being pressed
        Vec2 respawnlocation; // where to put the player when they lose life.
        bool pauseGameKey; // key pause bool
        bool isGodMode; //is players current mode godmode or normalmode.
        //weapons-player
        List<Weapon> unlockedWeapons = new List<Weapon>();
        //weapons-player-active
        List<Bitmap> bulletgraphics = new List<Bitmap>();
        Weapon activeWeapon = new Weapon();
        int maxbulletsallowed = 3;
        int currentbulletsonscreen = 0;
        bool firinglock;
        int firinglockcounter = 10;
        int weapon1speed = 1;
        int weapon1cyclespeed = 4;
        int selectedweapon = 1;
        List<Bullet> bullets = new List<Bullet>();

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
        //level graphics
        public static Sprite2d[] NoArtRefs = ArtData.GetArtRefs("NoArtRefs");
        public static Sprite2d[] PlainsArtRefs = ArtData.GetArtRefs("PlainsArtRefs");
        public static Sprite2d[] TitleMenuMapRefs = ArtData.GetArtRefs("TitleMenuMapRefs");
        public static Sprite2d[] UndergroundArtRefs = ArtData.GetArtRefs("UndergroundArtRefs");
        public static Sprite2d[] DesertArtRefs = ArtData.GetArtRefs("DesertArtRefs");
        public static Sprite2d[] ForestArtRefs = ArtData.GetArtRefs("ForestArtRefs");
        public static Sprite2d[] CastleArtRefs = ArtData.GetArtRefs("CastleArtRefs");
        //audio-sfx
        List <SFX> allSFX = new List<SFX>();
        string sfxPath = "assets/audio/sfx";
        private SFXEngineMUSIEF sfxInstance;
        //audio-music
        List<BGM> jukeBox = new List<BGM>();
        string bgmPath = "assets/audio/bgm";
        private BGMPlayer BGMPlayer = new BGMPlayer(new List<BGM>());

        /* menu stuff*/
        List<Bitmap> menuSpritesBitmap = new List<Bitmap>(); //holds menu object sprite bitmaps

        /* Gameplay stuff */
        //World 1
        WorldStructure harenimus = WorldStructure.GetHarenimus();
        //GameWorlds
        List<WorldStructure> worlds = new List<WorldStructure>();


        /// <summary>
        /// constructor for demogame. bases enginecore - window mode.
        /// </summary>
        public DemoGameRF() : base(new Vector2(320 + 16, 240 + 40), "Harold Harrisson the Rabbit Warrior")
        {

        }
        /// <summary>
        /// constructor for demogame. bases enginecore - fullscreen mode.
        /// </summary>
        public DemoGameRF(bool _isWindow) : base(new Vector2(320, 240), "Harold Harrisson the Rabbit Warrior")
        {
            isWindow = _isWindow;
        }

        public override void OnLoad()
        {
            //fps tools
            timeBeginPeriod(1); //set time measuring to milliseconds
            stopwatch.Start(); //make stopwatch go
            lastFrameTime = stopwatch.Elapsed.TotalMilliseconds; //get time

            //font tools
            PrivateFontCollection privateFonts = new PrivateFontCollection();
            privateFonts.AddFontFile("assets/fonts/arcade-legacy.ttf");
            FontFamily font = privateFonts.Families[0];
            Font debuglabelfont = new Font(font, 6);
            Font systemdebuglabelfont = new Font("Arcade Legacy", 6);

            //labels
            /* TODO: leave empty, in original demogame, thread is invoked,
            refactor so that a thread doesnt need to be invoked to avoid
            race conditions.
            */

            //logging tool
            Log.InitiateLogWindow();
            Log.Highlight($"Game is starting, current game: DemoGame", 3);

            //setup procedure
            BGColor = System.Drawing.Color.FromArgb(255,0,0,0);
            worlds.Add(harenimus);

            //audio tool
            SetSFXengineB();
            SetJukebox();

            //weapon config
            unlockedWeapons.Add(Weapon.GetWeapon("debug"));
        }

        private void SetSFXengineB()
        {
            sfxInstance = SFXEngineMUSIEF.Instance;
            foreach (string file in Directory.GetFiles(sfxPath))
            {
                string deconstructable = Path.GetFileName(file);
                string partialName = "Track";
                int locatedIndex = deconstructable.IndexOf(partialName, StringComparison.OrdinalIgnoreCase);
                int veerID = 1;
                int readForXCharCountID = 2;
                int veerName = 13;
                int readForXCharCountName = -1;
                int ss1 = locatedIndex + partialName.Length + veerName;
                int ss2 = deconstructable.Length - (ss1 + 5);

                //Log.Info(deconstructable);
                //Log.Info(ss1.ToString());
                //Log.Info(ss2.ToString());

                SFX newsfx = new SFX()
                {
                    Name = deconstructable.Substring(ss1, ss2),
                    ArrayID = int.Parse(deconstructable.Substring(locatedIndex + veerID + partialName.Length, readForXCharCountID)),
                    Filepath = sfxPath + "/" + Path.GetFileName(file).ToString()
                };
                allSFX.Add(newsfx);

            }
        }
        private void SetJukebox()
        {
            List<string> levelsNames = new List<string>();
            foreach (var level in levels)
            {
                levelsNames.Add(level.levelname);
            }
            //BGMPlayer bgm = new BGMPlayer(jukeBox);
            //foreach (var levelName in levelsNames)
            //{

            //}


            foreach (string file in Directory.GetFiles(bgmPath))
            {
                string deconstructable = Path.GetFileName(file);
                string partialName = "Track";
                int locatedIndex = deconstructable.IndexOf(partialName, StringComparison.OrdinalIgnoreCase);
                int veerID = 1;
                int readForXCharCountID = 2;
                int veerName = 5; // was 13
                int readForXCharCountName = -1;
                int ss1 = locatedIndex + partialName.Length + veerName;
                int ss2 = deconstructable.Length - (ss1 + 5);

                //Log.Info(deconstructable);
                //Log.Info(ss1.ToString());
                //Log.Info(ss2.ToString());

                BGM newbgm = new BGM()
                {
                    Name = deconstructable.Substring(ss1, ss2),
                    ArrayID = int.Parse(deconstructable.Substring(locatedIndex + veerID + partialName.Length, readForXCharCountID)),
                    Filepath = bgmPath + "/" + Path.GetFileName(file).ToString(),
                    //AssociatedLevelID = idInput
                };

                Log.Warning(newbgm.ArrayID.ToString());
                jukeBox.Add(newbgm);

            }
            BGMPlayer.songs = jukeBox;
            //Console.ReadLine();
        }
    }
}
