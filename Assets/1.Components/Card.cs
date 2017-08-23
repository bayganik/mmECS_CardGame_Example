using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class Card : MonoBehaviour
{
    //
    // Individual card in a deck
    //
    // Each gameObject will get this component filled during the DealAcardSystem
    //
    // The ranking depends on the card image that holds the faces of the cards
    // in our case images start with cardindex = 0 = 2 of hearts
    //
    public int cardIndex;                   // 0 - 51 e.g. cardfaces[faceIndex];
    public int cardFaceValue = 2;           // 0 two,.. 8 ten, 9 jack, 10 queen, 11 king, 12 Ace
    public int cardSuit = 0;                // 0 heart, 1 dimond, 2 clubs, 3 spade
    public bool cardFaceShowing = false;    // card face showing?
    public bool cardIsRed = true;           // could be used in game like Solitair
    public int cardValue = 2;               // 2 two, 10 ten, 10 jack, 10 queen, 10 king, 11 Ace 
    public int cardValueExtra = 0;          // 1 Ace can also be ONE in blackjack

    public int cardStack = 0;               // Stack of cards this belongs to -1 is the deck of cards to be dealt


    private void OnEnable()
    {
        //
        // Register component with ObjectTracker
        // When attached to a GameObject (entity)
        //
        ObjectTracker.Register(this.GetType(), this.gameObject);
    }
}