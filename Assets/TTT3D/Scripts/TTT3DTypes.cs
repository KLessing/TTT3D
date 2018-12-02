using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extra Namespace to prevent conflicts with other type definitions
namespace TTT3DTypes
{
    public enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

    public enum Token
    {
        SmallCross1, MediumCross1, LargeCross1, SmallCircle1, MediumCircle1, LargeCircle1,
        SmallCross2, MediumCross2, LargeCross2, SmallCircle2, MediumCircle2, LargeCircle2
    };    

    public enum Player { Cross, Circle }
}