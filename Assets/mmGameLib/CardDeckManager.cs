using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * This static manager deals with playing cards.  It handles one deck of card
 * Tracking its faces, values, colors and suits.
 * It uses "CardPrefab" in Resources folder
 * It uses "CardDeck_72X100" card image in Images folder (that is broken up into piecs)
 * 
 */
public class CardDeckManager
{
    static object syncRoot = new System.Object();       //object for locking
    static CardDeckManager _instance;
    //
    // One deck of cards
    //
    // deckTotal gives the bounds of cardsInDeck
    // currentDeckNumber is zero based, so 0 is deck one
    // currentCardNumber is zero based, so 0 is 2 of hearts (in our image)
    //
    private List<StacksItems> cardStacks;       // card stack definitions
    private GameObject[] cardDeck;              // card objects
    private int[] cardDeckPointer;              // shuffeled pointer for cardDeck
    private int deckTotal = 1;                  // total number of decks
    private GameObject _deckLocation;           // object in the game that is location of deck of cards

    private int _currentDeckNumber = 0;         // current deck of cards (if only one then value 0)
    private int _currentCardNumber = 0;         // current card Number  in the deck 
    private int _currentCardBack = 0;           // current back of a card
    private float _deckFanOut = 0.03f;          // value added to fan out the deck 
    //
    //znznznznznznznznznznznznznznzn
    // sprite image of each card
    //znznznznznznznznznznznznznznzn
    //
    // Pre-load this array in Editor by dragging Asset\Images\CardDeck_72100
    // cards 0-51
    //
    private Sprite[] _allImages;
    private Sprite[] cardfaces;
    //
    // Pre-load this array in Editor with cards 52-55, 57-64
    //
    private Sprite[] cardBacks;
    //
    // Joker is card 56
    //
    private Sprite cardJokers;
    private CardDeckManager()
    {
        if (_instance != null)
        {
            throw new CannotHaveTwoInstancesException();
        }
        _instance = this;

        _allImages = Resources.LoadAll<Sprite>("CardImages");
        //
        // load all the faces for cards 0-51 
        //
        instance.cardfaces = new Sprite[52];
        for (int i = 0; i < 52; i++)
        {
            instance.cardfaces[i] = _allImages[i];
        }
        //
        // back of cards 52-63 
        //
        instance.cardBacks = new Sprite[12];
        for (int i = 0; i < 12; i++)
        {
            instance.cardBacks[i] = _allImages[i + 52];
        }
        //
        // Joker is card 64
        //
        instance.cardJokers = new Sprite();
        instance.cardJokers = _allImages[64];

        instance._currentCardBack = 0;
        instance._currentCardNumber = 0;
        instance._currentDeckNumber = 0;

        _allImages = null;
    }
    /// <summary>
    /// Prior to this call, make sure the location of the deck of cards is determined.
    /// </summary>
    public static void CreateDeckOfCards()
    {
        if (DeckLocation == null)
            throw new DeckOfCardsMissingException();

        instance.cardDeck = new GameObject[52];
        instance.cardDeckPointer = new int[52];

        for (int i = 0; i < 52; i++)
        {
            GameObject cardObj = GameObject.Instantiate(Resources.Load("CardPrefab", typeof(GameObject))) as GameObject;
            cardObj.name = "Card_D" + instance._currentDeckNumber.ToString() + "_C" + i.ToString();
            SpriteRenderer cardObjRender = cardObj.GetComponent<Renderer>() as SpriteRenderer;
            cardObjRender.sortingOrder = i;
            cardObj.transform.parent = DeckLocation.transform;
            //
            // put a distance between cards, so it will look as if it is a deck (faned out)
            //
            float xloc = (float)(i * DeckFanOutDistance);
            cardObj.transform.localPosition = new Vector3(xloc, DeckLocation.transform.position.y, DeckLocation.transform.position.z);

            Card _card = cardObj.GetComponent<Card>();
            _card.cardIndex = i;
            _card.cardFaceShowing = false;              //face down only
            //
            // 0 heart, 1 dimond, 2 clubs, 3 spade
            //
            if (i < 13)
            {
                _card.cardIsRed = true;                //hearts
                _card.cardSuit = 0;
            }
            else if (i >= 13 && i <= 25)
            {
                _card.cardIsRed = true;                //diamond
                _card.cardSuit = 1;
            }
            else if (i >= 26 && i <= 38)
            {
                _card.cardIsRed = false;               //clubs
                _card.cardSuit = 2;
            }
            else
            {
                _card.cardIsRed = false;               //spades
                _card.cardSuit = 3;
            }
            //
            // This ranking depends on the card image that holds the face of the cards
            // 0 two, 1 three, 2 four, 3 five,... 8 ten, 9 jack, 10 queen, 11 king, 12 Ace
            //
            //
            _card.cardFaceValue = i % 13;
            //
            // If this is a blackjack game the jack,queen,king = 10 points
            // all number cards are their values, except Ace to be 1 or 11
            //

            if (_card.cardFaceValue <= 8)
            {
                _card.cardValue = _card.cardFaceValue + 2;
            }
            else if (_card.cardFaceValue > 8 && _card.cardFaceValue < 12)   //face cards
            {
                _card.cardValue = 10;
            }
            else
            {
                _card.cardValue = 1;                   //Ace is one
                _card.cardValueExtra = 11;             //Ace is also 11
            }

            instance._currentCardNumber = 0;
            cardObjRender.sprite = instance.cardBacks[CurrentCardBack];
            //
            // cards 0 - 51
            //
            instance.cardDeck[i] = cardObj;
            instance.cardDeckPointer[i] = i;
        }

        Shuffle();

    }
    public static void Shuffle()
    {
        //
        // now shuffle each deck with 52 cards
        //
        //int count = 51;
        //for (int j = count; j > 1; j--)
        //{
        //    GameObject temp = instance.cardDeck[j];
        //    int Number = UnityEngine.Random.Range(0, j + 1);

        //    instance.cardDeck[j] = instance.cardDeck[Number];
        //    instance.cardDeck[Number] = temp;
        //}
        int count = 51;
        for (int j = count; j > 1; j--)
        {
            int temp = instance.cardDeckPointer[j];
            int Number = UnityEngine.Random.Range(0, j + 1);

            instance.cardDeckPointer[j] = instance.cardDeckPointer[Number];
            instance.cardDeckPointer[Number] = temp;
        }

        instance._currentCardNumber = 0;
    }
    public static int GetACard()
    {
        if (CurrentCardNumber > 51)
            return -1;

        //
        // 5/16/2017 there is only ONE deck of cards
        //
        int cardPTR = instance.cardDeckPointer[CurrentCardNumber];

        CurrentCardNumber += 1;
        if (CurrentCardNumber > 51)
        {
            //
            // change carddeck image to out of cards
            //
            SpriteRenderer render = DeckLocation.GetComponent <Renderer>() as SpriteRenderer;
            render.sprite = instance.cardBacks[0];

        }
        //if (CurrentCardNumber > 51)
        //{
        //    return -1;
        //    //
        //    // Re-create/shuffle 
        //    //Shuffle();
        //    //cardPTR = instance.cardDeckPointer[CurrentCardNumber];
        //}

        return cardPTR;
    }
    public static GameObject GetCardObject(int cardPTR)
    {
        GameObject cardObj = instance.cardDeck[cardPTR];
        return cardObj;
    }
    public static GameObject GetCardFaceUp(int cardPTR)
    {
        GameObject cardObj = instance.cardDeck[cardPTR];
        SpriteRenderer cardObjRender = cardObj.GetComponent<Renderer>() as SpriteRenderer;
        cardObjRender.sprite = instance.cardfaces[cardPTR];
        return cardObj;
    }
    public static GameObject GetCardFaceDown(int cardPTR)
    {
        GameObject cardObj = instance.cardDeck[cardPTR];
        SpriteRenderer cardObjRender = cardObj.GetComponent<Renderer>() as SpriteRenderer;
        cardObjRender.sprite = instance.cardBacks[CurrentCardBack];
        return cardObj;
    }
    public static Sprite GetCardFace(int cardPTR)
    {
         return instance.cardfaces[cardPTR];

    }
    public static Sprite GetCardBack(int cardPTR)
    {
        return instance.cardBacks[CurrentCardBack];
    }
    /// <summary>
    /// Property for retreiving the singleton.  See msdn documentation.
    /// </summary>
    public static CardDeckManager instance
    {
        get
        {
            //check to see if it doesnt exist
            if (_instance == null)
            {
                lock (syncRoot)             //lock access, if it is already locked, wait.
                {
                    //the instance could have been made between 
                    //checking and waiting for a lock to release.

                    if (_instance == null)
                    {
                        _instance = new CardDeckManager();                  //create a new instancev
                    }
                }
            }
            return _instance;        //return either the new instance or the already built one.
        }
    }
    public static GameObject DeckLocation
    {
        get
        {
            return instance._deckLocation;
        }
        set
        {
            instance._deckLocation = value;
        }
    }
    public static int CurrentCardBack
    {
        get
        {
            return instance._currentCardBack;
        }
        set
        {
            instance._currentCardBack = value;
        }
    }
    public static int CurrentCardNumber
    {
        get
        {
            return instance._currentCardNumber;
        }
        set
        {
            instance._currentCardNumber = value;
        }
    }
    public static int CurrentDeckNumber
    {
        get
        {
            return instance._currentDeckNumber;
        }
        set
        {
            instance._currentDeckNumber = value;
        }
    }
    /// <summary>
    /// Fan out of cards (0.03 fans out to right, -0.03 fans out to left)
    /// </summary>
    public static float DeckFanOutDistance
    {
        get
        {
            return instance._deckFanOut;
        }
        set
        {
            instance._deckFanOut = value;
        }
    }
}
public class CannotHaveTwoInstancesException : Exception
{
    public CannotHaveTwoInstancesException() : base("There can be only one instance of a the CardDeck Services class. It is a singleton...") { }
}
public class DeckOfCardsMissingException : Exception
{
    public DeckOfCardsMissingException() : base("There must be a deck of cards object in the game...") { }
}
