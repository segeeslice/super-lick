using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 1f;
    public float airDrag = 0.0f;
    public float groundDrag = 10f;
    public float maxSpeed = 10f;
    private Vector3 _moveDirection;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        MovePlayer();
        rb.linearDamping = groundDrag;
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
        
        Debug.Log(_moveDirection.x);
        rb.AddForce(_moveDirection.normalized * moveSpeed*10f, ForceMode.Force);
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
    
    
    
}
