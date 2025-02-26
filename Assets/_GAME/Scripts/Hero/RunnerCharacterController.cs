using System.Collections;
using UnityEngine;

public class RunnerCharacterController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    [SerializeField] private float boostSpeed;
    public float BoostSpeed => boostSpeed;
    private bool isGameStart = false;
    private bool isRunning = false;
    private Coroutine powerSpeedCoroutine = null;

    [Header("Elements")]
    [SerializeField] private ParticleSystem boostSpeedParticle;
    private Animator animator;

    private void Awake()
    {
        LetterButtonManager.onTrueWord += TrueWord;
        RunnerGameManager.onRunnerGameStart += StartCharacterMove;
        FinishLine.onRunnerGameWin += FinishCharacterMove;
        FinishLine.onRunnerGameLose += FinishCharacterMove;
    }

    private void OnDestroy()
    {
        LetterButtonManager.onTrueWord -= TrueWord;
        RunnerGameManager.onRunnerGameStart -= StartCharacterMove;
        FinishLine.onRunnerGameWin -= FinishCharacterMove;
        FinishLine.onRunnerGameLose -= FinishCharacterMove;
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
            StartCoroutine(CharacterMoveCoroutine());
        }
    }

    void StartCharacterMove()
    {
        isGameStart = true;
        animator.Play("Run");
    }

    void FinishCharacterMove()
    {
        isGameStart = false;
    }

    IEnumerator CharacterMoveCoroutine()
    {
        isRunning = true;
        while (isGameStart)
        {
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
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
        powerSpeedCoroutine = null; // Reset the coroutine reference
    }

    void TrueWord(int guess)
    {
        if (powerSpeedCoroutine != null)
        {
            StopCoroutine(powerSpeedCoroutine); // Stop the existing coroutine
        }
        powerSpeedCoroutine = StartCoroutine(PowerSpeed(guess)); // Start a new coroutine
    }

    public float SetMoveSpeed(float speed)
    {
        moveSpeed += speed;
        currentSpeed = moveSpeed;
        return moveSpeed;
    }

    public float SetBoostSpeed(float speed)
    {
        boostSpeed += speed;
        return boostSpeed;
    }
}
