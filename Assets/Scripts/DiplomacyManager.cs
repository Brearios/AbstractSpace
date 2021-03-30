using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiplomacyManager : MonoBehaviour
{
    public static DiplomacyManager Instance;


    public void Awake()
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
