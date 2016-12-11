using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

/*=====================================================
CustomNetworkManager is my shining jewel.  The only
example unity had of a NetworkLobbyManager came with
a bunch of prefabs of a UI and although it did literally
everything you could want matchmaking wise, it felt like
a cheat, AND it was serverely uncustomizable.  Through 
the use of a few tutorials and what not, I figured the
best way to get an easy custom UI script would be
to just extend the NetworkManager!  The Unity API 
helped as well.
=====================================================*/

public class CustomNetworkManager : NetworkManager {

	public void CreateHost()
	{
		SetPort ();
		singleton.StartHost ();
	}

	public void ConnectToHost()
	{
        SetPort();
        singleton.networkAddress = GameObject.Find("inputIP").transform.FindChild("Text").GetComponent<Text>().text;
        Debug.Log(GameObject.Find("inputIP").transform.FindChild("Text").GetComponent<Text>().text);
		try
        {
            singleton.StartClient();
        }
        catch
        {
            Debug.Log("Unable to start client!");
        }
	}
			
	void SetPort()
	{
		singleton.networkPort = 7777;
	}

    //==============================================
    // Unity warns me and tells me that this is
    // going to be phased out and is depreciated
    //==============================================
	void OnLevelWasLoaded(int level)
	{
		if (level == 2) 
		{
            //Reset the listeners everytime we go back
			StartCoroutine(SetUpConnectionSceneButtons());
		}
	}

	IEnumerator SetUpConnectionSceneButtons()
	{
        yield return new WaitForSeconds(0.5f);
		GameObject.Find ("HostButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("HostButton").GetComponent<Button> ().onClick.AddListener (CreateHost);
		GameObject.Find ("JoinButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("JoinButton").GetComponent<Button> ().onClick.AddListener (ConnectToHost);
	}
}
