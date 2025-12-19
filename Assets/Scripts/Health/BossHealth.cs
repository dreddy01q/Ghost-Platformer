using UnityEngine;

public class BossHealth : EnemyHealth
{
    public GameObject Key;

    public override void defeated()
    {
        base.defeated();

        DropKey();
    }

    public void DropKey()
    {
        Key.SetActive(true);    
    }
}
