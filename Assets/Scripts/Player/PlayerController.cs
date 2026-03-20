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

    public GameObject plyAppereance;
   
    

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float rotationSpeed = 15f;
    private float standMoveSpeed = 5;
    private float crouchMoveSpeed = 5;
    private float slideMoveSpeed = 5;
    [SerializeField] float smoothTime = 0.2f;
    Vector3 playerMovement;
   
    
    float currentSpeed;
    float velocity;
    
    
    private GameManage gameManage;

    #endregion

    #region Animator Varibales
    
    
    private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
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
    public Vector3 PlyDirection { get => plyDirection; set => plyDirection = value; }
    public bool Crouching { get => crouching; set => crouching = value; }
    public bool Sliding { get => sliding; set => sliding = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    #endregion


    public GameObject MainCameraObject;
    public Transform MainCam;
    public GameObject PlayerUI;




    public delegate void ScareAction();
    public static event ScareAction OnScare;


    private PlayerJump playerJump;
    private PlayerAttack playerAttack;


    public override void OnNetworkSpawn()
    {
        ani = GetComponent<Animator>();

        //DisableCameras();

        if (IsOwner)
        {
            MainCameraObject.SetActive(true);
            PlayerUI.SetActive(true);
        }

        rb = GetComponent<Rigidbody>();
        soundEffects = GetComponent<PlayerSoundEffects>();

        rb.freezeRotation = true;

        playerJump = GetComponent<PlayerJump>();
        playerAttack =GetComponent<PlayerAttack>();

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
        SetSpeeds();
    }
    
    
    #region Start Set Values
    private void SetSpeeds()
    {
        standMoveSpeed = moveSpeed;
        crouchMoveSpeed = standMoveSpeed / 2;
        slideMoveSpeed = standMoveSpeed * 1.5f;
        //longJumpVelocity = moveSpeed * 2f;
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
       
    }
    
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        performMovement();
    }


    #region Player Input
    
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
            playerJump.OnJump(true);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerJump.OnJump(false);
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
            playerAttack.Scare();
        }
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            //OnScare();
        }
    }

    

    #endregion

    #region Movement

    private Vector3 plyDirection;

    void performMovement()
    {
        var adjustedDirection = Quaternion.AngleAxis(MainCam.eulerAngles.y, Vector3.up) * playerMovement;
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
    
    
    #region Crouch and Slide
    
    // Crouch and slide variables


    bool crouching = false;
    bool sliding = false;


    float slowSpeed = 150;

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


    
}
