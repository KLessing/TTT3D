namespace GGG3DConverter
{
    using GG3DTypes;
    using System.Collections.Generic;
    using UnityEngine;

    public class MoveConverter
    {
        // The Tokens to visualize above the Augmented Image of the GameField
        public GameObject[] Tokens;

        // Converts the given MoveString with String to a Move with GameObjects
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

    public static class TypeConverter
    {
        // Converts the given GameState with GameObjects to a StringState with Strings
        public static StringState ConvertState(GameState gameState)
        {
            StringState stringState = new StringState();

            foreach (var field in gameState)
            {
                Stack<string> tokenStringStack = new Stack<string>();

                // Convert Stack to Array to access not only the peek or pop the peek
                // NOTE: the stack may not be destructed (reference to current GameState)
                GameObject[] tokenGameObjectArray = field.Value.ToArray();

                // Iterate backwards to push in the right order
                for (int i = tokenGameObjectArray.Length - 1; i >= 0; i--)
                {
                    tokenStringStack.Push(tokenGameObjectArray[i].name);
                }

                stringState.Add(field.Key, tokenStringStack);
            }

            return stringState;
        }

        // Returns the Player enum for the given tokenName
        public static Player GetPlayerForTokenName(string tokenName)
        {
            return tokenName.Contains("Cross") ? Player.Cross : Player.Circle;
        }

        // Returns the value for the Size of the given tokenName
        public static int GetValueForTokenName(string tokenName)
        {
            return tokenName.Contains("Small") ? 1 : tokenName.Contains("Middle") ? 2 : 3;
        }


    }
}
