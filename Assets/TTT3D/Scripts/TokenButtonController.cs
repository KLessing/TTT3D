using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GG3DTypes;

public class TokenButtonController : MonoBehaviour {

    // The Game Controller with the Gamefield
    public GameController GameControllerPrefab;

    // The Buttons and the associated Prefabs and textFields
    // (The same indices are relevant)
    public Button[] TokenButtons;
    public GameObject[] TokenPrefabs;
    public Text[] TokenFieldText;

    private void OnEnable()
    {
        // Iterate Buttons / Prefabs 
        // (traditionel for to use the index)
        for (int i = 0; i < TokenButtons.Length; i++)
        {
            // Init the Field text as not visible
            TokenFieldText[i].text = "";           

            // Test if Token for the Button is Covered
            // and enable / disable the button accordingly
            if (GameControllerPrefab.TokenIsCovered(TokenPrefabs[i]))
            {
                TokenButtons[i].interactable = false;
                // Set Field text as not available to apply the Gobblit Gobblers Rules
                // (Don't show where the covered token is because its not visible in the original game also)
                TokenFieldText[i].text = "-";
            }
            else
            {
                var colors = TokenButtons[i].colors;

                // Test if token is on the peek of a field on the Gamefield
                if (GameControllerPrefab.TokenIsOnPeek(TokenPrefabs[i]))
                {
                    // Use yellow color
                    colors.normalColor = Color.yellow;
                    // Set the field initals as text above the button
                    TokenFieldText[i].text = GetFieldInitials(TokenPrefabs[i]);
                }
                else
                {
                    // Token is not on the gamefield and can be used freely
                    // Use lower opacity to distinguish between selection
                    colors.normalColor = new Color(1, 1, 1, 0.5f);
                }

                // Set the color and interactability
                TokenButtons[i].colors = colors;
                TokenButtons[i].interactable = true;
            }
        }

    }

    // Returns the field initals for a given token on the peek of the GameField
    private string GetFieldInitials(GameObject token)
    {
        // Get the field for the peek token
        Field? field = GameControllerPrefab.GetFieldForPeekToken(token);
        
        // Return the initals
        switch (field)
        {
            case Field.BottomLeft: return "BL";
            case Field.BottomMiddle: return "BM";
            case Field.BottomRight: return "BR";

            case Field.Middle: return "M";
            case Field.MiddleLeft: return "ML";
            case Field.MiddleRight: return "MR";

            case Field.TopLeft: return "TL";
            case Field.TopMiddle: return "TM";
            case Field.TopRight: return "TR";

            default: return "";
        }
    }
}
