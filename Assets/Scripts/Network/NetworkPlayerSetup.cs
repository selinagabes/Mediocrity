using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*====================================================
Here we set up the Network Player!  
Set up things like;
Camera
Name
UI
Proper Layers for remote and local players
====================================================*/
[RequireComponent(typeof(NetworkPlayer))]
public class NetworkPlayerSetup : NetworkBehaviour {

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string localLayerName = "LocalPlayer";

	void Start () 
	{
        //I found if I didn't always check I would get buggy behaviour on both instances, moreso the client
		if (isLocalPlayer) 
		{
            GetComponent<NetworkPlayer>().PlayerSetup();
            this.GetComponentInChildren<TextMesh>().text = "Player";
        }

        //Set Up Camera
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

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
        NetworkPlayer _nPlayer = GetComponent<NetworkPlayer>();

        NetworkGameManager.RegisterPlayer(_netID, _nPlayer);
	}
    //======================================
    // If DC'd or the client dies, she gone!
    //======================================
    void OnDisable()
    {
        NetworkGameManager.DeRegisterPlayer(transform.name);
    }

	public void SetupCamera()
	{
		if (isLocalPlayer)
		{
			Camera.main.transform.position = this.transform.position - this.transform.forward * 25 + this.transform.up * 3;
			Camera.main.transform.LookAt(this.transform.position);
			Camera.main.transform.parent = this.transform;
		}

	}

    //====================================
    // Assign both players to a layer on
    // each client, and they should be
    // different on both!
    //====================================
	void AssignLocalPlayer()
	{
		gameObject.layer = LayerMask.NameToLayer (localLayerName);
	}

	void AssignRemotePlayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}
}
