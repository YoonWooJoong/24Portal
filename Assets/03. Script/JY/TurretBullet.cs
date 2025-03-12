using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾ �浹�ϸ� �ڷ� �о �̸�- JYPlayer ���߿� �����ؾ���
            JYPlayer player = collision.gameObject.GetComponent<JYPlayer>();
            if (player != null)
            {
                Vector3 knockbackDirection = collision.transform.position - transform.position;
                knockbackDirection.y = 0;
                player.Knockback(knockbackDirection);

                Destroy(gameObject);
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
    
}
