using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class LiftController : MonoBehaviour
{
    [SerializeField] public Animator anim;

    [NonSerialized] public bool isActive;

    private GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.GetChild(1).rotation = new Quaternion(0, 0, 0, 0);
            obj.transform.position = new Vector3(obj.transform.position.x, anim.transform.position.y,
                obj.transform.position.z);
            obj.transform.localPosition = new Vector3(0, obj.transform.localPosition.y,
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
            anim.SetTrigger("isUp");
        }
    }
}