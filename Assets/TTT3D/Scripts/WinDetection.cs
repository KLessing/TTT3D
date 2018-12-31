namespace GGG3DWin
{
    using GG3DTypes;

    public static class WinDetection
    {
        // Compares the player tag on the given Fields and return the winner or null
        private static Player? ComparePlayerOnFields(string firstField, string secondField, string thirdField)
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
        public static Player? CheckWinner(GameState GameField)
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
}
