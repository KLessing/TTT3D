using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour {

    // The User Interface for Cross Token Selection
    public GameObject CrossTokenSelection;

    // The User Interface for Game Field Selection
    public GameObject GameFieldSelection;

    // The Selections
    private string selectedToken = "";
    private string selectedField = "";
    // TODO enum for individual Token and Field ids

    

    public void next()
    {
        if (selectedToken != "")
        {
            CrossTokenSelection.SetActive(false);
            GameFieldSelection.SetActive(true);
        }        
    }

    public void prev()
    {
        // Reset Selection
        selectedField = "";
        selectedToken = "";
        GameFieldSelection.SetActive(false);
        CrossTokenSelection.SetActive(true);        
    }

    public void confirm()
    {
        if (selectedToken != "" && selectedField != "")
        {
            Debug.Log("Selections confirmed");
            // TODO call game function (which disables this controller?!)
            selectedToken = "";
            selectedField = "";
        }
    }

    public void setToken(string tokenName)
    {
        selectedToken = tokenName;
        Debug.Log("Selected Token: " + selectedToken);
    }

    public void setField(string fieldName)
    {
        selectedField = fieldName;
        Debug.Log("Selected Field: " + selectedField);
    }
}
