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
    class PlayerAim : Component, Iloadable, IDrawable, IUpdateable
    {
        //Used to draw the line
        private Texture2D pointSprite;
        private Rectangle sourceRectPoint;

        public Vector2 EndPoint { get; set; }
        MouseState mouseS;
        Vector2 mousePos;
        public Vector2 PlayerAimVector { get; set; }

        public PlayerAim(GameObject obj) : base(obj)
        {
        }

        public void LoadContent(ContentManager content)
        {
            pointSprite = content.Load<Texture2D>("rectangle");
            sourceRectPoint = new Rectangle(0, 0, 1, 1);
        }

        public void Update(GameTime gameTime)
        {
            //Finds the direction of the aim line
            mouseS = Mouse.GetState();
            mousePos = new Vector2(mouseS.Position.X, mouseS.Position.Y);
            PlayerAimVector = mousePos - obj.position;
            PlayerAimVector = Vector2.Normalize(PlayerAimVector);
            PlayerAimVector *= 2000; //Sets a standard length for the aim line
            EndPoint = obj.position + PlayerAimVector;
        }

        /// <summary>
        /// Draws unique features to the player, which needs to be drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //finds the angle for which to rotate the aim line
            float angle = MathOperations.AngleBetweenVectorAndHorizontal(PlayerAimVector);

            //Finds the length of the aim line
            float length = TargetPoint(EndPoint);

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
