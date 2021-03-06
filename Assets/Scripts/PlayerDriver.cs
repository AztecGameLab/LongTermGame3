﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerDriver : Driver
{
    EntityController ec;

    GameObject weapon;

    // Represent movement values for horizontal and vertical axes.
    private float x, z;

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

    float groundCheckRadius;
    float groundCheckDistance;
    // Number of keys being held
    //public int keys = 0;
    // audio
    public AudioClip teleporterClip;
    private AudioSource clipSource;
    public AudioSource healthSource;

    //To keep from triggering the teleport sequence twice
    private bool teleporting = false;

    //Invert Mouse Y
    private float flippedYmult = 1;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        horizontalVelocity = new Vector3();
        verticalVelocity = new Vector3();
        horizontalLook = transform.eulerAngles.y;
        verticalLook = transform.eulerAngles.x;
        var charController = GetComponent<CharacterController>();
        groundCheckDistance = charController.height / 2;
        groundCheckRadius = charController.radius;

        //clipSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ec = gameObject.GetComponent<EntityController>();
        health = 100;

        float mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 0.5f);
        SetMouseSensitivity(mouseSensitivity);
        flippedYmult = PlayerPrefs.GetFloat("InvertMouseY", 1.0f);
    }

    void Update()
    {
        HudCanvas.instance.SetHealth(health);
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        this.sensitivity = 100 + ((sensitivity - 0.5f) * 2 * 90);

        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    public void FlipYAxis(bool flipped)
    {
        flippedYmult = flipped ? -1 : 1;

        PlayerPrefs.SetFloat("InvertMouseY", flippedYmult);
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

    // Returns true if player is grounded, false otherwise.
    private bool GroundCheck()
    {
        // Check an invisible sphere at the bottom of the controller and see if it collides with objects in the ground layer.
        return Physics.CheckSphere(transform.position + Vector3.down * groundCheckDistance, groundCheckRadius, LayerMask.GetMask("Ground"));
    }

    public override float GetHorizontalLook()
    {
        // Modifying mouse position based on sensitivity and accounting for framerate.
        mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime * 2;

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
        mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime * 2;

        verticalLook -= mouseY * flippedYmult;
        if (verticalLook > 90)
            verticalLook = 90;
        else if (verticalLook < -90)
            verticalLook = -90;
        return verticalLook;
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
                if (health < maxHealth)
                {
                    Destroy(otherObject);
                    ec.Heal(10);
                    healthSource.Play();
                }
            }
            else if (otherObject.tag.Equals("Weapon"))
            {
                //Weapon pickups are detected / handled by the WeaponSpawner
                /*
                Destroy(otherObject.GetComponent<BoxCollider>());
                Transform otherTrans = otherObject.transform;
                Transform camera = ec.childTransform;

                otherTrans.position = camera.position + camera.forward * 3f + camera.right * 0.5f + camera.up * -0.5f;
                otherTrans.rotation = camera.rotation;
                otherTrans.SetParent(camera);

                weapon = other.gameObject;
                */
            }
            else if (otherObject.tag.Equals("Ammo"))
            {



            }
            else if (otherObject.tag.Equals("Key"))
            {
                Destroy(otherObject);
                keyCount += 1;

            }
            /*else if (otherObject.tag.Equals("Teleporter"))
            {
                //next scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                // play audio
                clipSource.PlayOneShot(teleporterClip);
            }*/
            else if (otherObject.tag.Equals("Teleporter"))
            {
                if (!teleporting)
                {
                    teleporting = true;
                    StartCoroutine(StartTeleportation());
                }
            }
            IEnumerator StartTeleportation()
            {
                // play audio
                //clipSource.PlayOneShot(teleporterClip);

                AudioSource portalSource = otherObject.GetComponent<AudioSource>();
                portalSource.PlayOneShot(teleporterClip);

                //yield return new WaitForSecondsRealtime(2.17f);
                yield return new WaitForSecondsRealtime(1.5f);
                //next scene
                SceneManager.LoadScene("LoadScene");
            }
        }
    }

    //Called when health drops to 0
    protected override void OnDeath()
    {
        HudCanvas.instance.Die();
    }

}
