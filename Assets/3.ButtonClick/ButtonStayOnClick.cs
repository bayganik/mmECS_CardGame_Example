using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonStayOnClick : MonoBehaviour 
{
    //
    // This script attached an empty game object called "StayClick"
    // OnClick event of the ButtonStay is programmed with "StayClick"
    //     and the onClick() event present here
    //
    public void onClick()
    {
        Debug.Log("Stay click!");
        EventManager.TriggerEvent("DealCardEvent", "1");  //pass a ONE for dealer stack
    }

}