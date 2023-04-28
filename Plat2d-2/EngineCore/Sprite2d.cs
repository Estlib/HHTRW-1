using System.Collections.Generic;
using System.Drawing;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Box2DX.Common;
using System;

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
        private Bitmap bitmap;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Tag;
        }
        /// <summary>
        /// constructor for Sprite2d. taking in position and scale vectors aswell as the bitmaps directory and a tag string.
        /// This is the primary constructor for this object
        /// </summary>
        /// <param name="Position">Sprite location</param>
        /// <param name="Scale">Sprite size.</param>
        /// <param name="Directory">string for loading sprite from directory</param>
        /// <param name="Tag">name of the type of object this sprite is, "Air" "Ground" etc.</param>
        public Sprite2d(Vector2 Position, Vector2 Scale, string Directory, string Tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Directory = Directory;
            this.Tag = Tag;

            Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            this.Sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);

            Log.Info($"[SPRITE2D]({Directory} {Tag}) sprite has been registered");
            EngineCore.RegisterSprite(this);
        }
        /// <summary>
        /// constructor for a reference sprite. this only takes in a directory for the bitmap.
        /// it is used to create a sprite into memory for use by a different constructor in order to
        /// save memory space and disk access reads for faster, more efficient processing.
        /// it is a utility constructor for the object.
        /// </summary>
        /// <param name="Directory">directory string for the sprites location</param>
        public Sprite2d(string Directory)
        {
            this.IsReference = true; 
            //this.Isreference is defined internally because there is no need for this to ever be false, if its not a reference sprite, it will use any other constructor anyway.
            this.Directory = Directory;

            Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            this.Sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);

            Log.Info($"[SPRITE2D]({Directory} {Tag}) sprite has been registered");
            EngineCore.RegisterSprite(this);
        }
        /// <summary>
        /// constructor for a rendered Sprite2d. taking in position and scale vectors aswell as a reference sprite and a tag string.
        /// it is used to render static objects only(!) onto the screen.
        /// </summary>
        /// <param name="Position">Sprite location</param>
        /// <param name="Scale">Sprite size.</param>
        /// <param name="Reference">Sprite2d for using a sprite loaded into memory</param>
        /// <param name="Tag">name of the type of object this sprite is, "Air" "Ground" etc.</param>
        public Sprite2d(Vector2 Position, Vector2 Scale, Sprite2d Reference, string Tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;

            //Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            //Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            this.Sprite = Reference.Sprite; //sets the sprite bitmap to be the one from the reference sprite

            Log.Info($"[SPRITE2D]({Directory} {Tag}) sprite has been registered");
            EngineCore.RegisterSprite(this);
        }
        /// <summary>
        /// constructor for an animated Sprite2d. taking in position and scale vectors aswell as a bitmap image and a tag string.
        /// it is used to render player object only(!) onto the screen. The bitmap passed into it is animated in the main game loop using a bitmap list and methods external to this class
        /// </summary>
        /// <param name="Position">Sprite location.</param>
        /// <param name="Scale">Sprite size.</param>
        /// <param name="bitmap">Bitmap to use for the sprite.</param>
        /// <param name="Tag">Name of the type of object this sprite is, "Air" "Ground" etc.</param>
        public Sprite2d(Vector2 Position, Vector2 Scale, Bitmap bitmap, string Tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;

            //Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            //Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            this.Sprite = bitmap;

            Log.Info($"[SPRITE2D]({Directory} {Tag}) has been registered");
            EngineCore.RegisterSprite(this);
        }
        public void DestroyStatic(Sprite2d sprite)
        {
            if (sprite.Tag == "Enemy")
            {
                Log.Highlight($"Enemy has invoked DestroyBody");
            }
            EngineCore.world.DestroyBody(sprite.body);
        }
        /// <summary>
        /// Creates a static body into the world.
        /// used for tiles and immobile objects.
        /// </summary>
        public void CreateStatic()
        {
            // Define the ground body.
            bodyDef = new BodyDef();
            bodyDef.Position = new Vec2(this.Position.X-8, this.Position.Y-7);

            // Call the body factory which  creates the ground box shape.
            // The body is also added to the world.
            body = EngineCore.world.CreateBody(bodyDef);

            // Define the ground box shape.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.Density = 0.0f;

            // The extents are the half-widths of the box.
            shapeDef.SetAsBox(22.0f, 23.0f);

            // Add the ground shape to the ground body.
            body.CreateShape(shapeDef);

            body.SetUserData(this);
        }
        /// <summary>
        /// Creates a dynamic body into the world.
        /// used for player only currently.
        /// </summary>
        public void CreateDynamic()
        {
            Log.Info($"{this.Tag} is being made dynamic");
            // Define the dynamic body. We set its position and call the body factory.
            //bodyDef = new BodyDef();
            bodyDef.Position = new Vec2(this.Position.X, this.Position.Y);
            body = EngineCore.world.CreateBody(bodyDef);
            //body.IsBullet(); //TODO: why is this here, whats it do

            // Define another box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(1.0f, 1.0f);

            // Set the box density to be non-zero, so it will be dynamic.
            shapeDef.Density = 1000.0f;

            // Override the default friction.
            shapeDef.Friction = 2.0f;

            shapeDef.Restitution = 0.0f;

            try
            {
                // Add the shape to the body.
                body.CreateShape(shapeDef);  //enemy body assert error here

            }
            catch (Exception)
            {
                Log.Error($"Couldnt create a body for {this.Sprite}");
            }

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();

            body.SetUserData(this);
            Log.Info($"{this.Tag} has been made dynamic");
        }
        /// <summary>
        /// Method to apply an impulse to a dynamic body
        /// </summary>
        /// <param name="force">direction of the applied impulse in Vector2 format</param>
        /// <param name="point">the point where the impulse is applied toward</param>
        public void ApplyImpulse(Vector2 force, Vector2 point)
        {
            body.ApplyImpulse(new Vec2(force.X, force.Y), new Vec2(point.X, point.Y));
        }
        /// <summary>
        /// Method to apply a force to a dynamic body
        /// </summary>
        /// <param name="force">direction of the applied impulse in Vector2 format</param>
        /// <param name="point">the point where the impulse is applied toward</param>
        public void AddForce(Vector2 force, Vector2 point)
        {
            //body.SetLinearVelocity(new Vec2(force.X, force.Y));
            body.ApplyForce(new Vec2(force.X, force.Y), new Vec2(point.X, point.Y));
        }
        /// <summary>
        /// Method to set the velocity of a dynamic body
        /// </summary>
        /// <param name="velocity">Velocity Vector2</param>
        public void SetVelocity(Vector2 velocity)
        {
            body.SetLinearVelocity(new Vec2(velocity.X, velocity.Y));
        }
        /// <summary>
        /// Method to get the X axis velocity of a dynamic body.
        /// </summary>
        /// <returns>Float of the X velocity</returns>
        public float GetXVelocity()
        {
            return body.GetLinearVelocity().X;
        }
        /// <summary>
        /// Method to get the Y axis velocity of a dynamic body.
        /// </summary>
        /// <returns>Float of the Y velocity</returns>
        public float GetYVelocity()
        {
            return body.GetLinearVelocity().Y;
        }
        /// <summary>
        /// updates player position.
        /// </summary>
        public void UpdatePosition()
        {
            //Log.Warning("X is " + body.GetPosition().X + ". Y is " + body.GetPosition().Y);
            this.Position.X = (float)System.Math.Round(body.GetPosition().X);
            this.Position.Y = (float)System.Math.Round(body.GetPosition().Y);
            //this.Sprite = 
        }
        /// <summary>
        /// Method to collision check any sprite.
        /// when a collision is detected, b is returned. b is an object.
        /// when it isnt, null is returned
        /// </summary>
        /// <param name="tag">What tag to check for collisions with</param>
        /// <returns></returns>
        //public Sprite2d IsColliding(string tag, int currentLevel)
        //{
        //    var thisLevel = EngineCore.LevelSprites[currentLevel];
        //    for (int i = 0; i < thisLevel.Count; i++)
        //    {
        //        Sprite2d b = thisLevel[i];
        //        if (b.Tag == tag)
        //        {
        //            if (Position.X+8 < b.Position.X + b.Scale.X && //+8 trim off x
        //                Position.X-8 + Scale.X > b.Position.X &&
        //                Position.Y < b.Position.Y + b.Scale.Y &&
        //                Position.Y + Scale.Y > b.Position.Y)
        //            {
        //                //Log.Info($"i is {i}");
        //                //Log.Info($"b is {b}");
        //                return b;
        //            }
        //        }
        //    }
        //    //Log.Info($"b is null");
        //    return null;
        //}
        public Sprite2d IsColliding(string tag, int currentLevel)
        {
            var thisLevel = EngineCore.LevelSprites[currentLevel];
            for (int i = 0; i < thisLevel.Count; i++)
            {
                Sprite2d b = thisLevel[i];
                if (b.Tag == tag)
                {
                    if (Position.X + 8 < b.Position.X + b.Scale.X && //+8 trim off x
                        Position.X - 8 + Scale.X > b.Position.X &&
                        Position.Y < b.Position.Y + b.Scale.Y &&
                        Position.Y + Scale.Y > b.Position.Y)
                    {
                        //Log.Info($"i is {i}");
                        //Log.Info($"b is {b}");
                        return b;
                    }
                }
            }
            //Log.Info($"b is null");
            return null;
        }
        /// <summary>
        /// Destroys a sprite that calls this method.
        /// </summary>
        public void DestroySelf()
        {
            //Log.Info($"[SPRITE2D]({Tag}) has been destroyed");
            //EngineCore.world.DestroyBody(body);
            EngineCore.UnRegisterSprite(this); 
        }
        /// <summary>
        /// destructor for the sprite2d class
        /// </summary>
        //~Sprite2d()
        //{
        //    //if (body != null)
        //    //{
        //    //    DestroyStatic(this);
        //    //}
        //    Log.Info($"[SPRITE2D]({Tag}) has been destroyed using the destructor");
        //    //if (body != null)
        //    //{
        //    //    EngineCore.world.DestroyBody(body);
        //    //    Log.Highlight($"{this.body}");
        //    //}
        //    //Log.Info($"Unregistering {this}");
        //    EngineCore.world.DestroyBody(body);
        //    EngineCore.UnRegisterSprite(this);
        //    EngineCore.LevelSprites[DemoGame.currentLevel] = new List<Sprite2d> { this };
        //}
        /// <summary>
        /// unused method for updating the sprite in sprite2d class
        /// </summary>
        /// <param name="currentSprite"></param>
        /// <param name="playerSprites"></param>
        /// <returns></returns>
        public Sprite2d UpdateSprite(int currentSprite, List<string> playerSprites)
        {
            // unused and incomplete method, do not use, this is kept for future iteration.
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;

            //Image tmp = Image.FromFile($"assets/sprites/{Directory}.png");
            //Bitmap sprite = new Bitmap(tmp/*, (int)this.Scale.X, (int)this.Scale.Y*/);
            this.Directory = (playerSprites[currentSprite]);

            //return Sprite2d(Position,Scale,Directory,Tag);
            return this;
        }

        internal bool HasBody()
        {
            return this.body != null;
        }
    }
}
