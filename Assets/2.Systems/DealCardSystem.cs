using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DealCardSystem : MonoBehaviour 
{
    void OnEnable()
    {
        EventManager.StartListening("DealCardEvent", Handle);
    }
    void OnDisable()
    {
        EventManager.StopListening("DealCardEvent", Handle);
    }
    void Handle(string value)
    {
        int stack = Convert.ToInt32(value);

        //
        // We'll put a card under the stack that is requesting a card
        //
        int cardIndex;
        GameObject stackObj;
        GameObject cardObj;

        if (stack == 0)
            stackObj = GameObject.Find("PlayerStack");
        else
            stackObj = GameObject.Find("DealerStack");

        CardStackComponent cardStack = stackObj.GetComponent<CardStackComponent>();
        //
        // Add a card and sort the list so cards will appear in suit order
        //
        cardIndex = CardDeckManager.GetACard();
        //
        // end of deck?
        //
        if (cardIndex < 0)
            return;

        cardStack.cardsInStack.Add(cardIndex);
        //
        // To do Descending, first sort Ascending, then reverse the list
        //
        cardStack.cardsInStack.Sort();
        cardStack.cardsInStack.Reverse();
        //
        // Get the card obj
        //
        cardObj = CardDeckManager.GetCardFaceDown(cardIndex);

        Card cCard = cardObj.GetComponent<Card>();
        cCard.cardStack = stack;                        //this will tell us which stack holds this card
        cCard.cardFaceShowing = cardStack.faceUp;
        //
        // Determine where the card goes on screen
        //
        cardObj.transform.parent = stackObj.transform;
        cardObj.transform.localPosition = new Vector2(stackObj.transform.position.x + .07f, 0);
        //
        // trigger display of stack (if any are face up, then they will display)
        //
        EventManager.TriggerEvent("DisplayCardStackEvent", stack.ToString());
    }

}