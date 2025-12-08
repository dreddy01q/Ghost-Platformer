using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private PlayerController playerController;
    
    public ParticleSystem smokeParticle;
    
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
        smokeParticle.Play();
        playerController.GameManage.endGame(false);
    }
}
