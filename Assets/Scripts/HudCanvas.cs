using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HudCanvas : MonoBehaviour
{
    bool dead;
    public static HudCanvas instance;
    public Text health;
    public Image blood;
    public Text ammo;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetHealth(float h)
    {
        health.text = "Health: " + h;
    }

    public void SetAmmo(float a)
    {
        ammo.text = a.ToString();
    }

    public void TakeDamage()
    {
        if(dead)
            return;
            
        StartCoroutine(Blood());
    }

    IEnumerator Blood()
    {
        blood.enabled = true;
        yield return new WaitForSeconds(0.05f);
        blood.enabled = false;
    }

    public void Die()
    {
        dead = true;
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        blood.enabled = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
