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
    public Transform gunBarrel; // �Ѿ� �߻� ��ġ
    private Transform player;        
    private bool playerInRange = false;
    private float attackTimer = 0f;
    private float rotationSpeed = 200f; //ȸ�����ǵ�
    private bool InAngle = false;


    void Update()
    {
        // �÷��̾ ���� ���� ���� ������ ����
        if (playerInRange)
        {
            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                TrackPlayer();
                AttackPlayer();
            }

            else
            {
                // ���� ���� ���̸� ���ư��� ����
                Debug.Log("�÷��̾ ���� ���� �ۿ� ����");
            }
        }
        else
        {
            // �÷��̾ ���� ���� �ۿ� ���� ��
            Debug.Log("�÷��̾� ���Դ� ����");
        }

    }
   


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
            Debug.Log("�÷��̾� ����");
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            Debug.Log("�÷��̾� ����");
        }
    }

    /// <summary>
    /// �ͷ��� �÷��̾ ����
    /// </summary>
    void TrackPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(Vector3.forward, direction); // �ͷ� �հ� �÷��̾� ���� ���� ����
       

        if (angle < 45f)  // 90���� ������ 45���� �������� ȸ��
        {

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Debug.Log($"�÷��̾� ����: {direction}, ���� ����: {angle}");
            InAngle = true;
        }
        else
        {
            // �÷��̾ �ͷ��� �� 90�� ���� �ۿ� ���� ���� ȸ������ ����
            Debug.Log("�÷��̾ �� 90�� ���� �ۿ� ����");
            InAngle = false;
        }
    }

    /// <summary>
    /// �ͷ��� �÷��̾ ����
    /// </summary>
    void AttackPlayer()
    {
        if (player == null) return;

        attackTimer += Time.deltaTime;

        if (InAngle && attackTimer >= attackCooldown && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // �÷��̾ ���̴��� üũ
            if (IsPlayerVisible())
            {
                ShootBullet();
                attackTimer = 0f;  // ���� �� Ÿ�̸� ����
            }
            else
            {
                // �÷��̾ ������ ������ �������� ����
                Debug.Log("�÷��̾ ������ ���� �Ұ�");
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
    void OnDrawGizmos()
    {
        // ���� ������ ������ ������ ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // ���� ���� �� �׸���
    }

    /// <summary>
    /// �÷��̾�� �ͷ� ���̿� ��ֹ��� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    bool IsPlayerVisible()
    {
        RaycastHit hit;
        // �ͷ����� �÷��̾� �������� ������ ��� ��ֹ��� �ִ��� Ȯ��
        Vector3 directionToPlayer = player.position - gunBarrel.position;
        if (Physics.Raycast(gunBarrel.position, directionToPlayer, out hit, attackRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;  // �÷��̾ ����
            }
        }
        return false;  // ��ֹ��� ���� �÷��̾ ������
    }
}