using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulliganManager : MonoBehaviour
{
    public bool mulliganPhase = true;
    public List<Card> mulliganedCards = new List<Card>();
    public Transform player1Hand, player2Hand;
    public List<CardController> player1Cards = new List<CardController>(),
        player2Cards = new List<CardController>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawMulliganCards()
    {
        player1Cards.Clear();

    }
}
