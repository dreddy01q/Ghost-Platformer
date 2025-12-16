using System;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private GameManage GameManage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManage=GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManage.endGame(true);
        }
    }
}
