namespace GG3DWin
{
    using GG3DTypes;
    using GG3DConverter;

    // Static class used for win detection by strings to ensure usement for GameField and AI
    public static class WinDetection
    {
        // Compares the player on the given Fields and returns the winner or null
        // The parameters contain the TokenName for the peek Token on the fields
        private static Player? ComparePlayerOnFields(string firstField, string secondField, string thirdField)
        {
            // Get the Player on the first field
            Player firstFieldPlayer = TypeConverter.GetPlayerForTokenName(firstField);

            // Compare the Player to the Player on the second and third Field
            if (firstFieldPlayer == TypeConverter.GetPlayerForTokenName(secondField) &&
                firstFieldPlayer == TypeConverter.GetPlayerForTokenName(thirdField))
            {
                // Return the matching Player
                return firstFieldPlayer;
            }

            // Return null when Players don't match
            return null;            
        }

        // Check the GameField for Three same tokens in a Row
        // Return the winner or null
        public static Player? CheckWinner(StringState GameField)
        {
            int fieldIndex = 0;
            Player? winner = null;

            // Check Horizontal Winner
            while (winner == null && fieldIndex < 8)
            {
                // Check if fields are available (if they are they contain at least one token)
                if (GameField.ContainsKey((Field)fieldIndex) && GameField.ContainsKey((Field)fieldIndex + 1) && GameField.ContainsKey((Field)fieldIndex + 2))
                {
                    // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
                    winner = ComparePlayerOnFields(GameField[(Field)fieldIndex].Peek(),
                                                   GameField[(Field)fieldIndex + 1].Peek(),
                                                   GameField[(Field)fieldIndex + 2].Peek());
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
                    winner = ComparePlayerOnFields(GameField[(Field)fieldIndex].Peek(),
                                                   GameField[(Field)fieldIndex + 3].Peek(),
                                                   GameField[(Field)fieldIndex + 6].Peek());
                }

                fieldIndex++;
            }

            // Check Diagonal Winner
            if (winner == null && GameField.ContainsKey(Field.TopLeft) && GameField.ContainsKey(Field.Middle) && GameField.ContainsKey(Field.BottomRight))
            {
                // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
                winner = ComparePlayerOnFields(GameField[Field.TopLeft].Peek(),
                                               GameField[Field.Middle].Peek(),
                                               GameField[Field.BottomRight].Peek());
            }

            if (winner == null && GameField.ContainsKey(Field.TopRight) && GameField.ContainsKey(Field.Middle) && GameField.ContainsKey(Field.BottomLeft))
            {
                // Check the parent name (= Player Name) of the peek (= highest Token) on the Fields
                winner = ComparePlayerOnFields(GameField[Field.TopRight].Peek(),
                                               GameField[Field.Middle].Peek(),
                                               GameField[Field.BottomLeft].Peek());
            }


            return winner;
        }
    }
}
