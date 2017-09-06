using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    public abstract class Component
    {
        public GameObject obj { get; private set; }

        public Component(GameObject obj)
        {
            this.obj = obj;
        }
    }
}
