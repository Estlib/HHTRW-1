using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Box2DX.Common;


namespace Plat2d_2.EngineCore
{
    public class Sprite2d
    {
        public Vector2 Position = null;
        public Vector2 Scale = null;
        public string Directory = "";
        public string Tag = "";
        public Bitmap Sprite = null;
        public bool IsReference = false;
        BodyDef bodyDef = new BodyDef();
        Body body;
        private Vector2 vector21;
        private Vector2 vector22;
        private string v;

        public Sprite2d(Vector2 Position, Vector2 Scale, string Directory, string Tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Directory = Directory;
            this.Tag = Tag;

            Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            Sprite = sprite;

            Log.Info($"[SPRITE2D]({Tag}) has been registered");
            EngineCore.RegisterSprite(this);
        }
        public Sprite2d(string Directory)
        {
            this.IsReference = true;
            this.Directory = Directory;

            Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            Sprite = sprite;

            Log.Info($"[SPRITE2D]({Tag}) has been registered");
            EngineCore.RegisterSprite(this);
        }
        public Sprite2d(Vector2 Position, Vector2 Scale, Sprite2d Reference, string Tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;

            //Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            //Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            Sprite = Reference.Sprite;

            Log.Info($"[SPRITE2D]({Tag}) has been registered");
            EngineCore.RegisterSprite(this);
        }

        //public Sprite2d(Vector2 vector21, Vector2 vector22, List<Sprite2d> playerSprites, string v)
        //{
        //    this.vector21 = vector21;
        //    this.vector22 = vector22;
        //    this.playerSprites = playerSprites;
        //    this.v = v;
        //}

        public void CreateStatic()
        {
            // Define the ground body.
            bodyDef = new BodyDef();
            bodyDef.Position = new Vec2(this.Position.X, this.Position.Y);

            // Call the body factory which  creates the ground box shape.
            // The body is also added to the world.
            body = EngineCore.world.CreateBody(bodyDef);

            // Define the ground box shape.
            PolygonDef shapeDef = new PolygonDef();

            // The extents are the half-widths of the box.
            shapeDef.SetAsBox(16.0f, 16.0f);

            // Add the ground shape to the ground body.
            body.CreateShape(shapeDef);
        }
        public void CreateDynamic()
        {
            // Define the dynamic body. We set its position and call the body factory.
            //bodyDef = new BodyDef();
            bodyDef.Position = new Vec2(this.Position.X, this.Position.Y);
            body = EngineCore.world.CreateBody(bodyDef);
            body.IsBullet();

            // Define another box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(1.0f, 1.0f);

            // Set the box density to be non-zero, so it will be dynamic.
            shapeDef.Density = 1000.1f;

            // Override the default friction.
            shapeDef.Friction = 1.0f;

            shapeDef.Restitution = 0.00000001f;

            // Add the shape to the body.
            body.CreateShape(shapeDef);

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();
        }
        public void ApplyImpulse(Vector2 force, Vector2 point)
        {
            //body.SetBullet(true);
            body.ApplyImpulse(new Vec2(force.X, force.Y), new Vec2(point.X, point.Y));
        }
        public void AddForce(Vector2 force, Vector2 point)
        {
            //body.SetLinearVelocity(new Vec2(force.X, force.Y));
            body.ApplyForce(new Vec2(force.X, force.Y), new Vec2(point.X, point.Y));
        }
        public void SetVelocity(Vector2 velocity)
        {
            body.SetLinearVelocity(new Vec2(velocity.X, velocity.Y));
        }
        public float GetXVelocity()
        {
            return body.GetLinearVelocity().X;
        }
        public float GetYVelocity()
        {
            body.SetUserData(body);
            if (body == null)
            {
                Log.Error($"body is null");
                return body.GetPosition().Y;
            }
            else
            {
                return body.GetLinearVelocity().Y;
            }
        }
        public void UpdatePosition()
        {
            Log.Warning("X is " + body.GetPosition().X + ". Y is " + body.GetPosition().Y);
            this.Position.X = (float)System.Math.Round(body.GetPosition().X);
            this.Position.Y = (float)System.Math.Round(body.GetPosition().Y);
            //this.Sprite = 
        }
        //public bool IsColliding(Sprite2d a, Sprite2d b)
        //{
        //    if (a.Position.X < b.Position.X + b.Scale.X &&
        //        a.Position.X + a.Scale.X > b.Position.X &&
        //        a.Position.Y < b.Position.Y + b.Scale.Y &&
        //        a.Position.Y + a.Scale.Y > b.Position.Y)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public bool IsColliding(string tag)
        //{
        //    /*if (a.Position.X < b.Position.X + b.Scale.X &&
        //        a.Position.X + a.Scale.X > b.Position.X &&
        //        a.Position.Y < b.Position.Y + b.Scale.Y &&
        //        a.Position.Y + a.Scale.Y > b.Position.Y)
        //    {
        //        return true;
        //    }*/
        //    foreach(Sprite2d b in EngineCore.AllSprites)
        //    {
        //        if (b.Tag == tag)
        //        {
        //            if (Position.X < b.Position.X + b.Scale.X &&
        //                Position.X + Scale.X > b.Position.X &&
        //                Position.Y < b.Position.Y + b.Scale.Y &&
        //                Position.Y + Scale.Y > b.Position.Y)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}
        public Sprite2d IsColliding(string tag)
        {
            /*if (a.Position.X < b.Position.X + b.Scale.X &&
                a.Position.X + a.Scale.X > b.Position.X &&
                a.Position.Y < b.Position.Y + b.Scale.Y &&
                a.Position.Y + a.Scale.Y > b.Position.Y)
            {
                return true;
            }*/
            foreach (Sprite2d b in EngineCore.AllSprites)
            {
                if (b.Tag == tag)
                {
                    if (Position.X < b.Position.X + b.Scale.X && //+8 trim off x
                        Position.X + Scale.X > b.Position.X &&
                        Position.Y < b.Position.Y + b.Scale.Y &&
                        Position.Y + Scale.Y > b.Position.Y)
                    {
                        return b;
                    }
                }
            }

            return null;
        }
        public void DestroySelf()
        {
            Log.Info($"[SPRITE2D]({Tag}) has been destroyed");
            EngineCore.UnRegisterSprite(this); 
        }

        public Sprite2d UpdateSprite(int currentSprite, List<string> playerSprites)
        {

            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;

            //Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            //Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            this.Directory = (playerSprites[currentSprite]);

            //return Sprite2d(Position,Scale,Directory,Tag);
            return this;
        }
    }
}
