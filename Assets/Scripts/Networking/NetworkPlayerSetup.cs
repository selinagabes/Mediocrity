using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkPlayer))]
public class NetworkPlayerSetup : NetworkBehaviour {

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string localLayerName = "LocalPlayer";

    [SerializeField]
    GameObject playerUIprefab;
    private GameObject playerUIInstance;

	void Start () 
	{
		if (isLocalPlayer) 
		{
            GetComponent<NetworkPlayer>().PlayerSetup();
            this.GetComponentInChildren<TextMesh> ().text = "Player";

            //Create Player UI
            playerUIInstance = Instantiate(playerUIprefab);
            playerUIInstance.name = playerUIprefab.name;

            //Configure UI
            NetworkPlayerUI playerUI = playerUIInstance.GetComponent<NetworkPlayerUI>();
            if (playerUI == null)
                Debug.Log("No UI!");

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

    void OnDisable()
    {
        NetworkGameManager.DeRegisterPlayer(transform.name);

        Destroy(playerUIInstance);
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

	void AssignLocalPlayer()
	{
		gameObject.layer = LayerMask.NameToLayer (localLayerName);
	}

	void AssignRemotePlayer()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}
}
