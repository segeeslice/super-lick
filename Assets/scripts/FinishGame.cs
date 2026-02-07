// On trigger, checks if the player has successfully completed the game (caught all bugs)
// Attach this to an object with a collider that has "Is Trigger" enabled.
//
// Only checks end conditions when hit by a collider with the tag "Player".
// Provide it with a respawn location, for restarting, and all top-level bug objects.
// Bugs must have a object that is a renderer, for checking if the bug was collected.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject player;
    public List<GameObject> bugs;
    public int respawnDelayMs = 3000;

    protected List<Renderer> bugRenderers;
    protected bool respawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugRenderers = new List<Renderer>();
        respawning = false;

        foreach (GameObject bug in bugs)
        {
            Renderer objectRenderer = bug.GetComponentInChildren<Renderer>();
            if (objectRenderer is null)
            {
                throw new Exception("Received bug without object renderer");
            }
            bugRenderers.Add(objectRenderer);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (respawning)
        {
            return;
        }

        if (other.tag != "Player")
        {
            return;
        }

        bool allBugsGathered = bugRenderers.All(r => !r.enabled);
        if (allBugsGathered)
        {
            Debug.Log("Winner winner!");
        }
        else
        {
            Debug.Log("You missed one, idiot!");
        }

        respawning = true;
        DelayedReset();
    }

    async void DelayedReset()
    {
        await Task.Delay(respawnDelayMs);
        bugRenderers.ForEach(r => r.enabled = true);
        player.transform.position = spawnPoint.position;
        respawning = false;
    }
}
