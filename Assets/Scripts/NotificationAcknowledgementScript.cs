using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationAcknowledgementScript : MonoBehaviour
{
    public GameObject notificationTextActivator;
    public GameObject buttonActivator;
    public GameObject statusTextActivator;

    public void AcknowledgeNotificationAndDismiss()
    {
        if (GameManager.Instance.allocating)
        {
            notificationTextActivator.SetActive(false);
            statusTextActivator.SetActive(false);
            buttonActivator.SetActive(true);
            GameManager.Instance.isRunning = false;
        }
        else
        {
            notificationTextActivator.SetActive(false);
            buttonActivator.SetActive(false);
            statusTextActivator.SetActive(true);
            GameManager.Instance.isRunning = true;
        }
        //De-activate object
        //Activate other panels
        GameManager.Instance.isRunning = true;
    }
}
