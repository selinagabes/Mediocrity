using UnityEngine;
using System.Collections;

public class InstantiatePlayer : MonoBehaviour
{
    Vector3 spawnPoint;    
    public GameObject PlayerOne;
    // Use this for initialization
    void Start () {
        SpawnPlayerOne();
        PlayerOne = (GameObject)Resources.Load("Prefabs/Player");
	}
    void SpawnPlayerOne()
    {
        GameObject path = GameObject.FindGameObjectWithTag("LevelPath");
        Vector3 inverseSpawnPoint = path.GetComponentInChildren<MeshGenerator>().GetMinimumVertex();
        spawnPoint = new Vector3(inverseSpawnPoint.x+5, inverseSpawnPoint.z+1, path.GetComponentInChildren<MeshGenerator>().wallHeight/2);
       // Debug.Log(spawnPoint);      
        PlayerOne = (GameObject)Instantiate(PlayerOne, spawnPoint, new Quaternion());
        PlayerOne.SetActive(true);
    }
}
