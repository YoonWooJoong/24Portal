using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Transform otherPortal;    
    public int recursionLimit = 3; // 최대 재귀 깊이
    public Camera portalCamera; // 포탈 카메라
    public float cameraForwardOffset = 1.5f;
    public RenderTexture portalTexture; // 렌더 텍스처   


    private static int currentRecursionDepth = 0; // 현재 재귀 깊이
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
            Debug.LogError("다른 포탈이 설정되지 않았습니다!");
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
            Debug.LogError("플레이어에게 Rigidbody가 없습니다!");
            yield break;
        }

        // 텔레포트 전 속도 저장 및 초기화
        Vector3 previousVelocity = rb.velocity;
        Vector3 previousAngularVelocity = rb.angularVelocity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 1. 로컬 위치 계산
        Vector3 localPosition = transform.InverseTransformPoint(player.position);

        // 2. 새로운 위치 계산
        Vector3 newPosition = otherPortal.TransformPoint(-localPosition);

        // 3. 위치 보정
        newPosition += otherPortal.forward * 1f; // teleportOffset 이 삭제되었으므로 1f 로 하드 코딩

        // 카메라 앞쪽으로 이동
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
            Debug.LogWarning("레이캐스트 실패: 안전을 위해 기본 높이 사용");
            newPosition.y += yOffset;
        }
        Quaternion newRotation = currentCameraTransform.rotation;

        // 위치 및 회전 적용
        rb.position = newPosition;
        player.rotation = newRotation;

        // 5. 충돌 방지 (레이어 사용)
        player.gameObject.layer = LayerMask.NameToLayer("Teleporting");
        yield return new WaitForSeconds(0.8f);
        player.gameObject.layer = LayerMask.NameToLayer("Default");

        // 6. 딜레이 시간 동안 대기
        yield return new WaitForSeconds(0.1f);

        // 속도 다시 적용
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

