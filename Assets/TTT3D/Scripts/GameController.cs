using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;

public class GameController : MonoBehaviour {

    public Dictionary<Field, Token> GameField = new Dictionary<Field, Token>()
        {
            { Field.TopLeft, Token.SmallCross1 },
            { Field.Middle, Token.MediumCross2 },
            { Field.BottomRight, Token.LargeCross1 },
            { Field.BottomLeft, Token.LargeCircle1 },
            { Field.MiddleLeft, Token.LargeCircle2 }
        };
}
