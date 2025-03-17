using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform otherPortal;
    public int recursionLimit = 3;
    public Camera portalCamera;
    public float cameraForwardOffset = 0.0f;
    public RenderTexture portalTexture;
    public LayerMask teleportLayerMask;
    public float teleportSafetyRadius = 0.5f;
    public float defaultYOffset = 0.1f;
    public float maxRaycastDistance = 10f;

    public LayerMask occlusionLayer;

    public bool invertRotation = false; // 회전 반전 여부 (기본값: false)
    public bool useLocalUp = true; // 로컬 Up 벡터 사용 여부

    private static int currentRecursionDepth = 0;
    private bool canTeleport = true;
    private Renderer meshRenderer;
    private Collider playerCollider;

    void Start()
    {
        portalTexture = new RenderTexture(512, 256, 16);
        portalCamera.targetTexture = portalTexture;

        meshRenderer = GetComponentInChildren<Renderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer 컴포넌트가 없습니다!");
            enabled = false;
            return;
        }

        meshRenderer.material.mainTexture = portalTexture;
        portalCamera.enabled = false;
        UpdateFOV();
    }

    void Update()
    {
        RenderPortalView();

        if (meshRenderer.material.mainTexture != portalTexture)
        {
            meshRenderer.material.mainTexture = portalTexture;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (canTeleport && (teleportLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("이동합니다");
            playerCollider = other;
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

        // 텔레포트 전 상태 저장
        Vector3 previousVelocity = rb.velocity;
        Vector3 previousAngularVelocity = rb.angularVelocity;
        Quaternion previousRotation = player.rotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 1. 로컬 위치 계산
        Vector3 localPosition = transform.InverseTransformPoint(player.position);

        // 2. 새로운 위치 계산
        Vector3 newPosition = otherPortal.TransformPoint(-localPosition);

        // 3. 위치 보정
        newPosition += otherPortal.forward * cameraForwardOffset;

        // 4. Raycast 방향 설정
        Vector3 raycastDirection = useLocalUp ? -otherPortal.up : Vector3.down;

        // 5. 레이캐스트를 사용하여 안전한 위치 찾기
        RaycastHit hit;
        bool raycastSuccess = Physics.Raycast(newPosition, raycastDirection, out hit, maxRaycastDistance, ~teleportLayerMask);

        if (raycastSuccess)
        {
            newPosition.y = hit.point.y + defaultYOffset;
        }
        else
        {
            Debug.LogWarning("레이캐스트 실패: 안전을 위해 주변 탐색");
            Collider[] colliders = Physics.OverlapSphere(newPosition, teleportSafetyRadius, ~teleportLayerMask);
            float highestPoint = float.NegativeInfinity;

            foreach (Collider collider in colliders)
            {
                if (useLocalUp)
                {
                    Vector3 colliderPoint = collider.ClosestPoint(newPosition);
                    if (Vector3.Dot(otherPortal.up, colliderPoint - newPosition) > 0 && colliderPoint.y > highestPoint)
                    {
                        highestPoint = colliderPoint.y;
                    }
                }
                else
                {
                    if (collider.bounds.max.y > highestPoint)
                    {
                        highestPoint = collider.bounds.max.y;
                    }
                }
            }

            if (highestPoint > float.NegativeInfinity)
            {
                newPosition.y = highestPoint + defaultYOffset;
            }
            else
            {
                Debug.LogWarning("주변에 안전한 위치를 찾을 수 없음: 기본 높이 사용");
                newPosition.y += defaultYOffset;
            }
        }
                
        Quaternion portalRotationDifference = Quaternion.Inverse(transform.rotation) * otherPortal.rotation;                
        Vector3 rotationDifferenceEuler = portalRotationDifference.eulerAngles;
        Quaternion yRotation = Quaternion.Euler(0, rotationDifferenceEuler.y, 0);        
        Quaternion newRotation = yRotation;         
        float dotProduct = Vector3.Dot(transform.up, otherPortal.up);
        if (useLocalUp && dotProduct < -0.99f) 
        {        
            
            newPosition.y -= 0.5f;
        }
        
        if (invertRotation)
        {
            Vector3 eulerAngles = newRotation.eulerAngles;
            eulerAngles.y += 180f;
            newRotation = Quaternion.Euler(eulerAngles);
        }
                
        rb.position = newPosition;
        player.rotation = newRotation;
               
        rb.velocity = portalRotationDifference * previousVelocity;
        rb.angularVelocity = previousAngularVelocity;

        
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
        portalCamera.fieldOfView = 60f;
    }
}


