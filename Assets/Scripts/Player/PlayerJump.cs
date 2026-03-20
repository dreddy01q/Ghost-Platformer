using Unity.VisualScripting;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public GroundChecker groundChecker;
    private PlayerController playerController;
    private Animator ani;
    private Rigidbody rb;
    private PlayerSoundEffects soundEffects;
    private static readonly int SurprisedState = Animator.StringToHash("Base Layer.surprised");

    [SerializeField] float jumpForce = 10;

    // Variable Values
    private float standardJumpForce = 10;
    private float highJumpForce = 10;
    private float longJumpForce = 10;
    private float jumpVelocity = 10f;
    private float longJumpVelocity = 10f;

    private int jumpState = 0;      // 0=No Jump, 1=Jumping Up, 2=Coming Down, 3=Landed
    private bool startJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTimer();
        SetJumps();

        playerController = GetComponent<PlayerController>();
        ani =GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        soundEffects = GetComponent<PlayerSoundEffects>();
    }

    void SetTimer()
    {
        jumpTimer = new TimerCountdown(jumpDuration);
        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpState = 2;
    }
    private void SetJumps()
    {
        standardJumpForce = jumpForce;
        highJumpForce = jumpForce * 1.5f;
        longJumpForce = jumpForce * 0.75f;
    }


    private void Update()
    {
        countdownTimer();
    }
    private void FixedUpdate()
    {
        performJump();
    }

    public void OnJump(bool jump)
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
        if (playerController.Crouching)
        {

            // Long Jump from sliding
            if (playerController.Sliding)
            {
                playerController.MoveSpeed = longJumpVelocity;
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
            jumpVelocity = 0f;

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


    private TimerCountdown jumpTimer;
    private float jumpDuration = 0.1f;

    void countdownTimer()
    {
        jumpTimer.countdown(Time.deltaTime);
    }
}
