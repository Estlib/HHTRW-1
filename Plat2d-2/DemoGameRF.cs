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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plat2d_2
{
    public enum CallState
    {
        TitleScreen,
        WorldMap,
        PlayingLevel,
        RoomTransition,
        GameOver,
        Cutscene,
        OptionsMenu
    }
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
        int remainingJumpSteps = 0;
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
        int selectedweapon = 0;
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
        public static bool isPlayerRequestingScreen = true;
        public static int whichScreen = 0;
        public static bool isPlayerRequestingLevel = false;
        public static int[] whichLevel = { 1, 0 };
        public static bool isPlayerGoingNextRoom = false;
        public static int nextRoomElementInt = 1;
        Level activeLevel;
        public static bool isThisLevelClear = false;
        public static KeyMode currentKeyMode = KeyMode.KeyBoard_Form;
        public static int[] currentLevel = { 1, 1 };



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
            worlds.Add(utility);
            worlds.Add(harenimus);

            //audio tool
            SetSFXengineB();
            SetJukebox();

            //weapon config
            unlockedWeapons.Add(Weapon.GetWeapon("debug"));
            activeWeapon = unlockedWeapons[selectedweapon];

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
            isPlayerRequestingScreen = true;
            whichScreen = 0;
            isPlayerRequestingLevel = false;
            LoadArea(isPlayerRequestingScreen, whichScreen, worlds);
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

            if (activeWeapon.FiringLock)
            {
                if (activeWeapon.FiringLockTimer != 0)
                {
                    //Log.Info("Subtracting one from firinglockcounter");
                    activeWeapon.FiringLockTimer--;
                }
                else if (activeWeapon.FiringLockTimer == 0)
                {
                    activeWeapon.FiringLock = false;
                }
            }
            if (activeWeapon.FiringLock == false)
            {
                //Log.Info("setting firinglockcounter to 10");
                activeWeapon.FiringLockTimer = 10;
            }

            //enemies
            if (enemiesv2 != null)
            {
                AnimateEnemiesV2sys(false, animationClock);
            }
            if (bullets != null)
            {
                AnimateBullets();
            }
            //shooting again
            if (fire == true && activeWeapon.FiringLock == false)
            {
                if (activeWeapon.AmmoLeft != 0)
                {
                    activeWeapon.AmmoLeft -= activeWeapon.AmmoConsumption;
                    activeWeapon.FiringLock = true;
                    if (bullets.Count <= 2 || bullets == null)
                    {
                        //shoot new bullet

                        sfxInstance.Play("W1 - single shot");
                        if (facedirection == 0)
                        {
                            if (down) //fires bullet lower than when standing
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the left at a lower altitude");
                                var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X, player.Position.Y + 16), new Vector2(8, 8), bulletgraphics[0], "Bullet"), true, "weapon1");
                                //newbullet.sprite2d.CreateDynamic();
                                bullets.Add(newbullet);
                                //shoot bullet to the left of player
                            }
                            else
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the left");
                                var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X, player.Position.Y + 8), new Vector2(8, 8), bulletgraphics[0], "Bullet"), true, "weapon1");
                                //newbullet.sprite2d.CreateDynamic();
                                bullets.Add(newbullet);
                                //shoot bullet to the left of player

                            }
                        }
                        else if (facedirection == 1)
                        {
                            if (down)//fires bullet lower than when standing
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the right at a lower altitude");
                                var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X + 32, player.Position.Y + 16), new Vector2(8, 8), bulletgraphics[0], "Bullet"), false, "weapon1");
                                //newbullet.sprite2d.CreateDynamic();
                                bullets.Add(newbullet);
                                //shoot bullet to the right of player

                            }
                            else
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the right");
                                var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X + 32, player.Position.Y + 8), new Vector2(8, 8), bulletgraphics[0], "Bullet"), false, "weapon1");
                                //newbullet.sprite2d.CreateDynamic();
                                bullets.Add(newbullet);
                                //shoot bullet to the right of player

                            }
                        }
                        currentbulletsonscreen++;
                        //Log.Info($"Bullet fired. Limit {maxbulletsallowed}. Onscreen {currentbulletsonscreen}");
                    }
                    else
                    {
                        LogUtility.LogCurrentWeaponState($"Fired Bullet Limit reached. Limit {maxbulletsallowed}. Onscreen {currentbulletsonscreen}", true);
                    }
                }
            }

            //player control & collisions
            if (respawntester)
            {
                player.SetLocation(respawnlocation);
            }
            if (up) //applies an impulse in the up direction when up key boolean is true
            {
                player.ApplyImpulse(new Vector2(0, -160000), Vector2.Zero());
            }
            if (down) //return a console message when down key boolean is true
            {
                //Log.Warning("Down has no action currently. Sprite change is visual only.");
                //TODO: shrink the collider when ducking.
            }
            if (left) //applies a velocity to the player in the left direction when left key boolean is true
            {
                player.SetVelocity(new Vector2(-120, player.GetYVelocity()));
            }
            if (right) //applies a velocity to the player in the right direction when right key boolean is true
            {
                player.SetVelocity(new Vector2(120, player.GetYVelocity()));
            }
            if (jump) //performs jumpstepping when the jump key boolean is true
            {
                if (player.IsColliding("Ground") != null) //if player is colliding with the ground, only then allow the player to jump
                {
                    sfxInstance.Play("jump");
                    remainingJumpSteps = 9; //jump steps are set to 9 frames
                }
                jumpmode = true; //sets the jumpmode as true, the player is currently jumping
            }
            if (remainingJumpSteps > 0) //if the jump steps are greater than 0
            {
                player.SetVelocity(new Vector2(player.GetXVelocity(), -12800)); //then it applies a velocity to the player in the up direction, forming a jump
                remainingJumpSteps--; //subtract a frame from the jumpsteps
            }
            if (player.IsColliding("Ground") != null) //if player is colliding with ground then it sets current jumpmode to false, as player is not currently jumping and logs a message
            {
                jumpmode = false;
                //Log.Info("Player is colliding with Ground");
                //ground.DestroyStatic(player);
            }
            else if (player.IsColliding("Ground") == null) //if player is not colliding with ground then it logs a warning to the console that the player cant press jump key.
            {
                //Log.Warning("Player is not colliding with Ground and thus cannot jump.");
                //jumpstate handling for this is done in the if() structure that checks for jumpsteps
            }
            player.UpdatePosition(); //updates players position

            //enemies' bizarre encounter with boolet
            if (enemiesv2 != null)
            {
                for (int i = 0; i < enemiesv2.Count; i++)
                {
                    EnemyV2 enemyobject = enemiesv2[i];
                    enemyobject.sprite2d.UpdatePosition();
                    if (enemyobject.sprite2d.IsColliding("Bullet") != null)
                    {
                        foreach (var bullet in bullets)
                        {
                            if (bullet.sprite2d.IsColliding("Enemy") != null)
                            {
                                bullet.sprite2d.Tag = "RemoveThis";
                            }
                        }
                        sfxInstance.Play("enemy ow");
                        pointScoreTally += 250;
                        enemyobject.sprite2d.DestroySelf();
                        enemyobject.sprite2d.DestroyStatic(enemyobject.sprite2d);
                        enemiesv2.Remove(enemiesv2.ElementAt(i));
                        foreach (var bullet in bullets)
                        {
                            if (bullet.sprite2d.Tag != "Bullet")
                            {
                                bullet.sprite2d.DestroySelf();
                                bullets.Remove(bullet);
                            }
                        }
                    }
                }
            }

            //bullet cleanup
            if (bullets != null)
            {
                for (int i = 0; i < weapon1cyclespeed; i++)
                {
                    foreach (var bullet in bullets)
                    {
                        if (bullet.isfacingleft == true)
                        {
                            bullet.sprite2d.AdvanceLeft(weapon1speed);
                        }
                        else
                        {
                            bullet.sprite2d.AdvanceRight(weapon1speed);
                        }
                        //bullet.sprite2d.UpdatePosition();
                    }
                    foreach (var bullet in bullets)
                    {
                        if (bullet.sprite2d.Position.X - player.Position.X < -256 || bullet.sprite2d.Position.X - player.Position.X > 256)
                        {
                            bullet.sprite2d.DestroySelf();
                            bullets.Remove(bullet);
                        }
                    }

                }
                if (bullets.Count() == 0)
                {
                    LogUtility.LogCurrentWeaponState("No bullets remain on screen");
                }

            }

            //object collisions
            Sprite2d coin = player.IsColliding("Coin"); //checks for collisions between the player and the coin.
            if (coin != null) //if the coin is being touched
            {
                sfxInstance.Play("Gem collect");
                pointScoreTally += 100;
                crystalScoreTally++;
                Log.Info($"Coin is being touched. Current Crystalcount: {crystalScoreTally}"); //then it logs a message to the console
                coin.DestroySelf(); //and destroys the object
            }

            Sprite2d levelfinish = player.IsColliding("Finish"); //checks for collisions between the player and the level finishing trigger object.
            if (levelfinish != null) //if the trigger object is being touched
            {
                isPlayerRequestingLevel = true;
                //TODO; check if world is completed, then increment worldcounter, and load next world map instead
                whichLevel = new int[] { 1, -1 };
                Log.Info("Player has triggered level finish"); //then it logs a message to the console
                levelfinish.DestroySelf(); //destroys itself
                BGMPlayer.PlayNow(jukeBox.ElementAt(7).Filepath);
            }

            Sprite2d levelstart = player.IsColliding("Start");
            if (levelstart != null)
            {
                isPlayerRequestingLevel = true;
                whichLevel = levelstart.worldData;
                return;
            }

            Sprite2d nextroom = player.IsColliding("NextRoom");
            if (nextroom != null) //if the trigger object is being touched
            {
                isPlayerGoingNextRoom = true;
                whichLevel[1]++;
                Log.Info("Player is going to next room"); //then it logs a message to the console
                nextroom.DestroySelf(); //destroys itself
            }

            Sprite2d enemy = player.IsColliding("Enemy"); //checks for collisions between the player and the level finishing trigger object.
            Sprite2d bulletcollision = null;
            if (enemy != null) //if the trigger object is being touched
            {
                playerHealth--;
                bulletcollision = enemy.IsColliding("Bullet");
                //Log.Info($"Player has touched an enemy. Health left: {playerHealth}. Lives left: {playerLives}"); //then it logs a message to the console
            }
            if (bulletcollision != null)
            {
                pointScoreTally += 250;
                enemy.DestroySelf();
            }

            //frame measuring tools again
            double frameTime = stopwatch.Elapsed.TotalMilliseconds - frameStart;
            double remaining = MaxFT - frameTime;
            // coarse wait first
            if (remaining > 2.0)
            {
                Thread.Sleep((int)(remaining - 1.0));
            }
            // fine wait for the last bit
            while (stopwatch.Elapsed.TotalMilliseconds - frameStart < MaxFT)
            {
                Thread.SpinWait(20);
            }
            // final frame time after waiting
            frameTime = stopwatch.Elapsed.TotalMilliseconds - frameStart;
            fpsCounter++;
            if (fpsTimer.Elapsed.TotalMilliseconds >= 1000)
            {
                LogUtility.LogCurrentFrame($"FPS: {fpsCounter}, FrameTime: {frameTime}ms                   ");
                fpsCounter = 0;
                fpsTimer.Restart();
            }

        }
        private void AnimateBullets()
        {
            foreach (var bullet in bullets)
            {
                if (bullet.weaponName == "weapon1")
                {
                    if (bullet.sprite2d.Sprite == bulletgraphics[0])
                    {
                        bullet.sprite2d.Sprite = bulletgraphics[1];
                    }
                    else
                    {
                        bullet.sprite2d.Sprite = bulletgraphics[0];
                    }
                }
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
                if (whichLevel[1] == -1)
                {
                    Log.Error($"world {whichLevel[0]} with levelid {whichLevel[1]} does not exist");
                    isPlayerRequestingLevel = false;
                    isPlayerRequestingScreen = true;
                    whichScreen = 0;
                    return;
                }
                LoadLevel(worlds, whichLevel);
                isPlayerRequestingLevel = false;
            }
            else if (isPlayerGoingNextRoom)
            {
                UnloadLastLevel();
                LoadLevel(activeLevel, nextRoomElementInt);
                isPlayerGoingNextRoom = false;
            }
            else if (isPlayerRequestingScreen)
            {
                UnloadLastLevel();
                LoadArea(isPlayerRequestingScreen, whichScreen, worlds);
                isPlayerRequestingScreen = false;
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
            Area selectedLevel_a = new Area() { };
            Level selectedLevel_l = new Level() { };
            if (selectedWorld.Areas != null)
            {
                if (whichLevel[1]==99)
                {
                    selectedLevel_a = new Area
                    (
                        new List<Level> { selectedWorld.WorldMap },
                        0,
                        "World Map"
                    );
                }
                else
                {
                    selectedLevel_a = selectedWorld.Areas[whichLevel[1]];
                }
                    
            }
            if (selectedWorld.Levels != null)
            {
                selectedLevel_l = selectedWorld.Levels[whichLevel[1]];
            }

            if (whichLevel[1] != -1)
            {
                if (selectedWorld.WorldName != "Utility")
                {
                    if (selectedLevel_a.isAreaClear)
                    {
                        return;
                    }
                    else
                    {
                        LoadScreen(selectedLevel_a, whichLevel[1]);
                    }
                }
                else
                {
                    LoadLevel(selectedWorld.WorldMap, 0);
                }
            }
            else
            {
                Area wm = new Area
                    (
                        new List<Level> { selectedWorld.WorldMap },
                        0,
                        "World Map"
                    );
                LoadScreen(wm, 0);
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
                    if (world.WorldName == "Utility")
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

                            if (artTagDefinitions[tryint] == "Start")
                            {
                                var datasprite = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), artRefs[tryint], artTagDefinitions[tryint])/*.CreateStatic()*/;
                                datasprite.worldData = new int[] { 1, 99 };
                            }
                            else
                            {
                                new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), artRefs[tryint], artTagDefinitions[tryint])/*.CreateStatic()*/;
                            }
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
            //List<string> levelsNames = new List<string>();
            //foreach (var level in levels)
            //{
            //    levelsNames.Add(level.levelname);
            //}
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
        public override void UpdateHud()
        {
            //Log.Highlight("UpdateHud() has no function");
            //float aspect = ScreenSize.X / ScreenSize.Y;

            //float drawHeight = Window.ClientSize.Height;
            //float drawWidth = drawHeight * aspect;

            //float offsetX = (Window.ClientSize.Width - drawWidth) / 2f;
            //float offsetY = 0f;

            //float hudScaleX = drawWidth / ScreenSize.X;
            //float hudScaleY = drawHeight / ScreenSize.Y;

            //CrystalLabel.BackColor = System.Drawing.Color.Black;
            //CrystalLabel.ForeColor = System.Drawing.Color.White;
            //CrystalLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 32 * hudScaleX),
            //    (int)(offsetY + 216 * hudScaleY)
            //);

            //HealthLabel.BackColor = System.Drawing.Color.Black;
            //HealthLabel.ForeColor = System.Drawing.Color.White;
            //HealthLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 128 * hudScaleX),
            //    (int)(offsetY + 216 * hudScaleY)
            //);

            //LivesLabel.BackColor = System.Drawing.Color.Black;
            //LivesLabel.ForeColor = System.Drawing.Color.White;
            //LivesLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 160 * hudScaleX),
            //    (int)(offsetY + 216 * hudScaleY)
            //);

            //AmmoLabel.BackColor = System.Drawing.Color.Black;
            //AmmoLabel.ForeColor = System.Drawing.Color.White;
            //AmmoLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 188 * hudScaleX),
            //    (int)(offsetY + 216 * hudScaleY)
            //);

            //ScoreLabel.BackColor = System.Drawing.Color.Black;
            //ScoreLabel.ForeColor = System.Drawing.Color.White;
            //ScoreLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 64 * hudScaleX),
            //    (int)(offsetY + 216 * hudScaleY)
            //);

            //SelectedWeaponLabel.BackColor = System.Drawing.Color.Black;
            //SelectedWeaponLabel.ForeColor = System.Drawing.Color.White;
            //SelectedWeaponLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 32 * hudScaleX),
            //    (int)(offsetY + 224 * hudScaleY)
            //);

            //LevelLabel.BackColor = System.Drawing.Color.Black;
            //LevelLabel.ForeColor = System.Drawing.Color.Yellow;
            //LevelLabel.Location = new System.Drawing.Point(
            //    (int)(offsetX + 0 * hudScaleX),
            //    (int)(offsetY + 0 * hudScaleY)
            //);

            //if (CrystalLabel != null)
            //    CrystalLabel.Text = $"{DemoGame.crystalScoreTally}";

            //if (HealthLabel != null)
            //    HealthLabel.Text = $"{DemoGame.playerHealth}";

            //if (LivesLabel != null)
            //    LivesLabel.Text = $"{DemoGame.playerLives}";

            //if (AmmoLabel != null)
            //{
            //    if (weapon1Ammo < 0)
            //        AmmoLabel.Text = "∞";
            //    else
            //        AmmoLabel.Text = $"{DemoGame.weapon1Ammo}";
            //}

            //if (ScoreLabel != null)
            //    ScoreLabel.Text = $"{DemoGame.pointScoreTally}";

            //if (LevelLabel != null)
            //    LevelLabel.Text = $"{CheckLevel(levelclearingsforlabel)}";
        }
        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { up = true; }
            if (e.KeyCode == Keys.Left) { left = true; }
            if (e.KeyCode == Keys.Down) { down = true; }
            if (e.KeyCode == Keys.Right) { right = true; }
            if (e.KeyCode == Keys.Z) { fire = true; }
            if (e.KeyCode == Keys.Space) { jump = true; }
            if (e.KeyCode == Keys.Q) { respawntester = true; }
            if (e.KeyCode == Keys.Enter) { togglePause(); }
            //if (e.KeyCode == Keys.Return) { EngineCore.EngineCore.pausebuttoninput = !pausebuttoninput; }
        }
        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { up = false; }
            if (e.KeyCode == Keys.Left) { left = false; }
            if (e.KeyCode == Keys.Down) { down = false; }
            if (e.KeyCode == Keys.Right) { right = false; }
            if (e.KeyCode == Keys.Z) { fire = false; }
            if (e.KeyCode == Keys.Space) { jump = false; }
            if (e.KeyCode == Keys.Q) { respawntester = false; }
        }
        private void togglePause()
        {
            pauseGameKey = !pauseGameKey;
        }
    }
}
