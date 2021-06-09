using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIActivation : MonoBehaviour
{
    public GameObject buttonActivator;
    public GameObject statusTextActivator;
    public Text allocationButtonText;
    bool wasNotifying;

    public void ToggleAllocationButtons()
    {
        

        if (!GameManager.Instance.allocating)
        {
            TrackNotificationStatus();
            statusTextActivator.SetActive(false);
            buttonActivator.SetActive(true);
            allocationButtonText.text = "Return";
            GameManager.Instance.isRunning = false;
            PanelActivator.Instance.notificationTextActivator.SetActive(false);
        }
        else if (GameManager.Instance.allocating)
        {
            buttonActivator.SetActive(false);
            statusTextActivator.SetActive(true);
            allocationButtonText.text = "Allocate";
            if (wasNotifying)
            {
                PanelActivator.Instance.notificationTextActivator.SetActive(true);
                Debug.Log("Re-activating notification window on return from Allocation screen.");
            }
            else
            {
                GameManager.Instance.isRunning = true;
            }
        }
        GameManager.Instance.allocating = !GameManager.Instance.allocating;
    }
    public void TrackNotificationStatus()
    {
        if (PanelActivator.Instance.notificationTextActivator.activeSelf)
        {
            Debug.Log("Setting wasNotifying to true.");
            wasNotifying = true;
        }
        else
        {
            Debug.Log("Setting wasNotifying to false.");
            wasNotifying = false;
        }
    }
}
