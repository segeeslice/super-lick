using System.Collections.Generic;
using UnityEngine;

public class InitBugSpawns : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject[] spawnPoints;
    public List<GameObject> bugs;
    public GameObject bugPrefab;
    public UIUpdater uiUpdater;

    void Start()
    {
        SpawnBugs();

    }
    
    public void SpawnBugs()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            GameObject bug = Instantiate(bugPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            bug.gameObject.GetComponent<CollectBug>().uiUpdater = uiUpdater;
            bugs.Add(bug);
        }
    }
    
    public List<GameObject> GetBugs()
    {
        return bugs;
    }

    public void DestroyAllBugs()
    {
        foreach(GameObject bug in bugs)
        {
            Destroy(bug);
        }
    }
    


}
