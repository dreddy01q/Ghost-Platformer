using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    #region Variables

    private int playerID;

    public PlayerInput playerInput;
    private Rigidbody rb;
    private Animator ani;
    private Collider collider;
    
    private PlayerSoundEffects soundEffects;

    public GameObject scareOrigin;
    public GameObject plyAppereance;

    public GameObject MainCamera;
    
    public Transform mainCam;
    
    public GroundChecker groundChecker;
    

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 15f;
    private float standMoveSpeed = 5;
    private float crouchMoveSpeed = 5;
    private float slideMoveSpeed = 5;
    [SerializeField] float smoothTime = 0.2f;
    Vector3 playerMovement;
    
    // Jumping
    [SerializeField] float jumpForce = 10;
    private float standardJumpForce = 10;
    private float highJumpForce = 10;
    private float longJumpForce = 10;
    private float jumpVelocity = 10f;

    private float longJumpVelocity = 10f;
    private int jumpState = 0;      // 0=No Jump, 1=Jumping Up, 2=Coming Down, 3=Landed

    private bool startJump = false;
    
    float currentSpeed;
    float velocity;
    float ZeroF = 0f;
    
    
    private GameManage gameManage;

    #endregion

    #region Animator Varibales
    
    
    private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");
    private static readonly int AttackState = Animator.StringToHash("Base Layer.attack_shift");
    private static readonly int DissolveState = Animator.StringToHash("Base Layer.dissolve");
    private static readonly int AttackTag = Animator.StringToHash("Attack");

    public GameManage GameManage
    {
        get => gameManage;
        set => gameManage = value;
    }

    public bool IsInvisible
    {
        get => isInvisible;
        set => isInvisible = value;
    }
    public int PlayerID { get => playerID; set => playerID = value; }

    #endregion

    public override void OnNetworkSpawn()
    {
        ani = GetComponent<Animator>();

        //DisableCameras();

        if (IsOwner)
        {
            MainCamera.SetActive(true);
        }

        rb = GetComponent<Rigidbody>();
        soundEffects = GetComponent<PlayerSoundEffects>();

        rb.freezeRotation = true;

        GameManage = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
    }

    private void DisableCameras()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (!IsOwner)
            {
                cam.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        ani = GetComponent<Animator>();
       // mainCam = Camera.main.transform;
        
        rb = GetComponent<Rigidbody>();
        soundEffects = GetComponent<PlayerSoundEffects>();

        rb.freezeRotation = true;
        
        GameManage=GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManage>();
        
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTimer();

        SetSpeeds();
        SetJumps();;
    }
    
    
    #region Start Set Values
    
    void SetTimer()
    {
        jumpTimer = new TimerCountdown(jumpDuration);
        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpState = 2;
    }

    private void SetSpeeds()
    {
        standMoveSpeed = moveSpeed;
        crouchMoveSpeed = standMoveSpeed / 2;
        slideMoveSpeed = standMoveSpeed * 1.5f;
        longJumpVelocity = moveSpeed * 2f;
    }

    private void SetJumps()
    {
        standardJumpForce = jumpForce;
        highJumpForce = jumpForce * 1.5f;
        longJumpForce = jumpForce * 0.75f;
    }

    #endregion
    

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        getPlyJump();
        getPlyCrouch();
        getPlyMovement();
        getPlyInvisible();
        getPlyScare();
        
        countdownTimer();
    }
    
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        performMovement();
        performJump();
    }


    #region Player Input
    
    private void getPlyMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        playerMovement = new Vector3(horizontal, 0f, vertical);
        
//        Debug.Log(playerMovement);
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

    private void getPlyCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCrouch(true);
        }
        
        if (Input.GetKeyUp(KeyCode.C))
        {
            OnCrouch(false);
        }
    }
    
    private void getPlyInvisible()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnInvisible(true);
        }
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            OnInvisible(false);
        }
    }
    
    private void getPlyScare()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnScare(true);
        }
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            OnScare(false);
        }
    }

    

    #endregion

    #region Movement

    private Vector3 plyDirection;

    void performMovement()
    {
        var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * playerMovement;
        plyDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * Vector3.forward;

        if (adjustedDirection.magnitude > ZeroF)
        {
            handleRotation(adjustedDirection);
            performHorizontalMovement(adjustedDirection);
            
            SmoothSpeed(adjustedDirection.magnitude);
        }
        else
        {
            rb.linearVelocity = new Vector3(ZeroF, rb.linearVelocity.y, ZeroF);
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
        if (playerMovement.z != 0)
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


    #endregion
    
    #region Jump
    
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
        
        ani.CrossFade(SurprisedState, 0.1f, 0, 0);

        soundEffects.PlaySound(soundEffects.SoundType_Jump);
    }

    private void setJumpValues()
    {
        if (crouching)
        {

            // Long Jump from sliding
            if (sliding)
            {
                moveSpeed = longJumpVelocity;
                jumpForce = longJumpForce;
            }
            else
            {
                jumpForce = highJumpForce;
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
    
    // Crouch and slide variables
    bool crouching = false;
    float slowSpeed = 150;
    bool sliding = false;
    void OnCrouch(bool crouch)
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
        while (moveSpeed > crouchMoveSpeed && crouching) {

            sliding = true;
            moveSpeed -= slowSpeed * Time.deltaTime;
            yield return null;
        }
        sliding = false;
    }
    
    #endregion

    #region Invisible


    private bool isInvisible = false;

    /*
     * Currently just changes the player apperance to be inactive
     * In final version will be more refinined
     */
    private void OnInvisible(bool invisible)
    {
        IsInvisible = invisible;
        
        if (invisible)
        {
            plyAppereance.SetActive(false);
        }
        else
        {
            plyAppereance.SetActive(true);  
        }
    }

    #endregion

    #region Attack/Scare

    public int attackDamage = 5;
    private float attackRange = 5;
    void OnScare(bool scare)
    {
        RaycastHit hit;
        Ray downRay = new Ray(scareOrigin.transform.position, Vector3.forward);

        Debug.Log("Attempt scare");
        ani.SetTrigger("scare");
        soundEffects.PlaySound(soundEffects.SoundType_Attack);

        if (Physics.Raycast(downRay, out hit) && hit.distance <= attackRange) 
        //if (Physics.SphereCast(transform.position, 5, plyDirection, out hit, attackRange))
        {
            Debug.Log("Object hit");
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<HealthSystem>().TakeDamage(attackDamage);
            }
        }
        
        Debug.DrawRay(scareOrigin.transform.position, plyDirection, Color.red,5);
    }

    #endregion

    #region Timer
    
    private TimerCountdown jumpTimer;
    private float jumpDuration = 0.1f;
    
    void countdownTimer()
    {
        jumpTimer.countdown(Time.deltaTime);
    }

    #endregion
}
