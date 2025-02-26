using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float boostSpeed;
    private bool isGameStart = false;
    private bool isRunning = false;

    [Header("Elements")]
    [SerializeField] private ParticleSystem boostSpeedParticle;
    private Animator animator;

    private void Awake()
    {
        WordFinderManager.onWordCompleted += TrueWord;
        BoatGameManager.onBoatGameStart += StartBoatMove;
        FinishLine.onRunnerGameWin += FinishBoatMove;
        FinishLine.onRunnerGameLose += FinishBoatMove;
    }

    private void OnDestroy()
    {
        WordFinderManager.onWordCompleted -= TrueWord;
        BoatGameManager.onBoatGameStart -= StartBoatMove;
        FinishLine.onRunnerGameWin -= FinishBoatMove;
        FinishLine.onRunnerGameLose -= FinishBoatMove;


    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentSpeed = moveSpeed;
    }

    void Update()
    {
        if (isGameStart && !isRunning)
        {
            StartCoroutine(BoatMoveCoroutine());
        }
    }

    void StartBoatMove()
    {
        isGameStart = true;
        animator.Play("Run");
    }
    void FinishBoatMove()
    {
        isGameStart = false;
    }
    IEnumerator BoatMoveCoroutine()
    {
        isRunning = true;
        while (isGameStart)
        {
            transform.position += transform.right * -1 * currentSpeed * Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }

    IEnumerator PowerSpeed(int speed)
    {
        boostSpeedParticle.gameObject.SetActive(true);
        boostSpeedParticle.Play();
        currentSpeed = boostSpeed + speed * 0.1f;
        yield return new WaitForSeconds(3);
        currentSpeed = moveSpeed;
        boostSpeedParticle.gameObject.SetActive(false);
        boostSpeedParticle.Stop();
    }

    void TrueWord()
    {
        StartCoroutine(PowerSpeed(3));
    }
}
