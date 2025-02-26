using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using Demir.MissionSystem;

public class WordFinderManager : MonoBehaviour
{
    private int currentWordIndex;
    private List<string> allWords = new List<string>();

    [SerializeField] private TextAsset wordTextFile; // TextAsset olarak dosya yükleme
    [SerializeField] private Transform letterPanel, keyboardPanel;
    public GameObject letterPrefab;
    public TextMeshProUGUI remainingWordText;
    public int remainingWordCount;


    [SerializeField] private TextMeshProUGUI openLetterFeeText;
    [SerializeField] private TextMeshProUGUI OpenAllLetterFeeText;

    private List<string> correctWordLetters = new List<string>(); // Doðru kelime harfleri
    private List<string> selectableLetters = new List<string>(); // Seçilecek harfler

    [SerializeField] private ObjectFlow playerObjectFlow;
    [SerializeField] private ObjectFlow energyObjectFlow;

    public static Action onWordCompleted;

    [SerializeField] private GameObject finderTutorial;
    [SerializeField] private GameObject wrongLetterPanel;

    private void Start()
    {
        currentWordIndex = 0;
        LoadWordsFromTextAsset(); // Kelimeleri TextAsset'ten yükle
        OpenWord();
        if (!PlayerPrefs.HasKey("FinderTutorial"))
            finderTutorial.SetActive(true);
        else
            finderTutorial.SetActive(false);

    }

    private void LoadWordsFromTextAsset()
    {
        if (wordTextFile != null)
        {
            allWords = wordTextFile.text.Split('\n').Select(word => word.Trim()).ToList();
            allWords.RemoveAll(string.IsNullOrWhiteSpace); // Boþ satýrlarý kaldýr
        }
        else
        {
            Debug.LogError("TextAsset dosyasý atanmamýþ.");
        }
    }

    private void OpenWord()
    {
        if (allWords.Count == 0)
        {
            Debug.LogError("Kelime listesi boþ.");
            return;
        }

        string selectedWord = allWords[Random.Range(0, allWords.Count)];
        correctWordLetters = selectedWord.ToUpper().ToList().ConvertAll(c => c.ToString());

        foreach (string letter in correctWordLetters)
        {
            GameObject letterObject = Instantiate(letterPrefab);
            letterObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = letter;
            letterObject.transform.GetChild(0).gameObject.SetActive(false);
            letterObject.transform.SetParent(letterPanel, false);
        }

        selectableLetters = correctWordLetters.Concat(GenerateRandomLetters(1)).OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < keyboardPanel.childCount; i++)
        {
            if (i < selectableLetters.Count)
            {
                keyboardPanel.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = selectableLetters[i];
            }
            else
            {
                keyboardPanel.GetChild(i).gameObject.SetActive(false);
            }
        }

        StartCoroutine(RevealTrueLetters());
        StartCoroutine(EnableKeyboard());
    }

    private List<string> GenerateRandomLetters(int count)
    {
        List<string> randomLetters = new List<string>();
        for (int i = 0; i < count; i++)
        {
            char randomChar;
            do
            {
                randomChar = (char)('A' + Random.Range(0, 26));
            } while (correctWordLetters.Contains(randomChar.ToString()));

            randomLetters.Add(randomChar.ToString());
        }
        return randomLetters;
    }

    private IEnumerator RevealTrueLetters()
    {
        int revealedCount = 0;
        while (revealedCount < correctWordLetters.Count)
        {
            yield return new WaitForSeconds(.2f);
            revealedCount++;
        }
    }

    private IEnumerator EnableKeyboard()
    {
        int enabledCount = 0;
        while (enabledCount < keyboardPanel.childCount)
        {
            if (enabledCount < selectableLetters.Count)
            {
                // Animasyonlarý buraya ekleyebilirsiniz.
            }
            yield return new WaitForSeconds(.2f);
            enabledCount++;
        }

        for (int i = 0; i < keyboardPanel.childCount; i++)
        {
            if (i < selectableLetters.Count)
            {
                keyboardPanel.GetChild(i).GetComponent<Button>().enabled = true;
            }
        }
    }

    public void OnKeyboardButtonClick(string incomingLetter)
    {
        if (correctWordLetters.Contains(incomingLetter))
        {
            foreach (Transform letterTransform in letterPanel)
            {
                if (!letterTransform.GetChild(0).gameObject.activeInHierarchy)
                {
                    if (incomingLetter == letterTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
                    {
                        letterTransform.GetChild(0).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        else
        {
            DataManager.instance.AddGold(-10);
            StartCoroutine(WrongLetter());
        }

        if (AreAllLettersRevealed())
        {
            LoadNextWord();
            playerObjectFlow.Flow();
            energyObjectFlow.Flow();
            onWordCompleted?.Invoke();
            Demir.MissionSystem.MissionManager.Increment(EMissionType.FindWords, 1);
        }
    }
    IEnumerator WrongLetter()
    {
        wrongLetterPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        wrongLetterPanel.SetActive(false);
    }
    private void LoadNextWord()
    {
        currentWordIndex++;
        selectableLetters.Clear();
        correctWordLetters.Clear();

        if (currentWordIndex < allWords.Count)
        {
            foreach (Transform child in letterPanel)
            {
                Destroy(child.gameObject);
            }
            OpenWord();
        }
        else
        {
            // Tüm kelimeler bittiðinde yapýlacak iþlemler
        }
    }

    private bool AreAllLettersRevealed()
    {
        return letterPanel.childCount == correctWordLetters.Count &&
               letterPanel.Cast<Transform>().All(t => t.GetChild(0).gameObject.activeInHierarchy);
    }

    public void RevealRandomLetter()
    {
        if (DataManager.instance.TryPurchaseGold(10))
        {

            List<Transform> unrevealedLetters = letterPanel.Cast<Transform>()
                                                      .Where(t => !t.GetChild(0).gameObject.activeInHierarchy)
                                                      .ToList();
            if (unrevealedLetters.Any())
            {
                Transform randomLetter = unrevealedLetters[Random.Range(0, unrevealedLetters.Count)];
                randomLetter.GetChild(0).gameObject.SetActive(true);
            }

            if (AreAllLettersRevealed())
            {
                LoadNextWord();
            }

        }
    }

    public void RevealAllLetters()
    {
        if (DataManager.instance.TryPurchaseGold(10))
        {
            foreach (Transform letterTransform in letterPanel)
            {
                letterTransform.GetChild(0).gameObject.SetActive(true);
            }

            if (AreAllLettersRevealed())
            {
                LoadNextWord();
            }
        }
    }

    public void ClickButton()
    {
        string letter = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        OnKeyboardButtonClick(letter);
    }

    public void TutorialOff()
    {
        finderTutorial.SetActive(false);
        PlayerPrefs.SetInt("FinderTutorial", 1);

    }
}
