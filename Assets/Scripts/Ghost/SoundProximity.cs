using System;
using UnityEngine;

public class SoundProximity : MonoBehaviour
{
    public AudioSource Audio;
    public float playDistance = 15;

    private Transform plyPos;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            plyPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch(Exception e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (plyPos != null) {
            AdjustVolume();
        }
        else
        {
            Audio.volume = 0;
        }
    }

    private void AdjustVolume()
    {
        float distance = Vector3.Distance(gameObject.transform.position, plyPos.position);

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
