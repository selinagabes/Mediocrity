using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string localLayerName = "LocalPlayer";

	void Start () 
	{
		if (isLocalPlayer) 
		{
			GetComponent<NetworkPlayerMove> ().enabled = true;
			this.GetComponentInChildren<TextMesh> ().text = "Player";
		}

		SetupCamera ();

		//Set up Layers
		if (isLocalPlayer) 
		{
			AssignLocalPlayer ();
		} 
		else 
		{
			AssignRemotePlayer ();
		}
	}

	void SetupCamera()
	{
		if (isLocalPlayer)
		{
			Camera.main.transform.position = this.transform.position - this.transform.forward * 25 + this.transform.up * 3;
			Camera.main.transform.LookAt(this.transform.position);
			Camera.main.transform.parent = this.transform;
		}

	}

	void AssignLocalPlayer()
	{
		gameObject.layer = LayerMask.NameToLayer (localLayerName);
	}

	void AssignRemotePlayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void Update()
	{
		
	}
}
