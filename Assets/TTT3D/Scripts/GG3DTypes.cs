namespace GG3DTypes
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum Player { Cross, Circle };

    public enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

    // The different Token Sizes (The Index serves as comparison value)
    public enum TokenSize { Small, Medium, Large };

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

    // MoveRating Struct for MoveString and Rating combination for AI
    public struct MoveRating
    {
        public MoveString Move;
        public int Rating;

        public MoveRating(MoveString move, int rating)
        {
            Move = move;
            Rating = rating;
        }
    }
}