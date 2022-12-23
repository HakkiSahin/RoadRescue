using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TrailersControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    bool isActive;
    private GameObject obj;
    void Update()
    {
        if (isActive)
        {
            transform.GetChild(1).rotation = new Quaternion(0, 0, 0, 0);
            obj.transform.localPosition = new Vector3(0, obj.transform.localPosition.y+1.5f,
                0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rescue")
        {
            isActive = true;
            obj = other.gameObject;
            obj.transform.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}