using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DisplayCardStackSystem : MonoBehaviour 
{
    void OnEnable()
    {
        EventManager.StartListening("DisplayCardStackEvent", Handle);
    }
    void OnDisable()
    {
        EventManager.StopListening("DisplayCardStackEvent", Handle);
    }
    void Handle(string value)
    {
        int stack = Convert.ToInt32(value);

        GameObject stackObj;    //which stack to work with
        GameObject cardObj;     //current cardface(back)
        int cnt = 0;            //multiplier for offset

        if (stack == 0)
            stackObj = GameObject.Find("PlayerStack");
        else
            stackObj = GameObject.Find("DealerStack");

        Transform parentTransform = stackObj.transform;
        CardStackComponent cStack = stackObj.GetComponent<CardStackComponent>();
        //
        // Loop thru a list of card values (integers)
        //
        cnt = cStack.cardsInStack.Count;

        foreach (int cardIndex in cStack.cardsInStack)
        {
            cardObj = GameObject.Find("Card_D0" + "_C" + cardIndex.ToString());
            Vector3 temp = parentTransform.position + new Vector3(cnt * cStack.xOffset, cnt * cStack.yOffset);
            cardObj.transform.position = temp;
            Card cCard = cardObj.GetComponent<Card>();

            //
            // find what image component to disp for the Entity
            //
            SpriteRenderer srender = cardObj.GetComponent<Renderer>() as SpriteRenderer;
            if (cCard.cardFaceShowing)
                srender.sprite = CardDeckManager.GetCardFace(cardIndex);
            else
                srender.sprite = CardDeckManager.GetCardBack(cardIndex);
            //
            // reverse the order of sprites with 0 being the first card way back
            //
            srender.sortingOrder = cnt;                     
            cnt -= 1;
        }
    }

}