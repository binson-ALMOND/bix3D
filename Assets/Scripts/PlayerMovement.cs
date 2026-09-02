using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Parametry Ruchu")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float sprintSpeed = 11f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -19.62f;

    [Header("Kucanie i Wysokość")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float standingCameraY = 0.7f;
    [Header("Animacje")]
    [SerializeField] private Animator animator;
    
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int jumpHash = Animator.StringToHash("Jump");
    
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float x = 0f;
        float z = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) z -= 1f;
        
        bool isSprinting = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        float currentSpeed = walkSpeed;
        if (isSprinting) currentSpeed = sprintSpeed;

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * currentSpeed * Time.deltaTime);
        
        float targetHeight = standingHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime);
        
        if (playerCamera != null)
        {
            float targetCamY = standingCameraY;
            Vector3 camPos = playerCamera.localPosition;
            camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime);
            playerCamera.localPosition = camPos;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            if (animator != null) animator.SetTrigger(jumpHash);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        UpdateAnimations(move.magnitude, isSprinting);
    }

    private void UpdateAnimations(float inputMagnitude, bool isSprinting)
    {
        if (animator == null) return;
        float animSpeed = 0f;
        if (inputMagnitude > 0.1f)
        {
            animSpeed = isSprinting ? 2f : 1f;
        }
        
        animator.SetFloat(speedHash, animSpeed, 0.1f, Time.deltaTime);
        animator.SetBool(isGroundedHash, isGrounded);
    }
}