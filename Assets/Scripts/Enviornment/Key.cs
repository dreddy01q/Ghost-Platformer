using UnityEngine;

public class Key : MonoBehaviour
{
    public KeyDoor Door;

    private void OnTriggerEnter(Collider other)
    {
        Door.KeyAcquired();
        Destroy(gameObject);
    }
}
