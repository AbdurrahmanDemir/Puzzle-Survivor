using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class Card
{
    public string Name;
    public Sprite Icon;
}

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private List<Card> cardsData; // Kartlar�n isim ve ikonlar�
    public Sprite cardBack;      // Kartlar�n arka y�z�
    public Button flipButton;
    public List<GameObject> cards; // Sahnedeki kartlar

    private bool isFlipped = false;

    void Start()
    {
        // Butona t�klama olay�n� ekleyin
        flipButton.onClick.AddListener(OnFlipButtonClick);
    }

    void OnFlipButtonClick()
    {
        if (!isFlipped)
        {
            // Kartlar� d�nd�r�n ve rastgele bir kart verisi atay�n
            for (int i = 0; i < cards.Count; i++)
            {
                int randomIndex = Random.Range(0, cardsData.Count);
                FlipCard(cards[i], cardsData[randomIndex].Icon);
            }

            // Kazanma durumunu kontrol edin
            StartCoroutine(CheckWinCondition());

            isFlipped = true;
        }
        else
        {
            // Kartlar� tekrar arka y�z�ne d�nd�r�n
            foreach (var card in cards)
            {
                FlipCardBack(card);
            }

            isFlipped = false;
        }
    }

    void FlipCard(GameObject card, Sprite newFace)
    {
        // Kart� d�nd�rme animasyonu
        card.transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            card.GetComponent<Image>().sprite = newFace;
            card.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    void FlipCardBack(GameObject card)
    {
        // Kart� arka y�z�ne d�nd�rme animasyonu
        card.transform.DORotate(new Vector3(0, -90, 0), 0.25f).OnComplete(() =>
        {
            card.GetComponent<Image>().sprite = cardBack;
            card.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    IEnumerator CheckWinCondition()
    {
        // Animasyonlar�n tamamlanmas�n� bekleyin
        yield return new WaitForSeconds(0.5f);

        Sprite firstCardIcon = cards[0].GetComponent<Image>().sprite;
        if (cards[1].GetComponent<Image>().sprite == firstCardIcon &&
            cards[2].GetComponent<Image>().sprite == firstCardIcon &&
            cards[3].GetComponent<Image>().sprite == firstCardIcon)
        {
            Debug.Log("Tebrikler, para kazand�n�z!");
        }
        else
        {
            Debug.Log("Maalesef, kaybettiniz.");
        }
    }
}
