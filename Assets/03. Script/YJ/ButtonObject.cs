using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public Transform buttonTop;
    private float pressThreshold = 0.136f;
    private bool isPressed = false;
    public GameObject door_object;
    Door door;

    private void Start()
    {
        door = door_object.GetComponent<Door>();
    }


    private void Update()
    {
        if (buttonTop.localPosition.y <= pressThreshold)
        {
            
            if (!isPressed)
                isPressed = true;

            OnPressed();
        }
        else
        {
            isPressed = false;

            OffPressed();
        }
    }

    private void OnPressed()
    {
        door.IsOpen = true;
    }

    private void OffPressed()
    {
        door.IsOpen = false;
    }

}
