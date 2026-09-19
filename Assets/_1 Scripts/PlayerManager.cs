using UnityEngine;
using UnityEngine.EventSystems;
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

    private Rigidbody2D playerRigidbody;
    private Vector2 currentVelocity;
    private float moveDir;
    private Vector2 moveInput;
    private bool jumpQueued = false;
    private bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveDir = 0;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float targetSpeedX = currentVelocity.x;
        float accelRate = Mathf.Abs(targetSpeedX) > 0.01f ? acceleration : deceleration;

        float newVelocityX = Mathf.MoveTowards(
            playerRigidbody.linearVelocity.x,
            targetSpeedX,
            accelRate * Time.fixedDeltaTime
        );

        playerRigidbody.linearVelocity = new Vector2(newVelocityX, playerRigidbody.linearVelocity.y);
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
        if ((moveLeftAction.action.ReadValue<float>() != 0) || (moveRightAction.action.ReadValue<float>() != 0))
            moveDir = moveRightAction.action.ReadValue<float>() - moveLeftAction.action.ReadValue<float>();
        else 
            moveDir = 0;
        HandlePlayerMovement(moveDir);
    }

    private void HandlePlayerMovement(float moveDir)
    {
        if (jumpAction.action.WasPressedThisFrame())
        {
            jumpQueued = true;
        }

        currentVelocity.x = moveDir * maxMoveSpeed;
    }
}
