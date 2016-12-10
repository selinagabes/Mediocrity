using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    Dictionary<Vector3, GameObject> ToothSpawns= new Dictionary<Vector3, GameObject>();
    Dictionary<Vector3, GameObject> StairSpawns = new Dictionary<Vector3, GameObject>();
    Dictionary<Vector3, GameObject> PortalSpawns = new Dictionary<Vector3, GameObject>();

    Vector3 minVertex;
    public GameObject Death;
    public GameObject Tooth;
    public GameObject TeethStairs;
    public GameObject Stairs;
    public GameObject Portal;
 

    // Use this for initialization
    void Start()
    {
        Spawn();
    }
   
    public void Spawn()
    {
        ToothSpawns.Clear();
        StairSpawns.Clear();
        path.Clear();
        PortalSpawns.Clear();
    //TODO:: Find a way to get this dude without haardcoding it
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        minVertex = meshGen.GetMinimumVertex();
        path = meshGen.GetWalls().Where(w => w.z <= minVertex.z + 5 && w.x > minVertex.x).OrderBy(w => w.x).ToList();
        PlacePortal();
        PlaceTeeth();       
        PlaceStairs();      
        ReadjustMap();
    }
   
    void PlacePortal()
    {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        Vector3 maxVertex = meshGen.GetMaximumVertex();
        List<Vector3> beforeWallPath = path.Where(p => p.x >= maxVertex.x-6 & p.z <= maxVertex.z+5).ToList();
        maxVertex = beforeWallPath.First();
        Vector3 spawnPoint = new Vector3(maxVertex.x, maxVertex.z+1, 2.5f);
        GameObject portalSpawn = (GameObject)Instantiate(Portal, spawnPoint, new Quaternion());
        PortalSpawns.Add(spawnPoint, portalSpawn);
    }
    void PlaceTeeth()
    {
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();

        int numOfVertices= path.Count;

        for (int i = 1; i < 19; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint(3, ToothSpawns);

            GameObject currentTooth = (GameObject)Instantiate(Tooth, spawnPoint, new Quaternion());
            ToothSpawns.Add(spawnPoint, currentTooth);
          
        }
    }
    // Update is called once per frame
    void PlaceStairs()
    {
      //  MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        int numOfVertices = path.Count;

        int min = 10;
        int max = numOfVertices / 3;
        int index = Random.Range(min, max);

        for (int i = 1; i <= 3; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint(10, StairSpawns);

            GameObject currentStairs = (GameObject)Instantiate(Stairs, spawnPoint, new Quaternion());
            StairSpawns.Add(spawnPoint, currentStairs);
            min = max;
            max += (numOfVertices / 3);
            index = Random.Range(min, max);
            index = index < numOfVertices ? index : index - (min / i);
        
        }
    }
    Vector3 GetSpawnPoint(int spacing, Dictionary<Vector3, GameObject> spawns)
    {
        int min = spacing;
        int max = path.Count - spacing;
        int numOfVertexes = path.Count;
        int index = Random.Range(min, max);
        Vector3 spawnPoint = new Vector3();
        MeshGenerator meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        //check if it's out of range
        if (index > numOfVertexes)
            index %= numOfVertexes;
        spawnPoint = new Vector3(path[index].x, path[index].z + 1, 3);


        //check if it is in range of another staircase

        while (spawnPoint.x >= meshGen.GetMaximumVertex().x -(spacing)*2     //if it's too far ahead
            || spawns.ContainsKey(spawnPoint)
           // || PortalSpawns.Any(p=>p.Key.x <= spawnPoint.x+spacing)     //or it's too close to the portal
            || spawns.Any(s => s.Key.x >= spawnPoint.x - spacing        
                                && s.Key.x <= spawnPoint.x + spacing)   //or it's too close to other instantiations
            || (spawnPoint.x <= minVertex.x+spacing+5))                 //or its too soon
        {
            index = Random.Range(min, max);
            spawnPoint = new Vector3(path[index].x, path[index].z + 1,3);
        }
        if(PortalSpawns.Any(p => p.Key.x <= spawnPoint.x + spacing))
        {
            Debug.Log("suh");
        }
        return spawnPoint;
    }
    void ReadjustMap()
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
                Vector3 spawnPoint = new Vector3(clone.Key.x + 2, clone.Key.y - 5.5f, clone.Key.z + 6.5f);
                Instantiate(Death, spawnPoint, new Quaternion());
                Destroy(StairSpawns[clone.Key].gameObject);
            }
        }

    }

   
    void ReplaceTeeth()
    {
       
    }
}
