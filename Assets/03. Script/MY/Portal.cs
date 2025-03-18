using System.Collections;
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
    public LayerMask bulletLayer; 
    public bool invertRotation = false; 
    public bool useLocalUp = true; 

    private static int currentRecursionDepth = 0;
    private bool canTeleport = true;
    private Renderer meshRenderer;
    private Collider playerCollider;

    /// <summary>
    /// ������Ʈ ���� �� �ʱ�ȭ
    /// </summary>
    void Start()
    {
        portalTexture = new RenderTexture(512, 256, 16);
        portalCamera.targetTexture = portalTexture;

        meshRenderer = GetComponentInChildren<Renderer>();
        if (meshRenderer == null)
        {            
            enabled = false;
            return;
        }

        meshRenderer.material.mainTexture = portalTexture;
        portalCamera.enabled = false;
        UpdateFOV();
    }
    /// <summary>
    /// �� �����Ӹ��� ��Ż �並 ������
    /// </summary>
    void Update()
    {
        RenderPortalView();

        if (meshRenderer.material.mainTexture != portalTexture)
        {
            meshRenderer.material.mainTexture = portalTexture;
        }
    }
    /// <summary>
    /// �ٸ� Collider�� Ʈ���ſ� �������� �� ȣ�� ���̾� ���� Bullet
    /// </summary>
    /// <param name="other">������ Collider�Դϴ�.</param>
    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & bulletLayer) != 0)
        {           
            TeleportBullet(other.transform);
        }
        if (canTeleport && (teleportLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {            
            playerCollider = other;
            TeleportPlayer(other.transform);
        }
    }
    /// <summary>
    /// �Ѿ��� �ڷ���Ʈ
    /// </summary>
    /// <param name="bullet">�ڷ���Ʈ�� �Ѿ��� Transform</param>
    void TeleportBullet(Transform bullet)
    {
        StartCoroutine(TeleportBulletCoroutine(bullet));
    }
    /// <summary>
    /// �Ѿ� �ڷ���Ʈ �ڷ�ƾ
    /// </summary>
    /// <param name="bullet">�ڷ���Ʈ�� �Ѿ��� Transform</param>
    /// <returns>IEnumerator</returns>
    IEnumerator TeleportBulletCoroutine(Transform bullet)
    {
        Vector3 localPosition = transform.InverseTransformPoint(bullet.position);
        Vector3 newPosition = otherPortal.TransformPoint(-localPosition);
        newPosition += otherPortal.forward * 0.5f;

        Quaternion portalRotationDifference = Quaternion.Inverse(transform.rotation) * otherPortal.rotation;
        Quaternion newRotation = Quaternion.LookRotation(portalCamera.transform.forward);

        bullet.position = newPosition;
        bullet.rotation = newRotation;


        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = portalCamera.transform.forward * bulletRb.velocity.magnitude;
        }

        yield return null;
    }
    /// <summary>
    /// �÷��̾ �ڷ���Ʈ
    /// </summary>
    /// <param name="player">�ڷ���Ʈ�� �÷��̾��� Transform</param>
    void TeleportPlayer(Transform player)
    {
        if (otherPortal == null)
        {            
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
            yield break;
        }

        Vector3 previousVelocity = rb.velocity;
        Vector3 previousAngularVelocity = rb.angularVelocity;
        Quaternion previousRotation = player.rotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        Vector3 localPosition = transform.InverseTransformPoint(player.position);
        Vector3 newPosition = otherPortal.TransformPoint(-localPosition);
        newPosition += otherPortal.forward * cameraForwardOffset;
        Vector3 raycastDirection = useLocalUp ? -otherPortal.up : Vector3.down;
        RaycastHit hit;

        bool raycastSuccess = Physics.Raycast(newPosition, raycastDirection, out hit, maxRaycastDistance, ~teleportLayerMask);

        if (raycastSuccess)
        {
            newPosition.y = hit.point.y + defaultYOffset;
        }
        else
        {            
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
                newPosition.y = hit.point.y + defaultYOffset;
            }
            else
            {                
                newPosition.y += defaultYOffset;
            }
        }

        Quaternion portalRot = otherPortal.rotation;
        Quaternion portalYRot = Quaternion.Euler(0, otherPortal.eulerAngles.y, 0);
        Quaternion yInvertedRotation = Quaternion.Inverse(portalYRot);                
        Quaternion newRotation = yInvertedRotation * previousRotation;
        
        rb.position = newPosition;
        player.rotation = yInvertedRotation;
             
        rb.angularVelocity = previousAngularVelocity;
        canTeleport = true;
    }
    /// <summary>
    /// ��Ż �並 ������
    /// </summary>
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
    /// <summary>
    /// ī�޶��� �þ߰�(FOV)�� ������Ʈ�մϴ�.
    /// </summary>
    void UpdateFOV()
    {
        portalCamera.fieldOfView = 60f;
    }
}


