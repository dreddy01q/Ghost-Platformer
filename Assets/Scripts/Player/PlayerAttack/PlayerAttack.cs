using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerSoundEffects soundEffects;
    private Animator ani;


    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackRange = 5;

    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }

    private void Start()
    {
        soundEffects = GetComponent<PlayerSoundEffects>();
        ani=GetComponent<Animator>();
    }


    public void Scare()
    {
        ani.SetTrigger("scare");
        soundEffects.PlaySound(soundEffects.SoundType_Attack);
    }
}
