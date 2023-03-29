using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Plat2d_2.EngineCore;
using System.Windows.Forms;
using Box2DX.Common;

namespace Plat2d_2
{
    class DemoGame : EngineCore.EngineCore
    {
        Sprite2d player;
        Shape2d playercollision;
        //Sprite2d player2;

        bool left;
        bool right;
        bool up;
        bool down;
        bool jump;

        Vector2 lastPos = Vector2.Zero();

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
            Console.WriteLine("OnLoad works.");
            BGColor = Color.Black;
            //CameraZoom = new Vector2(.1f,.1f);
            Sprite2d groundRef = new Sprite2d( "tiles/noart/testblock1");
            Sprite2d airRef = new Sprite2d( "tiles/noart/testblock5");
            Sprite2d coinRef = new Sprite2d( "tiles/noart/testobject2");
            Sprite2d playerStand = new Sprite2d("player/wipspriteset/stand1");
            //player = new Shape2d(new Vector2(8, 8), new Vector2(32, 32), "Test");
            //player = new Sprite2d(new Vector2(8, 8), new Vector2(32, 32), "player/wipspriteset/stand1", "Player");
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
                        player = new Sprite2d(new Vector2(i * 16, j * 16), new Vector2(32, 32), playerStand, "Player");
                        player.CreateDynamic();
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

        }
        //int timeframe = 0;
        //float x = 1;
        int times = 0;
        int remainingJumpSteps = 0;
        Sprite2d playerStand = new Sprite2d("player/wipspriteset/stand1");
        Sprite2d playerRun1 = new Sprite2d("player/wipspriteset/run1");

        public override void OnUpdate()
        {
            if (player == null)
            {
                return;
            }
            times++;
            if (up)
            {
                //player.Position.Y -= 1;
                //player.AddForce(new Vector2(0, -1600), Vector2.Zero());
                player.ApplyImpulse(new Vector2(0, -160000), Vector2.Zero());
                //player.AddForce(new Vector2(0, -1600), new Vector2(0, -1600));
                //player.SetVelocity(new Vector2(0, -120));

                //sprite animation for up key action - none
                //up function changes weapon in the future, rn its a hover for testing purposes
            }
            if (down)
            {
                Log.Warning("Down has no action currently");
                //player.Position.Y += 1;
                //player.AddForce(new Vector2(0, 1600), Vector2.Zero());
                //player.ApplyImpulse(new Vector2(0, 1600), Vector2.Zero());
                //player.AddForce(new Vector2(0, 1600), new Vector2(0, 1600));
                //player.SetVelocity(new Vector2(0, 120

                //sprite animation for down key action
                //down function will be crouching, and changing hitbox smaller
                //if false, hitbox normal, else hitbox small
            }
            if (left)
            {
                //player.Position.X -= 1;
                //player.AddForce(new Vector2(-1600, 0), Vector2.Zero());
                //player.ApplyImpulse(new Vector2(-1600, 0), Vector2.Zero());
                //player.AddForce(new Vector2(-1600, 0), new Vector2(-1600, 0));
                player.SetVelocity(new Vector2(-120, 0));

                //sprite animation for walking left

            }
            if (right)
            {
                //player.Position.X += 1;
                //player.AddForce(new Vector2(1600, 0), Vector2.Zero());
                //player.ApplyImpulse(new Vector2(1600, 0), Vector2.Zero());
                //player.AddForce(new Vector2(1600, 0), new Vector2(1600, 0));
                player.SetVelocity(new Vector2(120, 0));

                //sprite animation for walking right
                //player = EngineCore.HHTRW.SpriteSystem.Player.PlayerRun(new Vector2(player.Position.X, player.Position.Y), new Vector2(32, 32), player, "player");
                //player = new Sprite2d(new Vector2(player.Position.X, player.Position.Y), new Vector2(player.Scale.X, player.Scale.Y), playerStand, "Player");
            }
            if (jump)
            {
                //player.Position.X += 1;
                //player.AddForce(new Vector2(1600, 0), Vector2.Zero());
                //player.ApplyImpulse(new Vector2(1600, 0), Vector2.Zero());
                //player.AddForce(new Vector2(1600, 0), new Vector2(1600, 0));
                if (player.IsColliding("Ground")!=null)
                {
                    remainingJumpSteps = 6;
                }

                //jumping animation
            }
            else
            {
                //standing animation
            }
            if (remainingJumpSteps > 0)
            {
                //player.AddForce(new Vector2(0, -4800000), Vector2.Zero());
                float Xvel = player.GetXVelocity();
                player.SetVelocity(new Vector2(Xvel, -4800));
                remainingJumpSteps--;
            }
            if (player.IsColliding("Ground") != null)
            {
                Log.Info("Player is colliding with Ground");
            }
            else if (player.IsColliding("Ground") == null)
            {
                Log.Warning("Player is not colliding with Ground and thus cannot jump.");
            }
            player.UpdatePosition();
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
