using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{    
    public Transform otherPortal;    
    public float teleportOffset = 0.5f;
    
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
        
        Vector3 localPosition = transform.InverseTransformPoint(player.position);
        Quaternion localRotation = Quaternion.Inverse(transform.rotation) * player.rotation;
                
        Vector3 newPosition = otherPortal.TransformPoint(localPosition);
        Quaternion newRotation = otherPortal.rotation * localRotation;
                
        newPosition += otherPortal.forward * teleportOffset;
                
        player.position = newPosition;
        player.rotation = newRotation;
    }
}
