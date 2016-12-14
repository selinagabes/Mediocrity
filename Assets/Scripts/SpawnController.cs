using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    public int numOfPlatforms = -1;
    public int numOfPowerUps = -1;
    public string SceneName;

    List<Vector3> path = new List<Vector3>();
    Dictionary<Vector3, GameObject> PlatformMap = new Dictionary<Vector3, GameObject>();
    Dictionary<Vector3, GameObject> PowerUpMap = new Dictionary<Vector3, GameObject>();


    Vector3 minVertex;
    Vector3 maxVertex;
    MeshGenerator meshGen;
    PowerUp _PowerUps;
    Platforms _Platforms;
    GameObject PlayerOne;
    void Start()
    {
        Spawn();
        // Invoke("Spawn", Time.deltaTime);
    }
    public void Spawn()
    {

        _PowerUps = new PowerUp();
        _Platforms = new Platforms();
        PlayerOne = (GameObject)Resources.Load("Prefabs/Player");
        meshGen = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
        minVertex = meshGen.GetMinimumVertex();
        maxVertex = meshGen.GetMaximumVertex();
        path = meshGen.GetWalls().Where(w => w.z <= minVertex.z + 5 && w.x > minVertex.x).OrderBy(w => w.x).ToList();
        SpawnPlayerOne();
        PlacePowerUps();
        PlacePlatforms();

    }

    void SpawnPlayerOne()
    {

      
       Vector3 spawnPoint = new Vector3(minVertex.x + 5, minVertex.z + 1, 2.5f);
        // Debug.Log(spawnPoint);      
        PlayerOne = (GameObject)Instantiate(PlayerOne, spawnPoint, new Quaternion());


        PlayerOne.SetActive(true);
    }
    void PlacePlatforms()
    {
        GameObject[] PlatformArray = _Platforms.GetAllPlatoforms(SceneName).ToArray();
        int numOfVertices = path.Count;
        int pathMin = 10;
        int pathMax = numOfPlatforms == -1 ? numOfVertices / PlatformArray.Count() : numOfVertices / numOfPlatforms;
        int randomPathIndex = Random.Range(pathMin, pathMax);
        int randomPlatformIndex = Random.Range(0, PlatformArray.Count() - 1);

        for (int i = 0; i < numOfPlatforms; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint(10, PlatformMap);
            GameObject platform = PlatformArray[randomPlatformIndex];
            platform = (GameObject)Instantiate(platform, spawnPoint, new Quaternion());
            PlatformMap.Add(spawnPoint, platform);

            randomPlatformIndex = Random.Range(0, PlatformArray.Count() - 1);

        }
    }
    void PlacePowerUps()
    {
        GameObject[] PowerArray = _PowerUps.GetAllPowerUps().ToArray();
        int numOfVertices = path.Count;
        int pathMin = 10;
        int pathMax = numOfPowerUps == -1 ? numOfVertices / PowerArray.Count() : numOfVertices / numOfPowerUps;
        int randomPathIndex = Random.Range(pathMin, pathMax);
        int randomPowerndex = Random.Range(0, PowerArray.Count() - 1);

        for (int i = 0; i < numOfPowerUps; i++)
        {
            Vector3 spawnPoint = GetSpawnPoint(3, PowerUpMap);
            GameObject power = PowerArray[randomPowerndex];
            power = (GameObject)Instantiate(power, spawnPoint, new Quaternion());
            PowerUpMap.Add(spawnPoint, power);

            randomPowerndex = Random.Range(0, PowerArray.Count() - 1);

        }
    }


    Vector3 GetSpawnPoint(int spacing, Dictionary<Vector3, GameObject> spawns)
    {
        int min = spacing;
        int max = path.Count - spacing;
        int numOfVertexes = path.Count;
        int index = Random.Range(min, max);
        Vector3 spawnPoint = new Vector3();

        //check if it's out of range
        if (index > numOfVertexes)
            index %= numOfVertexes;
        spawnPoint = new Vector3(path[index].x, path[index].z + 1, 3);


        //check if it is in range of another staircase

        while (spawnPoint.x >= maxVertex.x - (spacing) * 2     //if it's too far ahead
            || spawns.ContainsKey(spawnPoint)
            // || PortalSpawns.Any(p=>p.Key.x <= spawnPoint.x+spacing)     //or it's too close to the portal
            || spawns.Any(s => s.Key.x >= spawnPoint.x - spacing
                                && s.Key.x <= spawnPoint.x + spacing)   //or it's too close to other instantiations
            || (spawnPoint.x <= minVertex.x + spacing + 5))                 //or its too soon
        {
            index = Random.Range(min, max);
            spawnPoint = new Vector3(path[index].x, path[index].z + 1, 3);
        }

        return spawnPoint;
    }


    public class Platforms
    {
        public GameObject Crumblers;
        public GameObject TeethStairs;
        public GameObject Couch;
        public GameObject BookShelf;
        public Platforms()
        {
            TeethStairs = (GameObject)Resources.Load("Prefabs/Platforms/TeethStairs");
            BookShelf = (GameObject)Resources.Load("Prefabs/Platforms/BookCases");
            Couch = (GameObject)Resources.Load("Prefabs/Platforms/Couches");
            Crumblers = (GameObject)Resources.Load("Prefabs/Platforms/Crumblers");
        }

        public List<GameObject> GetAllPlatoforms(string scene)
        {
            List<GameObject> platforms = new List<GameObject>();
            if (scene == "Grandma" || scene == "Gen")
                platforms.Add(Couch);
            if (scene == "Library" || scene == "Gen")
                platforms.Add(BookShelf);


            platforms.Add(TeethStairs);

            return platforms;
        }


    }


    public class PowerUp
    {

        public GameObject Tooth;
        public GameObject Denture;
        public GameObject Portal;
        public GameObject Clock;
        public GameObject Shadow;

        public PowerUp()
        {
            Portal = (GameObject)Resources.Load("Prefabs/PowerUps/Portal");
            Tooth = (GameObject)Resources.Load("Prefabs/PowerUps/Tooth");
            Denture = (GameObject)Resources.Load("Prefabs/PowerUps/Denture");
            Clock = (GameObject)Resources.Load("Prefabs/PowerUps/TimeFreeze");
            Shadow = (GameObject)Resources.Load("Prefabs/PowerUps/Shadow");
        }

        public List<GameObject> GetAllPowerUps()
        {
            List<GameObject> powerUps = new List<GameObject>();
            powerUps.Add(Portal);
            powerUps.Add(Tooth);
            powerUps.Add(Tooth);
            powerUps.Add(Clock);
            powerUps.Add(Shadow);
            return powerUps;

        }

    }
}
