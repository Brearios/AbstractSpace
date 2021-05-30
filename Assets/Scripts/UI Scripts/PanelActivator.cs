using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelActivator : MonoBehaviour
{
    public static PanelActivator Instance;
    public GameObject inputWindowActivator;
    public GameObject menuPanel;
    public GameObject notificationTextActivator;
    public GameObject buttonActivator;
    public GameObject statusTextActivator;
    public GameObject OverviewTextBackground;
    public GameObject AllocationTextBackground;    
    public Text pauseButtonText;

    public List<GameObject> UIElementList;
    public void Awake()
    {
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        UIElementList.Add(inputWindowActivator);
        UIElementList.Add(menuPanel);
        UIElementList.Add(notificationTextActivator);
        UIElementList.Add(buttonActivator);
        UIElementList.Add(statusTextActivator);
        UIElementList.Add(OverviewTextBackground);
        UIElementList.Add(AllocationTextBackground);
    }

    void Update()
    {
        if (GameManager.Instance.isRunning)
        {
            pauseButtonText.text = "Pause";
        }

        if (!GameManager.Instance.isRunning)
        {
            pauseButtonText.text = "Resume";
        }

    }

    // Disable all UI elements so you only need to enable what's needed
    public void DisableUIElements()
    {
        foreach (GameObject UIElement in UIElementList)
        {
            UIElement.SetActive(false);
        }
    }

    public void AcknowledgeNotificationAndDismiss()
    {
        notificationTextActivator.SetActive(false);
        GameManager.Instance.isRunning = true;
    }

    public void CustomizeEmpire()
    {
        GameManager.Instance.isRunning = false;
        DisableUIElements();
        inputWindowActivator.SetActive(true);
    }
}
