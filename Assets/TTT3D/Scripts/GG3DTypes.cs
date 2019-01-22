namespace GG3DTypes
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum Player { Cross, Circle };

    public enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

    // The different Token Sizes (The Index serves as comparison value)
    public enum TokenSize { Small, Medium, Large };

    // Typedef workaround for constants
    public static class Constants {
        // The ai depth determines the count of calculated moves for the ai algrorithm
        public const int AI_DEPTH = 3;
        // The player which is controlled by the ai for singlePlayer modus
        // (not yet fully implemented in current ui structure; 
        // only used in ai controller for now)
        public const Player AI_PLAYER = Player.Cross;
        // The Player that starts the game
        // who makes the first move
        public const Player START_PLAYER = Player.Circle;
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

    // Move with TokenString for AI
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