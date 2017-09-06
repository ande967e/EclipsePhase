using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EclipsePhase
{
    static class CollisionCheck
    {
        /// <summary>
        /// Checks for collision between two figures/shapes with the use of SAT (Seperating Axis Theorem).
        /// </summary>
        /// <param name="edgesFigure1"></param>
        /// <param name="posFigure1"></param>
        /// <param name="edgesFigure2"></param>
        /// <param name="posFigure2"></param>
        /// <returns></returns>
        static public bool Check(Vector2[,] edgesFigure1, Vector2 posFigure1, Vector2[,] edgesFigure2, Vector2 posFigure2)
        {
            //Gets the axes for both figures.
            Vector2[] axes1 = Normals(edgesFigure1);
            Vector2[] axes2 = Normals(edgesFigure2);

            // loop over the axes1
            for (int i = 0; i < axes1.GetLength(0); i++)
            {
                Vector2 axis = axes1[i];
                // project both shapes onto the axis
                Projection p1 = FigureProjection(edgesFigure1, axis, posFigure1);
                Projection p2 = FigureProjection(edgesFigure2, axis, posFigure2);
                // do the projections overlap?
                if (!p1.Overlapping(p2))
                {
                    // then we can guarantee that the shapes do not overlap
                    return false;
                }
            }
            // loop over the axes2
            for (int i = 0; i < axes2.GetLength(0); i++)
            {
                Vector2 axis = axes2[i];
                // project both shapes onto the axis
                Projection p1 = FigureProjection(edgesFigure1, axis, posFigure1);
                Projection p2 = FigureProjection(edgesFigure2, axis, posFigure2);
                // do the projections overlap?
                if (!p1.Overlapping(p2))
                {
                    // then we can guarantee that the shapes do not overlap
                    return false;
                }
            }
            // if we get here then we know that every axis had overlap on it
            // so we can guarantee an intersection
            return true;
        }

        /// <summary>
        /// Checks for collision between two figures/shapes with the use of SAT (Seperating Axis Theorem), and returns the pushvector, which is zero if there is no collision.
        /// </summary>
        /// <param name="edgesFigure1"></param>
        /// <param name="posFigure1"></param>
        /// <param name="edgesFigure2"></param>
        /// <param name="posFigure2"></param>
        /// <returns></returns>
        static public Vector2 CheckV2(Vector2[,] edgesFigure1, Vector2 posFigure1, Vector2[,] edgesFigure2, Vector2 posFigure2)
        {
            //Gets the axes for both figures.
            Vector2[] axes1 = Normals(edgesFigure1);
            Vector2[] axes2 = Normals(edgesFigure2);

            //The push vectors
            List<Vector2> axisNorms = new List<Vector2>();

            // loop over the axes1
            for (int i = 0; i < axes1.GetLength(0); i++)
            {
                Vector2 axis = axes1[i];
                // project both shapes onto the axis
                Projection p1 = FigureProjection(edgesFigure1, axis, posFigure1);
                Projection p2 = FigureProjection(edgesFigure2, axis, posFigure2);
                // do the projections overlap?
                if (!p1.Overlapping(p2))
                {
                    // then we can guarantee that the shapes do not overlap, so return a vector with 0 in length
                    return Vector2.Zero;
                }
                else
                {
                    double pushScalar = p1.OverlappingV2(p2);
                    Vector2 norm = Vector2.Normalize(axis);
                    axisNorms.Add((float)pushScalar * norm);
                }
            }
            // loop over the axes2
            for (int i = 0; i < axes2.GetLength(0); i++)
            {
                Vector2 axis = axes2[i];
                // project both shapes onto the axis
                Projection p1 = FigureProjection(edgesFigure1, axis, posFigure1);
                Projection p2 = FigureProjection(edgesFigure2, axis, posFigure2);
                // do the projections overlap?
                if (!p1.Overlapping(p2))
                {
                    // then we can guarantee that the shapes do not overlap, so return a vector with 0 in length
                    return Vector2.Zero;
                }
                else
                {
                    double pushScalar = p1.OverlappingV2(p2);
                    Vector2 norm = Vector2.Normalize(axis);
                    axisNorms.Add((float)pushScalar * norm);
                }
            }
            // if we get here then we know that every axis had overlap on it
            // so we can guarantee an intersection

            //Finds the smallest push vector
            Vector2 pushVector = new Vector2(1000000, 0);
            for (int i = 0; i < axisNorms.Count; i++)
            {
                if (axisNorms[i].Length() < pushVector.Length())
                    pushVector = -axisNorms[i];
            }

            //Returns the push vector
            return pushVector;
        }


        /// <summary>
        /// Returns the normals of to the edges.
        /// <param name="edges"></param>
        /// <returns></returns>
        static private Vector2[] Normals(Vector2[,] edges)
        {
            Vector2[] normals = new Vector2[edges.GetLength(0)];
            // loop over the vertices
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                // the perpendicular method is just (x, y) => (-y, x) or (y, -x) depending on which direction the wanted normal should point
                normals[i] = new Vector2(-edges[i, 0].Y, edges[i, 0].X);
            }

            return normals;
        }

        /// <summary>
        /// Returns the projection of a figures vertixes onto an axis.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        static private Projection FigureProjection(Vector2[,] edgesFigure, Vector2 axis, Vector2 pos)
        {
            //When projecting a polygon onto an axis we; loop over all the vertices performing the dot product with the axis and storing the minimum and maximum.

            //Normalizes the axis to get accurate projections.
            Vector2 normAxis = axis;
            normAxis.Normalize();

            //formal for the dot product between two vectors:  Dot(v1, v2) = (dx1 * dx2) + (dy1 * dy2)

            //Sets a starting point for the min and max values of the projection.
            double min = DotProduct(normAxis, edgesFigure[0, 1] + pos);
            double max = min;
            //Projects the edges and replaces the min and/or max values if nessecary.
            for (int i = 1; i < edgesFigure.GetLength(0); i++)
            {
                // NOTE: the axis must be normalized to get accurate projections
                double p = DotProduct(normAxis, edgesFigure[i, 1] + pos);
                if (p < min)
                {
                    min = p;
                }
                else if (p > max)
                {
                    max = p;
                }
            }
            Projection proj = new Projection(min, max);
            return proj;
        }

        /// <summary>
        /// Returns the dot product value of two vectors.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        static public double DotProduct(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X) + (v1.Y * v2.Y);
        }
    }
}
