using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
    List<Vector3> path;
    Vector3 minVertex;
    public GameObject Tooth;
	// Use this for initialization
	void Start () {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        minVertex = meshGen.GetMinimumVertex();
        path = meshGen.GetWalls().Where(w=>w.z <= minVertex.z+5 && w.x > minVertex.x).OrderBy(w=>w.x).ToList();
        
        PlaceTeeth();        
	}
	
    void PlaceTeeth()
    {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();

        int numOfVertexes = path.Count;
        int min = 3;
        int max = numOfVertexes / 20;
        int index = Random.Range(min, max);

        
        
        for (int i =1; i< 19; i++)
        {
            Vector3 spawnPoint = new Vector3(path[index].x, path[index].z + 1, meshGen.wallHeight / 2);
            Instantiate(Tooth, spawnPoint, new Quaternion());
            min = max;
            max += (numOfVertexes / 20);
            index = Random.Range(min, max);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
