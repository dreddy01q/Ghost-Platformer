using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int healthMax = 3;
    
    private int health;

    private void Awake()
    {
        health = healthMax;
    }

    public int Health
    {
        get => health;
        set => health = value;
    }

    public virtual void takeDamage(int damage)
    {
        Health-=damage;
        if (Health < 0)
        {
            defeated();
        }
    }

    public virtual void defeated()
    {
        
    }
}
