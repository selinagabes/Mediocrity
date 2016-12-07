using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    int count = 0;
    Rigidbody rigidbody;
    Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();      
        Debug.Log("In Player Start");
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
}
