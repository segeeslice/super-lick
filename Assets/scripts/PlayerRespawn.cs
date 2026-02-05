// Attach this script to an object with a Collider and "Is Trigger" checked.
// Once colliding, it will respawn the player to the provided spawn point.
//
// Currently does not modify momentum in any way... which would only be problematic
// if we eventually decide to add fall damage.

using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.transform.position = spawnPoint.position;
        }
    }
}
