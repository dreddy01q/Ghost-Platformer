using System;
using UnityEngine;

public class Gap : MonoBehaviour
{
    private GameManage gameManage;
    
    private void Start()
    {
        gameManage = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManage.endGame(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        gameManage.endGame(false);
    }
}
