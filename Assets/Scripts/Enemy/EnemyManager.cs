using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyController[] enemies;

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.ManualUpdate();
        }
    }

    public void UpdatePlayerRefrence(GameObject playerController)
    {
        foreach (EnemyController enemy in enemies)
        {
            enemy.UpdatePlayer(playerController);
        }
    }
}
