using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    private Transform _player;
    public Transform player{
        get{
            if(_player != null){
                _player = this.GetComponent<PlayerDriver>().transform;
            }
            return _player;
        }
    }

    void Awake(){
        _player = this.GetComponent<PlayerDriver>().transform;
    }

    public virtual Vector3 GetMovement(){return Vector3.zero;}
    public virtual float GetVerticalLook(){return 0;}
    public virtual float GetHorizontalLook(){return 0;}
    public virtual bool GetPrimaryWeapon(){return false;}
    public virtual bool GetSecondaryWeapon(){return false;}
    public virtual bool GetMeleeWeapon(){return false;}
    public virtual bool interact(){return false;}
}
