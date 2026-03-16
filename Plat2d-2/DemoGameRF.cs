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
        List<Bitmap> playerSpritesBitmap = ArtData.PlayerSprites(); //holds player sprite bitmaps
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
        int playerSpeed = 10; //TODO; verify necessity
        int currentSprite;//TODO; verify necessity
        //weapons-player
        List<Weapon> unlockedWeapons = new List<Weapon>();
        //weapons-player-active
        List<Bitmap> bulletgraphics = new List<Bitmap>();
        Weapon activeWeapon;
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
        List<SFX> allSFX = new List<SFX>();
        string sfxPath = "assets/audio/sfx";
        private SFXEngineMUSIEF sfxInstance;
        //audio-music
        List<BGM> jukeBox = new List<BGM>();
        string bgmPath = "assets/audio/bgm";
        private BGMPlayer BGMPlayer = new BGMPlayer(new List<BGM>());

        /* menu stuff*/
        List<Bitmap> menuSpritesBitmap = ArtData.MenuSprites(); //holds menu object sprite bitmaps

        /* Gameplay stuff */
        //World 1
        WorldStructure harenimus = WorldStructure.GetHarenimus();
        WorldStructure utility = WorldStructure.GetScreens();
        //GameWorlds
        List<WorldStructure> worlds = new List<WorldStructure>();
        //Current gamestate
        public static bool isOnScreen = true;
        public static int whichScreen = 0;



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
            BGColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
            worlds.Add(harenimus);
            worlds.Add(utility);

            //audio tool
            SetSFXengineB();
            SetJukebox();

            //weapon config
            unlockedWeapons.Add(Weapon.GetWeapon("debug"));

            //player config
            //integer value for holding the current player sprite, through which the list is accessed and player is animated through the use of
            currentSprite = 0; //TODO; verify necessity

            foreach (var sfxR in allSFX)
            {
                sfxInstance.RegisterSound(sfxR.Name, sfxR.Filepath);
                Log.Info($"{sfxR.Name}");
            }
            LogUtility.ClearLineOnly(6);

            //Start game on title screen   \/
            isOnScreen = true;
            whichScreen = 0;
            LoadArea(isOnScreen, whichScreen, worlds);
        }

       

        /// <summary>
        /// Overall method for Area loading
        /// </summary>
        /// <param name="isOnScreen">Is the player on a screen or in a play area</param>
        /// <param name="whichScreen">What screen are they on</param>
        /// <param name="worlds">All the worlds game has</param>
        private void LoadArea(bool isOnScreen, int whichScreen, List<WorldStructure> worlds)
        {
            WorldStructure selectedWorld = WorldStructure.GetScreens(); //temp world
            if (isOnScreen) //if the displayed screen is a utility screen not a play area
            {
                //find utility world
                foreach (var world in worlds)
                {
                    if (world.WorldName == "utility")
                    {
                        //set as loadable source into empty temp world
                        selectedWorld = world;
                        break;
                    }
                }

                //load screen address
                LoadScreen(whichScreen, selectedWorld);
            }
        }

        private void LoadScreen(int whichScreen, WorldStructure selectedWorld)
        {
            Level loadTarget = selectedWorld.Levels[whichScreen];
            BGColor = loadTarget.levelColor; //set color from level
            enemiesv2 = new List<EnemyV2>(); //clear enemies
            Log.Info($"New Level is being loaded -> {loadTarget.levelname}");

            //load the layers
            List<string[,]> layersToRender = new List<string[,]>();
            layersToRender.Add(loadTarget.firstMostLayer);
            layersToRender.Add(loadTarget.objectRenderLayer);
            layersToRender.Add(loadTarget.lastMostLayer);
            layersToRender.Add(loadTarget.hudLayer);
            currentLevelEndSize = layersToRender.ElementAt(0).GetLength(1) * 16;
            foreach (var layer in layersToRender)
            {
                if (layersToRender.IndexOf(layer) == 1)
                {
                    RenderObjects(layer);
                }
                else
                {
                    RenderLayer(loadTarget.artRefs, layer, loadTarget.artTagDefinitions);
                }
            }
            PlayLevelTrack(loadTarget);

        }

        private void PlayLevelTrack(Level loadTarget)
        {
            if (loadTarget.levelname == "")
            {
                BGMPlayer.Stop();
            }
            else
            {
                foreach (var bgm in jukeBox)
                {
                    if (loadTarget.musicNumber == bgm.ArrayID)
                    {
                        Log.Highlight("Match success");
                        BGMPlayer.PlayNow(bgm.Filepath);
                        break;
                    }
                    else
                    {
                        Log.Error($"Match fail. levelname: {loadTarget.musicNumber} checked track: {bgm.ArrayID}");
                    }
                }
            }
        }

        private void RenderObjects(string[,] layer)
        {
            for (int i = 0; i < layer.GetLength(1); i++)
            {
                for (int j = 0; j < layer.GetLength(0); j++)
                {
                    if (layer[j, i] == "P")
                    {
                        player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerSpritesBitmap[0], "Player");
                        //player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerStand, "Player");
                        player.CreateDynamic();
                        //pass a list of sprites here, changing happens by animating list numbers, limited by if limits
                        //see https://www.youtube.com/results?search_query=box2d+tutorial+c%23
                        //playercollision = new Shape2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), "Player");
                        //playercollision.CreateDynamic();
                        respawnlocation = new Vec2(i * 16, j * 16);
                    }
                    if (layer[j, i] == "WE")
                    {
                        EnemyV2 enemyv2 = new EnemyV2
                            (
                                new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), walkingEnemySpritesBitmap[0], "Enemy"),
                                walkingEnemySpritesBitmap,
                                new List<int> { 12, 13, 14, 15, 16, 17 }, //walkleft
                                new List<int> { 1, 2, 3, 4, 5, 6 }, //walkright
                                new List<int> { 20 }, //jumpleft
                                new List<int> { 9 }, //jumpright
                                new List<int> { 11 }, //stillleft
                                new List<int> { 0 }, //stillright
                                new List<int> { 23 }, //fireleft
                                new List<int> { 23 }, //fireright
                                new List<int> { 23 }, //fly
                                new List<int> { 22 }, //error
                                i * 16,
                                j * 16,
                                0,
                                EnemyV2Data.walkingEnemy, //behaviourdata
                                0, //where is its action currently in behaviourloop
                                false,
                                true,
                                "Walking enemy",
                                "Enemy",
                                0
                            );

                        enemyv2.sprite2d.CreateDynamic();
                        enemiesv2.Add(enemyv2);

                    }
                    if (layer[j, i] == "JE")
                    {
                        EnemyV2 enemyv2 = new EnemyV2
                        (
                            new Sprite2d
                            (
                                new Vector2(i * 16, j * 16), 
                                new Vector2(32, 32), 
                                walkingEnemySpritesBitmap[0], 
                                "Enemy"
                                ),
                            walkingEnemySpritesBitmap,
                            new List<int> { 12, 13, 14, 15, 16, 17 }, //walkleft
                            new List<int> { 1, 2, 3, 4, 5, 6 }, //walkright
                            new List<int> { 18, 20 }, //jumpleft
                            new List<int> { 7, 9 }, //jumpright
                            new List<int> { 11 }, //stillleft
                            new List<int> { 0 }, //stillright
                            new List<int> { 23 }, //fireleft
                            new List<int> { 23 }, //fireright
                            new List<int> { 23 }, //fly
                            new List<int> { 22 }, //error
                            i * 16,
                            j * 16,
                            0,
                            EnemyV2Data.jumpingEnemy, //behaviourdata
                            0, //where is its action currently in behaviourloop
                            false,
                            true,
                            "Test - Jumping enemy",
                            "Enemy",
                            0
                        );

                        enemyv2.sprite2d.CreateDynamic();
                        enemiesv2.Add(enemyv2);

                    }

                }
            }
        }

        private void RenderLayer(Sprite2d[] artRefs, string[,] layer, string[] artTagDefinitions)
        {
            for (int i = 0; i < layer.GetLength(1); i++)
            {
                for (int j = 0; j < layer.GetLength(0); j++)
                {
                    bool skipcheck = false;
                    int tryint = 0;
                    int result = 0;
                    if (layer[j, i] == "  ")
                    {
                        skipcheck = true;
                    }
                    if (skipcheck == false)
                    {
                        if (int.TryParse(layer[j, i], out result))
                        {
                            tryint = result;
                        }
                        if (artTagDefinitions[tryint] == "Ground")
                        {
                            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), artRefs[tryint], artTagDefinitions[tryint]).CreateStatic();
                        }
                        else
                        {
                            new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), artRefs[tryint], artTagDefinitions[tryint])/*.CreateStatic()*/;
                        }
                    }
                }
            }
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
