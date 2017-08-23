using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CardStackComponent : MonoBehaviour
{
    //
    // id the card stack eg. Dealer, Player, Discard, etc
    //
    public string stackID;
    public float xOffset = 0.35f;       //disp offset on x-axis
    public float yOffset;               //disp offset on y-axis
    public float xPan;                  //disp panning on x-axis
    public float yPan;                  //disp panning on y-axis
    public bool faceUp = false;         //are cards face up for this stack?
    //
    // Tracking a Stack of cards 
    //
    public List<int> cardsInStack;
    public List<int> blockedStacks;     // Other stacks this one is blocking, if empty then none.
    public void Start()
    {
        cardsInStack = new List<int>();
    }
    private void OnEnable()
    {
        //
        // Register component with ObjectTracker
        // When attached to a GameObject (entity)
        //
        ObjectTracker.Register(this.GetType(), this.gameObject);
    }
}