using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;
using AugmentedImage;
using System.Linq;

public class GameController : MonoBehaviour {

    // GameField: Each Field has a List of Tokens sorted by the TokenSize
    // e.g. Index 0 = Large Token (Cross or Circle); Index 1 = Middle Token etc.
    public Dictionary<Field, List<Token>> GameField = new Dictionary<Field, List<Token>>();

        
    // Sets a Token on a Field on the GameField
    // (The validation is handeld via button enabling)
    // return true = set possible
    public bool SetTokenOnField(Token token, Field field)
    {
        bool res = false;

        if (!TokenIsCovered(token))
        {
            // Remove the Token if it's on an upper Field
            RemoveTokenFromField(token);

            // Check if the Field is empty
            if (GameField.ContainsKey(field))
            {
                // Compare with the upper Token on the Field
                if (GetTokenSize(token) > GetTokenSize(GameField[field][0]))
                {
                    // Place the Token on the highest position of the Field
                    GameField[field].Insert(0, token);
                    res = true;
                }
            }
            else
            {
                // Place the Token on the Field
                GameField.Add(field, new List<Token> { token });
                res = true;
            }
        }

        return res;

        // TODO check if game is won
    }


    // Return if selection possible or token is covered by a bigger token
    public bool TokenIsCovered(Token token)
    {
        bool res = false;
        int index = 0;

        while (!res && index < GameField.Count)
        {
            // Check if token is covered
            //res = GameField.ElementAt(index).Value[0] != token
            //   && GameField.ElementAt(index).Value.Contains(token);

            if (GameField.ElementAt(index).Value[0] == token)
            {
                // Token is on upper Field therefore it cant be covered
                return false;
            }
            else
            {
                // Search onward
                res = GameField.ElementAt(index).Value.Contains(token);
            }

            index++;
        }

        return res;
    }

    // Removes Token from upper Field (if possible)
    // and returns the success status
    // Return = true : the Token was removed successfully
    //          false : the Token could not be removed because
    //                  1. it is not on the Gamefield
    //                  2. it is covered by a bigger Token
    private bool RemoveTokenFromField(Token token)
    {
        bool res = false;
        int index = 0;

        while (!res && index < GameField.Count)
        {
            // Check if token is on upper field
            res = GameField.ElementAt(index).Value[0] == token;

            if (res)
            {
                // Check if the token was the only token on that field
                if (GameField.ElementAt(index).Value.Count == 1)
                {
                    // Remove field from dictionary
                    GameField.Remove(GameField.ElementAt(index).Key);
                }
                // Remove only the upper Token from the field
                GameField.ElementAt(index).Value.Remove(token);
            }

            index++;
        }

        return res;
    }

    // Returns the Size for a Token
    private int GetTokenSize(Token token)
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
}
