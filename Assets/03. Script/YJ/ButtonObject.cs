using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public Transform buttonTop;
    private float pressThreshold = 0.132f;
    private bool isPressed = false;
    public GameObject door_object;
    Door door;

    private void Start()
    {
        door = door_object.GetComponent<Door>();
    }


    private void Update()
    {
        //Debug.Log($"{buttonTop.localPosition.y}");
        if (buttonTop.localPosition.y <= pressThreshold)
        {
            //Debug.Log("눌렸습니다.");
            
            if (!isPressed)
                isPressed = true;

            OnPressed();
        }
        else
        {
            //Debug.Log("안눌렸습니다.");
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
