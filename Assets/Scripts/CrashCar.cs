using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            other.transform.parent.GetComponent<RoadController>().CarCrash -= 1;
            other.transform.parent.GetComponent<RoadController>().createdCar.RemoveAt(0);
            Destroy(other.gameObject);
        }

        if (other.tag == "Rescue")
        {
            Destroy(other.gameObject);
        }

        if (other.transform.GetComponent<CarController>() == true)
        {
            other.transform.GetChild(0).localRotation =
                Quaternion.Euler(-90, 0, other.transform.GetChild(0).localRotation.z + 180);
        }
    }
}