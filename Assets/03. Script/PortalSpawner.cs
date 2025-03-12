using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{   
    public GameObject portalPrefab;   
    public float maxSpawnDistance = 100f;    
    private GameObject portalA;
    private GameObject portalB;

    void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            SpawnPortal(ref portalA);
        }                
        if (Input.GetMouseButtonDown(1))
        {
            SpawnPortal(ref portalB);
        }
    }

    void SpawnPortal(ref GameObject portal)    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, maxSpawnDistance))
        {            
            if (portal != null)
            {
                Destroy(portal);
            }
                        
            portal = Instantiate(portalPrefab, hit.point, Quaternion.identity);            
            portal.transform.rotation = Quaternion.LookRotation(hit.normal);
                       
            if (portalA != null && portalB != null)
            {
                ConnectPortals();
            }
        }
        else
        {
            Debug.Log("포탈을 생성할 수 있는 위치를 찾지 못했습니다.");
        }
    }

    void ConnectPortals()
    {        
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
               
        portalAScript.otherPortal = portalB.transform;
        portalBScript.otherPortal = portalA.transform;

        Debug.Log("포탈 A와 B가 연결되었습니다.");
    }
}

