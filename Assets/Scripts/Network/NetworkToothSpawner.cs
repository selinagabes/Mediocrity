using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*==============================================
Originally built to spawn teeth, we figured it
would be super easy to control game functions
from a seperate class other than the player
scripts.  Wait until someone collects all the
teeth, and BOOM, they win!

So now this doubles as the Tooth Spawner
as well as the Game Timer, calling the 
appropriate function for each players UI
UPDATE:  This has mostly been moved to
the player UI for cleanliness as the UI
and the NetwrokPlayer class work closely
to accomplish most of our online stuff.
==============================================*/
public class NetworkToothSpawner : NetworkBehaviour {

    [SerializeField]
    private GameObject toothPrefab;
    [SerializeField]
    private float waitTime = 0.5f;
    [SerializeField]
    private float gameTime = 120f;

    //============================
    // All our private stuff!
    //============================
    private float toothTime;
    private float gameTimer;
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

    /*=====================================
    I had to throw this in here because when
    both the clients connected, one GUI would
    be ahead of the other.  By making the
    spawner wait a few seconds, it seemed
    to have cleared up any issues!
    ======================================*/
    IEnumerator StartGameIn3()
    {
        yield return new WaitForSeconds(1.5f);
        gameOn = true;
    }

    //Spawn Teeth Every Second in Random Places!
    //Unity has a great tutorial on this! (for spawning enemies, but essentially the same thing
    [Command]
    void CmdSpawnTeeth()
    {
        if (isServer)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-40.0f, 40.0f), 5.0f, 1f);
            Quaternion spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);
            RpcSpawnTeeth(spawnPosition, spawnRotation);
            Debug.Log("Spawning Tooth");
        }
    }

    [ClientRpc]
    void RpcSpawnTeeth(Vector3 sp, Quaternion sr)
    {
            var enemy = (GameObject)Instantiate(toothPrefab, sp, sr);
            NetworkServer.Spawn(enemy);
    }
}
