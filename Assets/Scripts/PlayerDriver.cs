using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriver : Driver
{
    EntityController ec;

    // Represent movement values for horizontal and vertical axes.
    private float x, z;

    /*
    [SerializeField]
    private AnimationCurve curve;
    private bool coroutineRunning;
    */

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

    // Represent mouse position, modified by sensitivity.
    private float mouseX, mouseY;

    // Represent the current rotation along the y and x axes in degrees.
    private float horizontalLook, verticalLook;

    // Mouse sensitivity: 100 = 100%.
    [SerializeField]
    private float sensitivity = 100f;

    private void Awake()
    {
        horizontalVelocity = new Vector3();
        verticalVelocity = new Vector3();
        horizontalLook = transform.eulerAngles.y;
        verticalLook = transform.eulerAngles.x;
    }

    private void Start()
    {
        ec = gameObject.GetComponent<EntityController>();
    }

    public override Vector3 GetMovement()
    {
        // Getting Horizontal and Vertical inputs.
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        // Applying force due to gravity.
        verticalVelocity.y -= gravity * Time.deltaTime;

        // If touching ground and not jumping up.
        if (GroundCheck() && verticalVelocity.y < 0)
        {
            // Regular ground movement.
            horizontalVelocity = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1f) * speed * modifier;
            
            /*
            // Momentum ground movement.
            if (!coroutineRunning && z > -0.01f && z < 0.01f && x > -0.01f && x < 0.01f)
            {
                StartCoroutine(Decelerate());
            }
            else
            {
                StopAllCoroutines();
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity / speed / modifier + transform.right * x * 0.35f + transform.forward * z * 0.35f, 1f) * speed * modifier;
            }
            */    


            // Space key to jump, adds vertical velocity.
            if (Input.GetKeyDown(KeyCode.Space))
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);

            // If not jumping, apply constant downward velocity to keep player attatched to ground when walking down slopes.
            else
                verticalVelocity = 5f * Vector3.down;
        }

        // Modified air movement, maintains velocity but accepts small influence from input. Cannot go faster than on the ground. 
        else
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity / speed / modifier + transform.right * x * 0.01f + transform.forward * z * 0.01f, 1f) * speed * modifier;

        // Combine our vertical and horizontal velocities.
        return verticalVelocity + horizontalVelocity;
    }

    /*
    private IEnumerator Decelerate()
    {
        float time = 0f;
        for (; ;)
        {
            time += Time.deltaTime;
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity / speed / modifier, curve.Evaluate(time)) * speed * modifier;
            yield return null;
        }
    }
    */

    // Returns true if player is grounded, false otherwise.
    private bool GroundCheck()
    {
        // Check an invisible sphere at the bottom of the controller and see if it collides with objects in the ground layer.
        return Physics.CheckSphere(transform.position + Vector3.down, .4f, LayerMask.GetMask("Ground"));
    }

    public override float GetHorizontalLook()
    {
        // Modifying mouse position based on sensitivity and accounting for framerate.
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * 2;

        horizontalLook += mouseX;
        if (horizontalLook > 360)
            horizontalLook -= 360;
        else if (horizontalLook < -360)
            horizontalLook += 360;
        return horizontalLook;
    }

    public override float GetVerticalLook()
    {
        // Modifying mouse position based on sensitivity and accounting for framerate.
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * 2;

        verticalLook -= mouseY;
        if (verticalLook > 90)
            verticalLook = 90;
        else if (verticalLook < -90)
            verticalLook = -90;
        return verticalLook;
    }

    public override bool GetPrimaryWeapon()
    {
        return true;
    }

    public override bool GetSecondaryWeapon()
    {
        return true;
    }

    public override bool GetMeleeWeapon()
    {
        return true;
    }

    public override bool interact()
    {
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            if (otherObject.tag.Equals("Health"))
            {
                Destroy(otherObject);
                ec.Heal(10);
            }
            else if (otherObject.tag.Equals("Weapon"))
            {
                Destroy(otherObject.GetComponent<BoxCollider>());
                Transform otherTrans = otherObject.transform;
                otherTrans.rotation = transform.rotation;
                otherTrans.position = transform.position + transform.forward * 0.5f +  transform.right * 0.5f + new Vector3(0, 0.5f, 0);
                otherTrans.SetParent(ec.childTransform);

            }
            else if (otherObject.tag.Equals("Ammo"))
            {



            }
            else if (otherObject.tag.Equals("Key"))
            {



            }
        }
    }
}
