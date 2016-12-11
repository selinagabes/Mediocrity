using UnityEngine;
using System.Collections;

//=====================================
// Scoreboard for the Game
//=====================================
public class NetworkScoreboard : MonoBehaviour {

	public void TurnOnSB()
    {
        //When we call this method from the NetworkPlayerUI script, let's get a list of all players!
        Debug.Log("Scoreboard On!");
        NetworkPlayer[] nPlayers = NetworkGameManager.GetPlayers();

        for (int i = 0; i < nPlayers.Length; i++)
        {
            Debug.Log(nPlayers[i].name);
        }
    }

    public void TurnOffSB()
    {

    }
}
