using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JYPlayer : MonoBehaviour
{
    public float knockbackForce = 2f;  // �з����� ��
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   /// <summary>
   /// �ͷ��� ��� �Ѿ� �°� �з����� �Լ�
   /// </summary>
   /// <param name="direction"></param>
    public void Knockback(Vector3 direction)
    {
        rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
    }
}
