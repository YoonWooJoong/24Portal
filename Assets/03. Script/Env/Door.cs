using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim;
    public bool isOpen;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("IsOpen", isOpen);
    }


}
