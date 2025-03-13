using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JYPlayer : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

}
