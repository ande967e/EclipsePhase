using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace EclipsePhase
{
    class SpriteRenderer : Component, IDrawable, Iloadable
    {
        public Texture2D Sprite { get; set; }
        public string SpriteName { get; set; }
        public float scaleFactor;
        public float Rotation { get; set; }
        public float layerDepth;
        public Vector2 Offset { get; set; }
        public Color Color { get; set; }

        public Rectangle SpriteRectangle {
            get => spriteRectangle.Value;
            set => spriteRectangle = value;
        }

        public Rectangle SpriteRectangleForCollision
        {
            get { return new Rectangle(0, 0,
                (int)(scale != null ? scale.Value.X : GameWorld.Instance.SpriteWidth * scaleFactor), 
                (int)(scale != null ? scale.Value.Y : GameWorld.Instance.SpriteHeight * scaleFactor)); }
        }

        private Rectangle? spriteRectangle;
        private Vector2? scale;
        private Vector2 origin = Vector2.Zero;

        public SpriteRenderer(GameObject obj, string spriteName, float rotation, float layerDepth, float? scaleFactor = null, Rectangle? sourceRectangle = null, Vector2? scale = null) : base(obj)
        {
            if (scaleFactor == null && scale == null)
                scaleFactor = 1;

            this.SpriteName = spriteName;
            this.Rotation = rotation;
            this.layerDepth = layerDepth;
            this.Color = Color.White;
            //this.spriteRectangle = sourceRectangle;
            this.scale = scale;

            if (scale != null)
                this.origin = scale.Value / 2;

            if (scaleFactor != null)
                this.scaleFactor = scaleFactor.Value;
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>(SpriteName);

            if (scale == null)
            {
                origin.X = GameWorld.Instance.SpriteWidth / 2;
                origin.Y = GameWorld.Instance.SpriteHeight / 2;
            }

            SpriteRectangle = new Rectangle(0, 0, GameWorld.Instance.SpriteWidth, GameWorld.Instance.SpriteHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (scale == null)
                spriteBatch.Draw(Sprite, obj.position + Offset, SpriteRectangle, Color, Rotation, origin, scaleFactor, SpriteEffects.None, layerDepth);
            else
                spriteBatch.Draw(Sprite, null, new Rectangle((obj.position + Offset).ToPoint(), scale.Value.ToPoint()), SpriteRectangle, origin, Rotation, null, Color, SpriteEffects.None, layerDepth);
        }
    }
}
