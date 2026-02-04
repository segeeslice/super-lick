using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class TongueGrapple : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public Animator playerAnimator;
    public LayerMask whatIsGrappleable;
    public Transform tonguePosition, camera, player;
    public float maxDistance = 10f;
    private SpringJoint joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void LateUpdate()
    {
        DrawTongue();
    }

    // called on Lick Input Action started
    public void OnLick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerAnimator.SetBool("isLicking", true);
            StartGrapple();
        }
        else if (context.canceled)
        {
            playerAnimator.SetBool("isLicking", false);
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        lr.positionCount = 2;
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
        }
        else
        {
            grapplePoint = camera.position + camera.forward * maxDistance;
        }
    }

    void DrawTongue ()
    {
        if (lr.positionCount == 2) {
            lr.SetPosition(0, tonguePosition.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

}
