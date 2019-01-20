using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using GG3DTypes;
using GG3DWin;
using GG3DConverter;


public class GameController : MonoBehaviour {

    // GameField: Each Field has a Stack of Tokens sorted by the TokenSize
    // The largest Token is at the top of the stack (Peek)
    public GameState GameField = new GameState();

    public void Reset()
    {
        GameField.Clear();
    }


    // Sets a Token on a Field on the GameField
    // (The validation is handeld via button enabling)
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
            if (GameField.ElementAt(index).Value.Peek().name == token.name)
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

    // Return if the given token is on a peek of a field on the gamefield
    // Needed for Button coloring in Token Button Controller
    public bool TokenIsOnPeek(GameObject token)
    {
        bool res = false;
        int index = 0;

        // check all used GameField Fields till 
        // Token found on Peek
        // or detected that Token is covered 
        while (!res && index < GameField.Count)
        {
            if (GameField.ElementAt(index).Value.Peek().name == token.name)
            {
                // Token is on peek
                return true;
            }

            index++;
        }

        return res;
    }

    // Return if the placement of a specific token on a specific field is possible
    public bool PlacementPossible(GameObject token, Field field)
    {      
        return !GameField.ContainsKey(field) || System.Convert.ToInt32(GameField[field].Peek().tag) < System.Convert.ToInt32(token.tag);                
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



}
