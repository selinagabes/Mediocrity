using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

/*==========================================
This script controls player shooting over
the network.
==========================================*/
public class NetworkPlayerShoot : NetworkBehaviour {

    //==================================
    // All declarations are local
    //==================================
    public NetworkPlayerWeapon RayGun;
	private bool dirRight = true;
	[SerializeField]
	private LayerMask mask;
	private string _id;

	void Start()
	{
		RegisterPlayer ();
	}

	void RegisterPlayer()
	{
        //What is our player going to be named
		_id = "Player " + GetComponent<NetworkIdentity> ().netId;
		transform.name = _id;
	}

	void Update()
	{
		if (isLocalPlayer) 
		{
            //Only show if the escape menu is NOT on
            if (!NetworkDCMenu.isOn)
            {
                if (Input.GetKeyDown("space"))
                {
                    if (CrossPlatformInputManager.GetAxis("Horizontal") > 0)
                        dirRight = true;
                    else if (CrossPlatformInputManager.GetAxis("Horizontal") < 0)
                        dirRight = false;

                    FireTheLaser();
                }
            }
		}
	}

    //========================================
    // OK! So, here we fire our gun on the client
    // side.  I am using Vector3.right and left
    // because they are relavent to the x acis and
    // do not care what way the cube or capsule 
    // is faceing.  I use a boolean dirRigh to determine
    // which we we faced last
    //========================================
    [Client]
	void FireTheLaser()
	{
		Ray ray;
		RaycastHit _hit;

		if (dirRight) 
		{	
			ray = new Ray (transform.position, new Vector3(1,0,0));
		} 
		else 
		{
			ray = new Ray (transform.position, new Vector3(-1,0,0));
		}

        if (Physics.Raycast(ray, out _hit, RayGun.range, mask))
        {
            //Tag the prefab of player to Player! (we check to see if it is our own player in NetworkPlayer
            if (_hit.collider.tag == "Player")
            {
                //Control the shooting by the server
                CmdShootHim(_hit.collider.name, RayGun.damage, transform.name);
            }
        }
	}

	[Command]
	void CmdShootHim(string _pid, int _dmg, string _source)
	{
		Debug.Log (_pid + " has been shot");

        NetworkPlayer _nPlayer = NetworkGameManager.GetPlayer(_pid);
        _nPlayer.RpcTakeDamage(_dmg,_source);
	}
}
