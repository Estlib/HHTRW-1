using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class Shape2d
    {
        public Vector2 Position = null;
        public Vector2 Scale = null;
        public string Tag = "";

        public Shape2d(Vector2 Position, Vector2 Scale, string Tag) 
        { 
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;

            Log.Info($"[SHAPE2d]({Tag}) has been registered");
            EngineCore.RegisterShape(this);
        }
        public void DestroySelf()
        {
            Log.Info($"[SHAPE2d]({Tag}) has been destroyed");
            EngineCore.UnRegisterShape(this);
        }
    }
}
