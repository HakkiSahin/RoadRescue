using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GarrageController : MonoBehaviour
{
    private int carCount = 0;
    [SerializeField] private List<Transform> camPostions;

    public GameObject camera;


    [SerializeField] private List<GameObject> panels;

    private List<GameObject> rescueCar;

    [SerializeField] private List<Transform> garrageRescuePos;

    [SerializeField] private GameObject car;

    [SerializeField] private bool isCarCreate = true;

    [SerializeField] private Transform parkAreas;

    [SerializeField] private List<GameObject> _cameras;

    [SerializeField] private GameObject switctEffect;

    [SerializeField] private List<GameObject> buttons;

    [SerializeField] private GameObject smoke;


    [SerializeField] private TextMeshProUGUI liftCountText;
    [SerializeField] private Transform liftCount;

    // Start is called before the first frame update
    void Start()
    {
        rescueCar = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GoGarrage();
        }

        CountLift();
        CreateRescueCar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CrashCar")
        {
            carCount++;
            Instantiate(smoke, other.transform.position, Quaternion.identity);
            other.transform.localScale = Vector3.one;
            other.transform.parent.GetComponent<CarController>().TurnCar();
            other.GetComponent<AICarController>().moveAnim = false;
            other.transform.GetChild(other.transform.childCount - 1).gameObject.SetActive(true);
            other.transform.parent = parkAreas;
            other.gameObject.layer = 2;
            other.transform.tag = "Rescue";
            other.transform.localPosition = new Vector3(1, 0, 0);
            other.transform.localScale *= 1.75f;

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                GameObject.Find("GameManager").GetComponent<TestScript>().CloseTutorial(1);
            }
        }
    }


    public void GoGarrage()
    {
        StartCoroutine(SwitchCam(true));
        GameObject.Find("GameManager").GetComponent<TestScript>().CloseTutorial(2);


        // camera.transform.position = camPostions[1].position;
    }

    int testCount = 0;

    private void CountLift()
    {
        testCount = 0;
        for (int i = 0; i < liftCount.childCount; i++)
        {
            if (liftCount.GetChild(i).childCount >= 2)
            {
                testCount++;
            }
        }

        liftCountText.text = testCount.ToString();
    }

    private void CreateRescueCar()
    {
        if (parkAreas.childCount > 0)
        {
            for (int i = 0; i < garrageRescuePos.Count; i++)
            {
                if (garrageRescuePos[i].gameObject.activeInHierarchy && garrageRescuePos[i].childCount <= 1)
                {
                    CountLift();
                    parkAreas.GetChild(0).GetComponent<NavMeshAgent>().enabled = true;
                    parkAreas.GetChild(0).GetComponent<NavMeshAgent>().SetDestination(garrageRescuePos[i].position);
                    parkAreas.GetChild(0).parent = garrageRescuePos[i];
                }
            }
        }
    }

    public void GoBase()
    {
        StartCoroutine(SwitchCam(false));
    }

    public void GoGarrageButton()
    {
        GoGarrage();
    }


    IEnumerator SwitchCam(bool switchCount)
    {
        switctEffect.SetActive(true);
        yield return new WaitForSeconds(.3f);
        if (switchCount)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
            _cameras[0].SetActive(false);
            _cameras[1].SetActive(true);
            panels[1].SetActive(true);
            panels[0].SetActive(false);
        }
        else
        {
            buttons[1].SetActive(true);
            buttons[0].SetActive(false);
            _cameras[1].SetActive(false);
            _cameras[0].SetActive(true);
            panels[0].SetActive(true);
            panels[1].SetActive(false);
        }

        yield return new WaitForSeconds(.3f);
        switctEffect.SetActive(false);
    }
}