using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class GameStartSystem : MonoBehaviour 
{
    public TextAsset StacksLayout;
    //
    // Set this using the Editor (object where deck of cards will be located)
    //
    public GameObject deckOfCards;
    //
    // The only job for this system is to start the game
    //
    private void Start()
    {
        //
        // Initialize the card deck 
        // (Trigger InitCardDeckEvent and give number of deck=0)
        //
        //EventManager.TriggerEvent("InitCardDeckEvent", "0");
        //
        CardDeckManager.DeckLocation = deckOfCards;         //where to stack the deck
        CardDeckManager.CurrentDeckNumber = 0;              //only one deck is allowed (default)
        CardDeckManager.CurrentCardBack = 8;                //red
        CardDeckManager.DeckFanOutDistance = 0.03f;         //fan out value to right for deck

        CardDeckManager.CreateDeckOfCards();
        //
        // Find where the stacks are by reading an XML file into a datatable
        //
        //CardStacks cs = new CardStacks();
        // cs.FilePath = new StringReader(StacksLayout.text);

        CardStacks cs = new CardStacks(new StringReader(StacksLayout.text));
        //string temp = Application.streamingAssetsPath;
        //cs.FilePath = temp.Replace("StreamingAssets", "Resources/" + StacksLayout.name + ".xml");

        //cs.Init();

        //StackCollection ic = StackCollection.Load("StacksDB");
        //foreach (StackItems item in ic.stacks)
        //{
        //    print(item.name);

        //}
    }

}