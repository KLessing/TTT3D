using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenButtonController : MonoBehaviour {

    // The Game Controller with the Gamefield
    public GameController GameControllerPrefab;

    // The Buttons and the associated Prefabs
    // (The same indices are relevant)
    public Button[] TokenButtons;
    public GameObject[] TokenPrefabs;

    // TODO use list instead of array?!

    void OnEnable()
    {
        // Iterate Buttons / Prefabs
        for (int i = 0; i < TokenButtons.Length; i++)
        {
            // Test if Token for the Button is Covered
            // and enable / disable the button accordingly
            if (GameControllerPrefab.TokenIsCovered(TokenPrefabs[i]))
            {
                TokenButtons[i].interactable = false;
            }
            else
            {
                TokenButtons[i].interactable = true;
            }
        }
    }
}
