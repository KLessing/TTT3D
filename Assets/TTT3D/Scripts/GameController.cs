using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;
using AugmentedImage;

public class GameController : MonoBehaviour {

    // TODO use list of Tokens 
    public Dictionary<Field, Token> GameField = new Dictionary<Field, Token>();

    // returns a Level for a Token
    private int GetTokenLevel(Token token)
    {
        switch (token)
        {
            case Token.SmallCross1: 
            case Token.SmallCircle1: 
            case Token.SmallCross2: 
            case Token.SmallCircle2: return 1;

            case Token.MediumCross1:
            case Token.MediumCircle1: 
            case Token.MediumCross2:
            case Token.MediumCircle2: return 2;

            case Token.LargeCross1:          
            case Token.LargeCircle1: 
            case Token.LargeCross2:
            case Token.LargeCircle2: return 3;

            default: return 0;
        }
    }
        
    // Sets a Token on a Field on the GameField
    // (The validation is handeld via button enabling)
    // return true = set possible
    public bool SetTokenOnField(Token token, Field field)
    {
        bool res = false;

        if (GameField.ContainsKey(field))
        {
            // Compare with previous token on the field
            if (GetTokenLevel(token) > GetTokenLevel(GameField[field]))
            {
                GameField[field] = token;
                res = true;
            }
        }
        else
        {
            GameField.Add(field, token);
            res = true;
        }
        return res;

        // TODO check if Token was on another field and remove it

        // TODO check if game is won
    }

    // TODO functions for testing if selection possible before next and confirm button
}
