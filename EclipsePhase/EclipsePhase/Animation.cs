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
    class Animation
    {
        public Rectangle[] Rectangles { get; set; }
        public float Fps { get; set; }
        public Vector2 Offset { get; set; }

        public int MyFrames { get; set; }

        public Animation(int frames, int yPos, int xStartFrame, int width, int height, float fps, Vector2 offset)
        {
            Rectangles = new Rectangle[frames];
            Offset = offset;
            Fps = fps;
            MyFrames = frames;
            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle((i + xStartFrame) * width, yPos, width, height);
            }
        }
    }
}