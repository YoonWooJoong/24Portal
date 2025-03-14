using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator anim;
    bool isIn;
    public bool IsIn
    {
        get { return isIn; }
        set { isIn = value; }
    }


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("IsIn", isIn);
    }

}
