using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private PlayerController playerController;
    
    public ParticleSystem smokeParticle;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Defeated()
    {
        base.Defeated();
        playerController.enabled = false;
        smokeParticle.Play();

        //Destroy(gameObject);

        playerController.GameManage.PlayerManager.PlayerDeath(playerController.PlayerID);
    }
}
