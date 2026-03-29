using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController[] enemies;

    private void Awake()
    {
        enemies=GetComponentsInChildren<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.ManualUpdate();
        }
    }
}
