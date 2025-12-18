using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int healthMax = 3;
    
    private int health;

    private UniversalSoundEffects soundEffects;

    private void Awake()
    {
        health = healthMax;
        soundEffects = GetComponent<UniversalSoundEffects>();
    }

    public int Health
    {
        get => health;
        set => health = value;
    }

    public virtual void takeDamage(int damage)
    {
        Health-=damage;
        if (Health <= 0)
        {
            defeated();
            PlaySound("defeat");
        }
        else
        {
            PlaySound("hit");
        }
    }

    public virtual void defeated()
    {
        
    }

    private void PlaySound(string type)
    {
        if (soundEffects == null)
        {
            if (type == "hit") 
            {
                soundEffects.PlaySound(soundEffects.SoundType_Hit);
            }
            if (type == "defeat")
            {
                soundEffects.PlaySound(soundEffects.SoundType_Defeat);
            }
        }
    }
}
