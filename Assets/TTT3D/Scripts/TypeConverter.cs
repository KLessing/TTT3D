namespace GG3DConverter
{
    using GG3DTypes;
    using System.Collections.Generic;
    using UnityEngine;


    // Static class for easy access without global variables
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

        // Converts the given GameState with GameObjects to a StringState with Strings
        public static StringState DeepCloneState(StringState gameState)
        {
            StringState stringState = new StringState();

            foreach (var field in gameState)
            {
                Stack<string> tokenStringStack = new Stack<string>();

                // Convert Stack to Array to access not only the peek or pop the peek
                // NOTE: the stack may not be destructed (reference to current GameState)
                string[] tokenGameObjectArray = field.Value.ToArray();

                // Iterate backwards to push in the right order
                for (int i = tokenGameObjectArray.Length - 1; i >= 0; i--)
                {
                    tokenStringStack.Push(tokenGameObjectArray[i]);
                }

                stringState.Add(field.Key, tokenStringStack);
            }

            return stringState;
        }

        // Returns the opponent of the given player (the other player)
        public static Player GetOpponent(Player player)
        {
            return player == Player.Cross ? Player.Circle : Player.Cross;
        }

        // Returns the Player enum for the given tokenName
        public static Player GetPlayerForTokenName(string tokenName)
        {
            return tokenName.Contains("Cross") ? Player.Cross : Player.Circle;
        }

        // Returns the value for the Size of the given tokenName
        public static int GetValueForTokenName(string tokenName)
        {
            return tokenName.Contains("Small") ? 1 : (tokenName.Contains("Medium") ? 2 : 3);
        }
    }
}
