using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;

//====================================
// Nothing to see here
//====================================
public class NetworkDCMenu : MonoBehaviour {

    public static bool isOn = false;

    private CustomNetworkManager nManager;

    void Start()
    {
        nManager = (CustomNetworkManager)NetworkManager.singleton;
    }

	public void Disconnect()
    {
        nManager.StopHost();
    }
}
