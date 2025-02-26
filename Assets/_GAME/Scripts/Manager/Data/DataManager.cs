using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class DataManager : MonoBehaviour
{
    public static DataManager instance;


    [Header(" Data ")]
    [SerializeField] private int gold;
    [SerializeField] private int xp;
    [SerializeField] private int energy;

    GameObject popUp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();

        DontDestroyOnLoad(gameObject);

        popUp= GameObject.FindGameObjectWithTag("PopUp");
        popUp.SetActive(false);
    }

    public bool TryPurchaseGold(int price)
    {
        if (price <= gold)
        {
            gold -= price;
            SaveData();
            UpdateGoldText();
            return true;
        }
        else
        {
            OpenPopUp(popUp, "NOT ENOUGH GOLD");
        }
        return false;
    }
    public void AddGold(int value)
    {
        gold += value;

        UpdateGoldText();

        SaveData();
    }
    public void AddXP(int value)
    {
        xp += value;

        UpdateXPText();

        SaveData();
    }
    private void UpdateGoldText()
    {
        Text coinText = GameObject.FindGameObjectWithTag("CoinText").GetComponent<Text>();
        coinText.text = gold.ToString();
    }
    private void UpdateXPText()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Text xpText = GameObject.FindGameObjectWithTag("XpText").GetComponent<Text>();
            xpText.text = xp.ToString();

        }

    }
    private void SaveData()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Energy", energy);
    }
    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            AddGold(100);
            AddEnergy(5);
        }
        xp = PlayerPrefs.GetInt("XP", 0);
        //sliderXP.maxValue = 500;
        //sliderXP.value = xp;

        energy = PlayerPrefs.GetInt("Energy", energy);


        Debug.Log("GOLD" + gold + "XP" + xp);


        SaveData();
        UpdateGoldText();
        UpdateXPText();
        UpdateEnergyText();
    }

    //energy

    public bool TryPurchaseEnergy(int price)
    {
        if (price <= energy)
        {
            energy -= price;
            SaveData();
            UpdateEnergyText();
            return true;
        }
        else
        {
            OpenPopUp(popUp, "NOT ENOUGH ENERGY");
        }
        return false;
    }
    private void UpdateEnergyText()
    {
        Text energyText = GameObject.FindGameObjectWithTag("EnergyText").GetComponent<Text>();
        energyText.text = energy.ToString();
    }
    public void AddEnergy(int value)
    {
        energy += value;

        UpdateEnergyText();

        SaveData();
    }

    public void OpenPopUp(GameObject go,string text)
    {
        if (go.activeSelf)
        {
            go.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => go.SetActive(false));
        }
        else
        {
            go.SetActive(true);
            go.transform.localScale = Vector3.zero;
            go.GetComponentInChildren<TextMeshProUGUI>().text = text;
            go.transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => StartCoroutine(ClosePanelAfterDelay(go,1.8f)));
        }
    }

    private IEnumerator ClosePanelAfterDelay(GameObject go,float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go.activeSelf)
        {
            go.transform.DOScale(Vector3.zero, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => go.SetActive(false));
        }
    }
}
