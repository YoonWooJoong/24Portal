using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Transform otherPortal;
    public float teleportOffset = 0.8f;
    public int recursionLimit = 5; // 최대 재귀 깊이
    public Camera portalCamera; // 포탈 카메라
    public RenderTexture portalTexture; // 렌더 텍스처

    private static int currentRecursionDepth = 0; // 현재 재귀 깊이

    void Start()
    {
        // 렌더 텍스처 생성 및 할당
        portalTexture = new RenderTexture(256, 256, 16);
        portalCamera.targetTexture = portalTexture;
        GetComponent<Renderer>().material.mainTexture = portalTexture;

        // FOV 초기화
        UpdateFOV();
    }

    void Update()
    {
        // 포탈 크기가 변경될 때 FOV 업데이트
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
            Debug.LogError("다른 포탈이 설정되지 않았습니다!");
            return;
        }

        // 재귀 깊이 초기화
        currentRecursionDepth = 0;

        // 텔레포트 실행
        TeleportRecursive(player);
    }

    void TeleportRecursive(Transform player)
    {
        // 최대 재귀 깊이 초과 시 렌더링 중단
        if (currentRecursionDepth >= recursionLimit) return;

        // 1. 로컬 위치 계산
        Vector3 localPosition = transform.InverseTransformPoint(player.position);

        // 2. 새로운 위치 계산
        Vector3 newPosition = otherPortal.TransformPoint(localPosition);

        // 3. 위치 보정
        newPosition += otherPortal.forward * teleportOffset;

        // 4. Y축 회전 계산
        float yRotationDifference = otherPortal.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
        Quaternion yRotation = Quaternion.Euler(0f, yRotationDifference, 0f);
        Quaternion newRotation = yRotation * player.rotation;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 텔레포트 전에 속도 저장
            Vector3 previousVelocity = rb.velocity;

            // 위치와 회전 변경
            rb.position = newPosition;
            player.rotation = newRotation;

            // 속도 초기화
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // 속도 다시 적용
            rb.velocity = previousVelocity;
        }
        else
        {
            Debug.LogError("플레이어에게 Rigidbody가 없습니다!");
        }

        // 5. 재귀 렌더링
        RenderPortalView();
    }

    void RenderPortalView()
    {
        // 재귀 깊이 증가
        currentRecursionDepth++;

        // 포탈 카메라 위치 및 회전 설정
        portalCamera.transform.position = otherPortal.position;
        portalCamera.transform.rotation = otherPortal.rotation;

        // 렌더링
        portalCamera.Render();
    }

    void UpdateFOV()
    {
        // 포탈의 크기를 기준으로 FOV 계산
        float portalWidth = transform.localScale.x;
        float distance = 1f; // 임의의 거리 값
        float fov = 2 * Mathf.Atan(portalWidth / (2 * distance)) * Mathf.Rad2Deg;

        // 카메라 FOV 업데이트
        portalCamera.fieldOfView = fov;
    }
}
