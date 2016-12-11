using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*================================================
NetowrkPlayer is our main player class.  no setup
is done here, but rather everything that controls
the player in game in done here.
=================================================*/
public class NetworkPlayer : NetworkBehaviour
{
    //========================================
    // Local Stuff
    //========================================
    public int kills;
    public int deaths;
    public float maxHealth = 100f;
    [SerializeField]
    private Behaviour[] disableWhenDead;
    private bool[] wasEnabled;
    public GameObject deathEffect;
    public GameObject spawnEffect;
    private bool FirstSetup = true;
    [SerializeField]
    GameObject playerUIprefab;
    private NetworkPlayerUI playerUI;
    private GameObject playerUIInstance;

    //========================================
    // Netowrk Stuff
    //========================================
    [SyncVar]
    private float currentHealth;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    //========================================
    // Player Setup
    // Call the server command, to tell the
    // client to set stuff up!
    //========================================
    public void PlayerSetup()
    {
        //Broadcast the setup
        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        //========================================
        // Get the status of all things attached
        // to the player the first time they spawn
        // so we can can save it and re enable it
        // on a respawn call
        //========================================
        if (FirstSetup)
        {
            wasEnabled = new bool[disableWhenDead.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableWhenDead[i].enabled;
            }
            FirstSetup = false;

            if (isLocalPlayer)
            {
                //Create Player UI
                playerUIInstance = Instantiate(playerUIprefab);
                playerUIInstance.name = playerUIprefab.name;

                //Configure UI
                playerUI = playerUIInstance.GetComponent<NetworkPlayerUI>();
                if (playerUI == null)
                    Debug.Log("No UI!");

                //Link the UI for things like healthbar or scoreboard (if created)
                playerUI.SetPlayer(this);
            }
        }
        SetStatsAndStuff();
    }

    //====================================
    // Enable everything we need on spawn
    //====================================
    public void SetStatsAndStuff()
    {
        isDead = false;

        //Enable Functions
        for (int i = 0; i < disableWhenDead.Length; i++)
        {
            disableWhenDead[i].enabled = wasEnabled[i];
        }

        //Enable Movement
        if(isLocalPlayer)
            GetComponent<NetworkPlayerMove>().enabled = true;

        //Spawn Effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        currentHealth = maxHealth;
    }

    [ClientRpc]
    public void RpcTakeDamage(float _amt, string _source)
    {
        if (isDead)
            return;

        currentHealth -= _amt;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            Killit(_source);
        }
    }

    //====================================
    // Disable everything on death
    //====================================
    private void Killit(string _source)
    {
        isDead = true;

        //Add a kill to the source of the raycast
        NetworkPlayer sourcePlayer = NetworkGameManager.GetPlayer(_source);
        if(sourcePlayer != null)
        {
            sourcePlayer.kills++;
        }

        deaths++;

        Debug.Log(transform.name + " is dead");

        //Disable Components
        for (int i = 0; i < disableWhenDead.Length; i++)
        {
            disableWhenDead[i].enabled = false;
        }

        //Explode!
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
        
        //Disable Movement
        if(isLocalPlayer)
            GetComponent<NetworkPlayerMove>().enabled = false;

        //Respawn
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        //Wait Determined Time To Respawn
        yield return new WaitForSeconds(7f);

        //Determine Spawn Point (I have it set to Round Robin)
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        //Wait to make sure all transforms are correct (for particles)
        yield return new WaitForSeconds(0.2f);

        //Spawn
        PlayerSetup();

        Debug.Log(transform.name + " Respawned!");
    }

    void OnDisable()
    {
        //Destroy(playerUIInstance);
    }
}
        
