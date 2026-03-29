using UnityEngine;

public class BossHealth : EnemyHealth
{
    public GameObject Key;

    public override void Defeated()
    {
        base.Defeated();

        DropKey();
    }

    public void DropKey()
    {
        Key.SetActive(true);    
    }
}
