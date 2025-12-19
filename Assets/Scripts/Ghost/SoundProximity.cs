using UnityEngine;

public class SoundProximity : MonoBehaviour
{
    public AudioSource Audio;
    public float playDistance = 15;

    private Transform pos;
    private Transform plyPos;

    // Start is called before the first frame update
    void Start()
    {
        plyPos = GameObject.FindGameObjectWithTag("Player").transform;
        pos = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustVolume();
    }

    private void AdjustVolume()
    {
        float distance = Vector3.Distance(plyPos.position, pos.position);

        if (distance > playDistance)
        {
            Audio.volume = 0;
        }
        else
        {
            float level = distance / playDistance;
            level = 1 - level;
            Audio.volume = level;
        }
    }

    public void StopSound()
    {
        Audio.Stop();
    }
}
