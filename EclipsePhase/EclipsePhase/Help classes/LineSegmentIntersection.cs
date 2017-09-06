using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    static class LineSegmentIntersection
    {

        /// <summary>
        /// Test whether two line segments intersect. If so, calculate the intersection point.
        /// <see cref="http://stackoverflow.com/a/14143738/292237"/>
        /// </summary>
        /// <param name="p">Vector to the start point of p.</param>
        /// <param name="p2">Vector to the end point of p.</param>
        /// <param name="q">Vector to the start point of q.</param>
        /// <param name="q2">Vector to the end point of q.</param>
        /// <param name="intersection">The point of intersection, if any.</param>
        /// <param name="considerOverlapAsIntersect">Do we consider overlapping lines as intersecting?
        /// </param>
        /// <returns>True if an intersection point was found.</returns>
        public static Vector2 LineSegementsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2)
        {
            Vector2 intersection = new Vector2();
            bool isIntersecting;

            var r = p2 - p;
            var s = q2 - q;
            var rxs = MathOperations.CrossProduct(r, s);
            var qpxr = MathOperations.CrossProduct((q - p), r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (MathOperations.IsZero(rxs) && MathOperations.IsZero(qpxr))
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if ((0 <= MathOperations.DotProduct((q - p), r) && MathOperations.DotProduct((q - p), r) <= MathOperations.DotProduct(r, r))
                    || (0 <= MathOperations.DotProduct((p - q), s) && MathOperations.DotProduct((p - q), s) <= MathOperations.DotProduct(s, s)))
                    isIntersecting = true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                isIntersecting = false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (MathOperations.IsZero(rxs) && !MathOperations.IsZero(qpxr))
                isIntersecting = false;

            // t = (q - p) x s / (r x s)
            var t = MathOperations.CrossProduct((q - p), s) / rxs;

            // u = (q - p) x r / (r x s)

            var u = MathOperations.CrossProduct((q - p), r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!MathOperations.IsZero(rxs) && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // An intersection was found.
                isIntersecting = true;

                // We can calculate the intersection point using either t or u.
                return intersection = p + t * r;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            isIntersecting = false;

            // Returns the first line's end point.
            return p2;
        }
    }
}
