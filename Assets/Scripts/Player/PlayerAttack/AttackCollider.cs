using System;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public AttackController AttackController;

    private void Awake()
    {
        //AttackController = this.GetComponentInParent<AttackController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthSystem opponentHealth = other.gameObject.GetComponent<HealthSystem>();

        Debug.Log(other.gameObject.name + " " + opponentHealth);

        if (opponentHealth != null) 
        {
            Debug.Log("Attacking " + other.gameObject.name);
            opponentHealth.TakeDamage(AttackController.AttackDamage);
        }
        else
        {
            other.name = gameObject.name;
        }
    }
}
