using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraController : MonoBehaviour
{

   GameObject player;
    private Vector3 offset;
    float distance = 20f;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player").First(p => p.activeSelf);
        transform.position = new Vector3(player.transform.position.x + distance, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x + distance, transform.position.y, transform.position.z);
    }
}
