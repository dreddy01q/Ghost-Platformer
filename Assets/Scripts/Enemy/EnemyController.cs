using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{
    private bool defeated = false;

    private EnemyAttack enemyAttack;
    private EnemyMovement enemyMovement;
    private EnemyDetection enemyDetection;
    private Animator ani;

    public Animator Ani
    {
        get => ani;
    }

    void Start()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyDetection=GetComponent<EnemyDetection>();

        ani = GetComponent<Animator>();
    }

    public void ManualUpdate()
    {
        if (!defeated) 
        {
            enemyMovement.MovementUpdate();
            if (enemyDetection.CurrentTargetPlayer != null)
            {
                enemyAttack.updateAttack(enemyDetection.CurrentTargetPlayer);
            }
        }
    }

    public void defeatEnemy()
    {
        defeated = true;
        Ani.SetFloat("movement", 0);
    }
}
