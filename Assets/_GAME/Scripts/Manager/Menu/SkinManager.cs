using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [Header("Elements")]
    GameObject player;
    [SerializeField] private GameObject[] heroPlayerSkin;
    [SerializeField] private GameObject[] skins;
    [SerializeField] private GameObject[] skinsButton;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LoadSkinData();

    }

    public void BuySkins(int Index)
    {
        if (DataManager.instance.TryPurchaseGold(100))
        {
            PlayerPrefs.SetInt("PlayerSkin" + Index, 1);
            for (int i = 0; i < skins.Length; i++)
            {
                if (PlayerPrefs.GetInt("PlayerSkin" + i, 0) == 1)
                {
                    skinsButton[i].transform.GetChild(6).gameObject.SetActive(true);
                    skinsButton[i].transform.GetChild(5).gameObject.SetActive(true);
                    skinsButton[i].transform.GetChild(4).gameObject.SetActive(false);
                }
                else
                {
                    skinsButton[i].transform.GetChild(6).gameObject.SetActive(false);
                    skinsButton[i].transform.GetChild(5).gameObject.SetActive(false);
                    skinsButton[i].transform.GetChild(4).gameObject.SetActive(true);
                }
            }
        }

       
    }

    public void SelectSkin(int Index)
    {
        PlayerPrefs.SetInt("SelectedSkin", Index);
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].SetActive(false);
        }
        skins[Index].SetActive(true);
    }

    public void LoadSkinData()
    {
        if(!PlayerPrefs.HasKey("PlayerSkin"+0))
            BuySkins(0);

        for (int i = 0; i < skins.Length; i++)
        {
            if(PlayerPrefs.GetInt("PlayerSkin"+i,0) == 1)
            {
                skinsButton[i].transform.GetChild(6).gameObject.SetActive(true);
                skinsButton[i].transform.GetChild(5).gameObject.SetActive(true);
                skinsButton[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else
            {
                skinsButton[i].transform.GetChild(6).gameObject.SetActive(false);
                skinsButton[i].transform.GetChild(5).gameObject.SetActive(false);
                skinsButton[i].transform.GetChild(4).gameObject.SetActive(true);
            }
        }

        int selectedSkin = PlayerPrefs.GetInt("SelectedSkin", 0);
        skins[selectedSkin].SetActive(true);

    }
}
