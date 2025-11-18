using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    private Rigidbody rb;
    private Animator ani;
    
    Transform mainCam;
    

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 15f;
    private float standMoveSpeed = 5;
    private float crouchMoveSpeed = 5;
    private float slideMoveSpeed = 5;
    [SerializeField] float smoothTime = 0.2f;
    Vector3 playerMovement;

    private void Awake()
    {
        mainCam = Camera.main.transform;
        
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        standMoveSpeed = moveSpeed;
        crouchMoveSpeed = standMoveSpeed / 2;
        slideMoveSpeed = standMoveSpeed * 1.5f;
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    float currentSpeed;
    float velocity;
    float ZeroF = 0f;


    #region Updates

    // Update is called once per frame
    void Update()
    {
        getPlyMovement();
    }

    private void getPlyMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        playerMovement = new Vector3(horizontal, 0f, vertical);
    }
    
    private void getPlyJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            
        }
    }

    private void handleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            crouching = true;
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            crouching = false;
        }
    }

    private void FixedUpdate()
    {
        performMovement();
    }

    #endregion

    #region Movement

    void performMovement()
    {
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * playerMovement;

        if (adjustedDirection.magnitude > ZeroF)
        {
            handleRotation(adjustedDirection);
            performHorizontalMovement(adjustedDirection);
        }
        else
        {
            rb.linearVelocity = new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
        }
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
        Vector3 velocity = adjustedDirection * (moveSpeed * Time.deltaTime);

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }


    #endregion
    
    
    #region Crouch and Slide
    
    bool crouching = false;
    void OnCrouch(bool crouch)
    {
        crouching = crouch;

        if (crouching)
        { 
            if (rb.linearVelocity.magnitude > 0)
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

    float slowSpeed = 300;
    bool sliding = false;

    IEnumerator SlowToSlide()
    {
        while (moveSpeed > crouchMoveSpeed && crouching) {

            sliding = true;
            moveSpeed -= slowSpeed * Time.deltaTime;
            yield return null;
        }
        sliding = false;
    }
    
    #endregion

}
