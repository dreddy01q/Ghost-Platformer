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
    public Animator Ani
    {
        get => ani;
        set => ani = value;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void updateAttack(GameObject targetPlayer)
    {
        checkAttack(targetPlayer);
    }

    private void checkAttack(GameObject targetPlayer)
    {
        if (checkAttackCooldown())
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

    private bool checkAttackCooldown()
    {
        if (attackCooldownCount <= 0)
        {
            return true;
        }
        else
        {
            attackCooldownCount -= Time.deltaTime;
            return false;
        }
    }

    private void enemyAttack()
    {
        attackCooldownCount = attackCooldown;

        Ani.SetTrigger("attack");


        // DEAD CODE?
        RaycastHit hit;

        Ray downRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(downRay, out hit) && hit.distance <= attackRange)
        {
            if (hit.collider.tag == "Player")
            {
                //hit.collider.GetComponent<HealthSystem>().takeDamage(attackDamage);
            }
            else
            {
                Debug.Log(hit.collider.tag);
            }

        }

        Debug.DrawRay(transform.position, transform.forward, Color.red, 20);
    }
}
