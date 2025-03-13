using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    private GameObject detectedItem;
    private Camera _camera;

    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;


    /// <summary>
    /// ��ȣ�ۿ� Ű�� ������ ��� ����
    /// </summary>
    public Action interaction;

    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                detectedItem = hit.collider.gameObject;
            }
            else
            {
                detectedItem = null;
            }
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (detectedItem == null)
        {
            return;
        }

        if (context.phase == InputActionPhase.Started)
        {
            //������ ��ȣ�ۿ� //ť�� ��� ��...

            //test code
            detectedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse); 
        }
    }
}
