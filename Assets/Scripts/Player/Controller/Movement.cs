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
        controller.SimpleMove(Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1f) * speed * modifier);
    }
}
