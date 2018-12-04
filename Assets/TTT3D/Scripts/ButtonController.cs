using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TTT3DTypes;

//using UnityEngine.Experimental.UIElements;

public class ButtonController : MonoBehaviour {

    public GameController GameControllerPrefab;

    // Returns Token from Button Name if possible
    private Token? GetTokenFromBtnName(string btnName)
    {
        return (Token)System.Enum.Parse(typeof(Token), btnName.Substring(3));
    }

    //void OnEnable()
    //{
    //    Button[] buttons = GetComponentsInChildren<Button>();

    //    foreach (Button btn in buttons)
    //    {
    //        if (btn.name.Contains("Cross") || btn.name.Contains("Circle"))
    //        {            
    //            Token? btnToken = GetTokenFromBtnName(btn.name);
    //            if (btnToken != null)
    //            {
    //                btn.interactable = GameControllerPrefab.TokenIsCovered((Token)btnToken);
    //            }
    //        }
    //    }
    //}
}
