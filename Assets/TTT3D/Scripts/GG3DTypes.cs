namespace GG3DTypes
{
    using System.Collections.Generic;
    using UnityEngine;

    // All possible (classic) Tic Tac Toe Player
    public enum Player { Cross, Circle };

    // All Fields on the GameField
    public enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

    // Typedef workaround for constants
    public static class Constants {
        // The ai depth determines the count of calculated moves for the ai algrorithm
        public const int AI_DEPTH = 3;
        // The player which is controlled by the ai for singlePlayer modus
        // (not yet fully implemented in current ui structure; 
        // only used in ai controller for now)
        public const Player AI_PLAYER = Player.Circle;
        // The Player that starts the game
        // who makes the first move
        public const Player START_PLAYER = Player.Circle;
        // The AI Quantifier values for the token sizes
        // These values determine which deposit the ai prefers
        // And may reduce baiting from the player
        public const int SMALL_QUANTIFIER = 1;
        public const int MEDIUM_QUANTIFIER = 2;
        public const int LARGE_QUANTIFIER = 3;
        
    };

    // Typedef workaround for a GameState with Tokens on Gamefield
    public class GameState : Dictionary<Field, Stack<GameObject>> {}

    // Game state with strings instead of Token 
    // (needed for AI for Intance Problem workaround)
    public class StringState : Dictionary<Field, Stack<string>> {};


    // A Move consists of a Token and a Field for the Placement
    public struct Move
    {
        public GameObject Token;
        public Field Field;

        public Move(GameObject token, Field field)
        {
            Token = token;
            Field = field;
        }
    }

    // Move with the token Name instead of the Token GameObject 
    // (needed for independent AI)
    public struct MoveString
    {
        // The string consists of the size, the player AND the number of the token (1 or 2)
        public string Token;
        public Field Field;

        public MoveString(string token, Field field)
        {
            Token = token;
            Field = field;
        }
    }
}