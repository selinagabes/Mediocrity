using UnityEngine;
using System.Collections;

public class Interpolater : MonoBehaviour {

    public GameObject slerper;
 
    void Update()
    {
        transform.position = new Vector3(slerper.transform.position.x, transform.position.y, transform.position.z);
    }
}
