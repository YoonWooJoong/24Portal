using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    private GameObject detectedItem;
    private Camera _camera;

    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private bool IsPicking = false;


    /// <summary>
    /// 상호작용 키를 눌렀을 경우 실행
    /// </summary>
    public Action interaction;

    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPicking && Time.time - lastCheckTime > checkRate) // 물건을 집지 않았을 경우 
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

    public void FixedUpdate()
    {
        if(IsPicking)
        {
            detectedItem.transform.position = _camera.transform.position + _camera.transform.rotation * (1.5f * Vector3.forward);
            detectedItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
            detectedItem.transform.rotation = _camera.transform.rotation;
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
            //아이템 상호작용 //큐브 들기 등...
            if(!IsPicking)
            {
                IsPicking = true;
            } 
            else
            {
                IsPicking = false;
                detectedItem.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity;
            }
        }
    }
}
