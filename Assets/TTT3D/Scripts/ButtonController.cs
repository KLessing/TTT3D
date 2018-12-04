using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTT3DTypes;

//using UnityEngine.Experimental.UIElements;

public class ButtonController : MonoBehaviour {

    GameController gameController;

    // Returns Token from Button Name if possible
    private Token? GetTokenFromBtnName(string btnName)
    {
        return (Token)System.Enum.Parse(typeof(Token), btnName.Substring(3));
    }

    void OnEnable()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        //GameController gameController; // = GetComponentInParent<GameController>();

        foreach (Button btn in buttons)
        {
            Token? btnToken = GetTokenFromBtnName(btn.name);
            if (btnToken != null)
            {
                btn.interactable = gameController.TokenIsCovered((Token)btnToken);
            }

        }
    }
}
