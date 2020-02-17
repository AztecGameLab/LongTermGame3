using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Represent movement values for horizontal and vertical axes.
    private float x, z;

    // The CharacterController component of this gameObject.
    private CharacterController controller;

    // Base speed value.
    [SerializeField]
    public float speed = 10f;

    // Speed modifier value. 1 = 100% speed. 0 = 0% speed.
    [SerializeField]
    public float modifier = 1;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Could normalize move vector to regulate speed at diagonals, but has side effect of wonky deceleration.

        controller.Move(move * speed * modifier * Time.deltaTime);
    }
}
