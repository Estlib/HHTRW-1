using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Plat2d_2.EngineCore;
using System.Windows.Forms;
using Box2DX.Common;
using System.IO;
using Box2DX.Dynamics;

namespace Plat2d_2
{
    class DemoGame : EngineCore.EngineCore
    {
        Sprite2d player;
        int steps = 0;
        int slowDownFrameRate = 1;
        int playerSpeed = 10;
        int currentSprite;
        List<Bitmap> playerSpritesBitmap = new List<Bitmap>();

        int facedirection;
        bool left;
        bool right;
        bool up;
        bool down;
        bool jump;
        bool jumpmode;
        bool nokey;
        bool LRDcheck;

        //Vector2 lastPos = Vector2.Zero();
        //List<Level> levels = new List<Level>();
        Level level1 = new Level("tiles/noart");

        string[,] currentLevel = 

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
        public DemoGame() :  base(new Vector2(615, 615),"HHTRW-engine1 demo")
        {

        }

        public override void OnLoad()
        {

            //List<Sprite2d> playerSprites = new List<Sprite2d>();
            Console.WriteLine("OnLoad works.");
            BGColor = System.Drawing.Color.Black;
            //CameraZoom = new Vector2(.1f,.1f);
            Sprite2d groundRef = new Sprite2d( "tiles/noart/testblock1");
            Sprite2d airRef = new Sprite2d( "tiles/noart/testblock5");
            Sprite2d coinRef = new Sprite2d( "tiles/noart/testobject2");

            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand1.png"))); //0
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run1.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run2.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run3.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run4.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run5.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run6.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand2.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3duck.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3jump.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/ALTfall.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand1flip.png"))); //11
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run1flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run2flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run3flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run4flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run5flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/run6flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand2flip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3duckflip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/stand3jumpflip.png")));
            playerSpritesBitmap.Add(new Bitmap(Image.FromFile($"assets/sprites/player/wipspriteset/ALTfallFLIP.png")));
            currentSprite = 0;

            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j,i] == "G")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), groundRef, "Ground").CreateStatic();
                    }
                    if (Map[j, i] == ".")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), airRef, "Air")/*.CreateStatic()*/;
                    }
                    if (Map[j, i] == "C")
                    {
                        new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(16, 16), coinRef, "Coin")/*.CreateStatic()*/;
                    }
                }
            }
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    if (Map[j, i] == "P")
                    {
                        player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerSpritesBitmap[0], "Player");
                        //player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerStand, "Player");
                        player.CreateDynamic();
                        //pass a list of sprites here, changing happens by animating list numbers, limited by if limits
                        //see https://www.youtube.com/results?search_query=box2d+tutorial+c%23
                        //playercollision = new Shape2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), "Player");
                        //playercollision.CreateDynamic();
                    }
                }
            }
            //player = new Sprite2d(new Vector2(64, 96), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
            //player2 = new Sprite2d(new Vector2(128, 192), new Vector2(32, 32), "player/wipspriteset/stand1", "Player2");
        }
        public override void OnDraw()
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
        }
        //int timeframe = 0;
        //float x = 1;
        int times = 0;
        int remainingJumpSteps = 0;
        int remainingJumpFrames = 0;
        int start;
        int end;

        private void AnimatePlayer(int start, int end)
        {
            //Log.Info("AnimatePlayer has been called");
            slowDownFrameRate += 1;
            if (slowDownFrameRate == 4)
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
        int[] jumpFramesL = new int[] {7, 9 };
        int[] jumpFramesR = new int[] {18, 20 };
        private void AnimatePlayerOneWayOnly(int start, int end, int[] frames)
        {
            Log.Info("AnimatePlayer has been called");
            steps++;
            if (steps > end || steps < start)
            {
                steps = end;
            }
            //player.UpdateSprite(steps);
            player.Sprite = playerSpritesBitmap[steps];
        }
        //int[] jumpFrames = new int[] { };
        //private void AnimatePlayerOneWayOnly(int[] frames)
        //{
        //    Log.Info("AnimatePlayer has been called");
        //    steps++;
        //    if (steps > end || steps < start)
        //    {
        //        steps = end;
        //    }
        //    //player.UpdateSprite(steps);
        //    currentSprite = steps;
        //}

        public override void OnUpdate()
        {
            if (player == null)
            {
                return;
            }
            times++;

            if (up)
            {
                player.ApplyImpulse(new Vector2(0, -160000), Vector2.Zero());
            }
            if (down)
            {
                Log.Warning("Down has no action currently. Sprite change is visual only.");
            }
            if (left)
            {
                player.SetVelocity(new Vector2(-120, player.GetYVelocity()));

            }
            if (right)
            {
                player.SetVelocity(new Vector2(120, player.GetYVelocity()));
            }
            if (jump)
            {
                if (player.IsColliding("Ground")!=null)
                {
                    remainingJumpSteps = 6;
                    remainingJumpFrames = 9;
                }
                jumpmode = true;
            }
            //else
            //{
            //    if (jump == false || down == false)
            //    {
            //        if (facedirection == 0)
            //        {

            //            AnimatePlayer(12, 12);

            //        }
            //        else if (facedirection == 1)
            //        {

            //            AnimatePlayer(1, 1);
            //        }
            //    }
            //    //standing animation
            //}
            //else 
            //{
            //    if (facedirection == 0)
            //    {
            //        AnimatePlayer(0, 1);
            //    }
            //    else if (facedirection == 1)
            //    {
            //        AnimatePlayer(11, 1);
            //    }
            //}
            if (remainingJumpSteps > 0)
            {
                //player.AddForce(new Vector2(0, -4800000), Vector2.Zero());
                player.SetVelocity(new Vector2(player.GetXVelocity(), -4800));
                remainingJumpSteps--;
            }
            if (player.IsColliding("Ground") != null)
            {
                jumpmode = false;
                Log.Info("Player is colliding with Ground");
            }
            else if (player.IsColliding("Ground") == null)
            {
                Log.Warning("Player is not colliding with Ground and thus cannot jump.");
            }
            player.UpdatePosition();
            //player.UpdateSprite(currentSprite, playerSpritesS);
            Sprite2d coin = player.IsColliding("Coin");
            if (coin != null)
            {
                Log.Info("Coin is being touched");
                coin.DestroySelf();
            }
            //if (player.IsColliding("Ground") != null)
            //{
            //    //Log.Info($"Collision is happening. {times}");
            //    //times ++;
            //    //player.Position.X = lastPos.X;
            //    //player.Position.Y = lastPos.Y;
            //}
            //else
            //{
            //    //lastPos.X = player.Position.X;
            //    //lastPos.Y = player.Position.Y;
            //}
            //CameraPosition.X++;
            //CameraAngle += .1f;
            //player.Position.X += x;          
            //if (timeframe>400)
            //{
            //    if (player != null)
            //    {
            //        player.DestroySelf();
            //        player = null;
            //    }
            //}
            //timeframe++;
            //Console.WriteLine($"Framecount: {frame}.");
            //frame++;
            Log.Info($"Currentsprite should be # {steps}");
            
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

        public bool NoCrouchWalk(bool LRDcheck)
        {

            return LRDcheck;
        }

        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = true; }
            if (e.KeyCode == Keys.A) { left = true; }
            if (e.KeyCode == Keys.S) { down = true; }
            if (e.KeyCode == Keys.D) { right = true; }
            if (e.KeyCode == Keys.Z) { jump = true; }
        }

        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { up = false; }
            if (e.KeyCode == Keys.A) { left = false; }
            if (e.KeyCode == Keys.S) { down = false; }
            if (e.KeyCode == Keys.D) { right = false; }
            if (e.KeyCode == Keys.Z) { jump = false; }
        }
    }
}
