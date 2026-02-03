using Unity.Cinemachine;
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
    public float rotationSpeed = 10f;
    private Vector3 _moveDirection;
    public Transform orientation;
    public Transform playerObj;
    public Animator playerAnimator;

    float horizontalInput;
    float verticalInput;

    public CameraStyle currentStyle;
    public CinemachineCamera exploreCamera;
    public CinemachineCamera aimCamera;

    public enum CameraStyle
    {
        Explore,
        Aim
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = playerObj.GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;

    }

    // use FixedUpdate to prevent framerate-limited movement
    private void FixedUpdate()
    {
        MovePlayer();
        UpdateAnimation();
        rb.linearDamping = groundDrag;
        SpeedControl();
        setCameraPriority();


    }

    private void setCameraPriority()
    {
        if (currentStyle == CameraStyle.Explore)
        {
            aimCamera.Priority = 0;
            exploreCamera.Priority = 10;
        }
        else
        {
            
            aimCamera.Priority = 10;
            exploreCamera.Priority = 0;
        }
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

        //Debug.Log(_moveDirection.x);
        rb.AddForce(_moveDirection.normalized * moveSpeed*10f, ForceMode.Force);
    }

    // update animation based on speed
    private void UpdateAnimation()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVelocity.magnitude;
        //Debug.Log(speed);

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
        Debug.Log(context.phase);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentStyle = CameraStyle.Aim;
        }
        else if (context.canceled)
        {
            currentStyle = CameraStyle.Explore;
        }
    } 

}
