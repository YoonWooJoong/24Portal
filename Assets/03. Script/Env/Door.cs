using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim;
    bool isOpen;
    /// <summary>
    /// 열리는 프로퍼티
    /// </summary>
    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; }
    }


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("IsOpen", isOpen);
    }


}
