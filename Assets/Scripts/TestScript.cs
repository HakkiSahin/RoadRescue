using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    public List<GameObject> cars;
    public List<GameObject> workers;

    private GameObject selectedCar;
    private GameObject selectedWorker;

    private Vector3 goPos;

    [SerializeField] private GameObject liftCar;

    [SerializeField] private TextMeshProUGUI yourMoneyText;

    [SerializeField] private GameObject workerUI;

    [SerializeField] private Transform carPanel;

    private int rescuedCar = 0;

    [SerializeField] private Transform fingers;

    // Start is called before the first frame update
    private bool test = true;

    void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("TutLevel"));
        if (PlayerPrefs.GetInt("TutLevel") == 1 && SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene("GameScene");
        }

        //Elephant.LevelStart(PlayerPrefs.GetInt("RealLevel")+1); //LevelStart
    }

    private bool goPosObject = false;

    private int panelSelect;
       int currentSS = 0;
        void ScreenShot()
        {
            if(Input.GetKeyDown(KeyCode.K)) 
            {
                ScreenCapture.CaptureScreenshot("game" + PlayerPrefs.GetInt("s") + ".png");
                PlayerPrefs.SetInt("s", PlayerPrefs.GetInt("s") +1);
                Time.timeScale=0.001f;
                Debug.Log("Selection");
            }
            if(Input.GetKeyDown(KeyCode.A))Time.timeScale=1;
        } 

    // Update is called once per frame
    void Update()
    {
    ScreenShot();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 goMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            goMousePos.x -= 0.5f;
            goMousePos.z -= 0.5f;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "CrashCar")
                {
                    try
                    {
                        selectedCar.GetComponent<CarController>().carMovePos = hit.transform.position;
                        OpenOutLine(hit.transform);
                        selectedCar.GetComponent<CarController>().carMove = true;
                        carPanel.GetChild(sCar).GetComponent<Button>().interactable = false;
                        selectedCar = null;
                        if (SceneManager.GetActiveScene().buildIndex == 0)
                        {
                            fingers.GetChild(1).gameObject.SetActive(false);
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        Debug.Log(ex.Message);
                        throw;
                    }
                }

                if (hit.transform.tag == "Rescue")
                {
                    try
                    {
                        if (hit.transform.childCount == 2)
                        {
                            selectedWorker.GetComponent<WorkerController>().workerMovePos =
                                hit.transform.GetChild(0).GetChild(2).transform.position;
                            selectedWorker.GetComponent<WorkerController>().workerMove = true;
                            workerUI.transform.GetChild(sWorker).GetComponent<Button>().interactable = false;
                            selectedWorker = null;
                            if (SceneManager.GetActiveScene().buildIndex == 0)
                            {
                                fingers.GetChild(4).gameObject.SetActive(false);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                if (hit.transform.tag == "Buyable")
                {
                    int price = int.Parse(hit.transform.GetChild(0).GetComponent<TextMeshPro>().text);
                    int yourMoney = int.Parse(yourMoneyText.text);
                    if (yourMoney >= price)
                    {
                        hit.transform.gameObject.SetActive(false);
                        liftCar.transform.GetChild(int.Parse(hit.transform.name)).gameObject.SetActive(true);
                        yourMoneyText.text = (yourMoney - price).ToString();
                        PlayerPrefs.SetInt("Lifts", int.Parse(hit.transform.name));
                        workerUI.transform.GetChild(int.Parse(hit.transform.name)).gameObject.SetActive(true);
                        workers[int.Parse(hit.transform.name)].SetActive(true);
                    }
                }
            }
        }
    }

    private void OpenOutLine(Transform hitTransform)
    {
        for (int i = 0; i < hitTransform.childCount - 1; i++)
        {
            if (hitTransform.GetChild(i).gameObject.activeSelf == true)
            {
                hitTransform.GetChild(i).GetComponent<Outline>().enabled = true;
                break;
            }
        }
    }

    private int sCar;

    public void SelectCar(int selectCar)
    {
        StartCoroutine(SelectButtonScale(carPanel.GetChild(selectCar).gameObject));

        selectedCar = cars[selectCar];
        sCar = selectCar;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            CloseTutorial(0);
        }
    }

    private int sWorker;

    public void SelectWorker(int selectWorker)
    {
        workerUI.transform.GetChild(selectWorker).GetChild(0).gameObject.SetActive(true);
        selectedWorker = workers[selectWorker];
        sWorker = selectWorker;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            CloseTutorial(3);
        }
    }

    public void ControlRescuedCar()
    {
        rescuedCar++;
        if (rescuedCar >= 10 * PlayerPrefs.GetInt("Car") + 1)
        {
            //Elephant.LevelEnd(PlayerPrefs.GetInt("RealLevel")+1); //LevelEnd here
            //PlayerPrefs.SetInt("RealLevel",PlayerPrefs.GetInt("RealLevel")+1);
            //Elephant.LevelStart(PlayerPrefs.GetInt("RealLevel")+1); //LevelStart
            rescuedCar = 0;
        }
    }

    public void CloseTutorial(int fingerIndex)
    {
        if (fingerIndex >= 0)
        {
            fingers.GetChild(fingerIndex).gameObject.SetActive(false);
        }

        fingers.GetChild(fingerIndex + 1).gameObject.SetActive(true);
    }


    public void OpenButton(int btnIndex)
    {
        carPanel.GetChild(btnIndex).GetComponent<Button>().interactable = true;
        carPanel.GetChild(btnIndex).GetChild(3).gameObject.SetActive(false);
    }

    public void OpenWorkButton(int btnIndex)
    {
        workerUI.transform.GetChild(btnIndex).GetComponent<Button>().interactable = true;
        workerUI.transform.GetChild(btnIndex).GetChild(0).gameObject.SetActive(false);
    }

    private Vector3 scale;

    IEnumerator SelectButtonScale(GameObject btn)
    {
        Vector3 scale = btn.transform.localScale;
        Vector3 defScale = scale;
        float i = 0.0f;
        float rate = (1.0f / 2) * 3;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            btn.transform.localScale = Vector3.Lerp(btn.transform.localScale, scale * 1.3f, i);
            yield return null;
        }

        i = 0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            btn.transform.localScale = Vector3.Lerp(btn.transform.localScale, defScale, i);
            yield return null;
        }

        btn.transform.GetChild(3).gameObject.SetActive(true);
    }
}
