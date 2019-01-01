namespace GG3DAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using GG3DTypes;
    using GGG3DWin;

    // Own MoveRating Struct for move and rating combination
    public struct MoveRating
    {
        public Move Move;
        public int Rating;

        public MoveRating(Move move, int rating)
        {
            Move = move;
            Rating = rating;
        }
    }

    public class AIController : MonoBehaviour
    {
        // All Cross Tokens
        public List<GameObject> CrossTokens;

        // All Circle Tokens
        public List<GameObject> CircleTokens;

        public Move GetBestMove(GameState state, Player player)
        {
            // The Recursion depth depends on the count of available Tokens
            // TODO TEST or use depth
            // int depth = availableTokens.Count / 4;

            // Fixed Recursion depth for noew
            // TODO Test variable otherwise constant for fixed
            int depth = 4;

            // Call Alpha Beta Search and return the best Move
            return AlphaBetaSearch(state, player, player, depth, int.MinValue, int.MaxValue).Move;
        }



        // Returns an Array of available player Tokens for the given Gamestate and Player
        // = All Player Tokens including the Tokens on the Peek of the Fields
        // but without the covered Tokens
        private List<GameObject> GetAvailableTokensForGameState(GameState state, Player player)
        {
            // Direct Init of all Tokens for the Player and the player String
            string playerString = player == Player.Cross ? "Cross" : "Circle";
            List<GameObject> allTokens = player == Player.Cross ? CrossTokens : CircleTokens;

            List<GameObject> coveredTokens = new List<GameObject>();
            List<GameObject> availableTokens = new List<GameObject>();

            // Iterate through all Fields with Tokens on the Gamefield
            foreach (Stack<GameObject> field in state.Values)
            {
                // Iterate through all Tokens on the Field
                foreach (GameObject token in field)
                {
                    // When Token is not the highest Token on the Field
                    if (token != field.Peek())
                    {
                        // The Token is covered
                        coveredTokens.Add(token);
                    }
                }
            }

            // Iterate through all Tokens of the Player
            foreach (GameObject token in allTokens)
            {
                // When Token is not covered
                if (!coveredTokens.Contains(token))
                {
                    // The Token is available
                    availableTokens.Add(token);                                        
                }
            }

            return availableTokens;
        }

        // Returns the allowed player Tokens for the given Gamestate on a specific Field on the GameField
        // = The Tokens that are allowed to be placed on the Field which have to be Bigger than the current highest Token on the Field
        // @param availableTokens All available tokens of the Player for the GameState
        private List<GameObject> GetAllowedTokensForField(GameState state, Field field, List<GameObject> availableTokens)
        {
            List<GameObject> allowedTokens = new List<GameObject>();

            // Is already a Token on the Field?
            if (state.ContainsKey(field))
            {
                // Iterate through available Tokens
                foreach (GameObject token in availableTokens)
                {
                    // Is the available Token "bigger" than the Token on the Field?
                    if (Convert.ToInt32(token.tag) > Convert.ToInt32(state[field].Peek().tag))
                    {
                        // Token is allowed
                        // allowedTokens.Add(token);

                        // TODO SPECIAL CASE => When these Tokens get used the previous Token is on Top again
                    }
                }
            }
            else
            {
                // No Token on the Field = all available Tokens are allowed
                allowedTokens = availableTokens;
            }

            return allowedTokens;
        }


        // The Alpha Beta Search Algorithm
        // @param state The state simulation of the current recursion call
        // @param player the AI Player for which the algorithm calculates the most valuable move
        // @param currentPlayer the current Player of the current recursion call
        // @param depth the current recursion depth
        // @param a the alpha rating for the ai move
        // @param b the beta rating for the oppenent move        
        private MoveRating AlphaBetaSearch(GameState state, Player player, Player currentPlayer, int depth, int a, int b)
        {
            Debug.Log("depth: " + depth);

            // Get all available tokens for the state and return nothing if none exist
            List<GameObject> availableTokens = GetAvailableTokensForGameState(state, currentPlayer);

            // Check the Rating for the state and return it if it has a result (win or recursion end)
            MoveRating? resultRating = GetStateRating(state, currentPlayer, depth, availableTokens);
            if (resultRating != null)
            {
                return (MoveRating) resultRating;
            }

            // Get all available tokens for the state and return nothing if none exist
            //List<GameObject> availableTokens = GetAvailableTokensForGameState(state, currentPlayer);
            // Just checked in get State Rating...            
            //if (availableTokens.Count == 0)
            //{
            //    return new MoveRating();
            //}

            // Start with the lowest possible values
            MoveRating currentRating = new MoveRating(new Move(), currentPlayer == player ? int.MinValue : int.MaxValue);
            
            // Iterate through all Fields of the Gamefield
            foreach (Field field in Enum.GetValues(typeof(Field)))
            {
                // Get the allowed Tokens to place on this Field
                List<GameObject> allowedTokens = GetAllowedTokensForField(state, field, availableTokens);

                // Iterate through all allowed Tokens for this Field
                foreach (GameObject token in allowedTokens)
                {
                    // COPY current state without reference
                    GameState stateForToken = new GameState(state);

                    // Simulate a new state with the allowed token on the Field:

                    // If the field already has a token
                    if (stateForToken.ContainsKey(field))
                    {
                        // Place the allowed Token above the old Token
                        stateForToken[field].Push(token);
                    }
                    else
                    {
                        // Otherwise add the Field with a new Stack with the allowed Token
                        Stack<GameObject> tokenStack = new Stack<GameObject>();
                        tokenStack.Push(token);
                        stateForToken.Add(field, tokenStack);
                    }

                    // Next recursion call
                    MoveRating newRating = AlphaBetaSearch(stateForToken, player, GetOpponent(player), depth - 1, a, b);

                    // Compare with current rating
                    if ((currentPlayer == player && newRating.Rating > currentRating.Rating) ||
                        (currentPlayer != player && newRating.Rating < currentRating.Rating))
                    {
                        currentRating = newRating;
                    }

                    // Alpha Beta special: just return if alpha beta are exceeded
                    if ((currentPlayer == player && currentRating.Rating >= b ) ||
                        (currentPlayer != player && currentRating.Rating <= a))
                    {
                        return currentRating;
                    }

                    // Update alpha beta values (dependent on the player and if the new value is higher / lower)
                    a = currentPlayer == player ? GetMax(a, currentRating.Rating) : a;
                    b = currentPlayer != player ? GetMin(b, currentRating.Rating) : b;
                }
            }

            // Return the best found rating
            return currentRating;
        }

        // Get Rating
        private MoveRating? GetStateRating(GameState state, Player player, int depth, List<GameObject> availableTokens)
        {
            Player? winner = WinDetection.CheckWinner(state);

            // If Player Win
            if (winner == player)
            {
                return new MoveRating(new Move(), int.MaxValue);
            }

            // If oppenent wins
            if (winner == GetOpponent(player))
            {
                return new MoveRating(new Move(), int.MinValue);
            }
            
            // If no winner and depth is reached or no more available tokens
            if (depth <= 0 || availableTokens.Count == 0)
            {
                return new MoveRating(new Move(), CalcStateRating(state, player) - CalcStateRating(state, GetOpponent(player)));
            }

            // Otherwise if no termination return null
            return null;            
        }

        // Calc Rating
        private int CalcStateRating(GameState state, Player player)
        {
            // Easy Solution for testing for now: check how many wins are possible for 3 in a rows
            return CheckThrees(state, player);
        }


        // Count the GameField for Three same tokens in a Row
        private int CheckThrees(GameState GameField, Player player)
        {
            string playerString = player == Player.Cross ? "Cross" : "Circle";
            int rating = 0;
            int fieldIndex = 0;
            Player? winner = null;

            // Check Horizontal
            while (winner == null && fieldIndex < 8)
            {  
                if (!GameField.ContainsKey((Field)fieldIndex) ||
                     GameField.ContainsKey((Field)fieldIndex) && GameField[(Field)fieldIndex].Peek().transform.parent.name != playerString &&
                    !GameField.ContainsKey((Field)fieldIndex + 1) ||
                     GameField.ContainsKey((Field)fieldIndex + 1) && GameField[(Field)fieldIndex + 1].Peek().transform.parent.name != playerString &&
                    !GameField.ContainsKey((Field)fieldIndex + 2) ||
                     GameField.ContainsKey((Field)fieldIndex + 2) && GameField[(Field)fieldIndex + 2].Peek().transform.parent.name != playerString)                                              
                {
                    rating++;
                }                

                fieldIndex += 3;
            }

            fieldIndex = 0;

            // Check Vertical
            while (winner == null && fieldIndex < 3)
            {
                if (!GameField.ContainsKey((Field)fieldIndex) ||
                     GameField.ContainsKey((Field)fieldIndex) && GameField[(Field)fieldIndex].Peek().transform.parent.name != playerString &&
                    !GameField.ContainsKey((Field)fieldIndex + 3) ||
                     GameField.ContainsKey((Field)fieldIndex + 3) && GameField[(Field)fieldIndex + 3].Peek().transform.parent.name != playerString &&
                    !GameField.ContainsKey((Field)fieldIndex + 6) ||
                     GameField.ContainsKey((Field)fieldIndex + 6) && GameField[(Field)fieldIndex + 6].Peek().transform.parent.name != playerString)
                {
                    rating++;
                }

                fieldIndex++;
            }

            // Check Diagonal
            if (!GameField.ContainsKey(Field.TopLeft) ||
                 GameField.ContainsKey(Field.TopLeft) && GameField[Field.TopLeft].Peek().transform.parent.name != playerString &&
                !GameField.ContainsKey(Field.Middle) ||
                 GameField.ContainsKey(Field.Middle) && GameField[Field.Middle].Peek().transform.parent.name != playerString &&
                !GameField.ContainsKey(Field.BottomRight) ||
                 GameField.ContainsKey(Field.BottomRight) && GameField[Field.BottomRight].Peek().transform.parent.name != playerString)
            {
                rating++;
            }
            

            if (!GameField.ContainsKey(Field.TopRight) ||
                GameField.ContainsKey(Field.TopRight) && GameField[Field.TopRight].Peek().transform.parent.name != playerString &&
                !GameField.ContainsKey(Field.Middle) ||
                GameField.ContainsKey(Field.Middle) && GameField[Field.Middle].Peek().transform.parent.name != playerString &&
                !GameField.ContainsKey(Field.BottomLeft) ||
                GameField.ContainsKey(Field.BottomLeft) && GameField[Field.BottomLeft].Peek().transform.parent.name != playerString)
            {
                rating++;
            }

            return rating;
        }

        /***** Helper Functions *****/

        private Player GetOpponent(Player player)
        {
            return player == Player.Cross ? Player.Circle : Player.Cross;
        }

        public int GetMax(int a, int b) { return a >= b ? a : b; }

        public int GetMin(int a, int b) { return a <= b ? a : b; }


    }

}
