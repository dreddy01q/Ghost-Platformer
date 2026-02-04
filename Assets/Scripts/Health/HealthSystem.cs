using System;
using UnityEditor.PackageManager;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int healthMax = 3;

    private int health;
    private UniversalSoundEffects soundEffects;

    private enum SoundEvent
    {
        Hit,
        Defeat
    }

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
        Health -= damage;

        if (Health <= 0)
        {
            defeated();
            PlaySound(SoundEvent.Defeat);
        }
        else
        {
            PlaySound(SoundEvent.Hit);
        }
    }

    public virtual void defeated()
    {
        
    }

    private void PlaySound(SoundEvent type)
    {
        // if the component isnt there, just do nothing (no crashing)
        if (soundEffects == null) return;

        switch (type)
        {
            case SoundEvent.Hit:
                soundEffects.PlaySound(soundEffects.SoundType_Hit);
                break;

            case SoundEvent.Defeat:
                soundEffects.PlaySound(soundEffects.SoundType_Defeat);
                break;
        }
    }
}
//***the enum version is nicer  because it prevents "defeat" / "deafeat" type mistakes***