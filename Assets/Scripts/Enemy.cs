using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;

    public float detectRange = 10;
    public float moveSpeed = 3;
    public float attackRange = 0.5f;
    
    void Start()
    {
        player=GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Checks to see if player is in range
        if (playerInRange())
        {
            // Once the player is in the enemies range, they will move towards them
            moveTowardsPlayer();
            
            // Once chasing the player, will check to see if they can attack
            checkAttack();
        }
    }

    // If the player is in attack range
    private bool playerInRange()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < detectRange)
        {
            return true;
        }
        return false;
    }

    private void moveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(this.transform.position,player.transform.position,moveSpeed*Time.deltaTime);
    }

    private void checkAttack()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            Debug.Log("Enemy Attack!");
        }
    }
}
