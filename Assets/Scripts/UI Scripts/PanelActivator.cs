using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public static PanelActivator Instance;
    public GameObject notificationTextActivator;
    public GameObject buttonActivator;
    public GameObject statusTextActivator;
    public GameObject OverviewTextBackground;
    public GameObject AllocationTextBackground;

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
    }

    public void AcknowledgeNotificationAndDismiss()
    {
        notificationTextActivator.SetActive(false);
        GameManager.Instance.isRunning = true;

        // REMOVE - unneeded because it works fine over the top of things
        //if (GameManager.Instance.allocating)
        //{
        //    notificationTextActivator.SetActive(false);
        //    statusTextActivator.SetActive(false);
        //    buttonActivator.SetActive(true);
        //    GameManager.Instance.isRunning = false;
        //}
        //else
        //{
        //    notificationTextActivator.SetActive(false);
        //    buttonActivator.SetActive(false);
        //    statusTextActivator.SetActive(true);
        //    GameManager.Instance.isRunning = true;
        //}
        //De-activate object
        //Activate other panels
        GameManager.Instance.isRunning = true;
    }
}
