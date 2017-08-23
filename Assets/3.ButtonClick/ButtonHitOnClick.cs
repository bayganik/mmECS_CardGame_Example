using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonHitOnClick : MonoBehaviour 
{
    //
    // This script attached an empty game object called "HitClick"
    // OnClick event of the ButtonHitMe is programmed with "HitClick"
    //     and the onClick() event present here
    //
    public void onClick()
    {
        Debug.Log("Hit click!");
        EventManager.TriggerEvent("DealCardEvent", "0");  //pass a zero for player stack
    }
}