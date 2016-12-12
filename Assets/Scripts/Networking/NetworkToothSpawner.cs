using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*==============================================
Originally built to spawn teeth, we figured it
would be super easy to control game functions
from a seperate class other than the player
scripts.  Wait until someone collects all the
teeth, and BOOM, they win!
==============================================*/
[RequireComponent(typeof(NetworkGameManager))]
public class NetworkToothSpawner : NetworkBehaviour {

    [SerializeField]
    private GameObject toothPrefab;
    [SerializeField]
    private float waitTime = 0.5f;
    [SerializeField]
    private float gameTime = 120f;

    private float toothTime;
    private float gameTimer;
    private int playerCount = 0;
    private bool gameOn = false;
    private bool gameSwitch = true;

    public bool GetGameStatusBoolean()
    {
        return gameSwitch;
    }

    [SerializeField]
    private int seconds;

    public int GetGameTime()
    {
        return seconds;
    }

    void Start()
    {
        toothTime = Time.time;
        gameTimer = Time.time;
    }

	void Update ()
    {
        //Make sure we have 2 players connected
        if(NetworkGameManager.GetPlayers().Length == 2)
        {
            if (gameSwitch)
            {
                StartCoroutine(StartGameIn3());
            }
        }

        if(gameOn)
        {
            //Call the spawn function every half second!
            if(Time.time > (toothTime + waitTime))
            {
                toothTime = Time.time;
                CmdSpawnTeeth();
            }

            if (Time.time > (gameTimer + 1f))
            {
                gameTimer = Time.time;
                seconds++;
            }
            
        }

        if(seconds >= (int)gameTime)
        {
            gameOn = false;
            gameSwitch = false;
        }
	}

    IEnumerator StartGameIn3()
    {
        yield return new WaitForSeconds(1f);
        gameOn = true;
    }

    //Spawn Teeth Every Second in Random Places!
    [Command]
    void CmdSpawnTeeth()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8.0f, 8.0f), 0.0f, 0.0f);
        Quaternion spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);
        RpcSpawnTeeth(spawnPosition, spawnRotation);
        Debug.Log("Spawning Tooth");
    }

    [ClientRpc]
    void RpcSpawnTeeth(Vector3 sp, Quaternion sr)
    {
        var enemy = (GameObject)Instantiate(toothPrefab, sp, sr);
        NetworkServer.Spawn(enemy);
    }
}
