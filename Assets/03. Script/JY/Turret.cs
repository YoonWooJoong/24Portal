using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float detectionRange = 10f;  
    public float attackRange = 15f;   
    public float attackCooldown = 0.5f;
    public float bulletForce = 5f;
    public GameObject bulletPrefab;  
    public Transform gunBarrel; // 총알 발사 위치
    private Transform player;        
    private bool playerInRange = false;
    private float attackTimer = 0f;

    void Update()
    {
        // 플레이어가 감지 범위 내에 있으면 추적
        if (playerInRange)
        {
           // TrackPlayer();
            AttackPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }

    /// <summary>
    /// 터렛이 플레이어를 추적
    /// </summary>
    void TrackPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(transform.forward, direction); // 터렛 앞과 플레이어 방향 간의 각도

        if (angle < 90f)  // 플레이어가 정면에
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    /// <summary>
    /// 터렛이 플레이어를 공격
    /// </summary>
    void AttackPlayer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Vector3 directionToPlayer = player.position - transform.position; //사이에 장애물이 있는지
            RaycastHit hit;

            if (!Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange))
            {
                ShootBullet();
                attackTimer = 0f;  // 공격 후 타이머 리셋
            }
        }
    }

    // 총알 발사
    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(gunBarrel.forward * bulletForce, ForceMode.VelocityChange);  // 총알 발사
    }
}