using UnityEngine;
using System.Collections;

public class EventTriggerTest : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            EventManager.TriggerEvent("test", "Kamran was here");
        }

        if (Input.GetKeyDown("s"))
        {
            //
            // spawn the prefab object by name
            //
            EventManager.TriggerEvent("Spawn", "minion");
        }

        if (Input.GetKeyDown("d"))
        {
            EventManager.TriggerEvent("Destroy", null);
        }

        if (Input.GetKeyDown("x"))
        {
            EventManager.TriggerEvent("Junk", null);
        }
    }
}