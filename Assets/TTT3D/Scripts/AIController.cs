﻿namespace GG3DAI
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using GG3DTypes;
    using GG3DWin;
    using GG3DConverter;


    public static class AIController
    {        
        private static readonly List<string> CrossTokenNames = new List<string> { "SmallCross1", "SmallCross2",  "MediumCross1", "MediumCross2", "LargeCross1", "LargeCross2" };

        private static readonly List<string> CircleTokenNames = new List<string> { "SmallCircle1", "SmallCircle2", "MediumCircle1", "MediumCircle2", "LargeCircle1", "LargeCircle2" };

        public static MoveString GetBestMove(StringState state, Player player)
        {
            Debug.Log("-------------");
            foreach (var field in state)
            {
                Debug.Log("field: " + field.Key);
                Debug.Log("peek token: " + field.Value.Peek());
            }

            // Execute the Alpha Beta Search with the appropriate params and return the Move String directly
            // start with current opponent because it will be switch after rating
            return AlphaBetaSearch(new MoveString(), state, player, GetOpponent(player), Constants.AI_DEPTH, int.MinValue, int.MaxValue).Move;
        }


        /***** Helper Functions *****/

        private static Player GetOpponent(Player player)
        {
            return player == Player.Cross ? Player.Circle : Player.Cross;
        }

        private static int GetMax(int a, int b) { return a >= b ? a : b; }

        private static int GetMin(int a, int b) { return a <= b ? a : b; }



        // Returns an Array of available player Tokens for the given Gamestate and Player
        // = All Player Tokens including the Tokens on the Peek of the Fields
        // but without the covered Tokens
        private static List<string> GetAvailableTokensForGameState(StringState state, Player player)
        {
            // Direct Init of all Tokens for the Player and the player String
            List<string> allTokens = player == Player.Cross ? CrossTokenNames : CircleTokenNames;

            List<string> coveredTokens = new List<string> ();
            List<string> availableTokens = new List<string>();

            // Iterate through all Fields with Tokens on the Gamefield
            foreach (Stack<string> field in state.Values)
            {
                // Iterate through all Tokens on the Field
                foreach (string token in field)
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
            foreach (string token in allTokens)
            {
                // When Token is not covered
                if (!coveredTokens.Contains(token))
                {
                    // The Token is available
                    availableTokens.Add(token);
                }
            }

            //Debug.Log("-------------");

            //foreach (string token in availableTokens)
            //{
            //    Debug.Log("available token: " + token);
            //}

            return availableTokens;
        }

        // Returns the allowed player Tokens for the given Gamestate on a specific Field on the GameField
        // = The Tokens that are allowed to be placed on the Field which have to be Bigger than the current highest Token on the Field
        // @param availableTokens All available tokens of the Player for the GameState
        private static List<string> GetAllowedTokensForField(StringState state, Field field, List<string> availableTokens)
        {
            List<string> allowedTokens = new List<string>();

            // Is already a Token on the Field?
            if (state.ContainsKey(field))
            {
                //Debug.Log("on field lvl"+Convert.ToInt32(state[field].Peek().tag));

                // Iterate through available Tokens
                foreach (string token in availableTokens)
                {
                    // Is the available Token "bigger" than the Token on the Field?
                    if (TypeConverter.GetValueForTokenName(token) > TypeConverter.GetValueForTokenName(state[field].Peek()))
                    {
                        //Debug.Log("bigger");
                        // Token is allowed
                        allowedTokens.Add(token);  //!!! THIS LINE BREAKS EVERYTHING???!!!                        
                    }
                }

                //Debug.Log("-------------");

                //Debug.Log("allowed on Field: " + field);

                //foreach (string token in allowedTokens)
                //{
                //    Debug.Log("allowed token: " + token);
                //}

            }
            else
            {
                // No Token on the Field = all available Tokens are allowed
                allowedTokens = availableTokens;
            }

            return allowedTokens;
        }


        // Returns a Gamestate without the given Token
        // Removes the Token from the given GameState if it is on the Gamestate
        // otherwise the given GameState will be returned
        // (only has to check the peeks because the covered Tokens are ignored by GetAvailableTokensForGameState)
        private static StringState RemoveTokenFromGameState(StringState state, string token)
        {
            // COPY the given state without reference
            StringState resultState = TypeConverter.DeepCloneState(state);

            // iterate through all available fields of the state
            foreach (var field in state)
            {
                // check if the token is on the peek of the field
                if (field.Value.Peek() == token)
                {
                    // when the field has more than one token
                    if (field.Value.Count > 1)
                    {
                        // remove the highest token from the field
                        resultState[field.Key].Pop();
                    }
                    // when the token is the only token on the field
                    else
                    {
                        // remove the field from the state
                        resultState.Remove(field.Key);
                    }
                }
            }

            return resultState;
        }


        // The Alpha Beta Search Algorithm
        // @param state The state simulation of the current recursion call
        // @param player the AI Player for which the algorithm calculates the most valuable move
        // @param currentPlayer the current Player of the current recursion call
        // @param depth the current recursion depth
        // @param a the alpha rating for the ai move
        // @param b the beta rating for the oppenent move        
        private static MoveRating AlphaBetaSearch(MoveString move, StringState state, Player player, Player currentPlayer, int depth, int a, int b)
        {

            // Check the Rating for the state and return it if it has a result (win or recursion end)
            MoveRating? resultRating = GetStateRating(move, state, player, depth);
            if (resultRating.HasValue)
            {
                Debug.Log("---------------");
                Debug.Log("currentPlayer = " + currentPlayer);
                Debug.Log("move field: " + move.Field);
                Debug.Log("move token: " + move.Token);
                Debug.Log("rating (not negated): " + resultRating.Value.Rating);

                if (currentPlayer == player)
                {
                    return resultRating.Value;
                }
                else
                {
                    // return negative rating for opponent
                    return new MoveRating(move, -resultRating.Value.Rating);
                }
                
            }

            // switch player
            currentPlayer = GetOpponent(currentPlayer);


            // Get all available tokens for the state
            List<string> availableTokens = GetAvailableTokensForGameState(state, currentPlayer);

            // Start with the lowest possible values and no move
            MoveRating bestRating = new MoveRating(new MoveString(), currentPlayer == player ? int.MinValue : int.MaxValue);

            // Iterate through all Fields of the Gamefield
            foreach (Field field in Enum.GetValues(typeof(Field)))
            {
                // Get the allowed Tokens to place on this Field
                List<string> allowedTokens = GetAllowedTokensForField(state, field, availableTokens);

                // Iterate through all allowed Tokens for this Field
                foreach (string token in allowedTokens)
                {
                    // COPY current state without reference
                    StringState stateForToken = TypeConverter.DeepCloneState(state);

                    // Simulate a new state with the allowed token on the current Field:

                    // Remove the token from the previous field (if already placed)
                    stateForToken = RemoveTokenFromGameState(stateForToken, token);

                    // If the field already has a token
                    if (stateForToken.ContainsKey(field))
                    {
                        Stack<string> tokenStack = new Stack<string>(stateForToken[field]);
                        tokenStack.Push(token);

                        // Place the allowed Token above the old Token
                        stateForToken.Remove(field);
                        stateForToken.Add(field, tokenStack);
                    }
                    else
                    {
                        // Otherwise add the Field with a new Stack with the allowed Token
                        Stack<string> tokenStack = new Stack<string>();
                        tokenStack.Push(token);
                        stateForToken.Add(field, tokenStack);
                    }

                    // only use the move for the first function call (otherwise passed on move param which was the first)
                    // Note: the state will be updated by the new move but we only need the first move for the end result
                    MoveString firstMove = depth == Constants.AI_DEPTH ? new MoveString(token, field) : move;

                    // Next recursion call
                    MoveRating newRating = AlphaBetaSearch(firstMove, stateForToken, player, currentPlayer, depth - 1, a, b);

                    // Compare with current best rating
                    if ((currentPlayer == player && newRating.Rating > bestRating.Rating) ||
                        (currentPlayer != player && newRating.Rating < bestRating.Rating))
                    {
                        // update best if better
                        bestRating.Rating = newRating.Rating;
                        bestRating.Move = firstMove;
                    }

                    // Alpha Beta special: just return if alpha beta are exceeded
                    if ((currentPlayer == player && bestRating.Rating >= b) ||
                        (currentPlayer != player && bestRating.Rating <= a))
                    {
                        Debug.Log("alpha beta pruning!!!!!!!!!!");
                        return bestRating;
                    }

                    // Update alpha beta values (dependent on the player and if the new value is higher / lower)
                    a = currentPlayer == player ? GetMax(a, bestRating.Rating) : a;
                    b = currentPlayer != player ? GetMin(b, bestRating.Rating) : b;
                }
            }

            Debug.Log("-----------------");
            Debug.Log("bestRating: " + bestRating.Rating);
            Debug.Log("bestRating move token: " + bestRating.Move.Token);
            Debug.Log("bestRating move field: " + bestRating.Move.Field.ToString());
            Debug.Log("-----------------");

            // Return the best found rating
            return bestRating;
        }

        // Get Rating
        // needs to returnoptional value to distinguish between calced value and not?
        // TOCHECK: 0 RESULT POSSIBLE? => use int result instead of unnecessary MoveRating
        private static MoveRating? GetStateRating(MoveString move, StringState state, Player player, int depth)
        {
            Player? winner = WinDetection.CheckWinner(state);

            // If Player Win
            if (winner == player)
            {
                Debug.Log("checking player wins");
                Debug.Log("token: " + move.Token);
                Debug.Log("field: " + move.Field);
                return new MoveRating(move, int.MaxValue);
            }

            // If oppenent wins
            if (winner == GetOpponent(player))
            {
                Debug.Log("checking player looses");
                Debug.Log("token: " + move.Token);
                Debug.Log("field: " + move.Field);
                return new MoveRating(move, int.MinValue);
            }

            // If no winner and depth is reached
            if (depth <= 0)
            {
                //Debug.Log("-----------------");
                //Debug.Log("move token: " + move.Token.name);
                //Debug.Log("move field: " + move.Field.ToString());
                //Debug.Log("player Rating: " + CalcStateRating(state, player));
                //Debug.Log("------");
                //Debug.Log("opponent Rating: " + CalcStateRating(state, GetOpponent(player)));
                //Debug.Log("------");

                return new MoveRating(move, CalcStateRating(state, player) - CalcStateRating(state, GetOpponent(player)));
            }

            // Otherwise if no termination return null
            return null;
        }

        // Calc Rating
        private static int CalcStateRating(StringState state, Player player)
        {
            // Easy Solution for testing for now: check how many wins are possible for 3 in a rows
            return CheckThrees(state, player);

            // TODO MORE CHECKS!!!
        }


        // Count the GameField for Three same tokens in a Row
        private static int CheckThrees(StringState GameField, Player player)
        {
            int rating = 0;
            int fieldIndex = 0;
            Player? winner = null;

            // Check Horizontal
            while (winner == null && fieldIndex < 8)
            {
                if ((!GameField.ContainsKey((Field)fieldIndex) ||
                     GameField.ContainsKey((Field)fieldIndex) && TypeConverter.GetPlayerForTokenName(GameField[(Field)fieldIndex].Peek()) == player) &&
                    (!GameField.ContainsKey((Field)fieldIndex + 1) ||
                     GameField.ContainsKey((Field)fieldIndex + 1) && TypeConverter.GetPlayerForTokenName(GameField[(Field)fieldIndex + 1].Peek()) == player) &&
                    (!GameField.ContainsKey((Field)fieldIndex + 2) ||
                     GameField.ContainsKey((Field)fieldIndex + 2) && TypeConverter.GetPlayerForTokenName(GameField[(Field)fieldIndex + 2].Peek()) == player))
                {
                    //Debug.Log("horizontal with fieldIndex " + fieldIndex);
                    rating++;
                }

                fieldIndex += 3;
            }

            fieldIndex = 0;

            // Check Vertical
            while (winner == null && fieldIndex < 3)
            {
                if ((!GameField.ContainsKey((Field)fieldIndex) ||
                     GameField.ContainsKey((Field)fieldIndex) && TypeConverter.GetPlayerForTokenName(GameField[(Field)fieldIndex].Peek()) == player) &&
                    (!GameField.ContainsKey((Field)fieldIndex + 3) ||
                     GameField.ContainsKey((Field)fieldIndex + 3) && TypeConverter.GetPlayerForTokenName(GameField[(Field)fieldIndex + 3].Peek()) == player) &&
                    (!GameField.ContainsKey((Field)fieldIndex + 6) ||
                     GameField.ContainsKey((Field)fieldIndex + 6) && TypeConverter.GetPlayerForTokenName(GameField[(Field)fieldIndex + 6].Peek()) == player))
                {
                    //Debug.Log("vertical with fieldIndex " + fieldIndex);
                    rating++;
                }

                fieldIndex++;
            }

            // Check Diagonal
            if ((!GameField.ContainsKey(Field.TopLeft) ||
                 GameField.ContainsKey(Field.TopLeft) && TypeConverter.GetPlayerForTokenName(GameField[Field.TopLeft].Peek()) == player) &&
                (!GameField.ContainsKey(Field.Middle) ||
                 GameField.ContainsKey(Field.Middle) && TypeConverter.GetPlayerForTokenName(GameField[Field.Middle].Peek()) == player) &&
                (!GameField.ContainsKey(Field.BottomRight) ||
                 GameField.ContainsKey(Field.BottomRight) && TypeConverter.GetPlayerForTokenName(GameField[Field.BottomRight].Peek()) == player))
            {
                //Debug.Log("diagonal top left");
                rating++;
            }


            if ((!GameField.ContainsKey(Field.TopRight) ||
                GameField.ContainsKey(Field.TopRight) && TypeConverter.GetPlayerForTokenName(GameField[Field.TopRight].Peek()) == player) &&
                (!GameField.ContainsKey(Field.Middle) ||
                GameField.ContainsKey(Field.Middle) && TypeConverter.GetPlayerForTokenName(GameField[Field.Middle].Peek()) == player) &&
                (!GameField.ContainsKey(Field.BottomLeft) ||
                GameField.ContainsKey(Field.BottomLeft) && TypeConverter.GetPlayerForTokenName(GameField[Field.BottomLeft].Peek()) == player))
            {
                // Debug.Log("diagonal top right");
                rating++;
            }

            return rating;
        }


    }

}