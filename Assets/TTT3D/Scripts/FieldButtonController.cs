using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldButtonController : MonoBehaviour {

    // The UI Controller which holds the selected Token
    // and calls the GameController
    public UserInterfaceController UserInterfaceControllerPrefab;

    // The Buttons
    public Button[] FieldButtons;

    void OnEnable()
    {
        // Iterate Buttons
        foreach (Button fieldButton in FieldButtons)
        {
            // Test if selected Token can be placed on this field
            // and enable / disable the button accordingly
            if (UserInterfaceControllerPrefab.PlacementOnFieldPossible(fieldButton.tag))
            {
                fieldButton.interactable = true;
            }
            else
            {
                fieldButton.interactable = false;
            }
        }
    }
}
