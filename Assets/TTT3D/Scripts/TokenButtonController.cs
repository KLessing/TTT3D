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

    void OnEnable()
    {
        // Iterate Buttons / Prefabs 
        // (traditionel for to use the index)
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
                var colors = TokenButtons[i].colors;

                // Test if token is on the peek of a field on the gamefield
                if (GameControllerPrefab.TokenIsOnPeek(TokenPrefabs[i]))
                {
                    // use yellow color
                    colors.normalColor = Color.yellow;
                }
                else
                {
                    // Token is not on the gamefield and can be used freely
                    // use lower opacity to distinguish between selection
                    colors.normalColor = new Color(1, 1, 1, 0.5f);
                }                

                TokenButtons[i].colors = colors;
            }
        }
    }
}
