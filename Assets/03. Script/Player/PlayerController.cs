
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelte;

    private Rigidbody _rigidbody;

    private Animator _animator;
    private PortalSpawner portalSpawner;

    private bool isLock;

    /// <summary>
    /// 좌 혹은 우 클릭시 실행
    /// </summary>
    public Action portalAction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        portalSpawner = GetComponent<PortalSpawner>();
        lookSensitivity = GameManager.Instance.MouseSensitivty;
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = walkSpeed;
        isLock = true;
    }

    private void FixedUpdate()
    {
        if (!isLock)
            return;
        Move();
    }

    private void LateUpdate()
    {
        if (!isLock)
            return;
        Look();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        if(dir.magnitude > 0.2f)
        {
           _animator.SetBool("IsMoving", true);
        } 
        else
        {
            _animator.SetBool("IsMoving", false);
        }

        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    private void Look()
    {
        camCurXRot += mouseDelte.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelte.x * lookSensitivity, 0);
    }

    private bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };


        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelte = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            _animator.SetTrigger("IsJump");
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isLock)
        {
            //GameManager.Instance.soundManager.PlaySFX(0);
            portalSpawner.SpawnPortalA();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isLock)
        {
            //GameManager.Instance.soundManager.PlaySFX(0);
            portalSpawner.SpawnPortalB();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveSpeed = runSpeed;
            _animator.SetBool("IsRun", true);
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            moveSpeed = walkSpeed;
            _animator.SetBool("IsRun", false);
        }
    }

    public void OnMouseRock(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isLock)
        {
            Cursor.lockState = CursorLockMode.None;
            isLock = false;
        }
        else if (context.phase == InputActionPhase.Started && !isLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isLock = true;
        }
    }
}
