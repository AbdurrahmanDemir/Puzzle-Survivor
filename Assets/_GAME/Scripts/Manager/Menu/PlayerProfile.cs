using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile instance;
    [Header("Elements")]
    public GameObject profilePanel;
    public Sprite[] background;
    public Sprite[] icon;
    public Transform backgroundParent;
    public Transform iconParent;
    public Transform teamLogo;
    public GameObject menuTeamLogo;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        for (int i = 0; i < backgroundParent.childCount; i++)
        {
            backgroundParent.GetChild(i).GetComponent<Image>().sprite = background[i];
        }
        for (int i = 0; i < iconParent.childCount; i++)
        {
            iconParent.GetChild(i).GetComponent<Image>().sprite = icon[i];
        }

        LoadTeamLogo(teamLogo);
        LoadTeamLogo(menuTeamLogo.transform);
    }
    public void SelectBackground(int number)
    {
        teamLogo.GetChild(0).GetComponent<Image>().sprite = background[number];
        PlayerPrefs.SetInt("TeamLogoBack", number);
    }
    public void SelectIcon(int number)
    {
        teamLogo.GetChild(1).GetComponent<Image>().sprite = icon[number];
        PlayerPrefs.SetInt("TeamLogoIcon", number);
    }
    public void LoadTeamLogo(Transform logo)
    {
        logo.GetChild(0).GetComponent<Image>().sprite = background[PlayerPrefs.GetInt("TeamLogoBack")];
        logo.GetChild(1).GetComponent<Image>().sprite = icon[PlayerPrefs.GetInt("TeamLogoIcon")];

    }

    public void ProfilePanelOpen()
    {
        if (!profilePanel.activeSelf)
        {
            profilePanel.SetActive(true);

        }
        else
        {
            profilePanel.SetActive(false);
            LoadTeamLogo(menuTeamLogo.transform);
        }

    }
}
