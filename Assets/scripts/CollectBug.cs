// Collects bugs!
// Attach to a component wiht a collider, configured as a trigger.
// Requires a child component that has a renderer (model)

using System;
using UnityEngine;

public class CollectBug : MonoBehaviour
{
    Renderer objectRenderer;
    public UIUpdater uiUpdater;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRenderer = GetComponentInChildren<Renderer>();
        if (objectRenderer == null)
        {
            throw new Exception("This script needs a child that has a renderer");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Player collected this! Shoop away
        if (other.CompareTag("Player"))
        {
            // get ref to the playerAudio on the player
            PlayerAudio playerAudio = other.GetComponentInChildren<PlayerAudio>();

            if (playerAudio != null)
            {
                playerAudio.PlayPickupSound();
            }

            uiUpdater.bugCount--;
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject); // Disable the renderer
    }
}
