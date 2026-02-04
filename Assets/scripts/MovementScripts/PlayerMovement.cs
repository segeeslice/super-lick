using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float airDrag = 0.0f;
    public float groundDrag = 10f;
    public float angularDrag = 10f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 10f;
    public Transform orientation;
    public Transform playerObj;
    public Animator playerAnimator;

    private Rigidbody rb;
    private Vector3 _moveDirection;

    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = groundDrag;
        rb.angularDamping = angularDrag;

        playerAnimator = playerObj.GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // use FixedUpdate to prevent framerate-limited movement
    private void FixedUpdate()
    {
        MovePlayer();
        UpdateAnimation();
        SpeedControl();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<Vector2>().y;
        horizontalInput = context.ReadValue<Vector2>().x;
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

    // added Lick here. We can move this to its own dedicated script if we want though
    public void OnLick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerAnimator.SetBool("isLicking", true);
        }
        else if (context.canceled)
        {
            playerAnimator.SetBool("isLicking", false);
        }
    }

}
