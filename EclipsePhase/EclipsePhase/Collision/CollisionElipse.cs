using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    class CollisionElipse : Component
    {
        public float LRadius { get; }
        public float SRadius { get; }

        //One coloum is the edges ([i,0]), where the second ([i,1]) is a translocation used to indicated the vectors true placement around the position.
        public Vector2[,] edges;
        public List<float> lengthOfEdges;

        //Indicates the number of edges that make up the polygon
        private int numberOfEdges = 36;

        //Used to draw the points
        private Texture2D pointSprite;
        private Rectangle sourceRectPoint;

        public CollisionElipse(GameObject obj, float lRadius, float sRadius) : base(obj)
        {
            this.LRadius = lRadius;
            this.SRadius = sRadius;
        }

        private void GenerateSides()
        {
            //The first point on the edge of the circle
            Vector2 vecStart = new Vector2(SRadius, 0);
            //The end point of the edge of the circle
            Vector2 vecEnd = new Vector2(0, 0);
            //The vector indicating the edge
            Vector2 edgeVec = new Vector2(0, 0);
            //Generates the rest of the points on the circles edge, and thereby makes the edges by combining two points.
            for (int i = 1; i < numberOfEdges; i++)
            {
                vecEnd = RotPointsAroundPointMath.RotatePoint(vecStart, Vector2.Zero, (360 / numberOfEdges));
                edgeVec = vecEnd - vecStart;
                edges[i, 0] = edgeVec;
                edges[i, 1] = vecStart;

                float length = edgeVec.Length();
                lengthOfEdges.Add(length);

                //Changes the start point for the next edge
                vecStart = vecEnd;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG //draws the points which makes up the collision box
            for (int i = 0; i < edges.GetLength(0) - 1; i++)
            {
                spriteBatch.Draw(pointSprite, edges[i, 1] + obj.position, sourceRectPoint, Color.Red, 1f, Vector2.Zero, 1f, SpriteEffects.None, 1);
            }
#endif
        }

        public void LoadContent(ContentManager content)
        {
            pointSprite = content.Load<Texture2D>("rectangle");
            sourceRectPoint = new Rectangle(0, 0, 2, 2);
        }
    }
}
