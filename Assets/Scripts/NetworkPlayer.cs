using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour {
	
	public int playerNumber;

	void Start () 
	{
		if (isLocalPlayer)
			GetComponent<NetworkPlayerMove> ().enabled = true;

		if (isLocalPlayer) //change to player prefs
			this.GetComponentInChildren<TextMesh> ().text = "Player";
	}

	void Update()
	{
		
	}
}
