using UnityEngine;
using UnityEngine.UI;
using GG3DTypes;
using GG3DAI;

public class UserInterfaceController : MonoBehaviour {

    /***** Controller Prefabs *****/

    // The GameController which contains the Gamefield
    public GameController GameControllerPrefab;

    // The AI Controller
    public AIController AIControllerPrefab;

    // TODO + The AI Level ?!


    /***** User Interfaces *****/

    // The User Interface for Cross Token Selection
    public GameObject CrossTokenSelection;

    // The User Interface for Circle Token Selection
    public GameObject CircleTokenSelection;

    // The User Interface for Game Field Selection
    public GameObject GameFieldSelection;

    // The User Interface to show when the Game was won
    // Shows the winner and a Button for a new Game
    public GameObject WinnerShowcaseUI;

    // The User Interface for the Player Count Selection
    // Responsible for AI usage
    public GameObject PlayerCountSelectionUI;


    /***** Global Variables *****/

    // The Current Player (Cross or Circle)
    private Player CurrentPlayer = Player.Cross;

    // The nullable selected Token
    private GameObject SelectedToken = null;

    // The nullable selected Field on the GameField
    private Field? SelectedField = null;

    // The Winner to check if game was won
    private Player? Winner = null;

    // AI Usage is true for Single Player Games
    // and false for Mutliplayer    
    private bool AIUsage = false;


    // Select the count of Players at the beginning of the Game
    // One Player means the second Player is played by an AI
    public void SelectPlayerCount(bool useAI)
    {
        AIUsage = useAI;
        PlayerCountSelectionUI.SetActive(false);
        CrossTokenSelection.SetActive(true);
    }

    // Reset the current Gamefield and start new Game
    public void NewGame()
    {
        GameControllerPrefab.Reset();
        WinnerShowcaseUI.SetActive(false);
        PlayerCountSelectionUI.SetActive(true);
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
            // Execute Move with selected options
            Winner = GameControllerPrefab.SetTokenOnField(new Move(SelectedToken, (Field) SelectedField));

            // Reset Selection
            SelectedField = null;
            SelectedToken = null;

            // Hide Game Field Selection UI
            GameFieldSelection.SetActive(false);

            // Execute the next move of the AI for SinglePlayer Mode
            // when no winner yet
            if (Winner == null && AIUsage)
            {
                // Change current Player
                CurrentPlayer = GetNextPlayer(CurrentPlayer);

                // get the best AI move for current Player and GameState
                Move bestMove = AIControllerPrefab.GetBestMove(GameControllerPrefab.GameField, CurrentPlayer);

                // Execute the best AI move in GameController
                Winner = GameControllerPrefab.SetTokenOnField(bestMove);
            }

            // Continue when still no winner
            if (Winner == null)
            { 
                // Change current Player and UI
                CurrentPlayer = GetNextPlayer(CurrentPlayer);
                ActivateTokenUI(CurrentPlayer);                            
            } 
            else
            {
                // Get the Text component to show the winner
                Text winnerMsg = WinnerShowcaseUI.GetComponentInChildren<Text>();

                // Set the winner text
                if (Winner == Player.Cross)
                {
                    winnerMsg.text = "Kreuz gewinnt";
                }
                else if (Winner == Player.Circle)
                {
                    winnerMsg.text = "Kreis gewinnt";
                }

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

    // Return the next Player
    private Player GetNextPlayer(Player current)
    {
        return current == Player.Cross ? Player.Circle : Player.Cross;
    }

    // Activate the Token Selection UI of the given Player
    private void ActivateTokenUI(Player current)
    {
        if (current == Player.Cross)
        {
            CrossTokenSelection.SetActive(true);
        }
        else if (current == Player.Circle)
        {
            CircleTokenSelection.SetActive(true);
        }
    }
}