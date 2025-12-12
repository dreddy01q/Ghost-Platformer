using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Collider collider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.IsInvisible)
            {
                collider.enabled = false;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        collider.enabled = true;
    }
}
