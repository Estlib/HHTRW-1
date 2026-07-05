using Box2DX.Common;
using Box2DX.Dynamics;
using Plat2d_2.EngineCore;
using Plat2d_2.EngineCore.ObjectControllers;
using Plat2d_2.EngineCore.ObjectTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plat2d_2
{
    public struct Destination
    {
        public int WorldNumber;
        public int AreaNumber;
        public int LevelNumber;
        public bool IsWorldMap;
        public Destination(int worldNumber, int areaNumber, int levelNumber)
        {
            WorldNumber = worldNumber;
            AreaNumber = areaNumber;
            LevelNumber = levelNumber;
            IsWorldMap = false;
        }
        public Destination(int worldNumber, bool isWorldMap = true)
        {
            WorldNumber = worldNumber;
            AreaNumber = 0;
            LevelNumber = 0;
            IsWorldMap = isWorldMap;
        }
    }
    public enum NavigationReason
    {
        None,
        LevelCleared,
        ReturnToMap,
        ToTitleScreen,
        ToOptions,
    }
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
        bool nextweapon;
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
        bool isInvincible = false;
        bool overrideSprite = false;
        int invincibilityForFrames = 125;
        int overrideSpriteForFrames = 5;
        int hasBeenInvincibleFor = 0;
        int invincibleFrameHasBeenDisplayedFor = 0;
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
        public static int playerHealth = 3;
        public static int playerLives = 5;
        List<Bitmap> HudBMP = ArtData.HudSprites();
        List<Bitmap> DigitBMP = ArtData.DigitSprites();
        List<Bitmap> WeaponIcons = ArtData.WeaponIconSprites();
        List<Bitmap> BarElements = ArtData.BarElementSprites();
        List<Bitmap> WeaponNames = ArtData.WeaponNameSprites();
        Sprite2d hud;
        List<HUDObject> Lives = new List<HUDObject>();
        List<HUDObject> Gems = new List<HUDObject>();
        List<HUDObject> ScoreNumbers = new List<HUDObject>();
        List<HUDObject> HealthLeftItems = new List<HUDObject>();
        List<HUDObject> AmmoLeftItems = new List<HUDObject>();
        HUDObject weaponicon;
        HUDObject weaponname;

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
        public static CallState state = CallState.TitleScreen;
        Level activeLevel;
        public static bool isThisLevelClear = false;
        public static KeyMode currentKeyMode = KeyMode.KeyBoard_Form;
        public static Destination reloadDestination = new Destination(0, 0, 0);
        public static Destination currentDestination = new Destination(0, 0, 0);
        public static bool transitionIsCalled = true;
        public static NavigationReason navCause = NavigationReason.ToTitleScreen;
        int keyTimeoutX = 25;
        Random rngFrom8To8 = new Random();
        int waitForThisManyFrames = 100;


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
            int gameHasWaitedFor = 0;
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
            DONE
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
            unlockedWeapons.Add(Weapon.GetWeapon(""));
            unlockedWeapons.Add(Weapon.GetWeapon("plasma"));
            unlockedWeapons.Add(Weapon.GetWeapon("flint"));
            unlockedWeapons.Add(Weapon.GetWeapon("debug"));
            activeWeapon = unlockedWeapons[selectedweapon];

            //player config
            //integer value for holding the current player sprite, through which the list is accessed and player is animated through the use of
            currentSprite = 0; //TODO; verify necessity
            currentKeyMode = KeyMode.KeyBoard_Form;

            foreach (var sfxR in allSFX)
            {
                sfxInstance.RegisterSound(sfxR.Name, sfxR.Filepath);
                Log.Info($"{sfxR.Name}");
            }
            LogUtility.ClearLineOnly(6);

            //Start game on title screen   \/
            state = CallState.TitleScreen;
            GoToLevel(state, reloadDestination, worlds);


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

            //playerdata buffervalues
            int bufferLives = playerLives;
            int bufferCrystalScoreTally = crystalScoreTally;

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
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ██ ░░ ░░ ░░ ");
                    break;
                case 1:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ██ ░░ ░░ ");
                    break;
                case 2:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ██ ░░ ");
                    break;
                case 3:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ░░ ██ ");
                    break;
                case 4:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ░░ █4 ");
                    break;
                default:
                    LogUtility.LogCurrentAnimationState($"Animation clock is currently {animationClock} ░░ ░░ ░░ ░░ ");
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

                sfxInstance.Play("GEMS 1-up");
            }

            //weapon selecting
            if (nextweapon && keyTimeoutX == 0)
            {
                keyTimeoutX = 25;
                sfxInstance.Play("ammo notification");
                if (selectedweapon != unlockedWeapons.Count)
                {
                    selectedweapon++;
                }
                else
                {
                    selectedweapon = 0;
                }
                activeWeapon = unlockedWeapons[selectedweapon];
                Log.Normal("Weapon in active slot: " + activeWeapon.WeaponName);
                Log.Info("FiringLock: " + activeWeapon.FiringLock.ToString());
                Log.Info("AmmoConsumption: " + activeWeapon.AmmoConsumption);
                Log.Info("FiringLockTimer: " + activeWeapon.FiringLockTimer);
                Log.Info("AmmoLeft: " + activeWeapon.AmmoLeft);
                Log.Info("MaxBulletCount: " + activeWeapon.MaxBulletCount);
                Log.Info("ThisWeaponType: " + activeWeapon.ThisWeaponType);
                Log.Info("SpriteCount: " + activeWeapon.Graphics.Count);
                //UpdateHud(); dont update entire hud
                SetCurrentWeapon(weaponicon, weaponname);
                SetHudAmmobar(AmmoLeftItems);
            }
            else
            {
                //sfxInstance.Play("error");
            }
            if (keyTimeoutX > 0)
            {
                keyTimeoutX--;
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
                activeWeapon.FiringLockTimer = activeWeapon.FiringLockTimerOriginal;
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

            if (up == true && down == true)
            {
                activeWeapon.AmmoLeft = 12;
            }

            if (activeWeapon.AmmoLeft < 1)
            {
                activeWeapon.FiringLock = true;
            }

            //shooting again
            if (fire == true && activeWeapon.FiringLock == false) //if player can fire and has pressed shoot button
            {
                if (activeWeapon.AmmoLeft != 0 || activeWeapon.AmmoLeft > 0) //if the ammoleft is not 0 or less
                {
                    if (activeWeapon.WeaponName != "debug") //and incase it is not debug weapon
                    {
                        activeWeapon.AmmoLeft -= activeWeapon.AmmoConsumption; //then remove some ammo
                    }
                    activeWeapon.FiringLock = true; //enable firinglock for current inhand
                    if (bullets.Count < activeWeapon.MaxBulletCount || bullets == null) //only if max bulletcount onscreen is not reached
                    {

                        //shoot new bullet
                        int heightmod = 0;
                        if (activeWeapon.WeaponName == "Willo") //Special condition for "Willo" weapon
                        {
                            heightmod = rngFrom8To8.Next(-6, 6);
                        }
                        else if (activeWeapon.WeaponName == "Väits") //Special condition for "Väits" weapon
                        {
                            heightmod = -8;
                        }
                        GunSound(activeWeapon.WeaponName); //play corresponding gunsound
                        if (facedirection == 0) //if player is facing left
                        {
                            if (down) //fires bullet lower than when standing
                            {

                                LogUtility.LogCurrentWeaponState("Bullet is fired to the left at a lower altitude");
                                if (activeWeapon.WeaponName == "Väits")
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X - 24, player.Position.Y + 16 + heightmod), new Vector2(24, 24), activeWeapon.Graphics[0], "Bullet"), true, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                else
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X, player.Position.Y + 16 + heightmod), new Vector2(8, 8), activeWeapon.Graphics[0], "Bullet"), true, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                //newbullet.sprite2d.CreateDynamic();
                                //shoot bullet to the left of player
                            }
                            else //and player is doing anything else, other than ducking
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the left");
                                if (activeWeapon.WeaponName == "Väits")
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X - 24, player.Position.Y + 8 + heightmod), new Vector2(24, 24), activeWeapon.Graphics[0], "Bullet"), true, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                else
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X, player.Position.Y + 8 + heightmod), new Vector2(8, 8), activeWeapon.Graphics[0], "Bullet"), true, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                //newbullet.sprite2d.CreateDynamic();
                                //shoot bullet to the left of player

                            }
                        }
                        else if (facedirection == 1)
                        {
                            if (down)//fires bullet lower than when standing
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the right at a lower altitude");
                                if (activeWeapon.WeaponName == "Väits")
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X + 32, player.Position.Y + 16 + heightmod), new Vector2(24, 24), activeWeapon.Graphics[0], "Bullet"), false, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                else
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X + 32, player.Position.Y + 16 + heightmod), new Vector2(8, 8), activeWeapon.Graphics[0], "Bullet"), false, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                //newbullet.sprite2d.CreateDynamic();
                                //shoot bullet to the right of player

                            }
                            else
                            {
                                LogUtility.LogCurrentWeaponState("Bullet is fired to the right");
                                if (activeWeapon.WeaponName == "Väits")
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X + 32, player.Position.Y + 8 + heightmod), new Vector2(24, 24), activeWeapon.Graphics[0], "Bullet"), false, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                else
                                {
                                    var newbullet = new Bullet(new Sprite2d(new Vector2(player.Position.X + 32, player.Position.Y + 8 + heightmod), new Vector2(8, 8), activeWeapon.Graphics[0], "Bullet"), false, activeWeapon.WeaponName);
                                    bullets.Add(newbullet);
                                }
                                //newbullet.sprite2d.CreateDynamic();
                                //shoot bullet to the right of player

                            }
                        }
                        currentbulletsonscreen++;
                        
                        //Log.Info($"Bullet fired. Limit {maxbulletsallowed}. Onscreen {currentbulletsonscreen}");
                    }
                    else
                    {
                        //sfxInstance.Play("error");
                        LogUtility.LogCurrentWeaponState($"Fired Bullet Limit reached. Limit {activeWeapon.MaxBulletCount}. Onscreen {currentbulletsonscreen}", true);
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
                player.ApplyImpulse(new Vector2(0, -160000), Vector2.Zero());
                //player.SetVelocity(new Vector2(player.GetXVelocity(), -12800)); //then it applies a velocity to the player in the up direction, forming a jump
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
            bool onGround = player.IsColliding("Ground") != null;

            if (onGround && player.GetYVelocity() > 0)
            {
                player.SetVelocity(new Vector2(player.GetXVelocity(), 0));
                remainingJumpSteps = 0;
                jumpmode = false;
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
                        SetHudScore(ScoreNumbers);
                        enemyobject.sprite2d.DestroySelf();
                        enemyobject.sprite2d.DestroyStatic(enemyobject.sprite2d);
                        enemiesv2.Remove(enemiesv2.ElementAt(i));
                        foreach (var bullet in bullets)
                        {
                            if (bullet.sprite2d.Tag != "Bullet")
                            {
                                if (bullet.weaponName != "Väits")
                                {
                                    bullet.sprite2d.DestroySelf();
                                    bullets.Remove(bullet);
                                }

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
                            if (bullet.weaponName != "Väits")
                            {
                                bullet.sprite2d.AdvanceLeft(weapon1speed);
                            }
                            else
                            {
                                bullet.sprite2d.AtPlayerLocation();
                            }
                        }
                        else
                        {
                            if (bullet.weaponName != "Väits")
                            {
                                bullet.sprite2d.AdvanceRight(weapon1speed);
                            }
                            else
                            {
                                bullet.sprite2d.AtPlayerLocation();
                            }
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
                        if (bullet.weaponName == "Willo")
                        {
                            Sprite2d hitblock = bullet.sprite2d.IsColliding("Ground");
                            if (hitblock != null)
                            {
                                bullet.sprite2d.DestroySelf();
                                bullets.Remove(bullet);
                            }
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
                SetHudScore(ScoreNumbers);
                SetHudGems(Gems);
                Log.Info($"Coin is being touched. Current Crystalcount: {crystalScoreTally}"); //then it logs a message to the console
                coin.DestroySelf(); //and destroys the object

            }

            Sprite2d levelfinish = player.IsColliding("Finish"); //checks for collisions between the player and the level finishing trigger object.
            if (levelfinish != null) //if the trigger object is being touched
            {

                transitionIsCalled = true;
                reloadDestination = new Destination(reloadDestination.WorldNumber);
                state = CallState.WorldMap;
                navCause = NavigationReason.LevelCleared;
                Log.Info("Player has triggered level finish"); //then it logs a message to the console
                levelfinish.DestroySelf(); //destroys itself
                BGMPlayer.PlayNow(jukeBox.ElementAt(6).Filepath);
                Thread.Sleep(5000);
                return;
            }

            Sprite2d levelstart = player.IsColliding("Start");
            if (levelstart != null)
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(1);
                navCause = NavigationReason.ReturnToMap;
                return;
            }

            Sprite2d setlevel1 = player.IsColliding("Level1");
            if (setlevel1 != null && CheckDestination(new Destination(currentDestination.WorldNumber, 0, 0), worlds[currentDestination.WorldNumber]))
            {
                Destination candidateDestination = new Destination(currentDestination.WorldNumber, 0, 0);
                transitionIsCalled = true;
                reloadDestination = candidateDestination;
                return;
            }
            Sprite2d setlevel2 = player.IsColliding("Level2");
            if (setlevel2 != null && CheckDestination(new Destination(currentDestination.WorldNumber, 1, 0), worlds[currentDestination.WorldNumber]))
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(currentDestination.WorldNumber, 1, 0);
                return;
            }
            Sprite2d setlevel3 = player.IsColliding("Level3");
            if (setlevel3 != null && CheckDestination(new Destination(currentDestination.WorldNumber, 2, 0), worlds[currentDestination.WorldNumber]))
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(currentDestination.WorldNumber, 2, 0);
                return;
            }
            Sprite2d setlevel4 = player.IsColliding("Level4");
            if (setlevel4 != null && CheckDestination(new Destination(currentDestination.WorldNumber, 3, 0), worlds[currentDestination.WorldNumber]))
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(currentDestination.WorldNumber, 3, 0);
                return;
            }
            Sprite2d setlevel5 = player.IsColliding("Level5");
            if (setlevel5 != null && CheckDestination(new Destination(currentDestination.WorldNumber, 4, 0), worlds[currentDestination.WorldNumber]))
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(currentDestination.WorldNumber, 4, 0);
                return;
            }
            Sprite2d setlevel6 = player.IsColliding("Level6");
            if (setlevel6 != null && CheckDestination(new Destination(currentDestination.WorldNumber, 5, 0), worlds[currentDestination.WorldNumber]))
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(currentDestination.WorldNumber, 5, 0);
                return;
            }

            Sprite2d nextroom = player.IsColliding("NextRoom");
            if (nextroom != null) //if the trigger object is being touched
            {
                transitionIsCalled = true;
                reloadDestination = new Destination(reloadDestination.WorldNumber, reloadDestination.AreaNumber, reloadDestination.LevelNumber + 1);
                Log.Info("Player is going to next room"); //then it logs a message to the console
                nextroom.DestroySelf(); //destroys itself
                sfxInstance.Play("enter door");
                return;
            }

            Sprite2d enemy = player.IsColliding("Enemy"); //checks if player is kissing an enemy
            Sprite2d bulletcollision = null;
            if (enemy != null & isInvincible == false) //if the trigger object is being touched
            {
                playerHealth--;
                SetHudHealthbar(HealthLeftItems);
                //todo, call hud update for health
                bulletcollision = enemy.IsColliding("Bullet");
                isInvincible = true;
                overrideSprite = true;
                //knockback on hurt
                if (facedirection == 0)
                {
                    player.ApplyImpulse(new Vector2(320000, -320000), Vector2.Zero());
                }
                else if (facedirection == 1)
                {
                    player.ApplyImpulse(new Vector2(-320000, -320000), Vector2.Zero());
                }
                //Log.Info($"Player has touched an enemy. Health left: {playerHealth}. Lives left: {playerLives}"); //then it logs a message to the console
            }
            if (bulletcollision != null)
            {
                pointScoreTally += 250;
                SetHudScore(ScoreNumbers);
                //Todo: call ammo count lowering too
                enemy.DestroySelf();
            }

            //hud management

            if (bufferLives != playerLives)
            {
                SetHudLives(Lives);
            }
            if (bufferCrystalScoreTally != crystalScoreTally)
            {
                SetHudGems(Gems);
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

        private void GunSound(string weaponName)
        {
            switch (weaponName)
            {
                case "debug":
                    sfxInstance.Play("W1 - single shot");
                    break;
                case "Väits":
                    sfxInstance.Play("swipe");
                    break;
                case "Barker":
                    sfxInstance.Play("flintlock");
                    break;
                case "Willo":
                    sfxInstance.Play("W3 - continuous shot");
                    break;
                default:
                    sfxInstance.Play("error");
                    break;
            }
        }

        private bool CheckDestination(Destination candidateDestination, WorldStructure worldStructure)
        {
            if (worldStructure.AreAreasClear[candidateDestination.AreaNumber] == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GoToLevel(CallState state, Destination reloadDestination, List<WorldStructure> worlds)
        {
            if (navCause == NavigationReason.ReturnToMap)
            {
                LoadScreen(new Area
                {
                    Levels = new List<Level> { worlds[reloadDestination.WorldNumber].WorldMap },
                    AreaNumber = 0,
                    AreaName = worlds[reloadDestination.WorldNumber].WorldMap.levelname
                },
                0
                );
            }
            else
            {
                LoadScreen(
                    worlds[reloadDestination.WorldNumber].Areas[reloadDestination.AreaNumber],
                    reloadDestination.LevelNumber
                    );
            }
        }
        private void AnimateBullets()
        {
            foreach (var bullet in bullets)
            {
                Weapon weapon = unlockedWeapons.FirstOrDefault(w => w.WeaponName == bullet.weaponName);
                switch (weapon.ThisWeaponType)
                {
                    case WeaponType.Wielded:
                        if (weapon == null || weapon.Graphics == null || weapon.Graphics.Count == 0)
                            continue;

                        int currentFrameW = weapon.Graphics.IndexOf(bullet.sprite2d.Sprite);

                        if (currentFrameW == -1 && animationClock == 3)
                        {
                            bullet.sprite2d.Sprite = weapon.Graphics[0];
                            continue;
                        }

                        int nextFrameW = currentFrameW + 1;
                        if (nextFrameW >= weapon.Graphics.Count && animationClock == 3)
                        {
                            bullet.sprite2d.DestroySelf();
                            bullets.Remove(bullet);
                        }

                        bullet.sprite2d.Sprite = weapon.Graphics[nextFrameW];
                        break;
                    case WeaponType.Shot:
                        if (weapon == null || weapon.Graphics == null || weapon.Graphics.Count == 0)
                            continue;

                        int currentFrame = weapon.Graphics.IndexOf(bullet.sprite2d.Sprite);

                        if (currentFrame == -1)
                        {
                            bullet.sprite2d.Sprite = weapon.Graphics[0];
                            continue;
                        }

                        int nextFrame = currentFrame + 1;
                        if (nextFrame >= weapon.Graphics.Count)
                            nextFrame = 0;

                        bullet.sprite2d.Sprite = weapon.Graphics[nextFrame];
                        break;
                    case WeaponType.Gravity:
                        break;
                    case WeaponType.Tool:
                        break;
                    case WeaponType.Screen:
                        break;
                    default:
                        break;
                }

            }
        }

        //private void AnimateBullets()
        //{
        //    foreach (var bullet in bullets)
        //    {
        //        switch (bullet.weaponName)
        //        {
        //            //case "Barker":
        //            //    Weapon barker = unlockedWeapons.SingleOrDefault(w => w.WeaponName == "Barker");
        //            //    int barkerTotalFrames = barker.Graphics.Count;
        //            //    int currentframeb = barker.Graphics.IndexOf(bullet.sprite2d.Sprite);
        //            //    if (currentframeb++ < barkerTotalFrames)
        //            //    {
        //            //        bullet.sprite2d.Sprite = barker.Graphics[currentframeb + 1];
        //            //    }
        //            //    else
        //            //    {
        //            //        bullet.sprite2d.Sprite = barker.Graphics[0];
        //            //    }
        //            //    break;
        //            case "debug":
        //                Weapon debug = unlockedWeapons.SingleOrDefault(w => w.WeaponName == "debug");
        //                int debugTotalFrames = debug.Graphics.Count-1;
        //                int currentframed = debug.Graphics.IndexOf(bullet.sprite2d.Sprite);
        //                if (currentframed++ < debugTotalFrames)
        //                {
        //                    bullet.sprite2d.Sprite = debug.Graphics[currentframed];
        //                }
        //                else
        //                {
        //                    bullet.sprite2d.Sprite = debug.Graphics[0];
        //                }
        //                break;
        //            case "Willo":
        //                Weapon willo = unlockedWeapons.SingleOrDefault(w => w.WeaponName == "Willo");
        //                int willoTotalFrames = willo.Graphics.Count;
        //                int currentframew = willo.Graphics.IndexOf(bullet.sprite2d.Sprite);
        //                if (currentframew++ < willoTotalFrames)
        //                {
        //                    bullet.sprite2d.Sprite = willo.Graphics[currentframew + 1];
        //                }
        //                else
        //                {
        //                    bullet.sprite2d.Sprite = willo.Graphics[0];
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        private void LoseLife()
        {
            Log.Info("LoseLife has been called");
            playerLives--;
            playerHealth = 3;
            Log.Info($"X = {player.Position.X}. Y = {player.Position.Y}");
            Log.Info($"respawn X = {respawnlocation.X}. respawnY = {respawnlocation.Y}");
            player.SetLocation(respawnlocation);
            Log.Info($"X = {player.Position.X}. Y = {player.Position.Y}");
            Log.Info($"respawn X = {respawnlocation.X}. respawnY = {respawnlocation.Y}");
            SetHudLives(Lives);
            SetHudHealthbar(HealthLeftItems);
            //TODO: kick player out of level, every time after they lose a life.

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
            if (isInvincible)
            {
                //Log.Highlight("isInvincible " + isInvincible + " /overrideSprite " + overrideSprite);
                //Log.Highlight("invincibilityForFrames " + invincibilityForFrames + " /overrideSpriteForFrames " + overrideSpriteForFrames);
                //Log.Highlight("hasBeenInvincibleFor " + hasBeenInvincibleFor+ " /invincibleFrameHasBeenDisplayedFor "+ invincibleFrameHasBeenDisplayedFor);
                if (hasBeenInvincibleFor < invincibilityForFrames)
                {
                    if (invincibleFrameHasBeenDisplayedFor < overrideSpriteForFrames)
                    {
                        
                        invincibleFrameHasBeenDisplayedFor++;
                    }
                    else
                    {
                        overrideSprite = !overrideSprite;
                        invincibleFrameHasBeenDisplayedFor = 0;
                    }
                    hasBeenInvincibleFor++;
                }
                else
                {
                    hasBeenInvincibleFor = 0;
                    isInvincible = false;
                }
            }
            if (!isInvincible)
            {
                player.Sprite = playerSpritesBitmap[steps];
            }
            else if (isInvincible & overrideSprite)                
            {
                player.Sprite = playerSpritesBitmap[23];
            }
            else
            {
                player.Sprite = playerSpritesBitmap[steps];
            }
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
            UpdateSpriteHud(CameraPosition.X);
        }

        private void UpdateSpriteHud(float x)
        {
            //Hud plack
            for (int i = 0; i < HUDSprites.Count; i++)
            {
                HUDSprites[i].Position.X = -(x - 32);
            }
            for (int i = 0; i < HUDObjects.Count; i++)
            {
                //HUDObjects[i].Position.X = -(x - 64);
                if (HUDObjects[i].Tag == "LifeElement0")
                {
                    HUDObjects[i].Position.X = -(x - 64);
                }
                if (HUDObjects[i].Tag == "LifeElement1")
                {
                    HUDObjects[i].Position.X = -(x - 72);
                }
                if (HUDObjects[i].Tag == "WeaponIconElement")
                {
                    HUDObjects[i].Position.X = -(x - 256);
                }
                if (HUDObjects[i].Tag == "WeaponNameElement")
                {
                    HUDObjects[i].Position.X = -(x - 200);
                }
            }
            for (int i = 0; i < HUDObjects.Count; i++)
            {
                //HUDObjects[i].Position.X = -(x - 64);
                if (HUDObjects[i].Tag == "CrystalElement0")
                {
                    HUDObjects[i].Position.X = -(x - 168);
                }
                if (HUDObjects[i].Tag == "CrystalElement1")
                {
                    HUDObjects[i].Position.X = -(x - 176);
                }
                if (HUDObjects[i].Tag == "CrystalElement2")
                {
                    HUDObjects[i].Position.X = -(x - 184);
                }
            }
            for (int i = 0; i < HUDObjects.Count; i++)
            {
                if (HUDObjects[i].Tag == "HealthElement0")
                {
                    HUDObjects[i].Position.X = -(x - 56);
                }
                if (HUDObjects[i].Tag == "HealthElement1")
                {
                    HUDObjects[i].Position.X = -(x - 64);
                }
                if (HUDObjects[i].Tag == "HealthElement2")
                {
                    HUDObjects[i].Position.X = -(x - 72);
                }
            }
            for(int i = 0;i < HUDObjects.Count;i++)
            {
                if (HUDObjects[i].Tag == "ScoreElement0")
                {
                    HUDObjects[i].Position.X = -(x - 96);
                }
                if (HUDObjects[i].Tag == "ScoreElement1")
                {
                    HUDObjects[i].Position.X = -(x - 104);
                }
                if (HUDObjects[i].Tag == "ScoreElement2")
                {
                    HUDObjects[i].Position.X = -(x - 112);
                }
                if (HUDObjects[i].Tag == "ScoreElement3")
                {
                    HUDObjects[i].Position.X = -(x - 120);
                }
                if (HUDObjects[i].Tag == "ScoreElement4")
                {
                    HUDObjects[i].Position.X = -(x - 128);
                }
                if (HUDObjects[i].Tag == "ScoreElement5")
                {
                    HUDObjects[i].Position.X = -(x - 136);
                }
                if (HUDObjects[i].Tag == "ScoreElement6")
                {
                    HUDObjects[i].Position.X = -(x - 144);
                }
                if (HUDObjects[i].Tag == "ScoreElement7")
                {
                    HUDObjects[i].Position.X = -(x - 152);
                }
            }
            for (int i = 0; i < HUDObjects.Count; i++)
            {
                if (HUDObjects[i].Tag == "AmmoElement0")
                {
                    HUDObjects[i].Position.X = -(x - 200);
                }
                if (HUDObjects[i].Tag == "AmmoElement1")
                {
                    HUDObjects[i].Position.X = -(x - 208);
                }
                if (HUDObjects[i].Tag == "AmmoElement2")
                {
                    HUDObjects[i].Position.X = -(x - 216);
                }
                if (HUDObjects[i].Tag == "AmmoElement3")
                {
                    HUDObjects[i].Position.X = -(x - 224);
                }
                if (HUDObjects[i].Tag == "AmmoElement4")
                {
                    HUDObjects[i].Position.X = -(x - 232);
                }
                if (HUDObjects[i].Tag == "AmmoElement5")
                {
                    HUDObjects[i].Position.X = -(x - 240);
                }
            }
        }

        private void GameStateHandler2()
        {
            if (transitionIsCalled)
            {
                if (reloadDestination.IsWorldMap == true)
                {
                    if (navCause == NavigationReason.LevelCleared)
                    {
                        SetLevelCleared(worlds, currentDestination);
                    }
                    UnloadLastLevel();
                    navCause = NavigationReason.ReturnToMap;
                    //Thread.Sleep(10);
                    GoToLevel(state, reloadDestination, worlds);
                    currentDestination = reloadDestination;
                    state = CallState.WorldMap;
                    //worlds[reloadDestination.WorldNumber].AreAreasClear = new List<bool> { false, false, false, false, false, false };

                }
                else if (currentDestination.WorldNumber == reloadDestination.WorldNumber
                    && currentDestination.AreaNumber == reloadDestination.AreaNumber
                    && currentDestination.LevelNumber != reloadDestination.LevelNumber)
                {
                    UnloadLastLevel();
                    state = CallState.RoomTransition;
                    navCause = NavigationReason.None;
                    //Thread.Sleep(10);
                    GoToLevel(state, reloadDestination, worlds);
                    currentDestination = reloadDestination;
                    state = CallState.PlayingLevel;
                }
                //
                else
                {
                    UnloadLastLevel();
                    navCause = NavigationReason.None;
                    GoToLevel(state, reloadDestination, worlds);
                    currentDestination = reloadDestination;
                    state = CallState.PlayingLevel;
                }
                transitionIsCalled = false;
            }
            //check wether to change level or go to screen or do nothing
        }

        private void SetLevelCleared(List<WorldStructure> worlds, Destination thisIsCleared)
        {

            worlds[thisIsCleared.WorldNumber]
                .Areas[thisIsCleared.AreaNumber].isAreaClear = true; //JZONE edit to allow replay of levels
            int areanum = worlds[thisIsCleared.WorldNumber]
                .Areas[thisIsCleared.AreaNumber].AreaNumber;
            int clearbool = worlds[thisIsCleared.WorldNumber].ClearAreas.IndexOf(areanum);
            worlds[thisIsCleared.WorldNumber]
                .AreAreasClear[clearbool] = true; //JZONE edit to allow replay of levels

            //also set world clear
            if (worlds[thisIsCleared.WorldNumber]
                .AreAreasClear.Contains(false))
            {
                worlds[thisIsCleared.WorldNumber].isWorldClear = false;
            }
            else
            {
                worlds[thisIsCleared.WorldNumber].isWorldClear = true;
            }
        }

        private void SetLevelClearedOld(List<WorldStructure> worlds, Level activeLevel)
        {
            int areanum = 0;
            int numindspot = 0;
            foreach (var world in worlds)
            {
                if (world.WorldName != "Utility" && world.isWorldClear != true)
                {
                    if (world.isWorldClear != true)
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
            if (state != CallState.RoomTransition)
            {
                RemoveHudSprites();
            }
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
                if (whichLevel[1] == 99)
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
                    if (layersToRender.IndexOf(layer) == 3)
                    {
                        //
                    }
                    RenderLayer(loadTarget.artRefs, layer, loadTarget.artTagDefinitions);
                }
            }
            if (reloadDestination.LevelNumber == 0)
            {
                PlayLevelTrack(loadTarget);
            }
            else
            {
                sfxInstance.Play("door close");
            }
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
                    List<string> hudelements = new List<string>() { "HUD", "Lives", "Score", "Health", "Gems", "AmmoLeft", "Weaponname", "W_Icon" };
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
                        if (hudelements.Contains(layer[j, i]))
                        {
                            switch (layer[j, i])
                            {
                                case "HUD":
                                    hud = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(256, 32), $"hud/{hudelements[0]}", false, "HUD");
                                    break;

                                case "Weaponname":
                                    string whatthiswepname = unlockedWeapons[selectedweapon].WeaponName;
                                    switch (whatthiswepname)
                                    {
                                        case "Debug":
                                            weaponname = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 + 8), new Vector2(48, 8), $"hud/debug_word", true, "WeaponNameElement"),
                                            WeaponNames,
                                            0
                                            );
                                            break;
                                        case "Barker":
                                            weaponname = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 + 8), new Vector2(48, 8), $"hud/barker_word", true, "WeaponNameElement"),
                                            WeaponNames,
                                            0
                                            );
                                            break;
                                        case "Väits":
                                            weaponname = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 + 8), new Vector2(48, 8), $"hud/väits_word", true, "WeaponNameElement"),
                                            WeaponNames,
                                            0
                                            );
                                            break;
                                        case "Willo":
                                            weaponname = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 + 8), new Vector2(48, 8), $"hud/willo_word", true, "WeaponNameElement"),
                                            WeaponNames,
                                            0
                                            );
                                            break;
                                        default:
                                            break;
                                    }
                                    break;

                                case "W_Icon":
                                    string whatthiswep = unlockedWeapons[selectedweapon].WeaponName;
                                    switch (whatthiswep)
                                    {
                                        case "Debug":
                                            weaponicon = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 - 8), new Vector2(16, 16), $"hud/debug_icon", true, "WeaponIconElement"),
                                            WeaponIcons,
                                            0
                                            );
                                            break;
                                        case "Barker":
                                            weaponicon = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 - 8), new Vector2(16, 16), $"hud/barker_icon", true, "WeaponIconElement"),
                                            WeaponIcons,
                                            0
                                            );
                                            break;
                                        case "Väits":
                                            weaponicon = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 - 8), new Vector2(16, 16), $"hud/väits_icon", true, "WeaponIconElement"),
                                            WeaponIcons,
                                            0
                                            );
                                            break; 
                                        case "Willo":
                                            weaponicon = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 - 8), new Vector2(16, 16), $"hud/willo_icon", true, "WeaponIconElement"),
                                            WeaponIcons,
                                            0
                                            );
                                            break;
                                        default:
                                            weaponicon = new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, j * 16 - 8), new Vector2(16, 16), $"hud/none_icon", true, "WeaponIconElement"),
                                            WeaponIcons,
                                            0
                                            );
                                            break;
                                    }

                                    break;


                                case "AmmoLeft":

                                    string digitNormalA = DigitNormalizer(activeWeapon.AmmoLeft, "AmmoLeft");
                                    Log.Info(digitNormalA);
                                    AmmoLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalA[0]}", true, "AmmoElement0"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    AmmoLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16 + 8, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalA[1]}", true, "AmmoElement1"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    AmmoLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16 + 16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalA[2]}", true, "AmmoElement2"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    AmmoLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16 + 24, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalA[3]}", true, "AmmoElement3"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    AmmoLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16 + 32, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalA[4]}", true, "AmmoElement4"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    AmmoLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16 + 40, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalA[5]}", true, "AmmoElement5"),
                                            BarElements,
                                            0
                                            )
                                        );

                                    break;

                                case "Lives":
                                    string digitNormal = DigitNormalizer(playerLives, "Lives");
                                    Lives.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormal[0]}", true, "LifeElement0"),
                                            DigitBMP,
                                            0
                                            )
                                        );
                                    Lives.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16 - 8, (j * 16)), new Vector2(8, 8), $"hud/{digitNormal[1]}", true, "LifeElement1"),
                                            DigitBMP,
                                            0
                                            )
                                        );
                                    break;

                                    
                                case "Health":
                                    string digitNormalH = DigitNormalizer(playerHealth, "Health");
                                    Log.Info(digitNormalH);
                                    HealthLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16, (j * 16)+8), new Vector2(8, 8), $"hud/{digitNormalH[0]}", true, "HealthElement0"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    HealthLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16+8, (j * 16)+8), new Vector2(8, 8), $"hud/{digitNormalH[1]}", true, "HealthElement1"),
                                            BarElements,
                                            0
                                            )
                                        );
                                    HealthLeftItems.Add(
                                        new HUDObject(
                                            new Sprite2d(new Vector2(i * 16+16, (j * 16)+8), new Vector2(8, 8), $"hud/{digitNormalH[2]}", true, "HealthElement2"),
                                            BarElements,
                                            0
                                            )
                                        );

                                    break;
                                case "Gems":

                                    string digitNormalG = DigitNormalizer(crystalScoreTally, "Gems");
                                        Gems.Add(
                                            new HUDObject(
                                                new Sprite2d(new Vector2(i * 16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalG[0]}", true, "CrystalElement0"),
                                                DigitBMP,
                                                0
                                                )
                                            );
                                        Gems.Add(
                                            new HUDObject(
                                                new Sprite2d(new Vector2(i * 16 + 8, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalG[1]}", true, "CrystalElement1"),
                                                DigitBMP,
                                                0
                                                )
                                            );
                                        Gems.Add(
                                            new HUDObject(
                                                new Sprite2d(new Vector2(i * 16 + 16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalG[2]}", true, "CrystalElement2"),
                                                DigitBMP,
                                                0
                                                )
                                            );
                                    break;
                                case "Score":
                                    string digitNormalS = DigitNormalizer(pointScoreTally, "Score");
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[0]}", true, "ScoreElement0"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+8, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[1]}", true, "ScoreElement1"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+16, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[2]}", true, "ScoreElement2"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+24, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[3]}", true, "ScoreElement3"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+32, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[4]}", true, "ScoreElement4"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+40, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[5]}", true, "ScoreElement5"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+48, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[6]}", true, "ScoreElement6"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    ScoreNumbers.Add(
                                        new HUDObject(
                                                new Sprite2d(new Vector2(i * 16+56, (j * 16)), new Vector2(8, 8), $"hud/{digitNormalS[7]}", true, "ScoreElement7"),
                                                DigitBMP,
                                                0
                                                )
                                        );
                                    break;
                                default:
                                    Log.Warning("Nothing has been rendered, but a hud element was detected: " + layer[j, i]);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private string DigitNormalizer(int normalizeint, string what)
        {
            int length = normalizeint.ToString().Length;
            if (what == "Lives")
            {
                if (normalizeint < 0)
                {
                    return "00";
                }
                else if (normalizeint >= 0 && normalizeint < 10)
                {
                    return "0" + normalizeint.ToString();
                }
                else if (normalizeint > 99)
                {
                    return "99";
                }
                else
                {
                    return normalizeint.ToString();
                }
            }
            else if (what == "Gems")
            {
                if (normalizeint < 0)
                {
                    return "000";
                }
                else if (normalizeint >= 0 && normalizeint < 10)
                {
                    return "00" + normalizeint.ToString();
                }
                else if (normalizeint >= 10 && normalizeint < 100)
                {
                    return "0" + normalizeint.ToString();
                }
                else if (normalizeint >= 100 && normalizeint < 1000)
                {
                    return normalizeint.ToString();
                }
                else
                {
                    return "999";
                }
            }
            else if (what == "Health")
            {
                bool isOdd = false;
                string item = "_";
                if ((normalizeint%2) != 0)
                {
                    item = "-";
                }
                else if ((normalizeint % 2) == 0)
                {
                    item = "=";
                }
                else
                {
                    item = "_";
                }
                if (normalizeint <= 0)
                {
                    return "___";
                }
                else if (normalizeint < 3)
                {
                    return item + "__";
                }
                else if (normalizeint < 5)
                {
                    return "=" + item + "_";
                }
                else if (normalizeint < 7)
                {
                    return "==" + item;
                }
                else
                {
                    return "===";
                }

            }
            else if (what == "AmmoLeft")
            {
                bool isOdd = false;
                string item = "_";
                if ((normalizeint%2) != 0)
                {
                    item = "-";
                }
                else if ((normalizeint % 2) == 0)
                {
                    item = "=";
                }
                else
                {
                    item = "_";
                }
                if (normalizeint <= 0)
                {
                    return "______";
                }
                else if (normalizeint < 3)
                {
                    return item + "_____";
                }
                else if (normalizeint < 5)
                {
                    return "=" + item + "____";
                }
                else if (normalizeint < 7)
                {
                    return "==" + item + "___";
                }
                else if (normalizeint < 5)
                {
                    return "===" + item + "__";
                }
                else if (normalizeint < 7)
                {
                    return "====" + item + "_";
                }
                else
                {
                    return "======";
                }

            }
            else if (what == "Score")
            {
                switch (normalizeint.ToString().Length)
                {
                    case 0:
                        return "00000000" + normalizeint.ToString();
                    case 1:
                        return "0000000" + normalizeint.ToString();
                    case 2:
                        return "000000" + normalizeint.ToString();
                    case 3:
                        return "00000" + normalizeint.ToString();
                    case 4:
                        return "0000" + normalizeint.ToString();
                    case 5:
                        return "000" + normalizeint.ToString();
                    case 6:
                        return "00" + normalizeint.ToString();
                    case 7:
                        return "0" + normalizeint.ToString();
                    default:
                        return "00000000";
                        break;
                }
            }
            else
            {
                return "00000000";
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

            SetHudLives(Lives);
            SetHudGems(Gems);
            SetHudScore(ScoreNumbers);
            SetCurrentWeapon(weaponicon, weaponname);
            SetHudHealthbar(HealthLeftItems);
            SetHudAmmobar(AmmoLeftItems);


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

        private void SetCurrentWeapon(HUDObject weaponicon, HUDObject weaponname)
        {
            string whatthiswep = unlockedWeapons[selectedweapon].WeaponName;
            foreach (var sprite in HUDObjects)
            {
                if (sprite.Tag == "WeaponIconElement")
                {
                    switch (whatthiswep)
                    {
                        case "Debug":
                            weaponicon.Display.Sprite = weaponicon.DisplayElements[3];
                            break;
                        case "Barker":
                            weaponicon.Display.Sprite = weaponicon.DisplayElements[2];
                            break;      
                        case "Väits":
                            weaponicon.Display.Sprite = weaponicon.DisplayElements[0];
                            break;      
                        case "Willo":
                            weaponicon.Display.Sprite = weaponicon.DisplayElements[1];
                            break;      
                        default:
                            weaponicon.Display.Sprite = weaponicon.DisplayElements[4];
                            break;
                    }
                }
                if (sprite.Tag == "WeaponNameElement")
                {
                    switch (whatthiswep)
                    {
                        case "Debug":
                            weaponname.Display.Sprite = weaponname.DisplayElements[3];
                            break;
                        case "Barker":
                            weaponname.Display.Sprite = weaponname.DisplayElements[2];
                            break;
                        case "Väits":
                            weaponname.Display.Sprite = weaponname.DisplayElements[0];
                            break;
                        case "Willo":
                            weaponname.Display.Sprite = weaponname.DisplayElements[1];
                            break;
                        default:
                            weaponname.Display.Sprite = weaponname.DisplayElements[4];
                            break;
                    }
                }
            }
           
        }

        private void SetHudScore(List<HUDObject> scoreNumbers)
        {
            string normalized = DigitNormalizer(pointScoreTally, "Score");
            Log.Info(normalized+" "+pointScoreTally);
            foreach (var sprite in HUDObjects)
            {
                switch (sprite.Tag)
                {
                    case "ScoreElement0":
                        ScoreNumbers[0].DisplayedDataInt = int.Parse(normalized[0].ToString());
                        sprite.Sprite = ScoreNumbers[0].DisplayElements[ScoreNumbers[0].DisplayedDataInt];
                        break;
                    case "ScoreElement1":
                        ScoreNumbers[1].DisplayedDataInt = int.Parse(normalized[1].ToString());
                        sprite.Sprite = ScoreNumbers[1].DisplayElements[ScoreNumbers[1].DisplayedDataInt];
                        break;
                    case "ScoreElement2":
                        ScoreNumbers[2].DisplayedDataInt = int.Parse(normalized[2].ToString());
                        sprite.Sprite = ScoreNumbers[2].DisplayElements[ScoreNumbers[2].DisplayedDataInt];
                        break;
                    case "ScoreElement3":
                        ScoreNumbers[3].DisplayedDataInt = int.Parse(normalized[3].ToString());
                        sprite.Sprite = ScoreNumbers[3].DisplayElements[ScoreNumbers[3].DisplayedDataInt];
                        break;
                    case "ScoreElement4":
                        ScoreNumbers[4].DisplayedDataInt = int.Parse(normalized[4].ToString());
                        sprite.Sprite = ScoreNumbers[4].DisplayElements[ScoreNumbers[4].DisplayedDataInt];
                        break;
                    case "ScoreElement5":
                        ScoreNumbers[5].DisplayedDataInt = int.Parse(normalized[5].ToString());
                        sprite.Sprite = ScoreNumbers[5].DisplayElements[ScoreNumbers[5].DisplayedDataInt];
                        break;
                    case "ScoreElement6":
                        ScoreNumbers[6].DisplayedDataInt = int.Parse(normalized[6].ToString());
                        sprite.Sprite = ScoreNumbers[6].DisplayElements[ScoreNumbers[6].DisplayedDataInt];
                        break;
                    case "ScoreElement7":
                        ScoreNumbers[7].DisplayedDataInt = int.Parse(normalized[7].ToString());
                        sprite.Sprite = ScoreNumbers[7].DisplayElements[ScoreNumbers[7].DisplayedDataInt];
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetHudGems(List<HUDObject> gems)
        {
            if (crystalScoreTally <= 0)
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "CrystalElement0")
                    {
                        Gems[0].DisplayedDataInt = 0;
                    }
                    if (sprite.Tag == "CrystalElement1")
                    {
                        Gems[1].DisplayedDataInt = 0;
                    }
                    if (sprite.Tag == "CrystalElement2")
                    {
                        Gems[2].DisplayedDataInt = 0;
                    }
                }
            }
            else if (crystalScoreTally > 0 && crystalScoreTally < 10)
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "CrystalElement2")
                    {
                        Gems[2].DisplayedDataInt = crystalScoreTally;
                    }
                }

            }
            else if (crystalScoreTally >= 10 && crystalScoreTally < 99)
            {
                string parsed = crystalScoreTally.ToString();

                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "CrystalElement0")
                    {
                        Gems[0].DisplayedDataInt = 0;
                    }
                    if (sprite.Tag == "CrystalElement1")
                    {
                        Gems[1].DisplayedDataInt = int.Parse(parsed[0].ToString());
                    }
                    if (sprite.Tag == "CrystalElement2")
                    {
                        Gems[2].DisplayedDataInt = int.Parse(parsed[1].ToString());
                    }
                }
            }
            else if (crystalScoreTally >= 100 && crystalScoreTally < 1000)
            {
                string parsed = crystalScoreTally.ToString();

                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "CrystalElement0")
                    {
                        Gems[0].DisplayedDataInt = int.Parse(parsed[0].ToString());
                    }
                    if (sprite.Tag == "CrystalElement1")
                    {
                        Gems[1].DisplayedDataInt = int.Parse(parsed[1].ToString());
                    }
                    if (sprite.Tag == "CrystalElement2")
                    {
                        Gems[2].DisplayedDataInt = int.Parse(parsed[2].ToString());
                    }
                }
            }
            else
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "CrystalElement0")
                    {
                        Gems[0].DisplayedDataInt = 9;
                    }
                    if (sprite.Tag == "CrystalElement1")
                    {
                        Gems[1].DisplayedDataInt = 9;
                    }
                    if (sprite.Tag == "CrystalElement2")
                    {
                        Gems[2].DisplayedDataInt = 9;
                    }
                }

            }
            foreach (var hudsprite in HUDObjects)
            {
                if (hudsprite.Tag == "CrystalElement0")
                {
                    hudsprite.Sprite = Gems[0].DisplayElements[Gems[0].DisplayedDataInt];
                }
                if (hudsprite.Tag == "CrystalElement1")
                {
                    hudsprite.Sprite = Gems[1].DisplayElements[Gems[1].DisplayedDataInt];
                }
                if (hudsprite.Tag == "CrystalElement2")
                {
                    hudsprite.Sprite = Gems[2].DisplayElements[Gems[2].DisplayedDataInt];
                }
            }
        }

        private void SetHudAmmobar(List<HUDObject> barelements)
        {
            if (activeWeapon.AmmoLeft <= 0)
            {
                foreach (var sprite in HUDObjects)
                {
                    switch (sprite.Tag)
                    {
                        case "AmmoElement0":
                            AmmoLeftItems[0].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement1":
                            AmmoLeftItems[1].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement2":
                            AmmoLeftItems[2].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement3":
                            AmmoLeftItems[3].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement4":
                            AmmoLeftItems[4].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement5":
                            AmmoLeftItems[5].DisplayedDataInt = 0;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                string converted = DigitNormalizer(activeWeapon.AmmoLeft, "AmmoLeft");
                int[] prep = new int[6];
                for (int i = 0; i < prep.Length; i++)
                {
                    if (converted[i] == '_')
                    {
                        prep[i] = 0;
                    }
                    else if (converted[i] == '-')
                    {
                        prep[i] = 1;
                    }
                    else
                    {
                        prep[i] = 2;
                    }
                }
                Log.Highlight(prep[0] + " " + prep[1] + " " + prep[2] + " " + prep[3] + " " + prep[4] + " " + prep[5]);
                foreach (var sprite in HUDObjects)
                {
                    switch (sprite.Tag)
                    {
                        case "AmmoElement0":
                            AmmoLeftItems[0].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement1":
                            AmmoLeftItems[1].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement2":
                            AmmoLeftItems[2].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement3":
                            AmmoLeftItems[3].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement4":
                            AmmoLeftItems[4].DisplayedDataInt = 0;
                            break;
                        case "AmmoElement5":
                            AmmoLeftItems[5].DisplayedDataInt = 0;
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach (var hudsprite in HUDObjects)
            {
                switch (hudsprite.Tag)
                {
                    case "AmmoElement0":
                        hudsprite.Sprite = AmmoLeftItems[0].DisplayElements[HealthLeftItems[0].DisplayedDataInt];
                        break;
                    case "AmmoElement1":
                        hudsprite.Sprite = AmmoLeftItems[1].DisplayElements[HealthLeftItems[1].DisplayedDataInt];
                        break;
                    case "AmmoElement2":
                        hudsprite.Sprite = AmmoLeftItems[2].DisplayElements[HealthLeftItems[2].DisplayedDataInt];
                        break;
                    case "AmmoElement3":
                        hudsprite.Sprite = AmmoLeftItems[3].DisplayElements[HealthLeftItems[3].DisplayedDataInt];
                        break;
                    case "AmmoElement4":
                        hudsprite.Sprite = AmmoLeftItems[4].DisplayElements[HealthLeftItems[4].DisplayedDataInt];
                        break;
                    case "AmmoElement5":
                        hudsprite.Sprite = AmmoLeftItems[5].DisplayElements[HealthLeftItems[5].DisplayedDataInt];
                        break;
                    default:
                        break;
                }
            }
        }
        private void SetHudHealthbar(List<HUDObject> barelements)
        {
            if (playerHealth <= 0)
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "HealthElement0")
                    {
                        HealthLeftItems[0].DisplayedDataInt = 0;
                    }
                    if (sprite.Tag == "HealthElement1")
                    {
                        HealthLeftItems[1].DisplayedDataInt = 0;
                    }
                    if (sprite.Tag == "HealthElement2")
                    {
                        HealthLeftItems[2].DisplayedDataInt = 0;
                    }
                }
            }
            else
            {
                string converted = DigitNormalizer(playerHealth, "Health");
                int[] prep = new int[3];
                for (int i = 0; i < prep.Length; i++)
                {
                    if (converted[i] == '_')
                    {
                        prep[i] = 0;
                    }
                    else if (converted[i] == '-')
                    {
                        prep[i] = 1;
                    }
                    else
                    {
                        prep[i] = 2;
                    }
                }
                Log.Highlight(prep[0] + " " + prep[1] + " " + prep[2]);
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "HealthElement0")
                    {
                        HealthLeftItems[0].DisplayedDataInt = prep[0];
                    }
                    if (sprite.Tag == "HealthElement1")
                    {
                        HealthLeftItems[1].DisplayedDataInt = prep[1];
                    }
                    if (sprite.Tag == "HealthElement2")
                    {
                        HealthLeftItems[2].DisplayedDataInt = prep[2];
                    }
                }
            }
            foreach (var hudsprite in HUDObjects)
            {
                if (hudsprite.Tag == "HealthElement0")
                {
                    hudsprite.Sprite = HealthLeftItems[0].DisplayElements[HealthLeftItems[0].DisplayedDataInt];
                }
                if (hudsprite.Tag == "HealthElement1")
                {
                    hudsprite.Sprite = HealthLeftItems[1].DisplayElements[HealthLeftItems[1].DisplayedDataInt];
                }
                if (hudsprite.Tag == "HealthElement2")
                {
                    hudsprite.Sprite = HealthLeftItems[2].DisplayElements[HealthLeftItems[2].DisplayedDataInt];
                }
            }
        }
        private void SetHudLives(List<HUDObject> lives)
        {
            //if lives are zero or lower, update both HUDObjects, to display " 0 0 "
            //if lives are > 0 and < 10, just update item [1]
            //if lives are >= 10, update both to display a new integer from playerlives
            //else display " 9 9 "
            if (playerLives <= 0)
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "LifeElement0")
                    {
                        Lives[0].DisplayedDataInt = 0;
                    }
                    if (sprite.Tag == "LifeElement1")
                    {
                        Lives[1].DisplayedDataInt = 0;
                    }
                }
            }
            else if (playerLives > 0 && playerLives < 10)
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "LifeElement1")
                    {
                        Lives[1].DisplayedDataInt = playerLives;
                    }
                }

            }
            else if (playerLives >= 10 && playerLives < 99)
            {
                string parsed = playerLives.ToString();

                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "LifeElement0")
                    {
                        Lives[0].DisplayedDataInt = int.Parse(parsed[0].ToString());
                    }
                    if (sprite.Tag == "LifeElement1")
                    {
                        Lives[1].DisplayedDataInt = int.Parse(parsed[1].ToString());
                    }
                }

            }
            else
            {
                foreach (var sprite in HUDObjects)
                {
                    if (sprite.Tag == "LifeElement0")
                    {
                        Lives[0].DisplayedDataInt = 9;
                    }
                    if (sprite.Tag == "LifeElement1")
                    {
                        Lives[1].DisplayedDataInt = 9;
                    }
                }

            }
            foreach (var hudsprite in HUDObjects)
            {
                if (hudsprite.Tag == "LifeElement0")
                {
                    hudsprite.Sprite = Lives[0].DisplayElements[Lives[0].DisplayedDataInt];
                }
                if (hudsprite.Tag == "LifeElement1")
                {
                    hudsprite.Sprite = Lives[1].DisplayElements[Lives[1].DisplayedDataInt];
                }
            }
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
            if (e.KeyCode == Keys.X) { nextweapon = true; }
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
            if (e.KeyCode == Keys.X) { nextweapon = false; }
        }
        private void togglePause()
        {
            pauseGameKey = !pauseGameKey;
        }
    }
}
