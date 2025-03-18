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
    /// 매 프레임마다 쉐이더에 포탈 정보를 업데이트
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
