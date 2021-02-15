using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIActivation : MonoBehaviour
{
    public GameObject buttonActivator;
    public GameObject statusTextActivator;
    public Text buttonText;

    public void ToggleAllocationButtons()
    {
        if (!GameManager.Instance.allocating)
        {
            statusTextActivator.SetActive(false);
            buttonActivator.SetActive(true);
            buttonText.text = "Return";
            GameManager.Instance.isRunning = false;
        }
        else if (GameManager.Instance.allocating)
        {
            buttonActivator.SetActive(false);
            statusTextActivator.SetActive(true);
            buttonText.text = "Allocate";
            GameManager.Instance.isRunning = true;
        }
        GameManager.Instance.allocating = !GameManager.Instance.allocating;
    }
}
