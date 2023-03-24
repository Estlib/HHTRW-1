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
            shapeDef.SetAsBox(32.0f, 32.0f);

            // Add the ground shape to the ground body.
            body.CreateShape(shapeDef);
        }
        public void CreateDynamic()
        {
            // Define the dynamic body. We set its position and call the body factory.
            //bodyDef = new BodyDef();
            bodyDef.Position = new Vec2(this.Position.X, this.Position.Y);
            body = EngineCore.world.CreateBody(bodyDef);

            // Define another box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(1f, 1f);

            // Set the box density to be non-zero, so it will be dynamic.
            shapeDef.Density = 1.0f;

            // Override the default friction.
            shapeDef.Friction = 0.3f;

            // Add the shape to the body.
            body.CreateShape(shapeDef);

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();
        }
        public void ApplyImpulse(Vector2 force, Vector2 point)
        {
            body.ApplyImpulse(new Vec2(force.X, force.Y), new Vec2(point.X, point.Y));
        }
        public void AddForce(Vector2 force, Vector2 point)
        {
            //body.SetBullet();
            //body.SetLinearVelocity(new Vec2(force.X, force.Y));
            body.ApplyForce(new Vec2(force.X, force.Y), new Vec2(point.X, point.Y));
        }
        public void SetVelocity(Vector2 velocity)
        {
            body.SetLinearVelocity(new Vec2(velocity.X, velocity.Y));
        }
        public void UpdatePosition()
        {
            Log.Warning("X is "+ body.GetPosition().X + ". Y is " + body.GetPosition().Y);
            this.Position.X = body.GetPosition().X;
            this.Position.Y = body.GetPosition().Y;
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
                    if (Position.X < b.Position.X + b.Scale.X &&
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
    }
}
