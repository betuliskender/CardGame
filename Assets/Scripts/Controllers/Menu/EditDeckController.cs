using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditDeckController : MonoBehaviour
{
    public static EditDeckController instance;

    private void Awake()
    {
        instance = this;  
    }

    
}
