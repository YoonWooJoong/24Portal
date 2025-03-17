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

    public bool invertRotation = false; // ȸ�� ���� ���� (�⺻��: false)
    public bool useLocalUp = true; // ���� Up ���� ��� ����

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
            Debug.LogError("MeshRenderer ������Ʈ�� �����ϴ�!");
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
            Debug.Log("�̵��մϴ�");
            playerCollider = other;
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

        // �ڷ���Ʈ �� ���� ����
        Vector3 previousVelocity = rb.velocity;
        Vector3 previousAngularVelocity = rb.angularVelocity;
        Quaternion previousRotation = player.rotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 1. ���� ��ġ ���
        Vector3 localPosition = transform.InverseTransformPoint(player.position);

        // 2. ���ο� ��ġ ���
        Vector3 newPosition = otherPortal.TransformPoint(-localPosition);

        // 3. ��ġ ����
        newPosition += otherPortal.forward * cameraForwardOffset;

        // 4. Raycast ���� ����
        Vector3 raycastDirection = useLocalUp ? -otherPortal.up : Vector3.down;

        // 5. ����ĳ��Ʈ�� ����Ͽ� ������ ��ġ ã��
        RaycastHit hit;
        bool raycastSuccess = Physics.Raycast(newPosition, raycastDirection, out hit, maxRaycastDistance, ~teleportLayerMask);

        if (raycastSuccess)
        {
            newPosition.y = hit.point.y + defaultYOffset;
        }
        else
        {
            Debug.LogWarning("����ĳ��Ʈ ����: ������ ���� �ֺ� Ž��");
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
                Debug.LogWarning("�ֺ��� ������ ��ġ�� ã�� �� ����: �⺻ ���� ���");
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


