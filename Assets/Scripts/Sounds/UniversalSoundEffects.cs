using UnityEngine;

public class UniversalSoundEffects : MonoBehaviour
{
    public AudioSource Sound_Hit;
    public AudioSource Sound_Attack;
    public AudioSource Sound_Defeat;

    private string soundType_Hit = "hit";
    private string soundType_Attack = "attack";
    private string soundType_Defeat = "defeat";

    public string SoundType_Hit { get => soundType_Hit; set => soundType_Hit = value; }
    public string SoundType_Attack { get => soundType_Attack; set => soundType_Attack = value; }
    public string SoundType_Defeat { get => soundType_Defeat; set => soundType_Defeat = value; }

    public virtual void PlaySound(string soundType)
    {
        switch (soundType)
        {
            case "hit":
                Sound_Hit.Play();
                break;
            case "attack":
                Sound_Attack.Play();
                break;
            case "defeat":
                Sound_Defeat.Play();
                break;
        }
    }
}
