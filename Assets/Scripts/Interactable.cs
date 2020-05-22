using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteract(Driver player) { }
    public virtual void OnPickup() { }
    public virtual void OnDrop() { }
}
