using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EclipsePhase
{
    class Translation : Component, IUpdateable
    {
        public Vector2 translationVec;

        public Translation(GameObject obj) : base(obj)
        {
        }

        public void Update(GameTime gameTime)
        {
            obj.position += translationVec;
            translationVec = Vector2.Zero;
        }
    }
}
