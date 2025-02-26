using Demir.MissionSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LetterButtonManager : MonoBehaviour
{
    [SerializeField] private WordManager wordManager;

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    [SerializeField] private ObjectFlow objectFlowPlayer;
    [SerializeField] private ObjectFlow objectFlowEnergy;

    private string guess = "";
    private bool canGuess = false;
    private List<string> guessedWords = new List<string>();

    [SerializeField] private TextMeshProUGUI wordText;

    public static Action<int> onTrueWord;

    private void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            graphicRaycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult r in results)
            {
                Button btn = r.gameObject.GetComponent<Button>();
                if (btn != null && btn.CompareTag("LetterButton") && btn.interactable)
                {
                    string letter = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                    wordText.text += letter;
                    guess += letter;
                    btn.interactable = false;
                    canGuess = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && canGuess)
        {
            Debug.Log("Tahmin: " + guess);

            if (wordManager.validWords.Contains(guess) && !guessedWords.Contains(guess))
            {
                Debug.Log("Kelime doðru");
                wordText.text = "TRUE!";
                guessedWords.Add(guess);
                int guessLength = guess.Length;
                onTrueWord?.Invoke(guessLength);
                objectFlowPlayer.Amount = guessLength;
                objectFlowPlayer.Flow();
                objectFlowEnergy.Flow();

                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    Debug.Log("Tutorialda mission system çalýþmaz");
                }
                else
                {
                    Demir.MissionSystem.MissionManager.Increment(EMissionType.FindWords, 1);
                }

                    

                DataManager.instance.AddEnergy(5);

                if (guessedWords.Count == wordManager.validWords.Count)
                {
                    if (!PlayerPrefs.HasKey("TutorialWord1"))
                    {
                        PlayerPrefs.SetInt("TutorialWord1",1);
                    }

                    else if (!PlayerPrefs.HasKey("TutorialWord2") && PlayerPrefs.HasKey("TutorialWord1"))
                        PlayerPrefs.SetInt("TutorialWord2", 1);

                    else if (!PlayerPrefs.HasKey("TutorialWord3")&& PlayerPrefs.HasKey("TutorialWord2"))
                        PlayerPrefs.SetInt("TutorialWord3", 1);


                    wordManager.DisplayAllValidWords();
                }
            }
            else
            {
                Debug.Log("Kelime yanlýþ veya daha önce tahmin edilmiþ");
                wordText.text = "FALSE!";
            }

            guess = "";
            canGuess = false;
            wordText.text = "";
            ResetButtons();
        }

    }

    private void ResetButtons()
    {
        foreach (var btn in GameObject.FindGameObjectsWithTag("LetterButton"))
        {
            btn.GetComponent<Button>().interactable = true;
        }
    }


}

