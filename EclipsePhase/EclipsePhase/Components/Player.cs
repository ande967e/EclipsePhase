using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace EclipsePhase
{
    public class Player : Component, IUpdateable, Iloadable, IDrawable
    {
        private Vector2 direction;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        public int Health { get; set; }
        private float speed;
        private float maxSpeed;
        private float minSpeed;
        private float acceleration;
        private float deacceleration;

        private Vector2 g; //Gravity vector
        private Vector2 push; //Push vector for when colliding with solid objects
        private Vector2 combinedPush; //All push vector added together
        private bool onGround;

        private float vel;
        private bool jumping;
        private float jumpSpeed;
        private float jumpSpeedStart;
        private float jumpDeacceleration;
        private float jumpSpeedAcceleration; //Kinda translates to the height one jumps
        private float jumpSpeedMax;
        private int horizontalDirection;

        public float gravity; //Gravity acceleration
        private float gravitySpeed;
        private float gravityMaxSpeed;

        private Vector2 translation;

        //Used to draw the line
        private Texture2D pointSprite;
        private Rectangle sourceRectPoint;

        public Player(GameObject obj, Vector2 direction, int health, bool antiGravity) : base(obj)
        {
            this.direction = direction;
            this.Health = health;

            this.gravity = 9;
            gravityMaxSpeed = 60;

            jumping = true;
            jumpSpeedStart = 30;
            jumpDeacceleration = 0.0001f;
            jumpSpeedAcceleration = 30;
            jumpSpeedMax = 210;

            speed = 0;
            minSpeed = 0;
            maxSpeed = 70;
            acceleration = 10f;
            deacceleration = 30f;
            horizontalDirection = 0;

            spriteRenderer = obj.GetComponent<SpriteRenderer>();
            animator = obj.GetComponent<Animator>();
            onGround = false;
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        /// <summary>
        /// Handles player movement.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Move(GameTime gameTime)
        {
            translation = Vector2.Zero;
            push = Vector2.Zero;
            combinedPush = Vector2.Zero;

            //Horizontal movement
            if (GameWorld.Instance.Keystate.IsKeyDown(Keys.D) || GameWorld.Instance.Keystate.IsKeyDown(Keys.A))
            {
                vel += acceleration;
                if (vel > maxSpeed)
                    vel = maxSpeed;
                if (GameWorld.Instance.Keystate.IsKeyDown(Keys.D))
                    horizontalDirection = 1;
                if (GameWorld.Instance.Keystate.IsKeyDown(Keys.A))
                    horizontalDirection = -1;
            }
            else
            {
                vel -= deacceleration;
                if (vel < 0)
                    vel = 0;
            }
            translation.X += vel * horizontalDirection / gameTime.ElapsedGameTime.Milliseconds;

            //Jump
            if (GameWorld.Instance.Keystate.IsKeyDown(Keys.Space) && !jumping)
            {
                jumpSpeed = jumpSpeedStart;
                jumping = true;
            }
            if (jumping)
            {
                jumpSpeed += jumpSpeedAcceleration;
                if (jumpSpeed > jumpSpeedMax)
                    jumpSpeed = jumpSpeedMax;
                translation.Y -= jumpSpeed / gameTime.ElapsedGameTime.Milliseconds;
            }

            //Gravity
            if (onGround)
                gravitySpeed = 0;
            gravitySpeed += gravity / gameTime.ElapsedGameTime.Milliseconds;
            if (gravitySpeed > gravityMaxSpeed)
                gravitySpeed = gravityMaxSpeed;

            translation.Y += gravitySpeed;

            //Collision
            Collision();

            obj.position += translation /*+ combinedPush*/;
        }

        /// <summary>
        /// Handles collision.
        /// </summary>
        private void Collision()
        {
            List<GameObject> tempColList = GameWorld.Instance.gameObjectPool.CollisionListForPlayer();
            //Checks for collision and adds to the final pushvector if nessecary
            for (int i = 0; i < tempColList.Count; i++)
            {
                if (tempColList[i].GetComponent<Environment>() != null)
                {
                    //Places the object ontop the environment
                    push = CollisionCheck.CheckV2(obj.GetComponent<CollisionRectangle>().Edges, obj.position + translation, tempColList[i].GetComponent<CollisionRectangle>().Edges, tempColList[i].position);
                    translation += push;
                    //Used for checks
                    combinedPush += push;
                }
            }
            //Checks if the final push vector is larger than 0
            if (combinedPush.Length() > 0 && combinedPush.Y < 0)
            {
                onGround = true;
                jumping = false;
            }
            else
            {
                onGround = false;
            }
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
