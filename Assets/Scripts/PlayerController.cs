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
            GameObject Zone = other.gameObject.GetComponent<ZoneGenerator>().Zone;
            MapGenerator currentMap = Zone.GetComponentInChildren<MapGenerator>();
            MeshGenerator meshGen1 = Zone.GetComponentInChildren<MeshGenerator>();
           meshGen1.wallVertices.Clear();
            float newX = currentMap.width / 2 + 0.5f;
            Vector3 pos = new Vector3(2*newX, 0, 0);
            //DELETE THE OLD ONE .. KEEP THE PORTAL THERE AND INSTATION AFTER
            GameObject currentZone = (GameObject)Instantiate(Zone, pos, new Quaternion());
            MeshGenerator meshGen = currentZone.GetComponentInChildren<MeshGenerator>();
          //  meshGen.wallVertices.Clear();
            Vector3 teleport = meshGen.GetMinimumVertex();
            teleport = new Vector3(teleport.x + currentZone.transform.position.x, teleport.z, 2.5f);
            transform.position = teleport;
            Debug.Log("New Zone");
        }

    }

}
