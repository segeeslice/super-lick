using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float airDrag = 0.0f; // TODO: need to implement this still
    public float groundDrag = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 10f;
    public float jumpForce = 7f;
    public Transform orientation;
    public Transform playerObj;
    public Animator playerAnimator;

    private Rigidbody rb;
    private Vector3 _moveDirection;

    private bool grounded;
    private bool canJump = true;

    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerAnimator = playerObj.GetComponent<Animator>();

    }

    // use FixedUpdate to prevent framerate-limited movement
    private void FixedUpdate()
    {
        CheckGround();
        MovePlayer();
        SpeedControl();
        ApplyGroundDrag();
        UpdateAnimation();
    }

    private void CheckGround()
    {
        // TODO: this raycast needs fine tuning
        grounded = Physics.Raycast(transform.position, Vector3.down, 1.5f);
        playerAnimator.SetBool("isGrounded", grounded);
        if (grounded)
        {
            playerAnimator.SetBool("isJumping", false);
        }
        // maybe not the best place for this but oh well
    }

    

    public void OnMove(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<Vector2>().y;
        horizontalInput = context.ReadValue<Vector2>().x;

        if (context.started)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        if (context.canceled)
        {
            playerAnimator.SetBool("isRunning", false);
        }
        
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // rotate playerObj to face the direction of movement
        if (_moveDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(_moveDirection.normalized, Vector3.up);

            playerObj.rotation = Quaternion.Slerp(
                playerObj.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        rb.AddForce(_moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    // update animation based on speed
    private void UpdateAnimation()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVelocity.magnitude;

        playerAnimator.SetFloat("MoveSpeed", speed);

        

        
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void ApplyGroundDrag()
    {
        if (!grounded) return;

        Vector3 vel = rb.linearVelocity;

        Vector3 horizontal = new Vector3(vel.x, 0f, vel.z);

        horizontal = Vector3.Lerp(
            horizontal,
            Vector3.zero,
            groundDrag * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector3(
            horizontal.x,
            vel.y,
            horizontal.z
        );
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (!grounded) return;
        Vector3 vel = rb.linearVelocity;

        playerAnimator.SetBool("isJumping", true);

        // Reset downward velocity so jumps are consistent
        vel.y = 0f;
        vel.y = jumpForce;

        rb.AddForce(vel, ForceMode.Impulse);

        grounded = false;
    }

    private void resetJump()
    {
        canJump = true;
    }

}
