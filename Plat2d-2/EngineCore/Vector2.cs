using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Vector2()
        {
            X = Zero().X;
            Y = Zero().Y;
        }
        public Vector2(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
            //Vector2.Zero();
        }
        /// <summary>
        /// Returns x and y as zero
        /// </summary>
        /// <returns></returns>
        public static Vector2 Zero()
        {
            return new Vector2(0, 0);
        }
    }
}
