using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ZoneGenerator : MonoBehaviour
{
    private GameObject ZoneInstance;
    private int NumOfZones = 0;
    public int MaxNumberOfZones = 10;
    Vector3 Position = new Vector3();
    GameObject ZonePrefab;
  
    void Awake()
    {
        BeginZone();
    }
    private void BeginZone()
    {
        ZonePrefab = (GameObject)Resources.Load("Prefabs/Zone");
        ZoneInstance = (GameObject)Instantiate(ZonePrefab, Position, new Quaternion());
    }
    public void RestartZone(Vector3 position)
    {
        Position = position;
        NumOfZones++;
        ZoneInstance.SetActive(false);
        Destroy(ZoneInstance.gameObject);
        BeginZone();
    }  
    
}
