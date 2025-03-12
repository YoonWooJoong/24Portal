using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.StartPosition.transform.position = this.gameObject.transform.position;
    }


}
