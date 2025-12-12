using System;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    public PlayerController playerController;
    
    private void Awake()
    {
        playerController = this.GetComponentInParent<PlayerController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        
        
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found on " + other.gameObject.name);
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            
            other.gameObject.GetComponent<EnemyHealth>().takeDamage(playerController.attackDamage);
            return;
            
            
            
            try
            {
                other.gameObject.GetComponent<EnemyHealth>().takeDamage(playerController.attackDamage);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
