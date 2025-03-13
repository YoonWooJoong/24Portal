using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{    
    public GameObject portalAPrefab; 
    public GameObject portalBPrefab; 
    public float maxSpawnDistance = 100f;    

    public GameObject portalA;
    public GameObject portalB;

    

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SpawnPortal(ref portalA, portalAPrefab);
        //}
        //
        //if (Input.GetMouseButtonDown(1))
        //{
        //    SpawnPortal(ref portalB, portalBPrefab);
        //}
    }

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


            float zOffset = 0.3f;
            float xOffset = 0.4f;
            Vector3 spawnPosition = hit.point + new Vector3(xOffset, 0f, zOffset);

            // ��Ż ���� �� ���� �Ҵ�
            GameObject newPortal = Instantiate(prefab, hit.point, Quaternion.identity);
            portal = newPortal;

            Portal portalScript = newPortal.GetComponent<Portal>();

            Quaternion additionalRotation = Quaternion.Euler(90f, 0f, 0f);
            portal.transform.rotation = Quaternion.LookRotation(hit.normal) * additionalRotation;

            // ��Ż�� ��� �����Ǿ����� ����
            if (portalA != null && portalB != null)
            {
                ConnectPortals();
            }
        }
        else
        {
            Debug.Log("��Ż�� ������ �� �ִ� ��ġ�� ã�� ���߽��ϴ�.");
        }
    }

    void ConnectPortals()
    {
        // ��Ż A�� B�� ��� �����Ǿ����� Ȯ��
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

        // portalA�� portalB�� null�� �ƴ��� Ȯ��
        if (portalAScript != null && portalBScript != null)
        {
            portalAScript.otherPortal = portalB.transform;
            portalBScript.otherPortal = portalA.transform;

            
            Debug.Log("��Ż A�� B�� ����Ǿ����ϴ�.");
        }
    }
}

