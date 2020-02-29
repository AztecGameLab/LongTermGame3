using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Driver : MonoBehaviour
{
    public abstract Vector3 GetMovement();
    public abstract float GetVerticalLook();
    public abstract float GetHorizontalLook();
    public abstract bool GetPrimaryWeapon();
    public abstract bool GetSecondaryWeapon();
    public abstract bool GetMeleeWeapon();
    public abstract bool interact();
}
