namespace GG3DTypes
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum Player { Cross, Circle };

    public enum Field { TopLeft, TopMiddle, TopRight, MiddleLeft, Middle, MiddleRight, BottomLeft, BottomMiddle, BottomRight };

    // The different Token Sizes (The Index serves as comparison value)
    public enum TokenSize { Small, Medium, Large };

    // Typedef workaround for a GameState with Tokens on Gamefield
    public class GameState : Dictionary<Field, Stack<GameObject>>
    {
        // Empty Constructor without param
        public GameState() { }

        // Constructor with one param for copy without reference
        public GameState(IDictionary<Field, Stack<GameObject>> dictionary) : base(dictionary) { }

        // Deep Copy Function
        public GameState DeepCopy(GameState oldState)
        {
            GameState newState = new GameState();

            foreach (var field in oldState)
            {
                Stack<GameObject> tokenStack = new Stack<GameObject>();

                foreach (GameObject token in field.Value)
                {
                    Debug.Log("old " + token.name);

                    tokenStack.Push(GameObject.Instantiate<GameObject>(token)); // falsche Reihenfolge?
                }

                foreach (GameObject token in tokenStack)
                {
                    Debug.Log("new " + token.name);
                }

                newState.Add(field.Key, tokenStack);
            }

            return newState;
        }
    }

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