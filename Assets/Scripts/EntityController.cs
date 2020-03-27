using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private Driver driver;
    private CharacterController characterController;
    public Transform childTransform;

    private void Awake()
    {
        driver = GetComponent<Driver>();
        characterController = GetComponent<CharacterController>();
        childTransform = transform.GetChild(0).transform;
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
        Interact();
    }

    private void Aim()
    {
        childTransform.localRotation = Quaternion.Euler(driver.GetVerticalLook() * Vector3.right);
        transform.rotation = Quaternion.Euler(driver.GetHorizontalLook() * Vector3.up);
    }

    private void Movement()
    {
        characterController.Move(driver.GetMovement() * Time.deltaTime);
    }

    private void SetLife(float value)
    {
        if (value <= 0)
        {
            Die();
            return;
        }

        if (value < Driver.maxHealth)
            driver.health = value;
        else
            driver.health = Driver.maxHealth;
    }

    private void Die()
    {
        Debug.Log("Died");
    }

    public void TakeDamage(float damage)
    {
        SetLife(driver.health - damage);
        Debug.Log("Damaged");
    }

    public void Heal(float health)
    {
        SetLife(health + driver.health);
        Debug.Log("Healed");
    }

    public void Interact()
    {
        RaycastHit hit;
        if(!Physics.Raycast(transform.position, transform.forward, out hit, 5)){
            return;
        }
        Interactable interact = hit.transform.gameObject.GetComponent<Interactable>();
        if(interact == null)
            return;

        interact.OnInteract();
    } 
}
