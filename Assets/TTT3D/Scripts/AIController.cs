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
        // All available token names for each player
        private static readonly List<string> CrossTokenNames = new List<string> { "SmallCross1", "SmallCross2",  "MediumCross1", "MediumCross2", "LargeCross1", "LargeCross2" };
        private static readonly List<string> CircleTokenNames = new List<string> { "SmallCircle1", "SmallCircle2", "MediumCircle1", "MediumCircle2", "LargeCircle1", "LargeCircle2" };


        /***** Public Function *****/

        // Returns the best AI Move for the given state
        public static MoveString GetBestMove(StringState state)
        {
            // Execute the Alpha Beta Search with the appropriate params and return the best move directly as a MoveString
            return AlphaBetaRoot(Constants.AI_DEPTH, state, Constants.AI_PLAYER);
        }
        

        /***** Private Functions *****/

        // Returns a List of available Tokennames for the given state and player
        // (All Player Tokens including the Tokens on the Peek of the Fields
        // but without the covered Tokens)
        private static List<string> GetAvailableTokensForGameState(StringState state, Player player)
        {
            // Direct Init of all Tokennames for the Player
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
                        // => add to covered tokens
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
                    // => add to available tokens
                    availableTokens.Add(token);
                }
            }

            return availableTokens;
        }


        // Returns the allowed player Tokennames for the given Gamestate on a specific Field on the GameField
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


        // Returns a List of all possible moves for the given player on the given state
        private static List<MoveString> GetPossibleMoves(StringState state, Player player)
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


        // Returns a new Gamestate without the given Token with new reference
        // Removes the Token from the given GameState if it is on the Gamestate
        // otherwise the given GameState will be returned with new reference
        // (only has to check the peeks because the covered Tokens are ignored by GetAvailableTokensForGameState)
        private static StringState RemoveTokenFromGameState(StringState state, string token)
        {
            // Deep clone the given state without reference
            StringState resultState = TypeConverter.DeepCloneState(state);

            // Iterate through all available fields of the state
            foreach (var field in state)
            {
                // Check if the token is on the peek of the field
                if (field.Value.Peek() == token)
                {
                    // When the field has more than one token
                    if (field.Value.Count > 1)
                    {
                        // Remove the highest token from the field
                        resultState[field.Key].Pop();
                    }
                    // When the token is the only token on the field
                    else
                    {
                        // Remove the field from the state
                        resultState.Remove(field.Key);
                    }
                }
            }

            return resultState;
        }


        // Return a new state with the given move on the given state with new reference
        private static StringState GetStateWithMove(StringState state, MoveString move)
        {
            // Deep clone previous state
            StringState resultState = TypeConverter.DeepCloneState(state);

            // Remove the token from the previous field (if already placed)
            resultState = RemoveTokenFromGameState(resultState, move.Token);

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

        // Starts the Alpha Beta Algorithm for every first move (root) by the start parameters
        // (The Player to start is the ai player)
        private static MoveString AlphaBetaRoot(int depth, StringState state, Player player)
        {
            // Start with the lowest possible value
            int bestValue = int.MinValue;
            // The best move result
            MoveString bestMove = new MoveString();

            // Get all first moves
            List<MoveString> possibleMoves = GetPossibleMoves(state, player);

            // Iterate through all possible moves
            foreach (MoveString move in possibleMoves)
            {
                // Execute the move on a new reference of the main state
                StringState moveState = GetStateWithMove(state, move);

                // Get the value for this route by alpha beta algorithm
                int value = AlphaBeta(depth - 1, moveState, player, int.MinValue, int.MaxValue);

                // Is the value for this route better than the previos value?
                if (value >= bestValue)
                {
                    // Overwrite the best value
                    bestValue = value;
                    // Save the appropriate move
                    bestMove = move;
                }
            }

            // Return only the best move without the value
            return bestMove;
        }

        // The recursive alpha beta function
        // Checks every possible move for every recursion step (depth)
        // Stops the search by the alpha and beta cap value
        // Optimization: Checks for win or loose when depth is not reached yet
        // Returns the value for the root move, calculated by the evaluation function         
        private static int AlphaBeta(int depth, StringState state, Player player, int alpha, int beta)
        {
            // Optimization: Checks for win or loose when depth is not reached yet
            // It is not necessary to look any further if win or loose
            // The earlier the detection the higher the value
            if (depth > 0)
            {                
                // Check for the winner (may be empty)
                Player? winner = WinDetection.CheckWinner(state);

                // Has the ai won the game?
                if (winner == Constants.AI_PLAYER)
                {
                    // Return high value
                    // The higher the left recursion count the better the value
                    return 1000 * Constants.AI_DEPTH;
                }

                // Has the ai lost the game?
                if (winner == TypeConverter.GetOpponent(Constants.AI_PLAYER))
                {
                    // Return low value
                    // The higher the left recursion count the lower the value                    
                    return -1000 * Constants.AI_DEPTH;
                }
            }
            // Otherwise the depth is reached (last recursion step)
            else
            {
                // Evaluate the leaf
                return EvaluateState(state, player);
            }

            // Switch player after win detection
            // After evaluation this won't be necessary anymore
            // But eval has to be calculated for the player for which the current recursion call was made
            player = TypeConverter.GetOpponent(player);

            // Get all possible moves for the current state and player
            List<MoveString> possibleMoves = GetPossibleMoves(state, player);            

            // If the Player is the ai the vale will be maxed
            if (player == Constants.AI_PLAYER)
            {
                // Start with lowest possible value
                int bestValue = int.MinValue; 

                // Iterate through each possible move
                foreach (MoveString move in possibleMoves)
                {
                    // Generate a new reference state with the move on the previous state
                    StringState moveState = GetStateWithMove(state, move);

                    // Check for the highest value in the recursion
                    bestValue = Math.Max(bestValue, AlphaBeta(depth - 1, moveState, player, alpha, beta));

                    // Alpha beta Pruning
                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                    {
                        return bestValue;
                    }
                }

                return bestValue;
            }
            // Otherwise the value will be minned for the opponent
            else
            {
                // Start with highest possible value
                int bestValue = int.MaxValue; 

                // Iterate through each possible move
                foreach (MoveString move in possibleMoves)
                {
                    // Generate a new reference state with the move on the previous state
                    StringState moveState = GetStateWithMove(state, move);

                    // Check for the lowest value in the recursion
                    bestValue = Math.Min(bestValue, AlphaBeta(depth - 1, moveState, player, alpha, beta));

                    // Alpha beta Pruning
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
        // Calls the Evaluation for each possible three field combination
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
        // (Used an array as fields parameter for iteration)       
        private static int EvaluateThreeFields(StringState state, Player player, Field[] fields)
        {
            int res = 0;
            int currentValue = 0;

            // Count the tokens for each player
            int playerTokenCounter = 0;
            int opponentTokenCounter = 0;            

            // Iterate through all three fields
            foreach (Field field in fields)
            {
                // Get the tokenString on the peek of the field or use an empty string
                string tokenString = state.ContainsKey(field) ? state[field].Peek() : "";

                // Evaluate the tokens on the field
                currentValue = EvaluateToken(player, tokenString);

                // The result is positive for the players token
                if (currentValue > 0)
                {
                    // Inc player token counter
                    playerTokenCounter++;
                    res += currentValue * (playerTokenCounter + 1);
                }
                // The result is negative for opponent token
                else if (currentValue < 0)
                {
                    // Inc opponent counter
                    opponentTokenCounter++;
                    // Add current Value * opponent counter
                    res += currentValue * (opponentTokenCounter + 1);
                }
                // Otherwise no change of res
            }

            return res;
        }

        // Evaluate a given tokenString of the given player
        private static int EvaluateToken(Player player, string tokenString)
        {
            int res = 0;

            // Is there a token on the field? (otherwise the value is 0)
            if (tokenString != "")
            {               
                // Get the value for the token
                res = GetTokenQuantifier(tokenString);

                // Is the token from the opponent?
                if (TypeConverter.GetPlayerForTokenName(tokenString) != player)
                {
                    // Invert the value
                    res *= -1;
                }
            }

            return res;
        }

        // Returns the quantifier for the size of the given tokenName
        private static int GetTokenQuantifier(string tokenName)
        {
            return tokenName.Contains("Small") ? Constants.SMALL_QUANTIFIER 
                                               : (tokenName.Contains("Medium") ? Constants.MEDIUM_QUANTIFIER 
                                                                               : Constants.LARGE_QUANTIFIER);
        }


        /***** DEBUG *****/

        // Debug function for the state
        // Debugs all fields with the peek token
        private static void DebugState(StringState state)
        {
            foreach (var field in state)
            {
                Debug.Log("field: " + field.Key);
                Debug.Log("peek token: " + field.Value.Peek());
            }
        }

        // Debug function for a move
        // Debugs the field and the token
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