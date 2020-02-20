using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    // Represent mouse position, modified by sensitivity.
    private float mouseX, mouseY;

    // Mouse sensitivity: 100 = 100%.
    [SerializeField]
    public float sensitivity = 100f;

    // The transform of the parent player gameObject, NOT THE CAMERA'S.
    private Transform playerTransform;

    // Value to keep track of up and down camera rotation for the purpose of clamping.
    float verticalRotation = 0f;

    private void Awake()
    {
        playerTransform = transform.parent;
    }

    private void Start()
    {
        // Hides mouse cursor while playing.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Modifying mouse position based on sensitivity and accounting for framerate.
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Clamping vertical camera rotation.
        verticalRotation += mouseY;
        ClampRotation();

        // Rotating camera vertically, and player body horizontally.
        transform.Rotate(Vector3.left * mouseY);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    // Clamps camera rotation to hard coded values so you can't look past straight up or down.
    private void ClampRotation()
    { 
        if (verticalRotation > 90f)
        {
            verticalRotation = 90f;
            mouseY = 0f;
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = 270f;
            transform.eulerAngles = eulerRotation;
        }
        else if (verticalRotation < -90f)
        {
            verticalRotation = -90f;
            mouseY = 0f;
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = 90f;
            transform.eulerAngles = eulerRotation;
        }
    }
}
