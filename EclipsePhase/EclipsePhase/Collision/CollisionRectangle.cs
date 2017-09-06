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
    class CollisionRectangle : Component, IDrawable, Iloadable
    {
        //One coloum is the edges ([i,0]), where the second ([i,1]) is a translocation used to indicated the vectors true placement around the position.
        public Vector2[,] Edges { get; set; }

        //Used to draw the points
        private Texture2D pointSprite;
        private Rectangle sourceRectPoint;

        Vector2 vecStart;
        Vector2 vecEnd;
        Vector2 edgeVec;

        public CollisionRectangle(GameObject obj) : base(obj)
        {
            Edges = new Vector2[4, 4];
            GenerateSides();
        }

        private void GenerateSides()
        {
            //The first point on the edge on the rectangle
            vecStart = new Vector2(0, 0);
            //The end point on the edge on the rectangle
            vecEnd = new Vector2(0, 0);
            //The vector indicating the edge
            edgeVec = new Vector2(0, 0);

            //First edge
            vecStart = new Vector2(obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            vecEnd = new Vector2(obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, -obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            Add(0);

            //Second edge
            vecStart = new Vector2(obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, -obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            vecEnd = new Vector2(-obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, -obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            Add(1);

            //Third edge
            vecStart = new Vector2(-obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, -obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            vecEnd = new Vector2(-obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            Add(2);

            //Fourth edge
            vecStart = new Vector2(-obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            vecEnd = new Vector2(obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Width / 2, obj.GetComponent<SpriteRenderer>().SpriteRectangleForCollision.Height / 2);
            Add(3);
        }

        private void Add(int i)
        {
            edgeVec = vecEnd - vecStart;
            float length = edgeVec.Length();
            Edges[i, 0] = edgeVec;
            Edges[i, 1] = vecStart;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG //draws the points which makes up the collision box
            for (int i = 0; i < Edges.GetLength(0) - 1; i++)
            {
                spriteBatch.Draw(pointSprite, Edges[i, 1] + obj.position, sourceRectPoint, Color.Red, 1f, Vector2.Zero, 1f, SpriteEffects.None, 1);
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
