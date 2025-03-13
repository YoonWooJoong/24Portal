using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float detectionRange = 10f;  
    public float attackRange = 10f;   
    public float attackCooldown = 0.2f;
    public float bulletForce = 5f;
    public GameObject bulletPrefab;  
    public Transform gunBarrel; // 총알 발사 위치
    private Transform player;        
    private bool playerInRange = false;
    private float attackTimer = 0f;
    private float rotationSpeed = 200f; //회전스피드
    private bool InAngle = false;


    void Update()
    {
        // 플레이어가 감지 범위 내에 있으면 추적
        if (playerInRange)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                TrackPlayer();
                AttackPlayer();
            }

            else
            {
                // 공격 범위 밖이면 돌아가지 않음
                Debug.Log("플레이어가 공격 범위 밖에 있음");
            }
        }
        else
        {
            // 플레이어가 감지 범위 밖에 있을 때
            Debug.Log("플레이어 들어왔다 나감");
        }

    }
   


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
            Debug.Log("플레이어 들어옴");
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            Debug.Log("플레이어 나감");
        }
    }

    /// <summary>
    /// 터렛이 플레이어를 추적
    /// </summary>
    void TrackPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(Vector3.forward, direction); // 터렛 앞과 플레이어 방향 간의 각도
       

        if (angle < 45f)  // 90도의 절반인 45도를 기준으로 회전
        {

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Debug.Log($"플레이어 방향: {direction}, 감지 각도: {angle}");
            InAngle = true;
        }
        else
        {
            // 플레이어가 터렛의 앞 90도 범위 밖에 있을 때는 회전하지 않음
            Debug.Log("플레이어가 앞 90도 범위 밖에 있음");
            InAngle = false;
        }
    }

    /// <summary>
    /// 터렛이 플레이어를 공격
    /// </summary>
    void AttackPlayer()
    {
        if (player == null) return;

        attackTimer += Time.deltaTime;

        if (InAngle && attackTimer >= attackCooldown && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // 플레이어가 보이는지 체크
            if (IsPlayerVisible())
            {
                ShootBullet();
                attackTimer = 0f;  // 공격 후 타이머 리셋
            }
            else
            {
                // 플레이어가 가려져 있으면 공격하지 않음
                Debug.Log("플레이어가 가려져 공격 불가");
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
    void OnDrawGizmos()
    {
        // 감지 범위를 빨간색 원으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // 감지 범위 원 그리기
    }

    /// <summary>
    /// 플레이어와 터렛 사이에 장애물이 있는지 확인하는 함수
    /// </summary>
    bool IsPlayerVisible()
    {
        RaycastHit hit;
        // 터렛에서 플레이어 방향으로 광선을 쏘아 장애물이 있는지 확인
        Vector3 directionToPlayer = player.position - gunBarrel.position;
        if (Physics.Raycast(gunBarrel.position, directionToPlayer, out hit, attackRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;  // 플레이어가 보임
            }
        }
        return false;  // 장애물에 의해 플레이어가 가려짐
    }
}