using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUIActivation : MonoBehaviour
{
    public GameObject buttonActivator;
    public GameObject statusTextActivator;
    public bool allocating;
    public Text buttonText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleAllocationButtons()
    {
        if (!allocating)
        {
            statusTextActivator.SetActive(false);
            buttonActivator.SetActive(true);
            buttonText.text = "Return";
        }
        else if (allocating)
        {
            buttonActivator.SetActive(false);
            statusTextActivator.SetActive(true);
            buttonText.text = "Allocate";
        }
        allocating = !allocating;
    }
}
