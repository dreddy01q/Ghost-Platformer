using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : NetworkBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] int attackDamage = 1;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] float attackCooldown = 3;
    private float attackCooldownCount = 0;

    private Animator ani;

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void updateAttack(GameObject targetPlayer)
    {
        attackCooldownCountdown();
        checkAttack(targetPlayer);
    }

    private void attackCooldownCountdown()
    {
        if (attackCooldownCount > 0)
        {
            attackCooldownCount -= Time.deltaTime;
        }
    }

    private void checkAttack(GameObject targetPlayer)
    {
        if (attackCooldownCount <= 0)
        {
            if (checkAttackRange(targetPlayer))
            {
                enemyAttack();
            }
        }
    }

    private bool checkAttackRange(GameObject targetPlayer)
    {
        return Vector3.Distance(transform.position, targetPlayer.transform.position) < attackRange;
    }

    private void enemyAttack()
    {
        attackCooldownCount = attackCooldown;
        ani.SetTrigger("attack");
    }
}
