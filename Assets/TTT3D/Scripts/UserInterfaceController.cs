﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;

public class UserInterfaceController : MonoBehaviour {

    // The User Interface for Cross Token Selection
    public GameObject CrossTokenSelection;

    // The User Interface for Circle Token Selection
    public GameObject CircleTokenSelection;

    // The User Interface for Game Field Selection
    public GameObject GameFieldSelection;

    // The Current Player (Cross or Circle)
    private Player currentPlayer = Player.Cross;

    // The nullable selected Token
    private Token? selectedToken = null;

    // The nullable selected Field on the GameField
    private Field? selectedField = null;


    // TODO enum for individual Token and Field ids?!    

    // Deactivates the current Token Selection UI and activates Field Selection Ui
    public void Next()
    {
        if (selectedToken != null)
        {
            if (currentPlayer == Player.Cross)
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
        selectedField = null;
        selectedToken = null;

        GameFieldSelection.SetActive(false);

        if (currentPlayer == Player.Cross)
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
        if (selectedToken != null && selectedField != null)
        {
            Debug.Log("Selections confirmed");

            // TODO call game function (which disables this controller?!)

            // Reset Selection
            selectedField = null;
            selectedToken = null;

            GameFieldSelection.SetActive(false);

            // Change current Player and UI
            if (currentPlayer == Player.Cross)
            {
                currentPlayer = Player.Circle;
                CircleTokenSelection.SetActive(true);
            }
            else
            {
                currentPlayer = Player.Cross;
                CrossTokenSelection.SetActive(true);
            }
        }
    }

    // Hint: Button on Click works only with max 1 string or bool parameter

    // Saves the selected Token
    public void SetToken(string tokenName)
    {
        // Convert string parameter to token type
        selectedToken = (Token) System.Enum.Parse(typeof(Token), tokenName);
        Debug.Log("Selected Token: " + selectedToken);
    }

    // Saves the selected Field
    public void SetField(string fieldName)
    {
        // Convert string parameter to field type
        selectedField = (Field)System.Enum.Parse(typeof(Field), fieldName);
        Debug.Log("Selected Field: " + selectedField);
    }
}
