using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkPlayer))]
public class NetworkPlayerSetup : NetworkBehaviour {

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string localLayerName = "LocalPlayer";

	void Start () 
	{
		if (isLocalPlayer) 
		{
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

        GetComponent<NetworkPlayer>().Setup();
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
        NetworkPlayer _nPlayer = GetComponent<NetworkPlayer>();

        NetworkGameManager.RegisterPlayer(_netID, _nPlayer);
	}

    void OnDisable()
    {
        NetworkGameManager.DeRegisterPlayer(transform.name);
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
}
