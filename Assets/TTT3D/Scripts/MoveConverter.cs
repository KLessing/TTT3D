using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG3DTypes;

// The class with all Tokens as GameObjects to convert a Movestring with a tokenName from the AI move calculation
// to a real move with the token as a GameObject for the Game
public class MoveConverter : MonoBehaviour
{
    // The Tokens to visualize above the Augmented Image of the GameField
    public GameObject[] Tokens;

    // Converts the given MoveString with String to a Move with GameObjects and returns it
    public Move ConvertMove(MoveString moveString)
    {
        // Iterate through Tokens to get the one with the searched name
        foreach (GameObject token in Tokens)
        {
            // Does the token have the name which is searched for?
            if (token.name == moveString.Token)
            {
                // Return the token combined with the field of the param
                return new Move(token, moveString.Field);
            }
        }

        // Return empty Move if nothing found (shouldn't happen)
        return new Move();
    }
}
