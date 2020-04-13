using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    private Transform _player;
    public const int maxHealth = 100;
    public float health;

    public int keyCount = 0;

    public Transform player{
        get{
            if(_player == null){
                _player = GameObject.FindObjectOfType<PlayerDriver>().transform;
            }
            return _player;
        }
    }

    void Awake(){
    
    }

    public void TakeDamage(float damage){
        health -= damage;
        if (health <= 0){
            health = 0;
            OnDeath();
        }
    }

    protected virtual void OnDeath() { }

    public virtual Vector3 GetMovement(){return Vector3.zero;}
    public virtual float GetVerticalLook(){return 0;}
    public virtual float GetHorizontalLook(){return 0;}
    public virtual bool interact(){return false;}
}
