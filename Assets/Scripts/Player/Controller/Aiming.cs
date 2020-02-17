using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    // Represent mouse position, modified by sensitivity.
    private float mouseX, mouseY;

    // Mouse sensitivity. 100 = 100%.
    [SerializeField]
    public float sensitivity = 100f;

    // The transform of the parent player gameObject, NOT THE CAMERA'S.
    private Transform playerTransform;

    // Value to keep track of up and down camera rotation.
    float xRotation = 0f;

    private void Awake()
    {
        playerTransform = transform.parent;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Modifying mouse position based on sensitivity and accounting for framerate.
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Rotating camera left and right (cannot move parent player gameObject this way, as it will just keep spinning). 
        transform.Rotate(Vector3.up * mouseX);

        // Setting player rotation equal to camera rotation so that the player gameObject moves and not just the camera.
        playerTransform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        // Rotating camera up and down. Making sure that camera's Y axis localRotation is 0 (parent player gameObject should now have the rotation).
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
