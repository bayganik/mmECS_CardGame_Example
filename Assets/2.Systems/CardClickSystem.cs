using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickSystem : MonoBehaviour
{
    Vector3 dist;
    float posX;
    float posY;
    void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rhInfo;
            bool didHit = Physics.Raycast(toMouse, out rhInfo, 500.0f);
            //
            // if true, we have data 
            // The BoxCollider for each card prefab sits on left half of the card
            // if you click on right half of card, nothing happens
            // because cards are fanned out to right
            //
            if (didHit)
            {
                //
                // card name and its location
                //
                Debug.Log(rhInfo.collider.name + " .... " + rhInfo.point);

                GameObject cardObj = GameObject.Find(rhInfo.collider.name);
                //
                // find what image to disp for the Entity
                //
                Card cCard = cardObj.GetComponent<Card>();

                //
                // toggle the display flag
                //
                cCard.cardFaceShowing = !cCard.cardFaceShowing;
                EventManager.TriggerEvent("DisplayCardStackEvent", cCard.cardStack.ToString());
            }
            else
            {
                Debug.Log("Clicked on empty space!");
            }

        }
    }
    void OnMouseDown()
    {
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posX = Input.mousePosition.x - dist.x;
        posY = Input.mousePosition.y - dist.y;

    }

    void OnMouseDrag()
    {
        Vector3 curPos =
                  new Vector3(Input.mousePosition.x - posX,
                  Input.mousePosition.y - posY, dist.z);

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }
}
