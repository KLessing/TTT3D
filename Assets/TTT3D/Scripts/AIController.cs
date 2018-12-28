using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GG3DTypes;

namespace GG3DAI
{
    public class AIController : MonoBehaviour
    {
        public static Move GetBestMove(Player player, GameState state)
        {
            // TODO need GameObject or independent stwing states

            // TEST move the middle object to top rigth
            //return new Move(state[Field.Middle].Peek(), Field.TopRight);

            return new Move();
        }
    }

}
