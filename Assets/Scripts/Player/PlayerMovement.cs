using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float smoothTime = 0.2f;

    float currentSpeed;
    float velocity;

    public Transform MainCam;

    private float standMoveSpeed = 5;
    private float crouchMoveSpeed = 5;
    private float slideMoveSpeed = 5;


    private Animator ani;
    private Rigidbody rb;

    Vector3 movement;
    private Vector3 plyDirection;
    public Vector3 Movement { get => movement; set => movement = value; }
    public Vector3 PlyDirection { get => plyDirection; set => plyDirection = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        SetSpeeds();
    }
    private void SetSpeeds()
    {
        standMoveSpeed = moveSpeed;
        crouchMoveSpeed = standMoveSpeed / 2;
        slideMoveSpeed = standMoveSpeed * 1.5f;
        //longJumpVelocity = moveSpeed * 2f;
    }

    private void FixedUpdate()
    {
        performMovement();
    }

    void performMovement()
    {
        var adjustedDirection = Quaternion.AngleAxis(MainCam.eulerAngles.y, Vector3.up) * movement;
        plyDirection = Quaternion.AngleAxis(MainCam.eulerAngles.y, Vector3.up) * Vector3.forward;

        if (adjustedDirection.magnitude > 0f)
        {
            handleRotation(adjustedDirection);
            performHorizontalMovement(adjustedDirection);

            SmoothSpeed(adjustedDirection.magnitude);
        }
        else
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        ani.SetFloat("move", adjustedDirection.magnitude);
    }

    void handleRotation(Vector3 adjustedDirection)
    {
        // Adjust rotation of player
        var targetRotation = Quaternion.LookRotation(adjustedDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    /*
     * Performs the players movement in direction
     */
    void performHorizontalMovement(Vector3 adjustedDirection)
    {
        if (movement.z != 0)
        {
            Vector3 velocity = adjustedDirection * (moveSpeed * Time.deltaTime);

            //Vector3 velocity = playerMovement * (moveSpeed * Time.deltaTime);

            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }
    }

    void SmoothSpeed(float value)
    {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }
}
