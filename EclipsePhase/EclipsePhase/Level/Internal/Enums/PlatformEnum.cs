using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EclipsePhase.Level.Internal
{
    /// <summary>
    /// All the indexes of diifferent Platform information, when reading a single line of text from the Xml document.
    /// </summary>
    internal enum PlatformEnum : int
    {
        PositionX = 0,
        PositionY = 1,
        ScaleX = 2,
        ScaleY = 3,
        ImagePath = 4,
        OverlayColor = 5,
        ColliderType = 6,
        ColliderInfo = 7
    }
}
