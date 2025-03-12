using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Transform otherPortal;
    public float teleportOffset = 0.8f;
    public int recursionLimit = 5; // �ִ� ��� ����
    public Camera portalCamera; // ��Ż ī�޶�
    public RenderTexture portalTexture; // ���� �ؽ�ó

    private static int currentRecursionDepth = 0; // ���� ��� ����

    void Start()
    {
        // ���� �ؽ�ó ���� �� �Ҵ�
        portalTexture = new RenderTexture(256, 256, 16);
        portalCamera.targetTexture = portalTexture;
        GetComponent<Renderer>().material.mainTexture = portalTexture;

        // FOV �ʱ�ȭ
        UpdateFOV();
    }

    void Update()
    {
        // ��Ż ũ�Ⱑ ����� �� FOV ������Ʈ
        UpdateFOV();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.transform);
        }
    }

    void TeleportPlayer(Transform player)
    {
        if (otherPortal == null)
        {
            Debug.LogError("�ٸ� ��Ż�� �������� �ʾҽ��ϴ�!");
            return;
        }

        // ��� ���� �ʱ�ȭ
        currentRecursionDepth = 0;

        // �ڷ���Ʈ ����
        TeleportRecursive(player);
    }

    void TeleportRecursive(Transform player)
    {
        // �ִ� ��� ���� �ʰ� �� ������ �ߴ�
        if (currentRecursionDepth >= recursionLimit) return;

        // 1. ���� ��ġ ���
        Vector3 localPosition = transform.InverseTransformPoint(player.position);

        // 2. ���ο� ��ġ ���
        Vector3 newPosition = otherPortal.TransformPoint(localPosition);

        // 3. ��ġ ����
        newPosition += otherPortal.forward * teleportOffset;

        // 4. Y�� ȸ�� ���
        float yRotationDifference = otherPortal.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        Quaternion yRotation = Quaternion.Euler(0f, yRotationDifference, 0f);
        Quaternion newRotation = yRotation * player.rotation;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // �ڷ���Ʈ ���� �ӵ� ����
            Vector3 previousVelocity = rb.velocity;

            // ��ġ�� ȸ�� ����
            rb.position = newPosition;
            player.rotation = newRotation;

            // �ӵ� �ʱ�ȭ
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // �ӵ� �ٽ� ����
            rb.velocity = previousVelocity;
        }
        else
        {
            Debug.LogError("�÷��̾�� Rigidbody�� �����ϴ�!");
        }

        // 5. ��� ������
        RenderPortalView();
    }

    void RenderPortalView()
    {
        // ��� ���� ����
        currentRecursionDepth++;

        // ��Ż ī�޶� ��ġ �� ȸ�� ����
        portalCamera.transform.position = otherPortal.position;
        portalCamera.transform.rotation = otherPortal.rotation;

        // ������
        portalCamera.Render();
    }

    void UpdateFOV()
    {
        // ��Ż�� ũ�⸦ �������� FOV ���
        float portalWidth = transform.localScale.x;
        float distance = 1f; // ������ �Ÿ� ��
        float fov = 2 * Mathf.Atan(portalWidth / (2 * distance)) * Mathf.Rad2Deg;

        // ī�޶� FOV ������Ʈ
        portalCamera.fieldOfView = fov;
    }
}
