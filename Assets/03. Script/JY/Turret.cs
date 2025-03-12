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
    public Transform gunBarrel; // �Ѿ� �߻� ��ġ
    private Transform player;        
    private bool playerInRange = false;
    private float attackTimer = 0f;

    void Update()
    {
        // �÷��̾ ���� ���� ���� ������ ����
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
    /// �ͷ��� �÷��̾ ����
    /// </summary>
    void TrackPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(transform.forward, direction); // �ͷ� �հ� �÷��̾� ���� ���� ����

        if (angle < 90f)  // �÷��̾ ���鿡
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    /// <summary>
    /// �ͷ��� �÷��̾ ����
    /// </summary>
    void AttackPlayer()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Vector3 directionToPlayer = player.position - transform.position; //���̿� ��ֹ��� �ִ���
            RaycastHit hit;

            if (!Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange))
            {
                ShootBullet();
                attackTimer = 0f;  // ���� �� Ÿ�̸� ����
            }
        }
    }

    // �Ѿ� �߻�
    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(gunBarrel.forward * bulletForce, ForceMode.VelocityChange);  // �Ѿ� �߻�
    }
}