using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    
    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
    }

    public override void defeated()
    {
        base.defeated();
        playerController.enabled = false;
        playerController.GameManage.endGame(false);
    }
}
