using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    
    //Main Save Side
    [SerializeField] private GameObject _rescueCars;
    [SerializeField] private GameObject _carUI;

    //Garrage Save Side
    [SerializeField] private GameObject _workers;
    [SerializeField] private GameObject _lifts;
    [SerializeField] private GameObject _workersUI;

    [SerializeField] private GameObject _liftBuy;
   
    void Start()
    {
        moneyText.text = PlayerPrefs.GetInt("Money") >= 0 ? PlayerPrefs.GetInt("Money").ToString() : "0";
        LoadData();
    }

    public void LoadData()
    {
        for (int i = 1; i < PlayerPrefs.GetInt("Lifts")+1; i++)
        {
            _workers.transform.GetChild(i).gameObject.SetActive(true);
            _lifts.transform.GetChild(i).gameObject.SetActive(true);
            _workersUI.transform.GetChild(i).gameObject.SetActive(true);
            _liftBuy.transform.GetChild(i-1).gameObject.SetActive(false);
        }

        for (int i = 1; i < PlayerPrefs.GetInt("Car")+1; i++)
        {
            _rescueCars.transform.GetChild(i-1).gameObject.SetActive(true);
            _carUI.transform.GetChild(i).gameObject.SetActive(true);
            _carUI.transform.GetChild(i+3).gameObject.SetActive(false);
        }
    }


    public void BuyCar(int carSelect)
    {
        Debug.Log(int.Parse(_carUI.transform.GetChild(carSelect + 3).GetChild(0).GetChild(0)
            .GetComponent<TextMeshProUGUI>().text));
        if (int.Parse(moneyText.text) > int.Parse(_carUI.transform.GetChild(carSelect+3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text))
        {
            _carUI.transform.GetChild(carSelect).gameObject.SetActive(true);
            _rescueCars.transform.GetChild(carSelect-1).gameObject.SetActive(true);
            _carUI.transform.GetChild(carSelect+3).gameObject.SetActive(false);
            
            moneyText.text = (int.Parse(moneyText.text) -
                              int.Parse(_carUI.transform.GetChild(carSelect + 3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text))
                                  .ToString();
            PlayerPrefs.SetInt("Car",carSelect);
        }
    }
}
