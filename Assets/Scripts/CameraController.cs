using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraController : MonoBehaviour
{

   GameObject player;
    private Vector3 offset;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player").First(p => p.activeSelf);
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
