using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasketballGameManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    private int countdownTime = 3;
    public static Action onBasketballGameStart;

    [Header("Match Settings")]
    int playerScore;
    int NPCScore;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI NPCText;
    

    [Header("Skin")]
    [SerializeField] private GameObject[] skins;

    public static Action onBasketGameFinish;
    private void Awake()
    {
        FinishLine.onPlayerBasket += PlayerBasket;
        FinishLine.onNPCBasket += NPCBasket;
    }
    private void OnDestroy()
    {
        FinishLine.onPlayerBasket -= PlayerBasket;
        FinishLine.onNPCBasket -= NPCBasket;
    }

    void Start()
    {
        LoadSkinData();
    
        StartCountdown();
    }
    void StartCountdown()
    {
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
            countdownText.text = "LET'S GO!";
            onBasketballGameStart.Invoke();
        });

        // Wait for 1 second and then disable the text
        countdownSequence.AppendInterval(1f).AppendCallback(() =>
        {
            countdownText.gameObject.SetActive(false);
        });

    }

    void PlayerBasket()
    {
        playerScore++;
        playerText.text = playerScore.ToString();

        if (playerScore >= 5)
            GameWin();
            
    }
    void NPCBasket()
    {
        NPCScore++;
        NPCText.text = NPCScore.ToString();

        if (NPCScore >= 5)
            GameLose();
    }

    void GameWin()
    {
        RunnerUIManager.instance.WinFinishPanelOpen(200, 10);
        onBasketGameFinish?.Invoke();
    }
    void GameLose()
    {
        RunnerUIManager.instance.LoseFinishPanelOpen();
        onBasketGameFinish?.Invoke();

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
}
