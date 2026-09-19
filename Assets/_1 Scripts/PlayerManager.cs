using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Inputs")]
    [SerializeField] public InputActionReference moveLeftAction;
    [SerializeField] public InputActionReference moveRightAction;
    [SerializeField] public InputActionReference jumpAction;

    
    [Header("Movement")]
    [SerializeField] private float maxMoveSpeed = 8f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 40f;
    [SerializeField] private float jumpForce = 12f;  
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private BoxCollider2D groundCheckBox;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpCooldown;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    private Rigidbody2D playerRigidbody;
    private Vector2 currentVelocity;
    private float moveDir;
    private Vector2 moveInput;
    private bool jumpQueued = false;
    private float timeUntilJump;
    private bool isFacingRight;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFacingRight = true;
        timeUntilJump = 0.0f;
        moveDir = 0;
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        HandlePlayerInputs();
        HandlePlayerMovement(moveDir);
        CheckPlayerGrounded();
        HandlePlayerPhysics();
    }

    private void HandlePlayerInputs()
    {
        //Handles left/right movement
        if ((moveLeftAction.action.ReadValue<float>() != 0) || (moveRightAction.action.ReadValue<float>() != 0))
        {
            moveDir = moveRightAction.action.ReadValue<float>() - moveLeftAction.action.ReadValue<float>();
            if (moveDir > 0) 
                isFacingRight = true;
            else 
                isFacingRight = false;
        }
        else
            moveDir = 0;

        //Handles jumping
        if ((jumpAction.action.ReadValue<float>() != 0) && timeUntilJump <= 0.0f)
        {
            jumpQueued = true;
            timeUntilJump = jumpCooldown;
        }
        else
            timeUntilJump -= Time.fixedDeltaTime;
    }

    private void HandlePlayerMovement(float moveDir)
    {
        currentVelocity.x = moveDir * maxMoveSpeed;
        spriteRenderer.flipX = !isFacingRight;
    }

    private void CheckPlayerGrounded()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckBox.bounds.center, groundCheckBox.bounds.size, 0f, groundLayerMask);
    }

    private void HandlePlayerPhysics()
    {
        float targetSpeedX = currentVelocity.x;
        float accelRate = Mathf.Abs(targetSpeedX) > 0.01f ? acceleration : deceleration;

        float newVelocityX = Mathf.MoveTowards(
            playerRigidbody.linearVelocity.x,
            targetSpeedX,
            accelRate * Time.fixedDeltaTime
        );

        Vector2 newVelocity = new Vector2(newVelocityX, playerRigidbody.linearVelocity.y);

        if(jumpQueued && isGrounded)
        {
            newVelocity.y = jumpForce;
            jumpQueued = false;
            playerAnimator.SetTrigger("Jump");
        }
        else if (jumpQueued && !isGrounded)
        {
            jumpQueued = false;
        }

        playerAnimator.SetFloat("Velocity Y", playerRigidbody.linearVelocity.y);
        playerRigidbody.linearVelocity = newVelocity;
    }

    /*
     * OnEnable and OnDisable blocks
     * allow pausing of the game so the 
     * player gets stuck and cant move
     */
    private void OnEnable()
    {
        moveLeftAction.action.Enable();
        moveRightAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveLeftAction.action.Disable();
        moveRightAction.action.Disable();
        jumpAction.action.Disable();
    }


    // Update is called once per frame
    void Update()
    {

    }


}
