using System.Collections;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    private EnemyController enemyController;
    
    public ParticleSystem smokeParticle;

    public EnemyController EnemyController { get => enemyController; set => enemyController = value; }

    void Start()
    {
        EnemyController = GetComponent<EnemyController>();
    }
    
    public override void TakeDamage(int damage)
    {
        Debug.Log("Take Damage");
        base.TakeDamage(damage);
        EnemyController.Ani.SetFloat("health", Health);
        EnemyController.Ani.SetTrigger("hit");
    }

    public override void Defeated()
    {
        base.Defeated();
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
