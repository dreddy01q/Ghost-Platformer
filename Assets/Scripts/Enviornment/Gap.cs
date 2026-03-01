using System;
using UnityEngine;

public class Gap : MonoBehaviour
{
    private GameManage gameManage;
    
    private void Start()
    {
        gameManage = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
    }

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
