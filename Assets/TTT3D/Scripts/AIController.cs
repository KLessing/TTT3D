using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG3DTypes;

namespace GG3DAI
{    
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
            List<GameObject> availableTokens = GetAvailableTokensForGameState(state, Player.Cross);

            // The Recursion depth depends on the count of available Tokens
            // TODO TEST or use depth
            // int depth = availableTokens.Count / 4;

            // Fixed Recursion depth for noew
            // TODO Test variable otherwise constant for fixed
            int depth = 4;

            // Call Alpha Beta Search and return the best Move
            return AlphaBetaSearch(state, Player.Cross, depth, int.MinValue, int.MaxValue);
        }



        // Returns an Array of available player Tokens for the given Gamestate
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
                // Iterate through all Tokens
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

        // Returns an Array of available player Tokens for the given Gamestate
        // on a specific Field on the GameField
        // @param TokensForState All available Tokens for the GameState
        // @param TokenOnField The currently highest Token on the Field
        private List<GameObject> GetAvailableTokensForField(List<GameObject> TokensForState, GameObject TokenOnField)
        {
            // Remove nothing if Field is Empty
            if (TokenOnField == null)
            {
                return TokensForState;
            }

            // Remove lower and same level of Tokens if Field contains Token

            return null;
        }

        private Move AlphaBetaSearch(GameState state, Player player, int depth, int a, int b)
        {
            // Recursion etc...

            return new Move();
        }

        // Get Rating
        private MoveRating GetStateRating(GameState state, Player player)
        {
            // If Player Win
            // return max int

            // If Other Player Win
            // return min int

            // TOCHECK if statements?!
            // return calcPlayerPoints - calcOtherPlayerPoints            

            return new MoveRating();
        }

        // Calc Rating
        private int CalcStateRating(GameState state)
        {
            return 0;
        }


    }

}
