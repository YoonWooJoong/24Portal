using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.uiManager.ShowUI<MainUI>();
        Cursor.lockState = CursorLockMode.None;
    }
}
