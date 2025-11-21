using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Variables

    public PlayerInput playerInput;
    private Rigidbody rb;
    private Animator ani;
    
    Transform mainCam;
    
    public GroundChecker groundChecker;
    

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 15f;
    private float standMoveSpeed = 5;
    private float crouchMoveSpeed = 5;
    private float slideMoveSpeed = 5;
    [SerializeField] float smoothTime = 0.2f;
    Vector3 playerMovement;
    

    #endregion


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
        SetTimer();
    }

    float currentSpeed;
    float velocity;
    float ZeroF = 0f;


    #region Updates

    // Update is called once per frame
    void Update()
    {
        getPlyJump();
        getPlyMovement();
        
        countdownTimer();
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
            OnJump(true);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJump(false);
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
        performJump();
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
    
    #region Timers
    
    private TimerCountdown jumpTimer;
    private float jumpDuration = 0.1f;

    void SetTimer()
    {
        jumpTimer = new TimerCountdown(jumpDuration);
        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpState = 2;
    }

    void countdownTimer()
    {
        jumpTimer.countdown(Time.deltaTime);
    }

    #endregion

    #region Jump
    
    // Jumping
    [SerializeField] float jumpForce = 10;
    private float standardJumpForce = 10;
    private float highJumpForce = 10;
    private float longJumpForce = 10;
    private float jumpVelocity = 10f;

    private float longJumpVelocity = 10f;
    private int jumpState = 0;      // 0=No Jump, 1=Jumping Up, 2=Coming Down, 3=Landed

    private bool startJump = false;
    
    void OnJump(bool jump)
    {
        // Player is starting to jump and is on the ground and not already jumping
        if (jump && !jumpTimer.IsRunning && groundChecker.IsGrounded)
        {
            // Sets jump values based on player status
            setJumpValues();
            
            // Starts the jump
            startJumpSequence();
        }
        else if (!jump && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();

            jumpState = 2;
        }
    }

    private void startJumpSequence()
    {
        startJump = true;
        jumpState = 1;
        jumpTimer.Start();
    }

    private void setJumpValues()
    {
        if (crouching)
        {
            // High Jump from still crouch
            if (rb.linearVelocity.magnitude == 0)
            {
                jumpForce = highJumpForce;
            }

            // Long Jump from sliding
            if (sliding)
            {
                moveSpeed = longJumpVelocity;
                jumpForce = longJumpForce;
            }
        }
        else
        {
            jumpForce = standardJumpForce;
        }
    }
    

    public void performJump()
    {
        // If not jumping and grounded, keep jump velocity at 0


        // Grounded and not jumping, velocity is 0
        if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpVelocity = ZeroF;

            if (jumpState == 2) 
            {
                jumpState = 3;
            }

            return;
        }

        // Jump Timer has ran out
        if (!jumpTimer.IsRunning)
        {
            Debug.Log("Jump");
            // Gravity takes over
            jumpVelocity += Physics.gravity.y * 2f * Time.fixedDeltaTime;
        }
        
        
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
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
