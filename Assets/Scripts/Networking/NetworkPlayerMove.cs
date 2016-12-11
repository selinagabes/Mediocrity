using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

//=======================
// Move the player!
//=======================
public class NetworkPlayerMove : MonoBehaviour {
	
	public Rigidbody rigidbody;
	public Vector3 velocity;

	void Start()
	{
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Update()
	{
        if(!NetworkDCMenu.isOn)
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * 10;
	}

	void FixedUpdate()
	{
		rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
	}
}
