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
        public static Move GetBestMove(GameState state, Player player)
        {
            // Call Alpha Beta Search

            GameObject[] availableTokens = GetAvailableTokensForGameState(state, player);
            int tokenCount = availableTokens.Length;

            // The Recursion depth depends on the count of available Tokens
            // TODO TEST or use static depth
            int depth = availableTokens.Length / 4;

            return AlphaBetaSearch(state, player, depth, int.MinValue, int.MaxValue);
        }

        // Returns an Array of available player Tokens for the given Gamestate
        // = All Player Tokens including the Tokens on the Peek of the Fields
        // but without the covered Tokens
        private static GameObject[] GetAvailableTokensForGameState(GameState state, Player player)
        {
            // Init all

            // Iterate through all Fields and remove the covered Tokens

            return null;
        }

        // Returns an Array of available player Tokens for the given Gamestate
        // on a specific Field on the GameField
        // @param TokensForState All available Tokens for the GameState
        // @param TokenOnField The currently highest Token on the Field
        private static GameObject[] GetAvailableTokensForField(GameObject[] TokensForState, GameObject TokenOnField)
        {
            // Remove nothing if Field is Empty
            if (TokenOnField == null)
            {
                return TokensForState;
            }

            // Remove lower and same level of Tokens if Field contains Token

            return null;
        }

        private static Move AlphaBetaSearch(GameState state, Player player, int depth, int a, int b)
        {
            // Recursion etc...

            return new Move();
        }

        // Get Rating
        private static MoveRating GetStateRating(GameState state, Player player)
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
        private static int CalcStateRating(GameState state)
        {
            return 0;
        }


    }

}
