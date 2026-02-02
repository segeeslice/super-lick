using UnityEngine;

public class InitCamera : MonoBehaviour
{
    public Transform cameraPosition;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
