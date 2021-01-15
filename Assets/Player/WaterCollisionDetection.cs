using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisionDetection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GetComponentInParent<PlayerController>().InWater(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        GetComponentInParent<PlayerController>().InWater(false);
    }
}
