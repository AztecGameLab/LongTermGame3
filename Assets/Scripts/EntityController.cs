using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private Driver driver;
    private CharacterController characterController;

    private void Awake()
    {
        driver = GetComponent<Driver>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Hides mouse cursor while playing.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Aim();
        Movement();
    }

    private void Aim()
    {
        Transform childTransform = transform.GetChild(0).transform;
        childTransform.localRotation = driver.GetVerticalLook();
        transform.rotation = driver.GetHorizontalLook();
    }

    private void Movement()
    {
        characterController.Move(driver.GetMovement() * Time.deltaTime);
    }

    private void PrimaryWeapon()
    {

    }

    private void SecondaryWeapon()
    {

    }

    private void MeleeWeapon()
    {

    }

    private void SetLife(float value)
    {

    }

    private void Die()
    {

    }

    public void TakeDamage(float damage)
    {

    }

    public void Heal(float health)
    {

    }

    public void Interact()
    {

    } 
}
