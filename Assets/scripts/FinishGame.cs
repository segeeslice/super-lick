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
using UnityEngine.UI;
using Unity.VisualScripting;

public class FinishGame : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject player;
    public Button resetButton;
    private List<GameObject> bugs;
    public int respawnDelayMs = 3000;
    public InitBugSpawns initBugSpawns;
    public UIUpdater uiUpdater;

    protected List<Renderer> bugRenderers;
    protected bool respawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugRenderers = new List<Renderer>();
        respawning = false;
        bugs = initBugSpawns.GetBugs();
        resetButton.gameObject.SetActive(false);
        uiUpdater.bugCount = 3;
        
        Cursor.lockState = CursorLockMode.Locked;
        
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

        Debug.Log(bugs);

        bugs.RemoveAll(bug => bug == null);




        if (bugs.Count == 0)
        {
            // get ref to the playerAudio on the player
            PlayerAudio playerAudio = other.GetComponentInChildren<PlayerAudio>();

            if (playerAudio != null)
            {
                playerAudio.PlayWinSound();
            }

            Debug.Log("Winner winner!");
            
            uiUpdater.StopTimer();

            Cursor.lockState = CursorLockMode.None;
            resetButton.gameObject.SetActive(true);
        }
        else
        {
            
        }

    }



    public void Reset()
    {

        uiUpdater.bugCount = 3;
        resetButton.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        initBugSpawns.DestroyAllBugs();
        initBugSpawns.SpawnBugs();
        player.transform.position = spawnPoint.position;
        respawning = false;
        uiUpdater.StartTimer();
    }
    
    public void Quit()
    {

        Application.Quit();
    }
}
