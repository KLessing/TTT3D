﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extra Namespace to prevent conflicts with other type definitions
namespace GG3DTypes
{
    public enum Player { Cross, Circle };

    public enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

    // The different Token Sizes (The Index serves as comparison value)
    public enum TokenSize { Small, Medium, Large };

    // Typedef workaround for a GameState with Tokens on Gamefield
    // (using works only in same file)
    public class GameState : Dictionary<Field, Stack<GameObject>> { };

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
}