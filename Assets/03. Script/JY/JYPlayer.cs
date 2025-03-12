using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JYPlayer : MonoBehaviour
{
    public float knockbackForce = 2f;  // 밀려나는 힘
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   /// <summary>
   /// 터렛이 쏘는 총알 맞고 밀려나는 함수
   /// </summary>
   /// <param name="direction"></param>
    public void Knockback(Vector3 direction)
    {
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
    }
}
