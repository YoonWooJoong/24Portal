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
    public Transform gunBarrel; // �Ѿ� �߻� ��ġ
    private Transform player;        
    private bool playerInRange = false;
    private float attackTimer = 0f;
    private float rotationSpeed = 200f; //ȸ�����ǵ�
    private bool InAngle = false;
    private bool isKnockedOver = false;  // �ͷ��� �Ѿ������� ����
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
        // �÷��̾ ���� ���� ���� ������ ����
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
            InAngle = true;
        }
        else
        {
            // �÷��̾ �ͷ��� �� 90�� ���� �ۿ� ���� ���� ȸ������ ����
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
        }
    }

    /// <summary>
    /// �Ѿ� �߻�
    /// </summary>
    void ShootBullet()
    {
        Vector3 bulletPosition = gunBarrel.position + Vector3.up * 0.9f;
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, gunBarrel.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(gunBarrel.forward * bulletForce, ForceMode.VelocityChange);  // �Ѿ� �߻�
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
    /// <summary>
    /// �ͷ��� �÷��̾ �浹 �� ������ ���߰� �Ѿ����� �ϴ� �Լ�
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� �浹 �� ������ ���߰� �Ѿ����� �����
            isKnockedOver = true;  // �ͷ��� �Ѿ���
            attackTimer = 0f;  // ���� Ÿ�̸� ����
            InAngle = false;  // ������ ����

            // Rigidbody�� �������� ���� �߰��Ͽ� �Ѿ����� �����
            rb.AddForce(Vector3.up * 0.1f, ForceMode.Impulse);  // ���� ���� �༭ �ణ �Ѿ�����
            rb.AddTorque(Vector3.up * 10f, ForceMode.Impulse);  // ȸ���µ� �־ �Ѿ������� ����
        }
    }
}