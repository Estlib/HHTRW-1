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
    public enum KeyMode
    {
        KeyBoard_Form, KeyBoard_WinAPI, Controller
    }
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
        int steps = 0;
        int slowDownFrameRate = 0;
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
        //huddata
        public static int crystalScoreTally = 0;
        public static int pointScoreTally = 0;
        public static int playerHealth = 100;
        public static int playerLives = 5;

        //enemies
        public static List<EnemyV2> enemiesv2 = new List<EnemyV2>(); //enemies that exist
        List<Bitmap> walkingEnemySpritesBitmap = EnemyV2.EnemySprites("walking"); //holds walking enemy type object sprite bitmaps
        //loggers
        public static bool logThisEnemy = false;
        public static int loggedEnemyArrayID = -1;
        public static bool exitTool = false;
        //animation
        int runRate = 3; //how many frames pass, before sprite is animated again
        public static int animationClock = 0; //where the animation clock currently is
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
        public static bool isPlayerRequestingLevel = false;
        public static int[] whichLevel = { 1, 0 };
        public static bool isPlayerGoingNextRoom = false;
        public static int nextRoomElementInt = 1;
        Level activeLevel;
        public static bool isThisLevelClear = false;
        public static KeyMode currentKeyMode = KeyMode.KeyBoard_Form;



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
            isPlayerRequestingLevel = false;
            LoadArea(isOnScreen, whichScreen, worlds);
            currentKeyMode = KeyMode.KeyBoard_Form;

        }

        public override void OnDraw()
        {
            // level state handling
            GameStateHandler2();

            // key detection
            if (currentKeyMode == KeyMode.KeyBoard_Form)
            {
                if (up)
                {
                    if (up == true && left == true)
                    {
                        up = false;
                    }
                    if (up == true && right == true)
                    {
                        up = false;
                    }
                    Log.Warning("Up has no animation currently.");
                }
                if (down)
                {
                    if (down == true && left == true)
                    {
                        down = false;
                    }
                    if (down == true && right == true)
                    {
                        down = false;
                    }
                    if (facedirection == 0)
                    {
                        AnimatePlayer(19, 19);
                    }
                    else if (facedirection == 1)
                    {
                        AnimatePlayer(8, 8);
                    }
                }
                if (left)
                {
                    facedirection = 0;
                    AnimatePlayer(12, 17);
                }
                if (right)
                {
                    facedirection = 1;
                    AnimatePlayer(1, 6);
                }
                if (jump)
                {

                    if (facedirection == 0)
                    {
                        AnimatePlayer(21, 21);
                    }
                    else if (facedirection == 1)
                    {
                        AnimatePlayer(10, 10);
                    }
                }
                if (jump == false && jumpmode == true)
                {
                    if (facedirection == 0)
                    {
                        AnimatePlayer(20, 20);
                    }
                    else if (facedirection == 1)
                    {
                        AnimatePlayer(9, 9);
                    }
                }
                if (StillInput(nokey))
                {
                    if (facedirection == 0)
                    {
                        AnimatePlayer(11, 11);
                    }
                    else if (facedirection == 1)
                    {
                        AnimatePlayer(0, 0);
                    }
                }
                if (pauseGameKey)
                {
                    Log.DebugFunction("Game halted.");
                    //sfxInstance.StopAll();
                    while (pauseGameKey != false)
                    {
                        if (exitTool != true)
                        {
                            DebugUtility.ToolMenu();
                        }
                    }
                    Log.DebugFunction("Halt ended.");
                }
            }

            // collisions
            int remainingJumpSteps = 0;

            // camera
            UpdatePlayerCamera();
        }

        public override void OnUpdate()
        {
            //frame measuring tools
            double frameStart = stopwatch.Elapsed.TotalMilliseconds;
            Log.runtimeframes++;

            //reset animationclock at runrate
            if (animationClock == runRate)
            {
                animationClock = 0;
            }
            animationClock++;
            // visualisation for animation clock in the console
            //Log.Info($"Animation clock is currently {animationClock}");
            switch (animationClock)
            {
                case 0:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ░░ ░░ ");
                    break;
                case 1:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ██ ░░ ░░ ░░ ");
                    break;
                case 2:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ██ ░░ ░░ ");
                    break;
                case 3:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ██ ░░ ");
                    break;
                case 4:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ░░ ██ ");
                    break;
                default:
                    break;
            }

            //update player statuses
            if (player.Position.Y >= 320 || playerHealth < 1)
            {
                LoseLife();
            }
            currentbulletsonscreen = bullets.Count();
            if (crystalScoreTally == 100)
            {
                crystalScoreTally = 0;
                playerLives++;
            }
            //shooting
            if (firinglock)
            {
                if (firinglockcounter != 0)
                {
                    //Log.Info("Subtracting one from firinglockcounter");
                    firinglockcounter--;
                }
                else if (firinglockcounter == 0)
                {
                    firinglock = false;
                }
            }
            if (firinglock == false)
            {
                //Log.Info("setting firinglockcounter to 10");
                firinglockcounter = 10;
            }
            //enemies
            if (enemiesv2 != null)
            {
                AnimateEnemiesV2sys(false);
            }
            if (bullets != null)
            {
                AnimateBullets();
            }
        }
        private void LoseLife()
        {
            Log.Info("LoseLife has been called");
            playerLives--;
            playerHealth = 100;
            Log.Info($"X = {player.Position.X}. Y = {player.Position.Y}");
            Log.Info($"respawn X = {respawnlocation.X}. respawnY = {respawnlocation.Y}");
            player.SetLocation(respawnlocation);
            Log.Info($"X = {player.Position.X}. Y = {player.Position.Y}");
            Log.Info($"respawn X = {respawnlocation.X}. respawnY = {respawnlocation.Y}");

        }
        private void AnimateEnemiesV2sys(bool loglevel, int animationClock)
        {
            if (loglevel)
            {
                Log.Info("AnimateEnemiesV2sys called");
            }

            for (int i = 0; i < enemiesv2.Count; i++)
            {
                if (loglevel)
                {
                    Log.Info("for loop accessed");
                }

                if (enemiesv2.ElementAt(i).sprite2d.HasBody())
                {
                    if (loglevel)
                    {
                        Log.Info("sprite has body");
                    }
                    if (logThisEnemy == true && i == loggedEnemyArrayID)
                    {
                        loglevel = true;
                    }
                    else
                    {
                        loglevel = false;
                    }
                    EnemyV2.AnimateThisV2Enemy(enemiesv2.ElementAt(i), animationClock, loglevel);
                    loglevel = false;
                    //}

                }
                else
                {
                    Log.Warning("sprite has no body");
                    EnemyV2.AnimateThisV2Enemy(enemiesv2.ElementAt(i), animationClock, loglevel);
                }

                
            }

        }
        public bool StillInput(bool nokey)
        {
            bool check;
            if (up == false && down == false && left == false && right == false && jump == false)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            nokey = check;
            return nokey;
        }
        private void AnimatePlayer(int start, int end)
        {
            //Log.Info("AnimatePlayer has been called");
            slowDownFrameRate += 1;
            if (slowDownFrameRate == 3)
            {
                steps++;
                slowDownFrameRate = 0;
            }
            if (steps > end || steps < start)
            {
                steps = start;
            }
            //player = new Sprite2d(new Vector2(player.Position.X, player.Position.Y), new Vector2(32, 32), playerSprites[steps], "Player");
            player.Sprite = playerSpritesBitmap[steps];
        }
        private void UpdatePlayerCamera()
        {
            if (player.Position.X <= 160)
            {
                CameraPosition.X = 0;
            }
            else if (player.Position.X > 160 && player.Position.X <= currentLevelEndSize - 160)
            {
                int diff = (int)player.Position.X - 160;
                CameraPosition.X = -diff;
            }
            else if (player.Position.X > currentLevelEndSize - 160)
            {
                if (currentLevelEndSize == 320)
                {
                    CameraPosition.X = currentLevelEndSize - 320;
                }
                else
                {
                    CameraPosition.X = -(currentLevelEndSize - 320);
                }
            }
        }
        private void GameStateHandler2()
        {
            if (isThisLevelClear)
            {
                SetLevelCleared(worlds, activeLevel);
            }
            //check wether to change level or go to screen or do nothing
            if (isPlayerRequestingLevel)
            {
                UnloadLastLevel();
                if (isOnScreen == true && whichScreen != -1)
                {
                    LoadArea(isOnScreen, whichScreen, worlds);
                }
                else
                {
                    LoadLevel(worlds, whichLevel);
                }
            }
            else if (isPlayerGoingNextRoom)
            {
                UnloadLastLevel();
                LoadLevel(activeLevel, nextRoomElementInt);
            }
            else
            {
                UnloadLastLevel();
                Log.Error("Cannot determine next level, returning to title screen.");
                LoadArea(true, 0, worlds);
            }

        }

        private void SetLevelCleared(List<WorldStructure> worlds, Level activeLevel)
        {
            int areanum = 0;
            int numindspot = 0;
            foreach (var world in worlds)
            {
                if (world.WorldName != "Utility" || world.isWorldClear != true)
                {
                    foreach (var area in world.Areas)
                    {
                        bool thisIsCleared = false;
                        foreach (var level in area.Levels)
                        {
                            if (level == activeLevel)
                            {
                                thisIsCleared = true;
                            }
                        }
                        area.isAreaClear = thisIsCleared;
                        areanum = area.AreaNumber;
                        break;
                    }
                }
                //what is the area number

                //does it exist in clearareas
                if (world.ClearAreas.Contains(areanum))
                {
                    //if yes, what index is it in clearareas
                    numindspot = world.ClearAreas.IndexOf(areanum);
                    //mark same index in areareasclear as true
                    world.AreAreasClear[numindspot] = true;
                }
                //check all areas, mark world clear
                bool hasAnyForgot = true;
                foreach (var areaclearbool in world.AreAreasClear)
                {
                    if (areaclearbool == false)
                    {
                        hasAnyForgot = false;
                    }
                    else 
                    { 
                        hasAnyForgot = true; 
                    }
                }
                if (hasAnyForgot == false)
                {
                    world.isWorldClear = true;
                }
            }
        }

        private void UnloadLastLevel()
        {
            RemoveEnemies();
            RemoveAllSprites();
        }
        private static void RemoveEnemies()
        {
            if (enemiesv2.Count != 0)
            {
                for (int i = 0; i < enemiesv2.Count; i++)
                {
                    enemiesv2.ElementAt(i).sprite2d.DestroySelf();
                    enemiesv2.ElementAt(i).sprite2d.DestroyStatic(enemiesv2.ElementAt(i).sprite2d);
                }
            }
        }
        private void LoadLevel(Level activeLevel, int nextRoomElementInt)
        {
            foreach (var world in worlds)
            {
                if (world.WorldName != "Utility")
                {
                    foreach (var area in world.Areas)
                    {
                        bool loadnext = false;
                        int areaElement = 0;
                        foreach (var level in area.Levels)
                        {
                            if (level == activeLevel)
                            {
                                areaElement = area.Levels.IndexOf(level) + 1;
                                loadnext = true;
                            }
                        }
                        if (loadnext)
                        {
                            LoadScreen(area, areaElement);
                        }

                    }
                }

            }
        }
        private void LoadLevel(List<WorldStructure> worlds, int[] whichLevel)
        {
            WorldStructure selectedWorld = worlds[whichLevel[0]];
            Area selectedLevel = selectedWorld.Areas[whichLevel[1]];
            if (selectedLevel.isAreaClear)
            {
                return;
            }
            else
            {
                LoadScreen(selectedLevel, 0);
            }
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
        private void LoadScreen(Area selectedLevel, int targetedLevel)
        {
            Level loadTarget = selectedLevel.Levels[targetedLevel];
            activeLevel = loadTarget;
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
        private void LoadScreen(int whichScreen, WorldStructure selectedWorld)
        {
            Level loadTarget = selectedWorld.Levels[whichScreen];
            activeLevel = loadTarget;
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
