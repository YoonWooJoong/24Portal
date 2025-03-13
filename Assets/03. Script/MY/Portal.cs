using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Transform otherPortal;    
    public int recursionLimit = 3; // �ִ� ��� ����
    public Camera portalCamera; // ��Ż ī�޶�
    public float cameraForwardOffset = 1.5f;
    public RenderTexture portalTexture; // ���� �ؽ�ó   


    private static int currentRecursionDepth = 0; // ���� ��� ����
    private bool canTeleport = true;

    void Start()
    {
        portalTexture = new RenderTexture(256, 256, 16);
        portalCamera.targetTexture = portalTexture;
        GetComponent<Renderer>().material.mainTexture = portalTexture;

        UpdateFOV();
        portalCamera.enabled = false;
    }

    void Update()
    {
        UpdateFOV();
        RenderPortalView();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport)
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

        StartCoroutine(TeleportCoroutine(player));
    }

    IEnumerator TeleportCoroutine(Transform player)
    {
        canTeleport = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("�÷��̾�� Rigidbody�� �����ϴ�!");
            yield break;
        }

        // �ڷ���Ʈ �� �ӵ� ���� �� �ʱ�ȭ
        Vector3 previousVelocity = rb.velocity;
        Vector3 previousAngularVelocity = rb.angularVelocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 1. ���� ��ġ ���
        Vector3 localPosition = transform.InverseTransformPoint(player.position);

        // 2. ���ο� ��ġ ���
        Vector3 newPosition = otherPortal.TransformPoint(-localPosition);

        // 3. ��ġ ����
        newPosition += otherPortal.forward * 1f; // teleportOffset �� �����Ǿ����Ƿ� 1f �� �ϵ� �ڵ�

        // ī�޶� �������� �̵�
        Transform currentCameraTransform = portalCamera.transform;
        newPosition += currentCameraTransform.forward * cameraForwardOffset;

        float yOffset = 0.2f;
        float xOffset = 0.6f;
        RaycastHit hit;
        if (Physics.Raycast(newPosition, Vector3.down, out hit, 10f))
        {
            newPosition.y = hit.point.y + yOffset;
            newPosition.x = hit.point.x + xOffset;
        }
        else
        {
            Debug.LogWarning("����ĳ��Ʈ ����: ������ ���� �⺻ ���� ���");
            newPosition.y += yOffset;
        }
        Quaternion newRotation = currentCameraTransform.rotation;

        // ��ġ �� ȸ�� ����
        rb.position = newPosition;
        player.rotation = newRotation;

        // 5. �浹 ���� (���̾� ���)
        player.gameObject.layer = LayerMask.NameToLayer("Teleporting");
        yield return new WaitForSeconds(0.8f);
        player.gameObject.layer = LayerMask.NameToLayer("Default");

        // 6. ������ �ð� ���� ���
        yield return new WaitForSeconds(0.1f);

        // �ӵ� �ٽ� ����
        rb.velocity = previousVelocity;
        rb.angularVelocity = previousAngularVelocity;

        yield return new WaitForSeconds(0.8f);
        canTeleport = true;
    }

    void RenderPortalView()
    {
        currentRecursionDepth++;

        if (currentRecursionDepth > recursionLimit)
        {
            currentRecursionDepth--;
            return;
        }

        portalCamera.transform.position = otherPortal.position;
        portalCamera.transform.rotation = otherPortal.rotation * Quaternion.Euler(-90f, 0f, 0f);

        portalCamera.enabled = true;
        portalCamera.Render();
        portalCamera.enabled = false;

        currentRecursionDepth--;
    }

    void UpdateFOV()
    {
        float portalWidth = transform.localScale.x;
        float distance = 1f;
        float fov = 2 * Mathf.Atan(portalWidth / (2 * distance)) * Mathf.Rad2Deg;
        portalCamera.fieldOfView = fov;
    }
}

