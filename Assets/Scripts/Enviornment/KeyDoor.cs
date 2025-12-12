using System;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public int keyNumber;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown("E"))
            {
                //other.gameObject.GetComponent<PlayerInventory>().checkInventory("Key", 1);
            }
        }
    }
}
