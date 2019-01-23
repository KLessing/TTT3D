using UnityEngine;
using UnityEngine.UI;
using GG3DTypes;
using GG3DAI;
using GG3DConverter;

// Controller for all UI Interfaces and most of the Game Logic
public class UserInterfaceController : MonoBehaviour {

    /***** Controller Prefabs *****/

    // The GameFieldController which contains the Gamefield
    public GameFieldController GameFieldControllerPrefab;

    // The MoveController to convert a moveString from the AI Result to an actual move with a Token
    public MoveConverter MoveConverterPrefab;


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

    // The User Interface for the New Game Confirmation
    // Starts a new game or returns to the previous Token Selection
    public GameObject NewGameConfirmationUI;


    /***** Global Variables *****/

    // The Current Player (Cross or Circle)
    private Player CurrentPlayer = Constants.START_PLAYER;

    // The nullable selected Token
    private GameObject SelectedToken = null;

    // The nullable selected Field on the GameField
    // (Field needs to be optional to be nullable 
    // because null is not in the Field Datatype)
    private Field? SelectedField = null;

    // AI Usage is true for Single Player Games
    // and false for Mutliplayer
    // Gets selected at the start of a Game    
    private bool AIUsage = false;


    /***** Public Functions that are called by Unity *****/

    // Selects the count of Players at the beginning of the Game and starts the first move
    // One Player Button selection means the ai is used for the defined AI player
    // Gets called by the appropriate PlayerCountSelectionUI
    public void SelectPlayerCount(bool useAI)
    {
        AIUsage = useAI;
        PlayerCountSelectionUI.SetActive(false);

        StartNewMove();
    }

    // Toggles the NewGameConfirmationUI
    // start = True => show (New Game button pressed in TokenSelectionUI)
    // start = false => hide (Cancel button pressed in NewGameConfirmationUI)
    public void ToggleNewGameConfirmation(bool start)
    {
        if (start)
        {
            ToggleTokenUI(CurrentPlayer, false);
            NewGameConfirmationUI.SetActive(true);
        }
        else
        {
            NewGameConfirmationUI.SetActive(false);
            ToggleTokenUI(CurrentPlayer, true);            
        }
    }

    // Reset the current Game and start new Game by enabling the first UI
    // Gets called by the appropriate WinnerShowcaseUI and NewGameConfirmationUI
    public void NewGame()
    {
        // Reset Gamefield and current Player
        GameFieldControllerPrefab.Reset();
        CurrentPlayer = Constants.START_PLAYER;

        // Hide possible open UIs and show the first UI
        NewGameConfirmationUI.SetActive(false);
        WinnerShowcaseUI.SetActive(false);
        PlayerCountSelectionUI.SetActive(true);           
    }

    // Deactivates the current Token Selection UI and activates the Field Selection UI
    // Gets called by the next Button of the current Token UI
    public void Next()
    {
        // Only if a Token is selected
        if (SelectedToken != null)
        {
            // Switch UI
            ToggleTokenUI(CurrentPlayer, false);
            GameFieldSelection.SetActive(true);
        }
    }

    // Deactivates Field Selection UI and activates the current Token Selection UI
    // Gets called by the back Button of the GameField Selection UI
    public void Prev()
    {
        // Reset Selection
        SelectedField = null;
        SelectedToken = null;

        // Switch UI
        GameFieldSelection.SetActive(false);
        ToggleTokenUI(CurrentPlayer, true);
    }

    // Executes the move with the selected Options and changes the current Player
    // Gets called by the confirm Button of the GameField Selection UI
    public void Confirm()
    {
        // Make shure that everything is selected
        // (especially the field because the GameField Selection is still active)
        if (SelectedToken != null && SelectedField != null)
        {
            // Create a move with the user selections
            Move move = new Move(SelectedToken, (Field) SelectedField);

            // Reset Selections
            SelectedField = null;
            SelectedToken = null;

            // Hide Game Field Selection UI
            GameFieldSelection.SetActive(false);        
            
            ExecuteMove(move);
        }
    }

    // Saves the selected Token
    // Gets called by unity Token Buttons
    public void SetToken(GameObject token)
    {
        SelectedToken = token;
    }

    // Saves the selected Field
    // Gets called by unity Field Buttons
    public void SetField(string fieldName)
    {
        // Convert string parameter to field type
        SelectedField = GetFieldEnumFromString(fieldName);
    }

    // Return if Placement of selected token is possible on field parameter
    // Needed for Button Controller for the GameField Selection UI to disable not allowed fields
    // Needs to get called from here because the selected token is needed
    public bool PlacementOnFieldPossible(string fieldName)
    {
        return GameFieldControllerPrefab.PlacementPossible(SelectedToken, GetFieldEnumFromString(fieldName));
    }


    /***** Private Helper Functions *****/

    // Starts a new move for the current Player by enabling the token selection ui for the player
    // When the current Player is controller by the ai the calculated best move is executed directly
    private void StartNewMove() {
        // Is ai used and is the current Player controlled by the ai?
        if (AIUsage && CurrentPlayer == Constants.AI_PLAYER) {
            // Get the best AI Move for the state
            MoveString moveString = AIController.GetBestMove(TypeConverter.ConvertState(GameFieldControllerPrefab.GameField));
            // Convert MoveString to Move with Token GameObject
            Move move = MoveConverterPrefab.ConvertMove(moveString);
            // Execute the best ai move directly
            ExecuteMove(move);
        }
        else {
            // Otherwise start a normal move for the CurrentPlayer
            ToggleTokenUI(CurrentPlayer, true);
        }
    }

    // Executes the given Move on the Gamefield
    // Handles the win directly if the current Player won with the move
    // Otherwise starts a new move for the next Player
    private void ExecuteMove (Move move) {
        // Execute the Move
        Player? winner = GameFieldControllerPrefab.SetTokenOnField(move);

        // When no winner yet switch player and start next move
        if (winner == null) {
            CurrentPlayer = TypeConverter.GetOpponent(CurrentPlayer);
            StartNewMove();
        }        
        // Otherwise handle win
        else
        {
            // Get the Text component to show the winner
            Text winnerMsg = WinnerShowcaseUI.GetComponentInChildren<Text>();
            
            // Set the win message for the winner
            winnerMsg.text = GetWinMessage((Player) winner);

            // Show Winner Message UI
            WinnerShowcaseUI.SetActive(true);
        }
    }

    // Returns the win message for a given player
    private string GetWinMessage(Player winner){
        string res = "";

        // player or ai won?
        if (AIUsage && winner == Constants.AI_PLAYER)
        {
            res += "KI - ";
        }
        else 
        {
            res += "Spieler - ";
        }               

        // which token won?
        if (winner == Player.Cross)
        {
            res += "Kreuz ";
        }
        else if (winner == Player.Circle)
        {
            res += "Kreis ";
        }

        return res + "gewinnt!";
    }

    // Parse the given string to a Field Enum
    // (Workaround for unity because buttons can only use primitive values as params)
    private Field GetFieldEnumFromString(string fieldName)
    {
        return (Field)System.Enum.Parse(typeof(Field), fieldName);
    }

    // Enable or disable the TokenUI for the current Player based on the given param
    private void ToggleTokenUI(Player player, bool enable) 
    {
        if (player == Player.Cross)
        {
            CrossTokenSelection.SetActive(enable);
        }
        else if (player == Player.Circle)
        {
            CircleTokenSelection.SetActive(enable);
        }
    }
}