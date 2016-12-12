using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();      
        
    }
    
    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * 10;
    }
    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.CompareTag("Tooth"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Death"))
        {
            Debug.Log("You dead");
            //other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("EndZone"))
        {
            gameObject.SetActive(false);
            var currentZone = GameObject.FindGameObjectWithTag("Zone");
            float nextZoneX = currentZone.GetComponentInChildren<MapGenerator>().width - currentZone.GetComponentInChildren<MeshGenerator>().wallHeight + currentZone.transform.position.x;
            Vector3 nextZonePos = new Vector3(nextZoneX, 0, 0);
            var nextZone = GameObject.FindGameObjectWithTag("ZoneManager");
            nextZone.GetComponent<ZoneGenerator>().RestartZone(nextZonePos);
            MeshGenerator nextMesh = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
            Vector3 nextMin = nextMesh.GetMinimumVertex();
            transform.position = new Vector3(nextMin.x + 5f, nextMin.z + 1f, 2.5f);
            gameObject.SetActive(true);
            //ONCE INSTANTIATED, TELEPORT TO THE MINIMUM VERTEX AND STUFF.......


            Debug.Log("New Zone");
        }

    }

}
