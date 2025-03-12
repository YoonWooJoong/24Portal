using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playrt : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 500f;
    public float jumpForce = 5f;
    public bool canAirControl = true;

    private Rigidbody rb;
    private Collider col; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>(); 

        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 없습니다!");
            enabled = false;
            return; 
        }

        if (col == null)
        {
            Debug.LogError("Collider 컴포넌트가 없습니다!");
            enabled = false;
            return; 
        }

       
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(x, 0, z).normalized;

        moveDirection = transform.TransformDirection(moveDirection);

        rb.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > moveSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
    }

    bool IsGrounded()
    {
        if (col == null) return false; 

        
        float distanceToGround = GetComponent<Collider>().bounds.extents.y;
        RaycastHit hit;
       
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround + 0.1f))
        {
            return true; 
        }
        else
        {
            return false; 
        }
    }
}

