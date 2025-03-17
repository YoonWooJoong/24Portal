using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.PlayerCreate();
        GameManager.Instance.uiManager.ShowUI<InGameUI>();
    }
}
