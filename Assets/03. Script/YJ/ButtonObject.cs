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
            Debug.Log("���Ƚ��ϴ�.");
            if(!isPressed)
                isPressed = true;
        }
        else
        {
            Debug.Log("�ȴ��Ƚ��ϴ�.");
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
