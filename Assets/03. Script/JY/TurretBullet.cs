using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public float maxDistance = 50f;  // 총알이 날아갈 최대 거리
    private Vector3 spawnPosition;   // 총알 발사 시 위치
    void Update()
    {
        // 발사된 총알이 일정 거리 이상 날아갔다면 파괴
        if (Vector3.Distance(spawnPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();//플레이어 충돌시 밀어냄
            
            if (playerRb != null)
            {
                Vector3 knockbackDirection = collision.transform.position - transform.position;
                knockbackDirection.y = 0;
                playerRb.AddForce(knockbackDirection.normalized * 1f, ForceMode.Impulse);// 플레이어 밀어내기 1f=넉백강도

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // 플레이어가 아닌 다른 것과 충돌하면 총알 파괴
            Destroy(gameObject);
        }
    }
    
}
