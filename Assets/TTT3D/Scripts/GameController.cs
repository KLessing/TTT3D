using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;
using AugmentedImage;

public class GameController : MonoBehaviour {

    public Dictionary<Field, Token> GameField = new Dictionary<Field, Token>()
    {
        { Field.TopLeft, Token.SmallCross1 },
        { Field.Middle, Token.MediumCross2 },
        { Field.BottomRight, Token.LargeCross1 },
        { Field.BottomLeft, Token.LargeCircle1 },
        { Field.MiddleLeft, Token.LargeCircle2 }
    };
        
    // Sets a Token on a Field on the GameField
    // (The validation is handeld via button enabling)
    public void SetTokenOnField(Token token, Field field)
    {
        // if already availalabe... blub bla
       GameField.Add(field, token);

       Debug.Log("GameField: " + GameField);
    }
}
