using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public InputActionReference moveAction;
    [SerializeField] public InputActionReference jumpAction;


    private Vector2 moveInput;
    private bool jumpQueued = false;
    private bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /*
     * OnEnable and OnDisable blocks
     * allow pausing of the game so the 
     * player gets stuck and cant move
     */
    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
