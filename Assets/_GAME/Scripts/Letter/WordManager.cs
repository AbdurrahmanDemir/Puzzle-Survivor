using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour
{
    private List<string> wordList3 = new List<string>();
    private List<string> wordList4 = new List<string>();
    private List<string> wordList5 = new List<string>();
    private List<string> wordList6 = new List<string>();
    private List<string> wordList7 = new List<string>();

    private char[] selectedLetters;

    public List<string> validWords = new List<string>();

    [Header("Text Assets")]
    public TextAsset wordFile3;
    public TextAsset wordFile4;
    public TextAsset wordFile5;
    public TextAsset wordFile6;
    public TextAsset wordFile7;

    [Header("Letter")]
    [SerializeField] private GameObject letter3;
    [SerializeField] private GameObject letter4;
    [SerializeField] private GameObject letter5;
    [SerializeField] private GameObject letter6;
    [SerializeField] private GameObject letter7;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI displayText;

    void Start()
    {
        LoadWordsFromTextAsset(wordFile3, wordList3);
        LoadWordsFromTextAsset(wordFile4, wordList4);
        LoadWordsFromTextAsset(wordFile5, wordList5);
        LoadWordsFromTextAsset(wordFile6, wordList6);
        LoadWordsFromTextAsset(wordFile7, wordList7);

        GenerateRandomLettersAndWords();

        //Debug.Log("Seçilen Harfler: " + string.Join(", ", selectedLetters));
        //Debug.Log("Geçerli Kelimeler: " + string.Join(", ", validWords));

        UpdateLetterUI();
    }

    void LoadWordsFromTextAsset(TextAsset textAsset, List<string> wordList)
    {
        if (textAsset != null)
        {
            // Dosya içeriðini satýrlara böl
            string[] lines = textAsset.text.Split('\n');
            wordList.Clear();

            foreach (string line in lines)
            {
                // Boþluklarý ve satýr sonlarýný kaldýr
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    wordList.Add(trimmedLine);
                }
            }
        }
        else
        {
            Debug.LogError("TextAsset bulunamadý.");
        }
    }


    void GenerateRandomLettersAndWords()
    {
        
            if (!PlayerPrefs.HasKey("TutorialWord1"))
            {
                string tutorialRandomWord = "NET";
                selectedLetters = tutorialRandomWord.ToCharArray();
                validWords.Add("NET");
                validWords.Add("TEN");


            }

            if (PlayerPrefs.HasKey("TutorialWord1") && !PlayerPrefs.HasKey("TutorialWord2"))
            {
                string tutorialRandomWord = "WON";
                selectedLetters = tutorialRandomWord.ToCharArray();
                validWords.Add("WON");
                validWords.Add("NOW");
                validWords.Add("OWN");



            }
            if (PlayerPrefs.HasKey("TutorialWord2") && !PlayerPrefs.HasKey("TutorialWord3"))
            {
                string tutorialRandomWord = "EASY";
                selectedLetters = tutorialRandomWord.ToCharArray();
                validWords.Add("YES");
                validWords.Add("SAY");
                validWords.Add("SEA");
                validWords.Add("EASY");

            }
            else if (PlayerPrefs.HasKey("TutorialWord3"))
            {

                List<string> allWords = new List<string>();

            allWords.AddRange(wordList3);
            allWords.AddRange(wordList4);
            allWords.AddRange(wordList4);
                allWords.AddRange(wordList5);
                allWords.AddRange(wordList5);
                allWords.AddRange(wordList6);
                allWords.AddRange(wordList7);

                if (allWords.Count == 0)
                {
                    Debug.LogError("Kelime listesi boþ. Lütfen dosya yollarýný ve içeriklerini kontrol edin.");
                    return;
                }

                string randomWord = allWords[Random.Range(0, allWords.Count)];

                selectedLetters = randomWord.ToCharArray();
                selectedLetters = selectedLetters.OrderBy(x => Random.value).ToArray();

                FindValidWords();
           
        }

        

    }

    void FindValidWords()
    {
        validWords.Clear();

        List<string> allWords = new List<string>();
        allWords.AddRange(wordList3);
        allWords.AddRange(wordList4);
        allWords.AddRange(wordList5);
        allWords.AddRange(wordList6);
        allWords.AddRange(wordList7);

        foreach (string word in allWords)
        {
            if (IsWordValid(word))
            {
                validWords.Add(word);
            }
        }
    }

    bool IsWordValid(string word)
    {
        List<char> letters = selectedLetters.ToList();

        foreach (char c in word)
        {
            if (letters.Contains(c))
            {
                letters.Remove(c);
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    void UpdateLetterUI()
    {
        letter3.SetActive(false);
        letter4.SetActive(false);
        letter5.SetActive(false);
        letter6.SetActive(false);
        letter7.SetActive(false);

        switch (selectedLetters.Length)
        {
            case 3:
                letter3.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    letter3.transform.GetChild(i).
                        GetComponentInChildren<Button>().
                        GetComponentInChildren<TextMeshProUGUI>().text = selectedLetters[i].ToString();
                }
                break;
            case 4:
                letter4.SetActive(true);
                for (int i = 0; i < 4; i++)
                {
                    letter4.transform.GetChild(i).
                        GetComponentInChildren<Button>().
                        GetComponentInChildren<TextMeshProUGUI>().text = selectedLetters[i].ToString();
                }
                break;
            case 5:
                letter5.SetActive(true);
                for (int i = 0; i < 5; i++)
                {
                    letter5.transform.GetChild(i).
                        GetComponentInChildren<Button>().
                        GetComponentInChildren<TextMeshProUGUI>().text = selectedLetters[i].ToString();
                }
                break;
            case 6:
                letter6.SetActive(true);
                for (int i = 0; i < 6; i++)
                {
                    letter6.transform.GetChild(i).
                        GetComponentInChildren<Button>().
                        GetComponentInChildren<TextMeshProUGUI>().text = selectedLetters[i].ToString();
                }
                break;
            case 7:
                letter7.SetActive(true);
                for (int i = 0; i < 7; i++)
                {
                    letter7.transform.GetChild(i).
                        GetComponentInChildren<Button>().
                        GetComponentInChildren<TextMeshProUGUI>().text = selectedLetters[i].ToString();
                }
                break;
        }
    }

    // Yeni kelime seçmek için metod
    public void ChangeSelectedWord()
    {
        if (DataManager.instance.TryPurchaseGold(10))
        {
            GenerateRandomLettersAndWords();
            UpdateLetterUI();
        }
    }

    // Geçerli kelimelerden rastgele birini görüntülemek için metod
    public void DisplayRandomValidWord()
    {
        if (DataManager.instance.TryPurchaseGold(50))
        {
            if (validWords.Count > 0)
            {
                string randomValidWord = validWords[Random.Range(0, validWords.Count)];
                displayText.text = "" + randomValidWord;
            }
            else
            {
                displayText.text = "Geçerli kelime yok.";
            }
        }
    }

    public void DisplayAllValidWords()
    {
        if (validWords.Count > 0)
        {
            displayText.text = "Geçerli Kelimeler: " + string.Join(", ", validWords);
        }
        else
        {
            displayText.text = "Geçerli kelime yok.";
        }

        // Yeni kelime seçimi
        GenerateRandomLettersAndWords();
        UpdateLetterUI();
    }
}
