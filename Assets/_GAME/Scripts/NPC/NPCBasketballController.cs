using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCBasketballController : MonoBehaviour
{
    [Header("Settings")]
    private float timer;
    private bool isGameStart = false;
    private bool isRunning = false;

    [Header("Elements")]
    [SerializeField] private GameObject[] skins;
    private Animator animator;
    [SerializeField] private GameObject basketball;
    [SerializeField] private BasketballBall basketballBall;

    [Header("UI")]
    [SerializeField] private Sprite[] Flags;
    [SerializeField] private Image NPCFlag;
    [SerializeField] private string[] Names;
    [SerializeField] private TextMeshProUGUI NPCName;

    private void Awake()
    {
        BasketballGameManager.onBasketballGameStart += StartGame;
        BasketballGameManager.onBasketGameFinish += FinishGame;
    }

    private void OnDestroy()
    {
        BasketballGameManager.onBasketballGameStart -= StartGame;
        BasketballGameManager.onBasketGameFinish -= FinishGame;

    }

    void Start()
    {
        int randomFlag = Random.Range(0, Flags.Length);
        NPCFlag.sprite = Flags[randomFlag];

        int randomName = Random.Range(0, Names.Length);
        NPCName.text = Names[randomName];

        int skinNumber = Random.Range(0, skins.Length);
        skins[skinNumber].SetActive(true);
       
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGameStart && !isRunning)
        {
            StartCoroutine(NPCBasketShoot());
        }
    }

    void StartGame()
    {
        isGameStart = true;
    }
    void FinishGame()
    {
        isGameStart = false;
    }

    IEnumerator NPCBasketShoot()
    {
        isRunning = true;
        while (isGameStart)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                NPCBoostSpeed();
                timer = 0;
            }


            yield return null; // Wait for the next frame
        }
    }

    IEnumerator PowerSpeed()
    {
        basketballBall.ShootBasketball();
        basketball.SetActive(false);
        animator.Play("Throw");
        yield return new WaitForSeconds(2f);
        basketball.SetActive(true);
    }

    void NPCBoostSpeed()
    {
        if (Random.Range(0, 3) < 1)
        {
            StartCoroutine(PowerSpeed());
        }
    }
}
