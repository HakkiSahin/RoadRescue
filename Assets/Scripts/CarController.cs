using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [NonSerialized] public bool carMove;
    [NonSerialized] public Vector3 carMovePos;
    [SerializeField] private GameObject _garrage;
    [SerializeField] private List<NavMeshAgent> _agents;
    private int maxCarryCount = 0;

    [SerializeField] private int rescueMoney;

    [SerializeField] private TextMeshProUGUI moneyText;

    [NonSerialized] public GameObject road;

    [SerializeField] private int carIndex;

    private int activeAgent = 0;

    public bool test;
    // Start is called before the first frame update

    public Transform parentObj;

    private TestScript testScript;

    [SerializeField] private Transform spawnPos;

    [SerializeField] private Animator anim;
    [SerializeField] private Transform animObject;

    void Start()
    {
        testScript = GameObject.Find("GameManager").GetComponent<TestScript>();
        SwapAgent(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (carMove)
        {
            GoToCrashCar(carMovePos);
        }

        if (transform.childCount > 0)
        {
            transform.GetChild(0).transform.localPosition = Vector3.back;
        }
    }


    public void GoToCrashCar(Vector3 movePos)
    {
        _agents[activeAgent].SetDestination(new Vector3(movePos.x, transform.position.y, movePos.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CrashCar" && other.transform.parent != transform && maxCarryCount < 1)
        {
            maxCarryCount++;
            StartCoroutine(WaitForConnect(other.transform));
        }

        if (other.tag == "Garrage")
        {
        }
    }

    public void TurnCar()
    {
        GameObject.Find("GameManager").GetComponent<TestScript>().OpenButton(carIndex);

        // transform.parent.transform.position = spawnPos.position;
        if (anim != null)
        {
            anim.enabled = false;
        }


        carMovePos = transform.position;
        transform.GetChild(0).localRotation =
            Quaternion.Euler(-90, 0, 0);
        
        
        maxCarryCount = 0;
        activeAgent = 0;
        SwapAgent(0);
    }

    IEnumerator WaitForConnect(Transform trans)
    {
        if (anim != null)
        {
            anim.GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(1f);

            trans.GetComponent<AICarController>().followItem = animObject;
            trans.GetComponent<AICarController>().moveAnim = true;
            //trans.transform.parent = animObject.transform;
        }

        yield return new WaitForSeconds(2f);
        testScript.ControlRescuedCar();
        CloseOutLine(trans);
        activeAgent = 1;
        SwapAgent(1);
        moneyText.text = (int.Parse(moneyText.text) + rescueMoney).ToString();
        PlayerPrefs.SetInt("Money", int.Parse(moneyText.text));
        carMovePos = _garrage.transform.position;
        RoadController obj = trans.parent.GetComponent<RoadController>();
        
        trans.parent.GetComponent<RoadController>().createdCar.RemoveAt(0);
        trans.transform.GetChild(trans.transform.childCount - 1).gameObject.SetActive(false);
        trans.transform.parent = transform;
        obj.ControlRoad();
    }

    private void CloseOutLine(Transform trans)
    {
        for (int i = 0; i < trans.childCount - 1; i++)
        {
            if (trans.GetChild(i).gameObject.activeSelf)
            {
                trans.GetChild(i).GetComponent<Outline>().enabled = false;
                break;
            }
        }
    }


    void SwapAgent(int swapIndex)
    {
        _agents[swapIndex].gameObject.SetActive(true);
        transform.parent = parentObj;
        _agents[swapIndex == 1 ? 0 : 1].transform.parent = transform;
        _agents[swapIndex].transform.parent = parentObj;
        transform.parent = _agents[swapIndex].transform;
        _agents[swapIndex == 1 ? 0 : 1].gameObject.SetActive(false);
    }
}