﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{    
    public GameObject portalAPrefab; 
    public GameObject portalBPrefab; 
    public float maxSpawnDistance = 100f;    

    public GameObject portalA;
    public GameObject portalB;   

    

    public void SpawnPortalA()
    {
        SpawnPortal(ref portalA, portalAPrefab);
    }

    public void SpawnPortalB()
    {
        SpawnPortal(ref portalB, portalBPrefab);
    }


    public void SpawnPortal(ref GameObject portal, GameObject prefab)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxSpawnDistance))
        {
            if (portal != null)
            {
                Destroy(portal);
            }
            Vector3 spawnPosition = hit.point + hit.normal * 0.01f;

            // 포탈 생성 및 변수 할당
            GameObject newPortal = Instantiate(prefab, spawnPosition, Quaternion.identity);
            portal = newPortal;

            Portal portalScript = newPortal.GetComponent<Portal>();

            Quaternion additionalRotation = Quaternion.Euler(90f, 0f, 0f);
            portal.transform.rotation = Quaternion.LookRotation(hit.normal) * additionalRotation;
            ConnectPortals();

            // 포탈이 모두 생성되었으면 연결
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
        // 포탈 A와 B가 모두 생성되었는지 확인
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

        // portalA와 portalB가 null이 아닌지 확인
        if (portalAScript != null && portalBScript != null)
        {
            portalAScript.otherPortal = portalB.transform;
            portalBScript.otherPortal = portalA.transform;

            
            Debug.Log("포탈 A와 B가 연결되었습니다.");
        }
    }
}

