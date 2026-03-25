using UnityEngine;

public class KeySpawn : MonoBehaviour
{
    public GameObject key;
    public GameObject[] KeySpawns;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //MATHS CONTENT PRESENT HERE
        int keyPos = Random.Range(0, KeySpawns.Length);
        key.transform.position = KeySpawns[keyPos].transform.position;
    }
}
