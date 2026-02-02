using Unity.Mathematics;
using UnityEngine;

public class spawnPlayer : MonoBehaviour
{

    public GameObject playerPrefab;
    public Vector3 spawnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(playerPrefab, spawnPosition, quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
