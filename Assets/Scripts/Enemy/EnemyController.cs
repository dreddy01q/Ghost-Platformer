using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerControler;

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
        //player = GameObject.FindGameObjectWithTag("Player");
        //playerControler = player.GetComponent<PlayerController>();
        Ani = GetComponent<Animator>();

        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void ManualUpdate()
    {
        //if (!defeated && !playerControler.IsInvisible)
        if (player!=null)
        {
            //agent.enabled = true;
            enemyMovement.MovementUpdate(true, player);
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
        Debug.Log("Identified player!");
    }
}
