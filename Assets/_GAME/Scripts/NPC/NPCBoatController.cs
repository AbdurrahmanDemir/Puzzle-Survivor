using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCBoatController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boostSpeed;
    private float timer;
    private bool isGameStart = false;
    private bool isRunning = false;

    [Header("Elements")]
    [SerializeField] private ParticleSystem boostSpeedParticle;
    [SerializeField] private GameObject[] skins;
    private Animator animator;

    [Header("UI")]
    [SerializeField] private Sprite[] Flags;
    [SerializeField] private Image NPCFlag;
    [SerializeField] private string[] Names;
    [SerializeField] private TextMeshProUGUI NPCName;
    private void Awake()
    {
        BoatGameManager.onBoatGameStart += StartGame;

        FinishLine.onRunnerGameWin += FinishBoatMove;
        FinishLine.onRunnerGameLose += FinishBoatMove;
    }

    private void OnDestroy()
    {
        BoatGameManager.onBoatGameStart -= StartGame;

        FinishLine.onRunnerGameWin -= FinishBoatMove;
        FinishLine.onRunnerGameLose -= FinishBoatMove;
    }

    void Start()
    {
        int randomFlag = Random.Range(0, Flags.Length);
        NPCFlag.sprite = Flags[randomFlag];

        int randomName = Random.Range(0, Names.Length);
        NPCName.text = Names[randomName];


        animator = GetComponentInChildren<Animator>();
        int skinNumber = Random.Range(0, skins.Length);
        skins[skinNumber].SetActive(true);
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (isGameStart && !isRunning)
        {
            StartCoroutine(CharacterMoveCoroutine());
        }
    }

    void StartGame()
    {
        isGameStart = true;
        animator.Play("Run");
    }
    void FinishBoatMove()
    {
        isGameStart = false;
    }
    IEnumerator CharacterMoveCoroutine()
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
            transform.position += transform.right * -1 * currentSpeed * Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }

    IEnumerator PowerSpeed()
    {
        boostSpeedParticle.gameObject.SetActive(true);
        boostSpeedParticle.Play();
        currentSpeed = boostSpeed;
        yield return new WaitForSeconds(3);
        currentSpeed = moveSpeed;
        boostSpeedParticle.gameObject.SetActive(false);
        boostSpeedParticle.Stop();
    }

    void NPCBoostSpeed()
    {
        if (Random.Range(0, 2) < 1)
        {
            StartCoroutine(PowerSpeed());
        }
    }
}
