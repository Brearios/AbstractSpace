using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInputController : MonoBehaviour
{
    public Text inputFieldTitleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StoreStringInput(string input)
    {
        GameManager.Instance.currentStoredInput = input;
        Debug.Log($"Text box input equals {GameManager.Instance.currentStoredInput}.");
    }

    public void DisplayInputFieldTitle(string input)
    {

    }
}
