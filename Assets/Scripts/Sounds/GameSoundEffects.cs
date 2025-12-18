using UnityEngine;

public class GameSoundEffects : MonoBehaviour
{
    public AudioSource Sound_Win;
    public AudioSource Sound_Lose;
    public AudioSource Sound_GhostFound;
    public AudioSource Sound_Music;

    private string soundType_Win = "win";
    private string soundType_Lose = "lose";
    private string soundType_GhostFound = "ghostFound";
    private string soundType_Music = "music";

    public string SoundType_Win { get => soundType_Win; set => soundType_Win = value; }
    public string SoundType_Lose { get => soundType_Lose; set => soundType_Lose = value; }
    public string SoundType_GhostFound { get => soundType_GhostFound; set => soundType_GhostFound = value; }
    public string SoundType_Music { get => soundType_Music; set => soundType_Music = value; }

    public virtual void PlaySound(string soundType, string function = "play")
    {
        AudioSource currentAudio = null;

        switch (soundType)
        {
            case "win":
                currentAudio = Sound_Win;
                break;
            case "lose":
                currentAudio = Sound_Lose;
                break;
            case "ghostFound":
                currentAudio = Sound_GhostFound;
                break;
            case "music":
                currentAudio = Sound_Music;
                break;
        } 

        if (function == "play")
        {
            currentAudio.Play();
        }
        if (function == "stop")
        {
            Debug.Log(currentAudio.name);
            currentAudio.Stop();
        }
    }
}
