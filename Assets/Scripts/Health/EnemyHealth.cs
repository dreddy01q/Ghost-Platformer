using UnityEngine;

public class EnemyHealth : HealthSystem
{
    private Enemy enemyController;
    
    public ParticleSystem smokeParticle;
    
    void Start()
    {
        enemyController = GetComponent<Enemy>();
    }
    
    public override void takeDamage(int damage)
    {
        Debug.Log("Take Damage");
        base.takeDamage(damage);
        enemyController.Ani.SetFloat("health", Health);
        enemyController.Ani.SetTrigger("hit");
    }

    public override void defeated()
    {
        base.defeated();
        enemyController.enabled = false;
        smokeParticle.Play();
        this.gameObject.SetActive(false);
    }
}
