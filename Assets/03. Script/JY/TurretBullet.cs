using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public float maxDistance = 50f;  // �Ѿ��� ���ư� �ִ� �Ÿ�
    private Vector3 spawnPosition;   // �Ѿ� �߻� �� ��ġ
    void Update()
    {
        // �߻�� �Ѿ��� ���� �Ÿ� �̻� ���ư��ٸ� �ı�
        if (Vector3.Distance(spawnPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();//�÷��̾� �浹�� �о
            
            if (playerRb != null)
            {
                Vector3 knockbackDirection = collision.transform.position - transform.position;
                knockbackDirection.y = 0;
                playerRb.AddForce(knockbackDirection.normalized * 1f, ForceMode.Impulse);// �÷��̾� �о�� 1f=�˹鰭��

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // �÷��̾ �ƴ� �ٸ� �Ͱ� �浹�ϸ� �Ѿ� �ı�
            Destroy(gameObject);
        }
    }
    
}
