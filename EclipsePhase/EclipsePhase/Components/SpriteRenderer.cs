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
        public Vector2 scaleFactor;
        public float Rotation { get; set; }
        public float layerDepth;
        public Rectangle SpriteRectangle { get; set; }
        public Vector2 Offset { get; set; }
        private Vector2 origin;
        public Color Color { get; set; }
        public Rectangle SpriteRectangleForCollision
        {
            get { return new Rectangle(0, 0, (int)(GameWorld.Instance.SpriteWidth * scaleFactor.X), (int)(GameWorld.Instance.SpriteHeight * scaleFactor.Y)); }
        }

        public SpriteRenderer(GameObject obj, string spriteName, float scaleFactor, float rotation, float layerDepth) : base(obj)
        {
            this.SpriteName = spriteName;
            this.scaleFactor = new Vector2(scaleFactor, scaleFactor);
            this.Rotation = rotation;
            this.layerDepth = layerDepth;
            this.Color = Color.White;
        }

        public SpriteRenderer(GameObject obj, string spriteName, Vector2 scaleFactor, float rotation, float layerDepth) : base(obj)
        {
            this.SpriteName = spriteName;
            this.scaleFactor = scaleFactor;
            this.Rotation = rotation;
            this.layerDepth = layerDepth;
            this.Color = Color.White;
        }

        public void LoadContent(ContentManager content)
        {
            Sprite = content.Load<Texture2D>(SpriteName);
            origin.X = GameWorld.Instance.SpriteWidth / 2;
            origin.Y = GameWorld.Instance.SpriteHeight / 2;
            SpriteRectangle = new Rectangle(0, 0, GameWorld.Instance.SpriteWidth, GameWorld.Instance.SpriteHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, obj.position + Offset, SpriteRectangle, Color, Rotation, origin, scaleFactor, SpriteEffects.None, layerDepth);
        }
    }
}
