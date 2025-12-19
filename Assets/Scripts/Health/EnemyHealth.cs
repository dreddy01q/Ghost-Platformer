using System.Collections;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    private Enemy enemyController;
    
    public ParticleSystem smokeParticle;

    public Enemy EnemyController { get => enemyController; set => enemyController = value; }

    void Start()
    {
        EnemyController = GetComponent<Enemy>();
    }
    
    public override void takeDamage(int damage)
    {
        Debug.Log("Take Damage");
        base.takeDamage(damage);
        EnemyController.Ani.SetFloat("health", Health);
        EnemyController.Ani.SetTrigger("hit");
    }

    public override void defeated()
    {
        base.defeated();
        EnemyController.defeatEnemy();
        smokeParticle.Play();

        StartCoroutine(WaitAndDestroy());

        //this.gameObject.SetActive(false);
    }
    
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
    
    
}
