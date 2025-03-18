using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float detectionRange = 10f;  
    public float attackRange = 10f;   
    public float attackCooldown = 0.1f;
    public float bulletForce = 5f;
    public GameObject bulletPrefab;  
    public Transform gunBarrel; // 총알 발사 위치
    private Transform player;        
    private bool playerInRange = false;
    private float attackTimer = 0f;
    private float rotationSpeed = 200f; //회전스피드
    private bool InAngle = false;
    private bool isKnockedOver = false;  // 터렛이 넘어졌는지 여부
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isKnockedOver)
        {
            return;
        }
        // 플레이어가 감지 범위 내에 있으면 추적
        if (playerInRange)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                TrackPlayer();
                AttackPlayer();
            }
        }
    }
   


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
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
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(Vector3.forward, direction); // 터렛 앞과 플레이어 방향 간의 각도
       

        if (angle < 45f)  // 90도의 절반인 45도를 기준으로 회전
        {

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            InAngle = true;
        }
        else
        {
            // 플레이어가 터렛의 앞 90도 범위 밖에 있을 때는 회전하지 않음
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
        }
    }

    /// <summary>
    /// 총알 발사
    /// </summary>
    void ShootBullet()
    {
        Vector3 bulletPosition = gunBarrel.position + Vector3.up * 0.9f;
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, gunBarrel.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(gunBarrel.forward * bulletForce, ForceMode.VelocityChange);  // 총알 발사
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
    /// <summary>
    /// 터렛과 플레이어가 충돌 시 공격을 멈추고 넘어지게 하는 함수
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 공격을 멈추고 넘어지게 만들기
            isKnockedOver = true;  // 터렛이 넘어짐
            attackTimer = 0f;  // 공격 타이머 리셋
            InAngle = false;  // 공격을 멈춤

            // Rigidbody의 물리적인 힘을 추가하여 넘어지게 만들기
            rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);  // 위로 힘을 줘서 약간 넘어지게
            rb.AddTorque(Vector3.up * 10f, ForceMode.Impulse);  // 회전력도 주어서 넘어지도록 만듬
        }
    }
}