using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : NetworkBehaviour
{

    private GameObject player;

    private Animator ani;
    private bool defeated = false;

    public Animator Ani
    {
        get => ani;
        set => ani = value;
    }

    private EnemyAttack enemyAttack;
    private EnemyMovement enemyMovement;

    void Start()
    {
        Ani = GetComponent<Animator>();

        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void ManualUpdate()
    {
        enemyMovement.MovementUpdate();

        //if (!defeated && !playerControler.IsInvisible)
        if (player != null && !defeated) 
        {
            enemyMovement.MovementUpdate();
            enemyAttack.updateAttack(player);
        }
    }

    public void defeatEnemy()
    {
        defeated = true;
        Ani.SetFloat("movement", 0);
    }

    public void UpdatePlayer(GameObject player)
    {
        this.player = player;
       // playerControler=player.GetComponent<PlayerController>();
    }
}
