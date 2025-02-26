using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class RunnerUIManager : MonoBehaviour
{
    public static RunnerUIManager instance;

    [SerializeField] private GameObject WordPanel;

    [Header("Elements")]
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;
    [SerializeField] private TextMeshProUGUI rewardGoldText;
    [SerializeField] private TextMeshProUGUI XPText;
    [SerializeField] private GameObject sceneLoadingPanel;
    public Slider loadingSlider;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        RunnerGameManager.onRunnerGameStart += GameStart;
        BoatGameManager.onBoatGameStart += GameStart;
        BasketballGameManager.onBasketballGameStart += GameStart;
    }

    private void OnDestroy()
    {
        RunnerGameManager.onRunnerGameStart -= GameStart;
        BoatGameManager.onBoatGameStart -= GameStart;
        BasketballGameManager.onBasketballGameStart -= GameStart;


    }


    void GameStart()
    {
        WordPanel.SetActive(false);
    }

    public void WinFinishPanelOpen(int rewardGold, int XP)
    {
        WinPanel.SetActive(true);

        WinPanel.transform.localScale = Vector3.zero;
        WinPanel.transform.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack);

        //finishPanelLevelText.text = "0";
        //DOTween.To(() => int.Parse(finishPanelLevelText.text), x => finishPanelLevelText.text = x.ToString(), levelIndex, 2f);

        rewardGoldText.text = "0";  
        DOTween.To(() => int.Parse(rewardGoldText.text), x => rewardGoldText.text = x.ToString(), rewardGold, 2f)
               .SetEase(Ease.OutQuad);

        XPText.text = "0"; 
        DOTween.To(() => int.Parse(XPText.text), x => XPText.text = x.ToString(), XP, 2f)
               .SetEase(Ease.OutQuad);  


        DataManager.instance.AddXP(XP);
        DataManager.instance.AddGold(rewardGold);
        DataManager.instance.AddEnergy(2);
    }
    public void LoseFinishPanelOpen()
    {
        LosePanel.SetActive(true);

        LosePanel.transform.localScale = Vector3.zero; 
        LosePanel.transform.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack);

        DataManager.instance.AddXP(1);
        DataManager.instance.AddGold(10);
    }

    public void MenuButton()
    {
        StartCoroutine(loadingScene("Menu"));
    }
    IEnumerator loadingScene(string arenaNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(arenaNumber);

        sceneLoadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 10f);
            loadingSlider.value = progress;
            yield return null;

        }

    }

}
