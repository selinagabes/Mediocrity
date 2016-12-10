using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class NetworkPlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";
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
		_id = "Player " + GetComponent<NetworkIdentity> ().netId;
		transform.name = _id;
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
            if (_hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerHasBeenShot(_hit.collider.name, RayGun.damage);
            }
        }
	}

	[Command]
	void CmdPlayerHasBeenShot(string _pid, int _dmg)
	{
		Debug.Log (_pid + " has been shot");

        NetworkPlayer _nPlayer = NetworkGameManager.GetPlayer(_pid);
        _nPlayer.RpcTakeDamage(_dmg);
	}
}
