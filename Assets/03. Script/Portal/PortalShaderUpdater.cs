using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalShaderUpdater : MonoBehaviour
{
    public Transform portalCenter; 
    private Renderer rend;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }
    /// <summary>
    /// �� �����Ӹ��� ���̴��� ��Ż ������ ������Ʈ
    /// </summary>
    void Update()
    {
        if (portalCenter != null)
        {
            rend.material.SetVector("_PortalCenter", portalCenter.position);
            rend.material.SetVector("_PortalNormal", portalCenter.forward);
        }
    }
}
