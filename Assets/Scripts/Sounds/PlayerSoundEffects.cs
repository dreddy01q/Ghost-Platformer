using UnityEngine;

public class PlayerSoundEffects : UniversalSoundEffects
{
    public AudioSource Sound_Jump;

    private string soundType_Jump = "hit";

    public string SoundType_Jump { get => soundType_Jump; set => soundType_Jump = value; }

    public override void PlaySound(string soundType)
    {
        base.PlaySound(soundType);

        switch (soundType)
        {
            case "jump":
                Sound_Jump.Play();
                break;
        }
    }
}
