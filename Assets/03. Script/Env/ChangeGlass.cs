using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGlass : MonoBehaviour
{
    public GameObject originalGlass; //�ȱ��� ����
    public GameObject secondGlass; //���� ����

    void Start()
    {
        secondGlass.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            originalGlass.SetActive(false);
            secondGlass.SetActive(true);

            Rigidbody rb = secondGlass.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
}
