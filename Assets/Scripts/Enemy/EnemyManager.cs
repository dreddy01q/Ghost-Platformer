using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy[] enemies;

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.ManualUpdate();
        }
    }
}
