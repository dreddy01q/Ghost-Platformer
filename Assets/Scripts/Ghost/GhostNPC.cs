using UnityEngine;

public class GhostNPC : MonoBehaviour
{
    private Animator Animator;

    private GameManage GameManager;
    private GhostManager GhostManager;

    private SoundProximity SoundProximity;

    private bool found = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator = this.GetComponent<Animator>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
        GhostManager = GameManager.GetComponent<GhostManager>();
        SoundProximity = GetComponent<SoundProximity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !found)
        {
            found = true;
            GameManager.GhostFound();
            SoundProximity.StopSound();
            Animator.SetTrigger("freed");
        }
    }
}
