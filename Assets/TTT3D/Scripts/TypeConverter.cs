namespace GG3DConverter
{
    using GG3DTypes;
    using System.Collections.Generic;
    using UnityEngine;


    // Static class used for converting
    public static class TypeConverter
    {
        // Converts the given GameState with GameObjects to a StringState with Strings
        public static StringState ConvertState(GameState gameState)
        {
            StringState stringState = new StringState();

            foreach (var field in gameState)
            {
                // NOTE: this is a string stack and can't be initialized directly with gameObject stack
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

        // Deep Clones the given StringState to a new StringState with other Reference
        public static StringState DeepCloneState(StringState gameState)
        {
            StringState stringState = new StringState();

            foreach (var field in gameState)
            {
                Stack<string> tokenStringStack = new Stack<string>(field.Value);

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
        // Returns 0 for invalid token names
        public static int GetValueForTokenName(string tokenName)
        {
            if (tokenName.Contains("Small"))
            {
                return 1;
            }
            if (tokenName.Contains("Medium"))
            {
                return 2;
            }
            if (tokenName.Contains("Large"))
            {
                return 3;
            }
            // Otherwise = no tokenName
            return 0;
        }
    }
}
