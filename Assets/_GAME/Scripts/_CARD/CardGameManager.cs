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
    [SerializeField] private List<Card> cardsData; // Kartlarýn isim ve ikonlarý
    public Sprite cardBack;      // Kartlarýn arka yüzü
    public Button flipButton;
    public List<GameObject> cards; // Sahnedeki kartlar

    private bool isFlipped = false;

    void Start()
    {
        // Butona týklama olayýný ekleyin
        flipButton.onClick.AddListener(OnFlipButtonClick);
    }

    void OnFlipButtonClick()
    {
        if (!isFlipped)
        {
            // Kartlarý döndürün ve rastgele bir kart verisi atayýn
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
            // Kartlarý tekrar arka yüzüne döndürün
            foreach (var card in cards)
            {
                FlipCardBack(card);
            }

            isFlipped = false;
        }
    }

    void FlipCard(GameObject card, Sprite newFace)
    {
        // Kartý döndürme animasyonu
        card.transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            card.GetComponent<Image>().sprite = newFace;
            card.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    void FlipCardBack(GameObject card)
    {
        // Kartý arka yüzüne döndürme animasyonu
        card.transform.DORotate(new Vector3(0, -90, 0), 0.25f).OnComplete(() =>
        {
            card.GetComponent<Image>().sprite = cardBack;
            card.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }

    IEnumerator CheckWinCondition()
    {
        // Animasyonlarýn tamamlanmasýný bekleyin
        yield return new WaitForSeconds(0.5f);

        Sprite firstCardIcon = cards[0].GetComponent<Image>().sprite;
        if (cards[1].GetComponent<Image>().sprite == firstCardIcon &&
            cards[2].GetComponent<Image>().sprite == firstCardIcon &&
            cards[3].GetComponent<Image>().sprite == firstCardIcon)
        {
            Debug.Log("Tebrikler, para kazandýnýz!");
        }
        else
        {
            Debug.Log("Maalesef, kaybettiniz.");
        }
    }
}
