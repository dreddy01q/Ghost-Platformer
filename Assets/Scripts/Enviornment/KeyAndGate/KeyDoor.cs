using System;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private bool keyHeld = false;
    public string text = "Key Acquired";

    GameManage GameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyAcquired()
    {
        GameManager.CanvasManager.showText(text);
        keyHeld = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (keyHeld)
            {
                Destroy(gameObject);
            }
        }
    }
}
