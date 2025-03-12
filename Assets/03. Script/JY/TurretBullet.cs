using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 충돌하면 뒤로 밀어냄 이름- JYPlayer 나중에 수정해야함
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
