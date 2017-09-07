using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    static class MathOperations
    {

        public static float DotProduct(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static float CrossProduct(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.Y) - (v1.Y * v2.X);
        }

        private static double Epsilon = 1e-10;
        public static bool IsZero(double d)
        {
            return Math.Abs(d) < Epsilon;
        }

        /// <summary>
        /// Finds the angle between a vector and the horizontal plan, and returns a float.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public static float AngleBetweenVectorAndHorizontal(Vector2 vec)
        {
            float angle = 0;
            //finds the angle for which to rotate the aim line
            if (vec.X == 0 && vec.Y == 0)
                angle = 0;
            else if (vec.X == 0)
                angle = (float)Math.Atan(Math.PI/2);
            if (vec.X > 0)
                angle = (float)Math.Atan(vec.Y / vec.X);
            if (vec.X < 0)
                angle = (float)Math.Atan(vec.Y / vec.X) + (float)Math.PI;
            return angle;
        }
    }
}
