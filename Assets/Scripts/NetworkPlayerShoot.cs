using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class NetworkPlayerShoot : NetworkBehaviour {

	public NetworkPlayerWeapon RayGun;
	private bool dirRight = true;

	[SerializeField]
	private LayerMask mask;

	void Start()
	{
		
	}

	void Update()
	{
		if (isLocalPlayer) 
		{
			if (Input.GetKeyDown ("space")) {
				if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0)
					dirRight = true;
				else if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0)
					dirRight = false;
			
				FireTheLaser ();
			}
		}
	}

	void FireTheLaser()
	{
		Ray ray;
		RaycastHit _hit;

		if (dirRight) 
		{	
			ray = new Ray (transform.position, Vector3.right);
		} 
		else 
		{
			ray = new Ray (transform.position, Vector3.left);
		}

		if(Physics.Raycast(ray, out _hit, RayGun.range, mask))
		{
			Debug.Log ("We hit " + _hit.collider.name);
		}
				
	}
}
