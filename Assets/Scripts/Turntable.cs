using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turntable : MonoBehaviour
{
    public float degreesPerSecond;
    public bool isWeapon;

    private float startTime;

    void Start()
    {
        startTime = Time.time;

        if (isWeapon)
        {
            Transform child = gameObject.transform.GetChild(0);
            GameObject weapon = child.GetChild(0).gameObject;

            if (weapon != null)
            {
                Vector3 weaponCenter = weapon.GetComponent<WeaponInfo>().weaponCenter;

                child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, -weaponCenter.x);
            }
        }
    }

    void Update()
    {
        gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, (Time.time - startTime) * degreesPerSecond, 0));
    }
}
