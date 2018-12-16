using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT3DTypes;

public class UserInterfaceController : MonoBehaviour {

    // The GameController which contains the Gamefield
    public GameController GameControllerPrefab;

    // The User Interface for Cross Token Selection
    public GameObject CrossTokenSelection;

    // The User Interface for Circle Token Selection
    public GameObject CircleTokenSelection;

    // The User Interface for Game Field Selection
    public GameObject GameFieldSelection;

    // The User Interface to show when the Game was won
    // Shows the winner and a Button for a new Game
    public GameObject WinnerShowcaseUI;       

    // The Current Player (Cross or Circle)
    private Player CurrentPlayer = Player.Cross;

    // The nullable selected Token
    private GameObject SelectedToken = null;

    // The nullable selected Field on the GameField
    private Field? SelectedField = null;

    // The Winner to check if game was won
    private Player? Winner = null;

    // Needed for new Game call?!
    // public void Start();

    public void NewGame()
    {
        Debug.Log("Start new Game!");

        WinnerShowcaseUI.SetActive(false);
        CrossTokenSelection.SetActive(true);

        //TODO Logic for Gamefield reset in Gamecontroller

    }

    // Deactivates the current Token Selection UI and activates Field Selection Ui
    public void Next()
    {
        if (SelectedToken != null)
        {
            if (CurrentPlayer == Player.Cross)
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
        SelectedField = null;
        SelectedToken = null;

        GameFieldSelection.SetActive(false);

        if (CurrentPlayer == Player.Cross)
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
        if (SelectedToken != null && SelectedField != null)
        {
            Winner = GameControllerPrefab.SetTokenOnField(SelectedToken, (Field) SelectedField);

            // Reset Selection
            SelectedField = null;
            SelectedToken = null;

            // Hide Game Field Selection UI
            GameFieldSelection.SetActive(false);

            // Continue game when no winner yet
            if (Winner == null)
            {
                // Change current Player and UI
                if (CurrentPlayer == Player.Cross)
                {
                    CurrentPlayer = Player.Circle;
                    CircleTokenSelection.SetActive(true);
                }
                else
                {
                    CurrentPlayer = Player.Cross;
                    CrossTokenSelection.SetActive(true);
                }
            } 
            else
            {
                // Show Winner Message UI
                WinnerShowcaseUI.SetActive(true);
            }


        }
    }

    // Saves the selected Token
    public void SetToken(GameObject token)
    {
        SelectedToken = token;
    }

    // Saves the selected Field
    public void SetField(string fieldName)
    {
        // Convert string parameter to field type
        SelectedField = GetFieldEnumFromString(fieldName);
    }

    // Return if Placement of selected token is possible on field parameter
    public bool PlacementOnFieldPossible(string fieldName)
    {
        return GameControllerPrefab.PlacementPossible(SelectedToken, GetFieldEnumFromString(fieldName));
    }

    // Parse the given string to a Field Enum
    private Field GetFieldEnumFromString(string fieldName)
    {
        return (Field)System.Enum.Parse(typeof(Field), fieldName);
    }
}