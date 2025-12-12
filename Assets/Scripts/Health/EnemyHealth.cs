using System.Collections;
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
        enemyController.defeatEnemy();
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
