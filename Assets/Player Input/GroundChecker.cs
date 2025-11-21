using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] float groundDistance = 0.05f;
    [SerializeField] LayerMask groundLayers;

    public bool IsGrounded { get; private set; }
    //public bool IsGrounded = true;

    void Update()
    {
       // IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayers);
        IsGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayers);
    }
}
