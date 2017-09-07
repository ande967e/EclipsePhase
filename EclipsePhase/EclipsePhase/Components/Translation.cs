using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EclipsePhase
{
    //This is to be added to an object after all other objects that can affect the objects location.

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
