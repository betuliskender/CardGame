using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EditDeckManager : MonoBehaviour
{

    public static EditDeckManager instance;
    public List<int> availableCards = new List<int>();
    public Dictionary<Card, int> chosenCards = new Dictionary<Card, int>();
    public List<Card> cards = new List<Card>();
    public CardPreviewController cardControllerPrefab;


    private void Awake()
    {
        instance = this;   
        SetupAvailableCards();
    }

    private void SetupAvailableCards()
    {
        var gridContent = GameObject.Find("GridContent");

        foreach (Card card in cards)
        {

            CardPreviewController newCard = Instantiate(cardControllerPrefab, gridContent.transform.root);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1, gridContent.transform);

        }

    }

    public void addChosenCard(Card card)
    {
        

        if(chosenCards.ContainsKey(card))
        {
            chosenCards[card] ++;
        }
        else
        {
            chosenCards.Add(card, 1);    
        }
        SetupChosenCardsView();
    }

    private void SetupChosenCardsView()
    {
        var image = GameObject.Find("Image");

        foreach (KeyValuePair<Card, int> card in chosenCards)
        {
            var GameObjectFound = GameObject.Find(card.Key.cardName);
            if (GameObjectFound != null)
            {
                TextMeshProUGUI textMeshProUGUI = GameObjectFound.GetComponent<TextMeshProUGUI>();
                textMeshProUGUI.text = card.Key.cardName + " x " + card.Value.ToString();
            }
            else
            {

            GameObject gameObject = new GameObject(card.Key.cardName);
            TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = card.Key.cardName + " x " + card.Value.ToString();
            

            gameObject.transform.SetParent(image.transform);

            }

        }
    }

    public List<Card> ChosenCardsToList(Dictionary<Card, int> chosenCards)
    {
        List<Card> cards = new List<Card>();
        foreach (KeyValuePair<Card, int> card in chosenCards)
        {
            for(int i = 0; i < card.Value; i++)
            {
                cards.Add(card.Key);
            }
        }

            return cards;
    }
}
