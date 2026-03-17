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
        playerControler = player.GetComponent<PlayerController>();
        Ani = GetComponent<Animator>();
    }

    public void ManualUpdate()
    {
        if (!defeated && !playerControler.IsInvisible)
        {
            //agent.enabled = true;
            enemyMovement.MovementUpdate(true, player);
        }
    }

    public void defeatEnemy()
    {
        defeated = true;
        Ani.SetFloat("movement", 0);
    }
}
