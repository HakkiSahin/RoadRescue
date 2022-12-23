using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AICarController : MonoBehaviour
{
    public PathCreator path;
    float speed = 2f;

    public float distanceTravelled;

    public bool move = false;

    private float time = 0f;

    [SerializeField] private float waitParkTime;

    [NonSerialized] public bool isRepair = false;

    [NonSerialized] public Transform followItem;

    // Start is called before the first frame update

    public bool moveAnim;


    void Start()
    {
        path = transform.parent.GetComponent<PathCreator>();
    }

    private void Update()
    {
        if (moveAnim)
        {
            FollowPosRot();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Park")
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
        }

        if (other.tag == "CrashCar")
        {
            if (other.transform.parent == transform.parent)
            {
                other.transform.parent.GetComponent<RoadController>().createCar = false;
                transform.tag = "CrashCar";
                gameObject.layer = 0;

                this.transform.GetChild(transform.childCount - 1).GetChild(2).gameObject.SetActive(true);
                other.transform.GetChild(transform.childCount - 1).GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Park")
        {
            time += Time.deltaTime;
            // turn seconds in float to int
            Debug.Log((int)(time % 60));
            if (time > waitParkTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Park")
        {
        }
    }

    // Update is called once per frame

    public void FollowPosRot()
    {
        if (Vector3.Distance(transform.position, followItem.position) >= .75f)
        {
            transform.position = Vector3.Lerp(transform.position, followItem.position, Time.deltaTime * 5f);
        }
        else
        {
            move = false;
            Debug.Log(moveAnim);
        }

        // transform.rotation = Quaternion.Lerp(transform.rotation, followItem.rotation, Time.deltaTime * 5f);
    }
}