using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

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
            Debug.Log("nope");
        }
	}
			
	void SetPort()
	{
		singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded(int level)
	{
		if (level == 2) 
		{
			StartCoroutine(SetUpConnectionSceneButtons());
		} 
		else 
		{
			SetUpPlaySceneButtons ();
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

	void SetUpPlaySceneButtons()
	{
		GameObject.Find ("BootButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find("BootButton").GetComponent<Button> ().onClick.AddListener (NetworkManager.singleton.StopHost);
	}
}
