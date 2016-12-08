using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public void StartupHost()
	{
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame()
	{
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	void SetIPAddress()
	{
		string ipAddress = GameObject.Find ("InputIP").transform.FindChild ("Text").GetComponent<Text> ().text;
		NetworkManager.singleton.networkAddress = ipAddress;
	}
			
	void SetPort()
	{
		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded(int level)
	{
		if (level == 2) 
		{
			SetUpConnectionSceneButtons ();
		} 
		else 
		{
			SetUpPlaySceneButtons ();
		}
	}

	void SetUpConnectionSceneButtons()
	{
		GameObject.Find ("HostButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("HostButton").GetComponent<Button> ().onClick.AddListener (StartupHost);
		GameObject.Find ("JoinButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("JoinButton").GetComponent<Button> ().onClick.AddListener (JoinGame);
	}

	void SetUpPlaySceneButtons()
	{
		GameObject.Find ("BootButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("BootButton").GetComponent<Button> ().onClick.AddListener (NetworkManager.singleton.StopHost);
	}
}
