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
            // Call Alpha Beta Search

            List<GameObject> availableTokens = GetAvailableTokensForGameState(state, player);
            int tokenCount = availableTokens.Count;

            // The Recursion depth depends on the count of available Tokens
            // TODO TEST or use depth
            int depth = availableTokens.Count / 4;

            return AlphaBetaSearch(state, player, depth, int.MinValue, int.MaxValue);
        }



        // Returns an Array of available player Tokens for the given Gamestate
        // = All Player Tokens including the Tokens on the Peek of the Fields
        // but without the covered Tokens
        private List<GameObject> GetAvailableTokensForGameState(GameState state, Player player)
        {
            // Direct Init of all Tokens for the Player and the player String
            List<GameObject> allTokens = player == Player.Cross ? CrossTokens : CircleTokens;
            string playerString = player == Player.Cross ? "Cross" : "Circle";

            // Iterate through all Fields and remove the covered Tokens
            foreach (var field in state)
            {
                // The Top Value will be ignored because it can still be used
                field.Value.Pop();

                // The Other values are covered and will therefore be removed from the available Tokens
                // When covered Tokens Left
                foreach (GameObject token in field.Value)
                {
                    // When the covered Token is a Token of the given Player
                    if (token.transform.parent.name == playerString)
                    {
                        // Remove it
                        allTokens.Remove(token);
                    }                        
                }
            }
            return allTokens;
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
