using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;
using AugmentedImage;
using System.Linq;

public class GameController : MonoBehaviour {

    // GameField: Each Field has a Stack of Tokens sorted by the TokenSize
    // The largest Token is at the top of the stack
    public Dictionary<Field, Stack<GameObject>> GameField = new Dictionary<Field, Stack<GameObject>>();

        
    // Sets a Token on a Field on the GameField
    // (The validation is handeld via button enabling)
    // return true = set possible
    public bool SetTokenOnField(GameObject token, Field field)
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
                if (GetTokenSize(token) > GetTokenSize(GameField[field].Peek()))
                {
                    // Place the Token on the highest position of the Field
                    GameField[field].Push(token);
                    res = true;
                }
            }
            else
            {
                Stack<GameObject> tokenStack = new Stack<GameObject>();
                tokenStack.Push(token);
                // Place the Token on the Field
                GameField.Add(field, tokenStack);
                res = true;
            }
        }        

        return res;

        // TODO check if game is won
    }


    // Return if selection possible or token is covered by a bigger token
    public bool TokenIsCovered(GameObject token)
    {
        bool res = false;
        int index = 0;

        // check all used GameField Fields till 
        // Token found on Peek
        // or detected that Token is covered 
        while (!res && index < GameField.Count)
        {
            Debug.Log("index: " + index);
            if (GameField.ElementAt(index).Value.Peek().name == token.name)
            {
                Debug.Log("PEEK");

                // Token is on upper Field therefore it cant be covered
                return false;
            }
            else
            {
                // Search onward
                res = GameField.ElementAt(index).Value.Contains(token); // token suche funktioniert nicht
                // eigene Implementation mit zugriff auf untere elemente

                // vieleicht mit condition für name
                Debug.Log("Contains: " + res);
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
    private bool RemoveTokenFromField(GameObject token)
    {
        bool res = false;
        int index = 0;

        while (!res && index < GameField.Count)
        {
            // Check if token is on upper field
            res = GameField.ElementAt(index).Value.Peek().name == token.name;

            if (res)
            {
                // Check if the token was the only token on that field
                if (GameField.ElementAt(index).Value.Count == 1)
                {
                    // Remove field from dictionary
                    GameField.Remove(GameField.ElementAt(index).Key);
                }
                else
                {
                    // Remove only the upper Token from the field
                    GameField.ElementAt(index).Value.Pop();
                }
            }

            index++;
        }

        return res;
    }

    // Returns the Size for a Token
    private int GetTokenSize(GameObject token)
    {
        switch (token.tag)
        {
            case "Small": return 1;
            case "Medium": return 2;
            case "Large": return 3;

            default: return 0;
        }
    }
}
