using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    public Transform buttonTop;
    private float pressThreshold = 0.132f;
    private bool isPressed = false;

    private void Update()
    {
        if (buttonTop.position.y <= pressThreshold)
        {
            Debug.Log("눌렸습니다.");
            if(!isPressed)
                isPressed = true;
        }
        else
        {
            Debug.Log("안눌렸습니다.");
            isPressed = false;
        }
    }

    private void OnPressed()
    {
        
    }

    private void OffPressed()
    {
        
    }

}
