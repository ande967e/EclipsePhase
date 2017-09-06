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
    public class Player : Component, IUpdateable
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
    }
}
