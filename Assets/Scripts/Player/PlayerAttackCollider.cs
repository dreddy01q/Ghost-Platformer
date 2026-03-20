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
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(playerAttack.AttackDamage);
            return;
        }
    }
}
