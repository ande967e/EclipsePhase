﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase
{
    interface IAnimateable
    {
        void OnAnimationDone(string animationName);
    }
}