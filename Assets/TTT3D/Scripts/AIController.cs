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
            return AlphaBetaSearch(state, Player.Cross, Player.Cross, depth, int.MinValue, int.MaxValue);
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


            Debug.Log("------------------");

            foreach (GameObject token in availableTokens)
            {
                Debug.Log("Available Token Name: " + token.name);
            }

            Debug.Log("------------------");


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
                        allowedTokens.Add(token);
                    }
                }
            }
            else
            {
                // No Token on the Field = all available Tokens are allowed
                allowedTokens = availableTokens;
            }


            Debug.Log("------------------");

            foreach (GameObject token in allowedTokens)
            {
                Debug.Log("Allowed Token For Field: " + field + " Name: " + token.name);
            }

            Debug.Log("------------------");


            return allowedTokens;
        }


        // The Alpha Beta Search Algorithm
        // @param state The state simulation of the current recursion call
        // @param player the AI Player for which the algorithm calculates the most valuable move
        // @param currentPlayer the current Player of the current recursion call
        // @param depth the current recursion depth
        // @param a the alpha rating for the ai move
        // @param b the beta rating for the oppenent move        
        private Move AlphaBetaSearch(GameState state, Player player, Player currentPlayer, int depth, int a, int b)
        {
            Move result = new Move();

            List<GameObject> availableTokens = GetAvailableTokensForGameState(state, currentPlayer);

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

                    // Check the Rating for the state simulation
                    int currentRating = GetStateRating(stateForToken, currentPlayer, depth);

                    // If the current Player is the ai player and the rating is higher than a
                    if (currentPlayer == player && currentRating > a)
                    {
                        // Update a and the best move
                        a = currentRating;
                        result = new Move(token, field);
                    }
                    // If the current Player is the oppenent and the rating is lower than b
                    else if (currentPlayer != player && currentRating < b)                        
                    {
                        // Update b and the best move
                        b = currentRating;
                        result = new Move(token, field);
                    }

                    // next search with the other player and lower recursion step
                    return AlphaBetaSearch(stateForToken, player, GetOpponent(player), depth-1, a, b);                    
                }
            }

            return result;
        }

        // Get Rating
        private int GetStateRating(GameState state, Player player, int depth)
        {
            Player? winner = WinDetection.CheckWinner(state);

            // If Player Win
            if (winner == player)
            {
                return int.MaxValue;
            }

            // If oppenent wins
            if (winner != null)
            {
                return int.MinValue;
            }
            
            // If no winner and depth is reached
            if (depth <= 0)
            {
                return CalcStateRating(state, player) - CalcStateRating(state, GetOpponent(player));
            }

            return 0;            
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
            string opponentString = player == Player.Cross ? "Circle" : "Cross";
            int rating = 0;
            int fieldIndex = 0;
            Player? winner = null;

            // Check Horizontal
            while (winner == null && fieldIndex < 8)
            {  
                if (GameField[(Field)fieldIndex].Peek().transform.parent.name != opponentString &&
                    GameField[(Field)fieldIndex + 1].Peek().transform.parent.name != opponentString &&
                    GameField[(Field)fieldIndex + 2].Peek().transform.parent.name != opponentString)                                              
                {
                    rating++;
                }                

                fieldIndex += 3;
            }

            fieldIndex = 0;

            // Check Vertical
            while (winner == null && fieldIndex < 3)
            {
                if (GameField[(Field)fieldIndex].Peek().transform.parent.name != opponentString &&
                    GameField[(Field)fieldIndex + 3].Peek().transform.parent.name != opponentString &&
                    GameField[(Field)fieldIndex + 6].Peek().transform.parent.name != opponentString)
                {
                    rating++;
                }

                fieldIndex++;
            }

            // Check Diagonal
            if (GameField[Field.TopLeft].Peek().transform.parent.name != opponentString &&
                GameField[Field.Middle].Peek().transform.parent.name != opponentString &&
                GameField[Field.BottomRight].Peek().transform.parent.name != opponentString)
            {
                rating++;
            }
            

            if (GameField[Field.TopRight].Peek().transform.parent.name != opponentString &&
                GameField[Field.Middle].Peek().transform.parent.name != opponentString &&
                GameField[Field.BottomLeft].Peek().transform.parent.name != opponentString)
            {
                rating++;
            }            
            
            return rating;
        }

        private Player GetOpponent(Player player)
        {
            return player == Player.Cross ? Player.Circle : Player.Cross;
        }

    }

}
