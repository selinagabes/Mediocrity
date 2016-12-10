using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ZoneGenerator : MonoBehaviour
{
    public GameObject Zone; 
   void GenerateNextZone()
    {

        List<GameObject> layers = Zone.GetComponentsInChildren<GameObject>().ToList();
        foreach(GameObject map in layers)
        {
            map.GetComponent<MapGenerator>().GenerateMap();
        }
    }
   
    
}
