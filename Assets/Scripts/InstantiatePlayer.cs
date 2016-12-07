using UnityEngine;
using System.Collections;

public class InstantiatePlayer : MonoBehaviour {
    Vector3 spawnPoint;
    public bool IsTwoPlayer;
    public GameObject PlayerTwo;
    public GameObject PlayerOne;
    // Use this for initialization
    void Start () {
        SpawnPlayerOne();
        if (IsTwoPlayer)
            SpawnPlayerTwo();
	}
    void SpawnPlayerOne()
    {
       
        GameObject path = GameObject.FindGameObjectWithTag("LevelPath");
        Vector3 inverseSpawnPoint = path.GetComponentInChildren<MeshGenerator>().GetMinimumVertex();
        spawnPoint = new Vector3(inverseSpawnPoint.x, inverseSpawnPoint.z+1, path.GetComponentInChildren<MeshGenerator>().wallHeight/2);
       // Debug.Log(spawnPoint);      
        PlayerOne = (GameObject)Instantiate(PlayerOne, spawnPoint, new Quaternion());
           
        
        PlayerOne.SetActive(true);
    }
    void SpawnPlayerTwo()
    {
        Vector3 spawnPt = new Vector3(spawnPoint.x +5, spawnPoint.y, spawnPoint.z);
        PlayerTwo = (GameObject)Instantiate(PlayerOne, spawnPt, new Quaternion());
        PlayerTwo.SetActive(true);
    }
    
}
