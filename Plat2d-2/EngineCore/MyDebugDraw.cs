using Box2DX.Common;
using Box2DX.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plat2d_2.EngineCore
{
    public class MyDebugDraw : DebugDraw
    {
        public override void DrawPolygon(Vec2[] vertices, int vertexCount, Color color)
        {
            // draw a polygon with the given vertices and color
        }

        public override void DrawSolidPolygon(Vec2[] vertices, int vertexCount, Color color)
        {
            // draw a filled polygon with the given vertices and color
        }

        public override void DrawCircle(Vec2 center, float radius, Color color)
        {
            // draw a circle with the given center, radius, and color
        }

        public override void DrawSolidCircle(Vec2 center, float radius, Vec2 axis, Color color)
        {
            // draw a filled circle with the given center, radius, axis, and color
        }

        public override void DrawSegment(Vec2 p1, Vec2 p2, Color color)
        {
            // draw a line segment between the two given points with the given color
        }

        public override void DrawXForm(XForm xf)
        {
            // draw the given transform
        }
    }
}
