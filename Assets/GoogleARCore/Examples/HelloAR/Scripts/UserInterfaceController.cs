using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour {

    // The User Interface for Cross Token Selection
    public GameObject CrossTokenSelection;

    // The User Interface for Game Field Selection
    public GameObject GameFieldSelection;

    // The Selections
    private string selectedToken;
    private string selectedField;
    // TODO enum for individual Token and Field ids


    public void next()
    {
        CrossTokenSelection.SetActive(false);
        GameFieldSelection.SetActive(true);
    }

    public void prev()
    {
        GameFieldSelection.SetActive(false);
        CrossTokenSelection.SetActive(true);        
    }

    public void save()
    {
        // call game function (which disables this controller?!)
        selectedToken = "";
        selectedField = "";
    }

    public void setToken(string tokenName)
    {
        selectedToken = tokenName;
        Debug.Log("selected Token: " + selectedToken);
    }

    public void setField(string fieldName)
    {
        selectedField = fieldName;
        Debug.Log("selected Field: " + selectedField);
    }
}
