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
            Debug.Log("New Zone");
        }

    }

}
