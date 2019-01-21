namespace GG3DAI
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
            // Execute the Alpha Beta Search with the appropriate params and return the best move directly as a MoveString
            return AlphaBetaRoot(Constants.AI_DEPTH, state, player);
        }
        

        // Returns a List of available player Tokens for the given Gamestate and Player
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
                // Iterate through available Tokens
                foreach (string token in availableTokens)
                {
                    // Is the available Token "bigger" than the Token on the Field?
                    if (TypeConverter.GetValueForTokenName(token) > TypeConverter.GetValueForTokenName(state[field].Peek()))
                    {
                        //Debug.Log("bigger");
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

            return allowedTokens;
        }


        // returns all possible moves for the player and the state
        public static List<MoveString> GetPossibleMoves(StringState state, Player player)
        {
            List<MoveString> result = new List<MoveString>();

            // Get all available tokens for the state
            List<string> availableTokens = GetAvailableTokensForGameState(state, player);

            // Iterate through all Fields of the Gamefield
            foreach (Field field in Enum.GetValues(typeof(Field)))
            {
                // Get the allowed Tokens to place on this Field
                List<string> allowedTokens = GetAllowedTokensForField(state, field, availableTokens);

                // Iterate through all allowed Tokens for this Field
                foreach (string token in allowedTokens)
                {
                    result.Add(new MoveString(token, field));
                }
            }

            return result;
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


        // Return a new state with the move on the state (with other reference)
        public static StringState GetStateWithMove(StringState state, MoveString move)
        {
            // Deep clone previous state
            StringState resultState = TypeConverter.DeepCloneState(state);

            // Remove the token from the previous field (if already placed)
            resultState = RemoveTokenFromGameState(resultState, move.Token);

            // TODO DIRECT init of state with token remove function?

            // If the field already has a token
            if (resultState.ContainsKey(move.Field))
            {
                Stack<string> tokenStack = new Stack<string>(resultState[move.Field]);
                tokenStack.Push(move.Token);

                // Place the allowed Token above the old Token
                resultState.Remove(move.Field);
                resultState.Add(move.Field, tokenStack);
            }
            else
            {
                // Otherwise add the Field with a new Stack with the allowed Token
                Stack<string> tokenStack = new Stack<string>();
                tokenStack.Push(move.Token);
                resultState.Add(move.Field, tokenStack);
            }

            return resultState;
        }


        /***** ALPHA BETA *****/

        // iterates through all possible root moves
        public static MoveString AlphaBetaRoot(int depth, StringState state, Player player)
        {
            // start with the lowest possible value
            int bestValue = int.MinValue;
            // the best move result
            MoveString bestMove = new MoveString();

            // get all first moves
            List<MoveString> possibleMoves = GetPossibleMoves(state, player);

            foreach (MoveString move in possibleMoves)
            {
                StringState moveState = GetStateWithMove(state, move);

                int value = AlphaBeta(depth - 1, moveState, player, int.MinValue, int.MaxValue);

                if (value >= bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }

            return bestMove;
        }


        public static int AlphaBeta(int depth, StringState state, Player player, int alpha, int beta)
        {
            // eval if leaf
            if (depth == 0)
            {
                return EvaluateState(state, player);
            }

            // Switch player after win detection
            // after evaluation this won't be necessary anymore
            // but eval has to be calculated for the previous player
            player = TypeConverter.GetOpponent(player);

            List<MoveString> possibleMoves = GetPossibleMoves(state, player);            

            if (player == Constants.AI_PLAYER)
            {
                // start with lowest possible value
                int bestValue = int.MinValue + 1; // TODO test if + 1 necessary

                foreach (MoveString move in possibleMoves)
                {
                    StringState moveState = GetStateWithMove(state, move);

                    bestValue = Math.Max(bestValue, AlphaBeta(depth - 1, moveState, player, alpha, beta));

                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                    {
                        return bestValue;
                    }
                }

                return bestValue;
            }
            else
            {
                // start with highest possible value
                int bestValue = int.MaxValue - 1; // TODO test if - 1 necessary

                foreach (MoveString move in possibleMoves)
                {
                    StringState moveState = GetStateWithMove(state, move);

                    bestValue = Math.Min(bestValue, AlphaBeta(depth - 1, moveState, player, alpha, beta));

                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                    {
                        return bestValue;
                    }
                }

                return bestValue;
            }
        }

        
        // Evaluate the given State for the given Player
        private static int EvaluateState(StringState state, Player player)
        {
            int res = 0;
            int fieldIndex = 0;

            // Check Horizontal
            for (fieldIndex = 0; fieldIndex < 8; fieldIndex += 3)
            {
                res += EvaluateThreeFields(state, player, new Field[] { (Field)fieldIndex, (Field)fieldIndex + 1, (Field)fieldIndex + 2 });
            }

            // Check Vertical
            for (fieldIndex = 0; fieldIndex < 3; fieldIndex++)
            {
                res += EvaluateThreeFields(state, player, new Field[] { (Field)fieldIndex, (Field)fieldIndex + 3, (Field)fieldIndex + 6 });
            }

            // Check Diagonal
            res += EvaluateThreeFields(state, player, new Field[] { Field.TopLeft, Field.Middle, Field.BottomRight });
            res += EvaluateThreeFields(state, player, new Field[] { Field.TopRight, Field.Middle, Field.BottomLeft });

            return res;
        }

        // Returns the evaluation value for three Fields on the given State for the given Player        
        private static int EvaluateThreeFields(StringState state, Player player, Field[] fields) // array for iteration
        {
            int res = 0;
            // Count of the tokens of the player
            int playerTokenCounter = 0;
            int opponentTokenCounter = 0;

            int currentValue = 0;

            foreach (Field field in fields)
            {
                string tokenString = state.ContainsKey(field) ? state[field].Peek() : "";

                // Evaluate the tokens on the field
                currentValue = EvaluateToken(player, tokenString);
                // the result is positive for player token
                if (currentValue > 0)
                {
                    // inc player counter
                    playerTokenCounter++;
                    res += currentValue * (playerTokenCounter + 1);
                }
                // the result is negative for opponent token
                else if (currentValue < 0)
                {
                    // inc opponent counter
                    opponentTokenCounter++;
                    // add current Value * opponent counter
                    res += currentValue * (opponentTokenCounter + 1);
                }
                // Otherwise no change of res
            }

            // does not win directly if this is used
            //// Check if win
            //if (playerTokenCounter == 3)
            //{
            //    return int.MaxValue;
            //}

            //// or loose
            //if (opponentTokenCounter == 3)
            //{
            //    return int.MinValue;
            //}
           
            // otherwiese return eval result
            return res;
        }

        private static int EvaluateToken(Player player, string tokenString)
        {
            int res = 0;

            // is there a token on the field? (otherwise the value is 0)
            if (tokenString != "")
            {               
                // Get the value for the token (currently 1 - 3 for small - large)
                res = TypeConverter.GetValueForTokenName(tokenString);

                // is the token from the opponent?
                if (TypeConverter.GetPlayerForTokenName(tokenString) != player)
                {
                    // invert the value
                    res *= -1;
                }
            }

            return res;
        }


        /***** DEBUG *****/

        private static void DebugState(StringState state)
        {
            foreach (var field in state)
            {
                Debug.Log("field: " + field.Key);
                Debug.Log("peek token: " + field.Value.Peek());
            }
        }

        private static void DebugMove(MoveString move)
        {
            Debug.Log("field: " + move.Field);
            Debug.Log("token: " + move.Token);
        }


        /***** TEST CASES *****/

        // The following functions serves as Test Case for Special states
        // Usement: call in first alpha beta call instead of param


        // The User win is inevitable no matter which move the ai uses
        // Expected: There will be no move result. 
        //           The UI Controller shows player win announcment directly
        // Previous Behaviour: Exception
        private static StringState UserWinInevitable()
        {
            StringState testState = new StringState();

            Stack<string> stringStack1 = new Stack<string>();
            stringStack1.Push("MediumCross1");
            testState.Add(Field.TopLeft, stringStack1);

            Stack<string> stringStack2 = new Stack<string>();
            stringStack2.Push("LargeCross1");
            testState.Add(Field.Middle, stringStack2);

            Stack<string> stringStack3 = new Stack<string>();
            stringStack3.Push("LargeCross2");
            testState.Add(Field.TopRight, stringStack3);

            Stack<string> stringStack4 = new Stack<string>();
            stringStack4.Push("MediumCross2");
            stringStack4.Push("LargeCircle1");
            testState.Add(Field.MiddleLeft, stringStack4);

            Stack<string> stringStack5 = new Stack<string>();
            stringStack5.Push("LargeCircle2");
            testState.Add(Field.BottomLeft, stringStack5);

            return testState;
        }

        // The ai is able to win with the next move and should prioritize this 
        // instead of the prevention of the opponent win.
        // Expected: Move Large Circle 2 to Top Right Field (previous peek MediumCross1)
        // Previous Behaviour: Move Large Circle 2 to Top Middle Field
        // Used ai depth: 3 (depth 1 & 2 works...) !!!
        // LX1   -   MX1 => LO2
        // LX2  LO1  LO2 => -
        // MO1   -    -
        private static StringState WinPriority()
        {
            StringState testState = new StringState();

            Stack<string> stringStack1 = new Stack<string>();
            stringStack1.Push("LargeCross1");
            testState.Add(Field.TopLeft, stringStack1);

            Stack<string> stringStack2 = new Stack<string>();
            stringStack2.Push("LargeCross2");
            testState.Add(Field.MiddleLeft, stringStack2);

            Stack<string> stringStack3 = new Stack<string>();
            stringStack3.Push("MediumCircle1");
            testState.Add(Field.BottomLeft, stringStack3);

            Stack<string> stringStack4 = new Stack<string>();
            stringStack4.Push("LargeCircle1");
            testState.Add(Field.Middle, stringStack4);

            Stack<string> stringStack5 = new Stack<string>();
            stringStack5.Push("LargeCircle2");
            testState.Add(Field.MiddleRight, stringStack5);

            Stack<string> stringStack6 = new Stack<string>();
            stringStack6.Push("MediumCross1");
            testState.Add(Field.TopRight, stringStack6);

            return testState;
        }

    }

}