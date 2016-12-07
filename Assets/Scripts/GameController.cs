using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
    List<Vector3> path;
    Vector3 minVertex; 
	// Use this for initialization
	void Start () {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        minVertex = meshGen.GetMinimumVertex();
        path = meshGen.GetWalls().Where(w=>w.z <= minVertex.z+5 && w.x > minVertex.x).ToList();
        Debug.Log(path[0]); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
