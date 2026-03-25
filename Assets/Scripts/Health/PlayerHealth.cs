using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private PlayerController playerController;
    private PlayerDeath playerDeath;
    public ParticleSystem smokeParticle;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerDeath=GetComponent<PlayerDeath>();
    }
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Defeated()
    {
        base.Defeated();
        playerController.SetPlayerSpawn(false);
        smokeParticle.Play();

        playerController.plyAppereance.SetActive(false);

        playerDeath.SetPlayerDeath();

        //bool gameOver = playerController.GameManage.PlayerManager.PlayerDeath(playerController.PlayerArrayId);
        //if (!gameOver)
        {
            playerDeath.StartRespawn();
        }
    }
}
