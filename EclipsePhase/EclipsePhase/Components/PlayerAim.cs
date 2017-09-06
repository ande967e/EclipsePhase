using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    class PlayerAim : Component, Iloadable, IDrawable
    {
        //Used to draw the line
        private Texture2D pointSprite;
        private Rectangle sourceRectPoint;

        public PlayerAim(GameObject obj) : base(obj)
        {
        }

        public void LoadContent(ContentManager content)
        {
            pointSprite = content.Load<Texture2D>("rectangle");
            sourceRectPoint = new Rectangle(0, 0, 1, 1);
        }

        /// <summary>
        /// Draws unique features to the player, which needs to be drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Finds the direction of the aim line
            MouseState mouseS = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouseS.Position.X, mouseS.Position.Y);
            Vector2 temp = mousePos - obj.position;
            temp = Vector2.Normalize(temp);
            temp *= 2000; //Sets a standard length for the aim line
            Vector2 endPoint = obj.position + temp;
            float angle = 0;

            //finds the angle for which to rotate the aim line
            if (temp.X == 0 && temp.Y == 0)
                angle = 0;
            else if (temp.X == 0)
                angle = 0;
            if (temp.X > 0)
                angle = (float)Math.Atan(temp.Y / temp.X);
            if (temp.X < 0)
                angle = (float)Math.Atan(temp.Y / temp.X) + (float)Math.PI;

            //Finds the length of the aim line
            float length = TargetPoint(endPoint);

#if DEBUG //draws the line
            spriteBatch.Draw(pointSprite, obj.position, sourceRectPoint, Color.Red, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 1);
#endif
        }

        private float TargetPoint(Vector2 endPoint)
        {
            Vector2 temp = Vector2.Zero;
            Vector2 interSectionPoint = endPoint;

            for (int i = 0; i < GameWorld.Instance.gameObjectPool.ActiveEnvironmentList.Count(); i++)
            {
                for (int j = 0; j < GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].GetComponent<CollisionRectangle>().Edges.GetLength(0); j++)
                {
                    //Start and endpoint of the line which is checked against
                    Vector2 q1 = GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].position + GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].GetComponent<CollisionRectangle>().Edges[j, 1];
                    Vector2 q2 = GameWorld.Instance.gameObjectPool.ActiveEnvironmentList[i].GetComponent<CollisionRectangle>().Edges[j, 0] + q1;

                    //Checks
                    temp = LineSegmentIntersection.LineSegementsIntersect(obj.position, endPoint, q1, q2);
                    if ((temp - obj.position).Length() < (interSectionPoint - obj.position).Length())
                        interSectionPoint = temp;
                }
            }

            //Finds the length from player to the intersection point
            float length = (interSectionPoint - obj.position).Length();

            return length;
        }
    }
}
