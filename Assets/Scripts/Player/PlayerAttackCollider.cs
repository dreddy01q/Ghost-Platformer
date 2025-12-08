using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    public PlayerController playerController;
    
    private void Awake()
    {
        playerController=this.GetComponent<PlayerController>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HealthSystem>().takeDamage(playerController.attackDamage);
        }
    }
}
