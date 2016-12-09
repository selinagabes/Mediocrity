using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
    List<Vector3> path;
    Dictionary<Vector3, GameObject> ToothSpawns;
    Dictionary<Vector3, GameObject> StairSpawns;
    Vector3 minVertex;
    public GameObject Tooth;
    public GameObject TeethStairs;
    public GameObject Stairs;
   
	// Use this for initialization
	void Start () {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        minVertex = meshGen.GetMinimumVertex();
        path = meshGen.GetWalls().Where(w=>w.z <= minVertex.z+5 && w.x > minVertex.x).OrderBy(w=>w.x).ToList();
        ToothSpawns = new Dictionary<Vector3, GameObject>();
        StairSpawns = new Dictionary<Vector3, GameObject>();
        PlaceTeeth();
        PlaceStairs();
        ReadjustMap();  
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
            GameObject currentTooth= (GameObject)Instantiate(Tooth, spawnPoint, new Quaternion());
            ToothSpawns.Add(spawnPoint, currentTooth);
            min = max;
            max += (numOfVertexes / 20);
            index = Random.Range(min, max);
        }
    }
	// Update is called once per frame
	void PlaceStairs () {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();

        int numOfVertexes = path.Count;
        int min = (int)minVertex.x + 10;
        int max = numOfVertexes / 3;
        int index = Random.Range(min, max);

        for (int i = 1; i <= 3; i++)
        {
            Vector3 spawnPoint = new Vector3(path[index].x, path[index].z+2f, 2.5f);
            GameObject currentStairs =(GameObject) Instantiate(Stairs, spawnPoint, new Quaternion());
            StairSpawns.Add(spawnPoint, currentStairs);
            min = max+10;
            max += (numOfVertexes-10 / 3);
            index = Random.Range(min, max);
        }
    }
    void ReadjustMap()
    {
        ReplaceTeeth();
        ResetMeshBelowStairs();
    }

    void ReplaceTeeth()
    {
        foreach (var clone in StairSpawns)
        {
            if (ToothSpawns.Any(t => t.Key.x >= clone.Key.x - 6 && t.Key.x <= clone.Key.x + 6))
            {

                foreach (var tClone in ToothSpawns)
                {
                    if (tClone.Key.x >= clone.Key.x - 6 && tClone.Key.x <= clone.Key.x + 6)
                    {
                        Destroy(ToothSpawns[tClone.Key].gameObject);
                    }
                }
                Instantiate(TeethStairs, clone.Key, new Quaternion());
                Destroy(StairSpawns[clone.Key].gameObject);
            }
        }
    }
}
