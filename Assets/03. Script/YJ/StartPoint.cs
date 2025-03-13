using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.startPosition = this.gameObject;
    }


}
