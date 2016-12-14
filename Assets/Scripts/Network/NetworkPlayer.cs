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
    [SerializeField]
    private int teeth;
    [SerializeField]
    private int points;

    public void AddPoints(int p)
    {
        points += p;
    }

    public int GetPoints()
    {
        return points;
    }
    
    public float maxHealth = 100f;

    private bool FirstSetup = true;
    [SerializeField]
    GameObject playerUIprefab;
    private NetworkPlayerUI playerUI;
    private GameObject playerUIInstance;
    private NetworkToothSpawner GameMaster;
    private NetworkPlayer otherPlayer;

    //========================================
    // Animator Stuff
    //========================================
    public Animator animator;
    public SpriteRenderer sr;
    Rigidbody rb;
    public bool dirRight = true;

    private float lastPosition;

    //========================================
    // Netowrk Stuff
    //========================================
    [SyncVar]
    private float currentHealth;

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    //========================================
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
    void Start()
    {
        //Sprite Stuff
        rb = GetComponent<Rigidbody>();
        animator = rb.GetComponentInChildren<Animator>();
        sr = rb.GetComponentInChildren<SpriteRenderer>();
        GameMaster = GameObject.Find("ToothSpawner").GetComponent<NetworkToothSpawner>();
    }

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

    //=====================================
    // Update
    //=====================================
    void Update()
    {
        //This updates the direction of each player nicely!
        if (lastPosition < transform.position.x)
        {
            sr.flipX = true;
        }
        else if (lastPosition > transform.position.x)
        {
            sr.flipX = false;
        }

        lastPosition = transform.position.x;
    }

    //====================================
    // Enable everything we need on spawn
    //====================================
    public void SetStatsAndStuff()
    {
        isDead = false;

        //Enable Movement and Shooting
        if (isLocalPlayer)
        {
            GetComponent<NetworkPlayerMove>().enabled = true;
            GetComponent<NetworkPlayerShoot>().enabled = true;
        }

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
            sourcePlayer.AddPoints(1000);
        }

        deaths++;

        //Penalty for death? Small considering the respawn time!
        points -= 250;
        if (points < 0)
            points = 0;

        //Disable Movement
        if (isLocalPlayer)
        {
            GetComponent<NetworkPlayerMove>().enabled = false;
            GetComponent<NetworkPlayerShoot>().enabled = false;
        }

        //Respawn
        StartCoroutine(Respawn());
    }

    //=============================================
    // Keep them dead for 7 seconds, or eventually
    // some sort of global time!
    //=============================================
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

    //======================================
    // Trigger for Teeth!
    // It seems the network transforms 
    // take care of the server
    // client stuff here!
    //======================================
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Tooth"))
        {
            other.gameObject.SetActive(false);
            teeth++;
            points += 100;
        }
    }

    //Called from the UI when time runs out.  Hackey, but efficient
    //Kill controls, everything!
    public void GameOver()
    {
        //Disable Movement
        if (isLocalPlayer)
        {
            GetComponent<NetworkPlayerMove>().enabled = false;
            GetComponent<NetworkPlayerShoot>().enabled = false;
        }

    }
}   

