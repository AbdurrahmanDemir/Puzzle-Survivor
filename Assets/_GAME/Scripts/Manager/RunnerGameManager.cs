using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerGameManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    private int countdownTime = 3;
    [SerializeField] private Slider runSlider;
    GameObject finishLine;
    GameObject mainHero;

    public static Action onRunnerGameStart;

    [Header("Skin")]
    [SerializeField] private GameObject[] skins;

    [Header("Power Buttos")]
    [SerializeField] private TextMeshProUGUI normalSpeedText;
    [SerializeField] private TextMeshProUGUI sprintSpeedText;
    [SerializeField] private TextMeshProUGUI normalSpeedFeeText;
    [SerializeField] private TextMeshProUGUI sprintSpeedFeeText;

    int normalClickNumber = 1;
    int sprintClickNumber = 1;
    private void Awake()
    {
        FinishLine.onRunnerGameWin += GameWin;
        FinishLine.onRunnerGameLose += GameLose;
    }
    private void OnDestroy()
    {
        FinishLine.onRunnerGameWin -= GameWin;
        FinishLine.onRunnerGameLose -= GameLose;
    }

    void Start()
    {
        LoadSkinData();
        finishLine = GameObject.FindGameObjectWithTag("FinishLine");
        mainHero = GameObject.FindGameObjectWithTag("Player");
        StartCountdown();
        runSlider.maxValue = 34.4f;
        runSlider.minValue= mainHero.transform.position.z;
        UpdateVisual();

    }
    private void FixedUpdate()
    {
        runSlider.value = mainHero.transform.position.z;
    }
    public void StartCountdown()
    {
        //if (SceneManager.GetActiveScene().name == "Tutorial")
        //{
        //    if (TutorialManager.instance.GetTutorialState() == false)
        //        return;
        //}
        

        countdownText.text = countdownTime.ToString();

        // Countdown sequence
        Sequence countdownSequence = DOTween.Sequence();

        for (int i = countdownTime; i > 0; i--)
        {
            countdownSequence.Append(DOTween.To(() => countdownTime, x => countdownTime = x, i - 1, 1f)
                                              .OnUpdate(() => countdownText.text = countdownTime.ToString())
                                              .OnStart(() =>
                                              {
                                                  // Animate scale and fade out
                                                  countdownText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack);
                                                  countdownText.DOFade(0f, 0.5f).SetEase(Ease.InQuad);
                                              })
                                              .OnComplete(() =>
                                              {
                                                  // Reset scale and fade in
                                                  countdownText.transform.DOScale(1f, 0.5f).SetEase(Ease.InBack);
                                                  countdownText.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
                                              }));

            // Add a delay to show each number for 1 second
            if (i > 1)
            {
                countdownSequence.AppendInterval(1.1f);
            }
        }

        countdownSequence.AppendCallback(() =>
        {
            countdownText.text = "LET'S GO";
        });

        // Wait for 1 second and then disable the text
        countdownSequence.AppendInterval(1f).AppendCallback(() =>
        {
            countdownText.gameObject.SetActive(false);
            onRunnerGameStart?.Invoke();
        });

    }

    void GameWin()
    {
        RunnerUIManager.instance.WinFinishPanelOpen(100, 5);
    }
    void GameLose()
    {
        RunnerUIManager.instance.LoseFinishPanelOpen();
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadSkinData()
    {
        int selectedSkin = PlayerPrefs.GetInt("SelectedSkin", 0);
        skins[selectedSkin].SetActive(true);

    }

    public void IncreaseNormalSpeed()
    {
        if (DataManager.instance.TryPurchaseEnergy(normalClickNumber * 5))
        {
            mainHero.GetComponent<RunnerCharacterController>().SetMoveSpeed(0.1f);
            UpdateVisual();
            normalClickNumber++;
            normalSpeedFeeText.text= (normalClickNumber * 5).ToString();
        }
        
    }
    public void IncreaseSprintSpeed()
    {
        
        if (DataManager.instance.TryPurchaseEnergy(sprintClickNumber * 5))
        {
            mainHero.GetComponent<RunnerCharacterController>().SetBoostSpeed(0.1f);
            UpdateVisual();
            sprintClickNumber++;
            sprintSpeedFeeText.text = (sprintClickNumber * 5).ToString();
        }
    }

    void UpdateVisual()
    {
        normalSpeedText.text = mainHero.GetComponent<RunnerCharacterController>().MoveSpeed.ToString();
        sprintSpeedText.text = mainHero.GetComponent<RunnerCharacterController>().BoostSpeed.ToString();
        
    }

}
