using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform MainCam;
    private Animator ani;
    private Rigidbody rb;

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float smoothTime = 0.2f;

    [Header("Movement Values")]
    float currentSpeed;
    float velocity;
    Vector3 movement;
    Vector3 plyDirection;

    [Header("Movement Speeds")]
    private float standMoveSpeed = 5;
    private float crouchMoveSpeed = 5;
    private float slideMoveSpeed = 5;

    [Header("Crouch/Slide")]
    bool crouching = false;
    bool sliding = false;
    float slowSpeed = 150;


    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public Vector3 Movement { get => movement; set => movement = value; }
    public bool Crouching { get => crouching; set => crouching = value; }
    public bool Sliding { get => sliding; set => sliding = value; }

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
    }

    private void FixedUpdate()
    {
        performMovement();
    }


    #region Standard Horizontal Movement

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

    void performHorizontalMovement(Vector3 adjustedDirection)
    {
        if (movement.z != 0)
        {
            Vector3 velocity = adjustedDirection * (moveSpeed * Time.deltaTime);

            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }
    }

    void SmoothSpeed(float value)
    {
        currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
    }

    #endregion


    #region Crouch and Slide

    public void OnCrouch(bool crouch)
    {
        crouching = crouch;
        ani.SetBool("crouch", crouch);

        if (crouching)
        {
            if (Mathf.Round(rb.linearVelocity.magnitude) > 0)
            {
                moveSpeed = slideMoveSpeed;
                StartCoroutine(SlowToSlide());
            }
            else
            {
                moveSpeed = crouchMoveSpeed;
            }
        }
        else
        {
            moveSpeed = standMoveSpeed;
            StopCoroutine(SlowToSlide());
        }
    }

    /*
     * Gradually slower player down to crouch if moving
     */
    IEnumerator SlowToSlide()
    {
        while (moveSpeed > crouchMoveSpeed && crouching)
        {

            sliding = true;
            moveSpeed -= slowSpeed * Time.deltaTime;
            yield return null;
        }
        sliding = false;
    }

    #endregion
}
