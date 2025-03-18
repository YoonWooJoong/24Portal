using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{    
    public GameObject portalAPrefab; 
    public GameObject portalBPrefab; 
    public float maxSpawnDistance = 100f;
    public LayerMask portalSpawnLayer;

    public GameObject portalA;
    public GameObject portalB;


    /// <summary>
    /// Portal Spawn Layer에서 마우스 커서의 레이캐스트 충돌 지점에 포탈 A를 생성
    /// </summary>
    public void SpawnPortalA()
    {
        SpawnPortal(ref portalA, portalAPrefab);
    }
    /// <summary>
    /// Portal Spawn Layer에서 마우스 커서의 레이캐스트 충돌 지점에 포탈 B를 생성
    /// </summary>
    public void SpawnPortalB()
    {
        SpawnPortal(ref portalB, portalBPrefab);
    }

    /// <summary>
    /// Portal Spawn Layer에서 마우스 커서의 레이캐스트 충돌 지점에 포탈 프리팹을 생성합니다.
    /// 이미 포탈이 존재하면 기존 포탈을 대체합니다.
    /// </summary>
    /// <param name="portal">생성된 포탈을 저장할 GameObject에 대한 참조</param>
    /// <param name="prefab">인스턴스화할 포탈 프리팹</param>
    public void SpawnPortal(ref GameObject portal, GameObject prefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitAll; 
        RaycastHit hitPortalLayer; 
        
        bool hasHitAll = Physics.Raycast(ray, out hitAll, maxSpawnDistance);       
        bool hasHitPortalLayer = Physics.Raycast(ray, out hitPortalLayer, maxSpawnDistance, portalSpawnLayer);

        if (hasHitPortalLayer && (!hasHitAll || hitPortalLayer.distance <= hitAll.distance))
        {
            if (portal != null)
            {
                Destroy(portal);
            }
            Vector3 spawnPosition = hitPortalLayer.point + hitPortalLayer.normal * 0.01f;
            GameObject newPortal = Instantiate(prefab, spawnPosition, Quaternion.identity);
            portal = newPortal;
            Portal portalScript = newPortal.GetComponent<Portal>();
                        
            bool isFloor = Mathf.Abs(Vector3.Dot(hitPortalLayer.normal, Vector3.up)) > 0.9f;
            Quaternion finalRotation = Quaternion.identity;           
            finalRotation = Quaternion.LookRotation(hitPortalLayer.normal);

            if (isFloor)
            {               
                Vector3 playerPosition = Camera.main.transform.position;
                Vector3 portalToPlayer = playerPosition - spawnPosition;
                portalToPlayer.y = 0;                
                Quaternion playerYRotation = Quaternion.LookRotation(portalToPlayer);                               
                Vector3 rotationEuler = finalRotation.eulerAngles;
                finalRotation = Quaternion.Euler(rotationEuler.x, playerYRotation.eulerAngles.y, rotationEuler.z);
            }           
            portal.transform.rotation = finalRotation;
            ConnectPortals();
        }
        else
        {
            
        }
    
        
    }
    /// <summary>
    /// 포탈 A와 포탈 B를 서로의 "otherPortal" 참조를 설정하여 연결.
    /// </summary>
    void ConnectPortals()
    {       
        if (portalA == null || portalB == null) return;

        Portal portalAScript = portalA.GetComponent<Portal>();
        Portal portalBScript = portalB.GetComponent<Portal>();

        if (portalAScript == null)
        {
            portalAScript = portalA.AddComponent<Portal>();
        }
        if (portalBScript == null)
        {
            portalBScript = portalB.AddComponent<Portal>();
        }
                
        if (portalAScript != null && portalBScript != null)
        {
            portalAScript.otherPortal = portalB.transform;
            portalBScript.otherPortal = portalA.transform;

            
            
        }
    }
}

