using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Represent movement values for horizontal and vertical axes.
    private float x, z;

    // The CharacterController component of this gameObject.
    private CharacterController controller;

    private Vector3 horizontalVelocity;
    private Vector3 verticalVelocity;

    // Base speed value, default to 10.
    [SerializeField]
    private float speed = 10f;

    // Speed modifier value, defualt to 1. 1 = 100% speed. 0 = 0% speed.
    [SerializeField]
    private float modifier = 1f;

    // Gravity value, default to 10. Earth gravity = 9.81.
    [SerializeField]
    private float gravity = 10f;

    // Jump height, default to 3.
    [SerializeField]
    private float jumpHeight = 3f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        verticalVelocity = new Vector3();
    }

    private void Update()
    {
        // Calculating horizontal velocity by combining x and z inputs to a single vector of magnitude 1, then multiplying by base speed and modifier.
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        horizontalVelocity = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1f) * speed * modifier;

        // Applying force due to gravity.
        verticalVelocity.y -= gravity * Time.deltaTime;

        // Checking if grounded and verticalVelocity is negative. We check for negative velocity to prevent player from being stuck to the ground when trying to jump.
        if (GroundCheck() && verticalVelocity.y < 0)
        {
            // Player is grounded, can press space to jump.
            if (Input.GetKeyDown(KeyCode.Space))
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            // Player is not jumping, apply negative vertical velocity. Helps keep player attached to ground, especially slopes. Think of it as being magnetized to the ground. 
            else
                verticalVelocity = 5f * Vector3.down;
        }

        // Combine our vertical and horizontal velocities and multipy by deltaTime and have the controller move.
        controller.Move((verticalVelocity + horizontalVelocity) * Time.deltaTime);
    }

    // Returns true if player is grounded, false otherwise.
    private bool GroundCheck()
    {
        // Check an invisible sphere at the bottom of the controller and see if it collides with objects in the ground layer.
        return Physics.CheckSphere(transform.position + Vector3.down, .4f, LayerMask.GetMask("Ground"));
    }
}
