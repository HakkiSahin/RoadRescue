using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class RoadController : MonoBehaviour
{
    [SerializeField] List<GameObject> _cars;
    public bool createCar = true;

    [NonSerialized] public List<GameObject> createdCar = new List<GameObject>();

    [NonSerialized] public int CarCrash = 0;

    [SerializeField] float carMoveRot;

    [SerializeField] private float baseSpawnTime = 3f;

    [SerializeField] private float CarSpeed = 2.5f;

    void Start()
    {
        StartCoroutine(CreateCar());
    }

    AICarController aiCarController;

    void Update()
    {
        for (int i = 0; i < createdCar.Count; i++)
        {
            aiCarController = createdCar[i].GetComponent<AICarController>();
            if (createCar)
            {
                aiCarController.move = true;

                if (createdCar[i].transform.tag == "CrashCar")
                    aiCarController.distanceTravelled += CarSpeed * 2f * Time.deltaTime;

                else aiCarController.distanceTravelled += CarSpeed * Time.deltaTime;


                createdCar[i].transform.position = aiCarController.path.path
                    .GetPointAtDistance(aiCarController.distanceTravelled);

                createdCar[i].transform.rotation = aiCarController.path.path
                    .GetRotationAtDistance(aiCarController.distanceTravelled);
            }
            else if (i < CarCrash - foo && !createCar)
            {
                aiCarController.distanceTravelled += CarSpeed * Time.deltaTime;

                createdCar[i].transform.position = aiCarController.path.path
                    .GetPointAtDistance(aiCarController.distanceTravelled);

                createdCar[i].transform.rotation = aiCarController.path.path
                    .GetRotationAtDistance(aiCarController.distanceTravelled);
            }
        }
    }

    public void ControlRoad()
    {
        createCar = true;
        for (int i = 0; i < createdCar.Count; i++)
        {
            if (createdCar[i].transform.tag == "CrashCar")
            {
                createCar = false;
                break;
            }
        }
    }


    private bool test = true;

    private int carCount = 0;

    private int foo;

    IEnumerator CreateCar()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Index:" + SceneManager.GetActiveScene().buildIndex);
            if (createCar)
            {
                float randomSpawnTime = Random.Range(baseSpawnTime * 10, (baseSpawnTime + 2) * 10) / 10;


                yield return new WaitForSeconds(randomSpawnTime);

                GameObject obj = Instantiate(_cars[Random.Range(0, _cars.Count)], transform.GetChild(0).position,
                    Quaternion.identity, transform);

                int a = Random.Range(0, obj.transform.childCount - 1);
                obj.transform.GetChild(a).gameObject.SetActive(true);
                obj.transform.GetChild(a).transform.Rotate(obj.transform.GetChild(a).transform.rotation.x,
                    obj.transform.GetChild(a).transform.rotation.y, carMoveRot < 0 ? 0 : 180);
                createdCar.Add(obj);

                if (createdCar.Count > 4 && Random.Range(0, 100) > 10 && transform.name == "Road_3" && test)
                {
                    test = false;
                    createCar = false;
                    GameObject.Find("GameManager").GetComponent<TestScript>().CloseTutorial(-1);
                    CarCrash = Random.Range(2, createdCar.Count - 2);
                    createdCar[CarCrash].transform.tag = "CrashCar";

                    createdCar[CarCrash].gameObject.layer = 0;

                    createdCar[CarCrash].transform.GetChild(createdCar[CarCrash].transform.childCount - 1).gameObject
                        .SetActive(true);

                    // createdCar[CarCrash].transform.GetComponent<Animator>().enabled = true;
                }
            }
        } // tutorials
        else
        {
            #region test

            // if (createCar)
            // {
            //     float randomSpawnTime = Random.Range(baseSpawnTime * 10, (baseSpawnTime + 2) * 10) / 10;
            //
            //
            //     yield return new WaitForSeconds(3);
            //
            //     GameObject obj = Instantiate(_cars[Random.Range(0, _cars.Count)], transform.GetChild(0).position,
            //         Quaternion.identity, transform);
            //     carCount++;
            //     int a = Random.Range(0, obj.transform.childCount - 2);
            //     obj.transform.GetChild(a).gameObject.SetActive(true);
            //     obj.transform.GetChild(a).transform.Rotate(obj.transform.GetChild(a).transform.rotation.x,
            //         obj.transform.GetChild(a).transform.rotation.y, carMoveRot < 0 ? 0 : 180);
            //     createdCar.Add(obj);
            //
            //     if (createdCar.Count > 4 && Random.Range(0, 100) > 90 - carCount)
            //     {
            //         createCar = false;
            //         CarCrash = Random.Range(1, createdCar.Count - 2);
            //         createdCar[CarCrash].transform.tag = "CrashCar";
            //         createdCar[CarCrash].gameObject.layer = 0;
            //
            //         createdCar[CarCrash].transform.GetChild(createdCar[CarCrash].transform.childCount - 1).gameObject
            //             .SetActive(true);
            //         // createdCar[CarCrash].transform.GetComponent<Animator>().enabled = true;
            //         carCount = 0;
            //     }
            // }

            #endregion

            if (createCar)
            {
                float randomSpawnTime = Random.Range(baseSpawnTime * 10, (baseSpawnTime + 2) * 10) / 10;
                yield return new WaitForSeconds(3);
                GameObject obj = Instantiate(_cars[Random.Range(0, _cars.Count)], transform.GetChild(0).position,
                    Quaternion.identity, transform);
                carCount++;
                int a = Random.Range(0, obj.transform.childCount - 2);
                obj.transform.GetChild(a).gameObject.SetActive(true);
                obj.transform.GetChild(a).transform.Rotate(obj.transform.GetChild(a).transform.rotation.x,
                    obj.transform.GetChild(a).transform.rotation.y, carMoveRot < 0 ? 0 : 180);
                createdCar.Add(obj);

                if (createdCar.Count > 4 && Random.Range(0, 100) > 90 - carCount)
                {
                    if (true) // Crush
                    {
                        foo = 0;
                        createCar = false;
                        CarCrash = Random.Range(2, createdCar.Count - 2);
                        createdCar[CarCrash].transform.tag = "CrashCar";
                        createdCar[CarCrash].gameObject.layer = 0;

                        createdCar[CarCrash].transform.GetChild(createdCar[CarCrash].transform.childCount - 1)
                            .gameObject
                            .SetActive(true);
                        createdCar[CarCrash].transform.GetChild(createdCar[CarCrash].transform.childCount - 1)
                            .GetChild(1).gameObject.SetActive(true);
                        // createdCar[CarCrash].transform.GetComponent<Animator>().enabled = true;
                        carCount = 0;
                    }
                    else // Engine Failure
                    {
                        foo = 1;
                        // createCar = false;
                        CarCrash = Random.Range(3, createdCar.Count - 2);
                        createdCar[CarCrash].transform.tag = "CrashCar";
                        createdCar[CarCrash].gameObject.layer = 0;

                        createdCar[CarCrash].transform.GetChild(createdCar[CarCrash].transform.childCount - 1)
                            .gameObject
                            .SetActive(true);
                        
                        createdCar[CarCrash].transform.GetChild(createdCar[CarCrash].transform.childCount - 1)
                            .GetChild(0).gameObject.SetActive(true);

                        // createdCar[CarCrash].transform.GetComponent<Animator>().enabled = true;
                        carCount = 0;
                    }
                }
            }
        }

        yield return null;
        StartCoroutine(CreateCar());
    }
}