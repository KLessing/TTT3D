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
    // Return the winner or null
    public Player? SetTokenOnField(GameObject token, Field field)
    {
        Player? winner = null;

        // Only do something when token is not covered        
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
                }
            }
            else
            {
                Stack<GameObject> tokenStack = new Stack<GameObject>();
                tokenStack.Push(token);
                // Place the Token on the Field
                GameField.Add(field, tokenStack);
            }

            // Check if game is won
            winner = CheckWinner();
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

    // Return if the placement of a specific token on a specific field is possible
    public bool PlacementPossible(GameObject token, Field field)
    {      
        return !GameField.ContainsKey(field) || GetTokenSize(GameField[field].Peek()) < GetTokenSize(token);                
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

    /***** Check Winner *****/

    // The number of fields
    private int FIELD_COUNT = System.Enum.GetNames(typeof(Field)).Length;


    // Compares the player tag on the given Fields and return the winner or null
    private Player? ComparePlayerOnFields(string firstField, string secondField, string thirdField)
    {
        if (firstField == "Cross" && secondField == "Cross" && thirdField == "Cross")
            return Player.Cross;
        else if (firstField == "Circle" && secondField == "Circle" && thirdField == "Circle")
            return Player.Circle;
        else
            return null;
    }


    // Check the GameField for Three same tokens in a Row
    // Return the winner or null
    private Player? CheckWinner()
    {
        int fieldIndex = 0;
        Player? winner = null;

        // Check Horizontal Winner
        while (winner == null && fieldIndex < FIELD_COUNT - 1)
        {
            // Check if fields are available (if they are they contain at least one token)
            if (GameField.ContainsKey((Field)fieldIndex) && GameField.ContainsKey((Field)fieldIndex + 1) && GameField.ContainsKey((Field)fieldIndex + 2))
            {
                // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
                winner = ComparePlayerOnFields(GameField[(Field)fieldIndex].Peek().transform.parent.name,
                                               GameField[(Field)fieldIndex + 1].Peek().transform.parent.name,
                                               GameField[(Field)fieldIndex + 2].Peek().transform.parent.name);
            }

            fieldIndex += 3;            
        }

        fieldIndex = 0;

        // Check Vertical Winner
        while (winner == null && fieldIndex < 3)
        {
            // Check if fields are available (if they are they contain at least one token)
            if (GameField.ContainsKey((Field)fieldIndex) && GameField.ContainsKey((Field)fieldIndex + 3) && GameField.ContainsKey((Field)fieldIndex + 6))
            {
                // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
                winner = ComparePlayerOnFields(GameField[(Field)fieldIndex].Peek().transform.parent.name,
                                               GameField[(Field)fieldIndex + 3].Peek().transform.parent.name,
                                               GameField[(Field)fieldIndex + 6].Peek().transform.parent.name);
            }

            fieldIndex++;
        }

        // Check Diagonal Winner
        if (winner == null && GameField.ContainsKey(Field.TopLeft) && GameField.ContainsKey(Field.Middle) && GameField.ContainsKey(Field.BottomRight))
        {
            // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
            winner = ComparePlayerOnFields(GameField[Field.TopLeft].Peek().transform.parent.name,
                                           GameField[Field.Middle].Peek().transform.parent.name,
                                           GameField[Field.BottomRight].Peek().transform.parent.name);
        }

        if (winner == null && GameField.ContainsKey(Field.TopRight) && GameField.ContainsKey(Field.Middle) && GameField.ContainsKey(Field.BottomLeft))
        {
            // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
            winner = ComparePlayerOnFields(GameField[Field.TopRight].Peek().transform.parent.name,
                                           GameField[Field.Middle].Peek().transform.parent.name,
                                           GameField[Field.BottomLeft].Peek().transform.parent.name);
        }


        return winner;
    }
}
