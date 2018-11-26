using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour {

    // The User Interface for Cross Token Selection
    public GameObject CrossTokenSelection;

    // The User Interface for Circle Token Selection
    public GameObject CircleTokenSelection;

    // The User Interface for Game Field Selection
    public GameObject GameFieldSelection;

    // The Current Player (Cross or Circle)
    private string currentPlayer = "Cross";

    // The selected Token (Cross or Circle)
    private string selectedToken = "";

    // The selected Field on the GameField
    private string selectedField = "";


    // TODO enum for individual Token and Field ids?!    

    // Deactivates the current Token Selection UI and activates Field Selection Ui
    public void Next()
    {
        if (selectedToken != "")
        {
            if (currentPlayer == "Cross")
            {
                CrossTokenSelection.SetActive(false);
            }
            else
            {
                CircleTokenSelection.SetActive(false);
            }

            GameFieldSelection.SetActive(true);
        }
    }

    // Deactivates Field Selection UI and activates the current Token Selection Ui
    public void Prev()
    {
        // Reset Selection
        selectedField = "";
        selectedToken = "";

        GameFieldSelection.SetActive(false);

        if (currentPlayer == "Cross")
        {
            CrossTokenSelection.SetActive(true);
        }
        else
        {
            CircleTokenSelection.SetActive(true);
        }
    }

    // Executes the move with the selected Options and changes the current Player
    public void Confirm()
    {
        if (selectedToken != "" && selectedField != "")
        {
            Debug.Log("Selections confirmed");
            // TODO call game function (which disables this controller?!)
            selectedToken = "";
            selectedField = "";

            GameFieldSelection.SetActive(false);

            // Change current Player and UI
            if (currentPlayer == "Cross")
            {
                currentPlayer = "Circle";
                CircleTokenSelection.SetActive(true);
            }
            else
            {
                currentPlayer = "Cross";
                CrossTokenSelection.SetActive(true);
            }
        }
    }

    // Saves the selected Token
    public void SetToken(string tokenName)
    {
        selectedToken = tokenName;
        Debug.Log("Selected Token: " + selectedToken);
    }

    // Saves the selected Field
    public void SetField(string fieldName)
    {
        selectedField = fieldName;
        Debug.Log("Selected Field: " + selectedField);
    }
}
