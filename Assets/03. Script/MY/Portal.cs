using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public Transform otherPortal;
    public int recursionLimit = 3; // 최대 재귀 깊이
    public Camera portalCamera; // 포탈 카메라
    public float cameraForwardOffset = 0.5f;
    public RenderTexture portalTexture; // 렌더 텍스처
    public LayerMask teleportLayerMask; // 텔레포트 가능한 레이어 마스크

    private static int currentRecursionDepth = 0; // 현재 재귀 깊이
    private bool canTeleport = true;

    [Header("Mesh Panel Settings")]
    public Vector3 meshPanelPosition = Vector3.zero;
    public Vector3 meshPanelRotation = Vector3.zero;
    public Vector3 meshPanelScale = Vector3.one;
    public string targetChildName = "Portal";


    private Material portalMaterial;
    private GameObject meshPanel; // 메시 패널

    void Start()
    {        
        portalTexture = new RenderTexture(256, 256, 16);
        portalCamera.targetTexture = portalTexture;

        meshPanel = GameObject.CreatePrimitive(PrimitiveType.Plane); // Plane 메시 사용
        meshPanel.name = "PortalMeshPanel"; // 이름 설정

        // 3. 타겟 자식 오브젝트 찾기
        Transform targetChild = transform.Find(targetChildName);
        if (targetChild == null)
        {
            Debug.LogError("타겟 자식 오브젝트를 찾을 수 없습니다: " + targetChildName);
            enabled = false;
            return;
        }

        // 4. 메시 패널의 부모를 타겟 자식 오브젝트로 설정
        meshPanel.transform.SetParent(targetChild, false);

        // 5. 메시 패널 위치, 회전, 크기 설정
        meshPanel.transform.localPosition = meshPanelPosition;
        meshPanel.transform.localRotation = Quaternion.Euler(meshPanelRotation);
        meshPanel.transform.localScale = meshPanelScale;

        // 6. 메시 패널 머티리얼 설정
        Renderer meshRenderer = meshPanel.GetComponent<Renderer>();
        portalMaterial = new Material(Shader.Find("Unlit/Texture")); // Unlit 셰이더 사용
        portalMaterial.mainTexture = portalTexture;
        meshRenderer.material = portalMaterial;

        // 7. 컬링 설정 (양면 렌더링)
        portalMaterial.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Off);

        // 8. 카메라 설정
        portalCamera.enabled = false;
    }

    void Update()
    {
        UpdateFOV();
        RenderPortalView();
    }

    void OnTriggerEnter(Collider other)
    {
        if (canTeleport && (teleportLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("이동합니다");
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
        newPosition += otherPortal.forward * 0.1f;

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
        player.gameObject.layer = LayerMask.NameToLayer("Player");

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
        portalCamera.transform.rotation = otherPortal.rotation * Quaternion.Euler(0f, 0f, 0f);

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
    void OnDestroy()
    {
        // 메시 패널 삭제
        if (meshPanel != null)
        {
            Destroy(meshPanel);
        }

        // 렌더 텍스처 해제
        if (portalTexture != null)
        {
            portalTexture.Release();
        }
    }
}

