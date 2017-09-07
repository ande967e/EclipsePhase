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
        public bool OnGround { get; set; }

        private float vel;
        private bool jumping;
        private float jumpSpeed;
        private float jumpSpeedStart;
        private float jumpDeacceleration;
        private float jumpSpeedAcceleration; //Kinda translates to the height one jumps
        private float jumpSpeedMax;
        private int horizontalDirection;

        private Vector2 translation;

        public Player(GameObject obj, Vector2 direction, int health, bool antiGravity) : base(obj)
        {
            this.direction = direction;
            this.Health = health;

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
            OnGround = false;
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
            Jump(gameTime);
        }

        /// <summary>
        /// Handles player movement.
        /// </summary>
        /// <param name="gameTime"></param>
        private void Move(GameTime gameTime)
        {
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

            obj.GetComponent<Translation>().translationVec.X += vel * horizontalDirection / gameTime.ElapsedGameTime.Milliseconds;
        }

        /// <summary>
        /// Handles Player jump.
        /// </summary>
        /// <param name="gameTime"></param>
        private void Jump(GameTime gameTime)
        {
            //Checks if on ground 
            if (obj.GetComponent<Gravity>() != null && obj.GetComponent<Gravity>().OnGround)
                jumping = false;

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
                obj.GetComponent<Translation>().translationVec.Y -= jumpSpeed / gameTime.ElapsedGameTime.Milliseconds;
            }
        }        
    }
}
