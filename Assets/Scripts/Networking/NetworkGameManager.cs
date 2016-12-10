using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkGameManager : MonoBehaviour {

    public static NetworkGameManager instance;

    public NetworkMatchSettings matchSettings;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than 1 game manager in scene");
        }
        else
        {
            instance = this;
        }
    }

    #region Player Tracking

    private const string PLAYER_ID_PREFIX = "Player ";

	private static Dictionary<string,NetworkPlayer> players = new Dictionary<string, NetworkPlayer>(); 

	public static void RegisterPlayer(string _netID, NetworkPlayer _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add (_playerID, _player);
		_player.transform.name = _playerID;
	}

    public static NetworkPlayer GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    public static void DeRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    #endregion
    
}
