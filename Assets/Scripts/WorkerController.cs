using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class WorkerController : MonoBehaviour
{
    [NonSerialized] public bool workerMove;

    [NonSerialized] public Vector3 workerMovePos;
    [SerializeField] private float workerWaitTime = 10f;
    [SerializeField] private GameObject _garrage;
    private NavMeshAgent _agent;
    private Transform startPos;
    [SerializeField] private Transform rescuePos;

    [SerializeField] private int repairMoney;
    [SerializeField] private TextMeshProUGUI moneyText;

    private Animator anim;

    [SerializeField] private GameObject animPanel;

    [SerializeField] private int workerIndex;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        startPos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (workerMove)
        {
            GoToCrashCar(workerMovePos);
        }
    }

    public void GoToCrashCar(Vector3 movePos)
    {
        anim.SetTrigger("isRun");
        _agent.SetDestination(new Vector3(movePos.x, transform.position.y, movePos.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rescue")
        {
            workerMove = false;

            if (other.transform.GetComponent<NavMeshAgent>() != null &&
                !other.transform.GetComponent<AICarController>().isRepair)
            {
                other.transform.GetComponent<NavMeshAgent>().enabled = false;
                other.transform.GetComponent<AICarController>().isRepair = true;
                StartCoroutine(WaitForConnect(other.transform));
                anim.SetTrigger("isRepair");
            }
        }
    }

    IEnumerator WaitForConnect(Transform trans)
    {
        
        
        yield return new WaitForSeconds(workerWaitTime);
        GameObject.Find("GameManager").GetComponent<TestScript>().OpenWorkButton(workerIndex);

        moneyText.text = (int.Parse(moneyText.text) + repairMoney).ToString();
        PlayerPrefs.SetInt("Money", int.Parse(moneyText.text));

        trans.parent.GetComponent<LiftController>().anim.SetTrigger("isDown");

        trans.parent.GetComponent<LiftController>().isActive = false;

        trans.transform.position = new Vector3(trans.transform.position.x, 0.4f, trans.transform.position.z);

        trans.GetComponent<NavMeshAgent>().enabled = true;
        trans.GetComponent<NavMeshAgent>().SetDestination(rescuePos.position);
        FollowerPool._pool.ActivateDeactivate(true, trans.transform.position, 3);
        workerMovePos = _garrage.transform.position;
        workerMove = true;
        anim.SetTrigger("isIdle");

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(NextLevel());
            PlayerPrefs.SetInt("TutLevel", 1);
        }
    }

    IEnumerator NextLevel()
    {
        animPanel.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("GameScene");
    }
}