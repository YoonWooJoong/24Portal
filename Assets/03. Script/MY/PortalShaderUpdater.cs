using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalShaderUpdater : MonoBehaviour
{
    public Transform portalCenter; // Æ÷Å»ÀÇ Transform
    private Renderer rend;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (portalCenter != null)
        {
            rend.material.SetVector("_PortalCenter", portalCenter.position);
            rend.material.SetVector("_PortalNormal", portalCenter.forward);
        }
    }
}
