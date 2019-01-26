using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using GG3DTypes;
using GG3DWin;
using GG3DConverter;


public class GameFieldController : MonoBehaviour {

    // GameField: Each Field has a Stack of Tokens sorted by the TokenSize
    // The largest Token is at the top of the stack (Peek)
    public GameState GameField = new GameState();

    // Resets the GameField    
    public void Reset()
    {
        GameField.Clear();
    }

    // Sets a Token on a Field from the given Move on the GameField
    // (The Validation is handled via button enabling.
    //  There the placement is allowed!)
    // Return the winner or null
    public Player? SetTokenOnField(Move move)
    {
        Player? winner = null;

        // Only do something when token is not covered        
        if (!TokenIsCovered(move.Token))
        {
            // Remove the Token if it's on an upper Field
            RemoveTokenFromField(move.Token);

            // Check if the Field is empty
            if (GameField.ContainsKey(move.Field))
            {
                // Compare with the upper Token on the Field
                if (System.Convert.ToInt32(move.Token.tag) > System.Convert.ToInt32(GameField[move.Field].Peek().tag))
                {
                    // Place the Token on the highest position of the Field
                    GameField[move.Field].Push(move.Token);
                }
            }
            else
            {
                Stack<GameObject> tokenStack = new Stack<GameObject>();
                tokenStack.Push(move.Token);
                // Place the Token on the Field
                GameField.Add(move.Field, tokenStack);
            }

            // Check if game is won with the converted state of string tokens
            winner = WinDetection.CheckWinner(TypeConverter.ConvertState(GameField));
        }        

        return winner;
    }


    // Returns if the given token is covered by a bigger token
    public bool TokenIsCovered(GameObject token)
    {
        // Check all used GameField Fields till 
        // Token found on Peek
        // or detected that Token is covered
        for (int index = 0; index < GameField.Count; index++)
        {
            // If the token is on the peek of the field
            if (GameField.ElementAt(index).Value.Peek().name == token.name)
            {
                // The token is not covered
                return false;
            }

            // If the rest of the stack contains the token
            if (GameField.ElementAt(index).Value.Contains(token))
            {
                // The token is covered
                return true;
            }
        }

        // Otherwise token is not on the GameField and therefore not covered
        return false;
    }

    // Returns if the given token is on a peek of a field on the GameField
    // Needed for Button coloring in Token Button Controller
    public bool TokenIsOnPeek(GameObject token)
    {
        // check all used GameField Fields till 
        // Token found on Peek
        // or detected that Token is covered 
        for(int index = 0; index < GameField.Count; index++)
        {
            if (GameField.ElementAt(index).Value.Peek().name == token.name)
            {
                // Token is on peek
                return true;
            }
        }

        // Token not found on GameField peek
        return false;
    }

    // Returns the field for the given peek token
    // Returns null if the token is not on the peek of a field on the Gamefield
    // Gets called from Token Button Controller for peek tokens
    public Field? GetFieldForPeekToken(GameObject token)
    {
        foreach(var fieldStack in GameField)
        {
            if (fieldStack.Value.Peek() == token)
            {
                return fieldStack.Key;
            }
        }

        return null;
    }

    // Returns if the placement of a specific token on a specific field is possible
    // True = Placement is possible
    public bool PlacementPossible(GameObject token, Field field)
    {      
        return !GameField.ContainsKey(field) || System.Convert.ToInt32(GameField[field].Peek().tag) < System.Convert.ToInt32(token.tag);                
    }

    // Removes Token from upper Field (if possible)
    // and returns the success status
    // Return = true : the Token was removed successfully
    //          false : the Token could not be removed because:
    //                  1. it is not on the Gamefield
    //                  2. it is covered by a bigger Token
    private void RemoveTokenFromField(GameObject token)
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
    }



}
