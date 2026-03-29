using System;
using UnityEngine;

public class Gap : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        PlayerDeath(other);
    }

    private void PlayerDeath(Collision other)
    {
        try
        {
            other.gameObject.GetComponent<PlayerHealth>().Defeated();
        }
        catch{

        }
    }
}
