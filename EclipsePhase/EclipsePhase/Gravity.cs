using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace EclipsePhase
{
    class Gravity : Component, IUpdateable
    {
        public Gravity(GameObject obj) : base(obj)
        {
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}
