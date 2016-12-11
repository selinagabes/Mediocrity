using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*=======================================================
This is where we make a directory of all the players
connected to the server.  This was one of the few parts
I had trouble with, however I found an excellent tutorial
over at Brackeys.com, and he explains how to use this quite
nicely.

Here we put the players in a dictionary using Player 1 or 
Player 2, or Player 3, etc, as the key, and this is how 
we communicate who shot who, who died, etc, and make the
whole game possible.
========================================================*/

public class NetworkGameManager : MonoBehaviour {

    public static NetworkGameManager instance;
    
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

	private static Dictionary<string,NetworkPlayer> players = new Dictionary<string, NetworkPlayer>(); 

	public static void RegisterPlayer(string _netID, NetworkPlayer _player)
	{
		string _playerID = "Player " + _netID;
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

    public static NetworkPlayer[] GetPlayers()
    {
        return players.Values.ToArray();
    }
}
