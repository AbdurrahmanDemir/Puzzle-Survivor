using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasketballCharacterController : MonoBehaviour
{
    [SerializeField] private BasketballBall basketballBall;
    Animator animator;
    [Header("Elements")]
    [SerializeField] private GameObject basketball;
    [SerializeField] private Slider basketballSlider;
    [SerializeField] private TextMeshProUGUI basketballSliderText;
    int letterCount;
    int clickNumber = 1;

    [SerializeField] private int requiredLetters=10;
    [SerializeField] private TextMeshProUGUI requiredLettersText;
    [SerializeField] private TextMeshProUGUI requiredLettersFeeText;
    [SerializeField] private TextMeshProUGUI sliderRequiredLettersText;

    private void Awake()
    {
        LetterButtonManager.onTrueWord += TrueWord;
        BasketballGameManager.onBasketballGameStart += StartBasketball;

    }

    private void OnDestroy()
    {
        LetterButtonManager.onTrueWord -= TrueWord;
        BasketballGameManager.onBasketballGameStart -= StartBasketball;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        basketballSlider.maxValue = 10;
    }

    public void TrueWord(int number)
    {
        letterCount+= number;
        basketballSlider.value = letterCount;
        basketballSliderText.text = letterCount.ToString()+ " / " + requiredLetters;
        Debug.Log(letterCount);

        if (letterCount >= 8)
        {
            ThrowBall();
            letterCount = 0;
            basketballSlider.value = letterCount;
            basketballSliderText.text = letterCount.ToString()+ " / "+ requiredLetters;
        }

    }

    public void UpdateRequiredLetter()
    {
        if(requiredLetters==1)
            return; 
        
        if (DataManager.instance.TryPurchaseEnergy(10*clickNumber))
        {
            requiredLetters--;
            requiredLettersText.text = requiredLetters.ToString();
            sliderRequiredLettersText.text= $"Collect <color=red>{requiredLetters}</color> letters to score a basket";
            clickNumber++;
            requiredLettersFeeText.text = (10 * clickNumber).ToString();
        }
    }
    void ThrowBall()
    {
        basketballBall.ShootBasketball();
        basketball.SetActive(false);
        animator.Play("Throw");

    }

    void StartBasketball()
    {

    }

}
