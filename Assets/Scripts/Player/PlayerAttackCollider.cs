using System;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    public PlayerAttack playerAttack;
    
    private void Awake()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
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
            
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(playerAttack.AttackDamage);
            return;
            
            
            
            try
            {
                other.gameObject.GetComponent<EnemyHealth>().TakeDamage(playerAttack.AttackDamage);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
