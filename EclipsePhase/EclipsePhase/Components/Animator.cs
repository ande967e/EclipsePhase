using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EclipsePhase
{
    class Animator : Component, IUpdateable
    {
        public Dictionary<string, Animation> spriteFrames { get; set; }

        private SpriteRenderer spriteRenderer;
        private float currentIndex;
        private float fps;
        private float timeElapsed;
        private Rectangle[] rectangles;
        private string frameName;

        public Rectangle[] Rectangles { get { return rectangles; } }

        public Animator(GameObject obj) : base(obj)
        {
            fps = 5;
            ////initialize the spriterenderer class
            //this.spriteRenderer = obj.GetComponent<SpriteRenderer>();

            //initialize the dictionary
            spriteFrames = new Dictionary<string, Animation>();
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentIndex = fps * timeElapsed;

            if (currentIndex > rectangles.Length - 1)
            {
                OnAnimationDone(frameName);
                timeElapsed = 0;
                currentIndex = 0;
            }
            obj.GetComponent<SpriteRenderer>().SpriteRectangle = rectangles[(int)currentIndex];
        }

        public void CreateAnimation(Animation animation, string name)
        {
            spriteFrames.Add(name, animation);
        }

        public void CheckAnimation(string frameName)
        {
            if (this.frameName != frameName)
            {
                //checks if it's a new animation
                this.rectangles = spriteFrames[frameName].Rectangles;
                //sets the rectangle
                obj.GetComponent<SpriteRenderer>().SpriteRectangle = rectangles[0];
                //sets the offset
                obj.GetComponent<SpriteRenderer>().Offset = spriteFrames[frameName].Offset;
                //sets the animation name
                this.frameName = frameName;
                //sets the fps
                this.fps = spriteFrames[frameName].Fps;

                //resets the animation 

                timeElapsed = 0;
                currentIndex = 0;
            }
        }
        public void OnAnimationDone(string animationName)
        {
            foreach (Component component in obj.components)
            {
                if (component is IAnimateable)
                {
                    (component as IAnimateable).OnAnimationDone(animationName);
                }
            }
        }
    }
}
