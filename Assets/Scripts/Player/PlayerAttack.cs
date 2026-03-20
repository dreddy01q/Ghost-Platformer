using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private GameObject scareOrigin;
    private PlayerController playerController;
    private PlayerSoundEffects soundEffects;
    private Animator ani;


    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackRange = 5;

    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }

    private void Start()
    {
        playerController=GetComponent<PlayerController>();
        soundEffects = GetComponent<PlayerSoundEffects>();
        ani=GetComponent<Animator>();
    }


    public void Scare()
    {
        // Am I using this anymore?


        RaycastHit hit;
        Ray downRay = new Ray(scareOrigin.transform.position, Vector3.forward);

        ani.SetTrigger("scare");
        soundEffects.PlaySound(soundEffects.SoundType_Attack);

        if (Physics.Raycast(downRay, out hit) && hit.distance <= attackRange)
        //if (Physics.SphereCast(transform.position, 5, plyDirection, out hit, attackRange))
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<HealthSystem>().TakeDamage(attackDamage);
            }
        }

        Debug.DrawRay(scareOrigin.transform.position, playerController.PlyDirection, Color.red, 5);
    }
}
