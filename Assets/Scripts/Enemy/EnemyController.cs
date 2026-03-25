using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{
    private bool defeated = false;

    private EnemyAttack enemyAttack;
    private EnemyMovement enemyMovement;
    private Animator ani;

    public Animator Ani
    {
        get => ani;
    }

    void Start()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();

        ani = GetComponent<Animator>();
    }

    public void ManualUpdate()
    {
        if (!defeated) 
        {
            enemyMovement.MovementUpdate();
            if (enemyMovement.CurrentTargetPlayer != null)
            {
                enemyAttack.updateAttack(enemyMovement.CurrentTargetPlayer);
            }
        }
    }

    public void defeatEnemy()
    {
        defeated = true;
        Ani.SetFloat("movement", 0);
    }
}
