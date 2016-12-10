using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableWhenDead;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool FirstSetup = true;

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
        if (FirstSetup)
        {
            wasEnabled = new bool[disableWhenDead.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableWhenDead[i].enabled;
            }
            FirstSetup = false;
        }
        SetStatsAndStuff();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.K))
                RpcTakeDamage(9999);
        }
    }

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
    public void RpcTakeDamage(int _amt)
    {
        if (isDead)
            return;

        currentHealth -= _amt;
        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Killit();
        }
    }

    private void Killit()
    {
        isDead = true;

        //Disable Components
        Debug.Log(transform.name + " is dead");

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
        yield return new WaitForSeconds(NetworkGameManager.instance.matchSettings.respawnTime);

        //Determine Spawn Point
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        //Wait to make sure all transforms are correct (for particles)
        yield return new WaitForSeconds(0.1f);

        //Spawn
        PlayerSetup();

        Debug.Log(transform.name + " Respawned!");
    }
}
        
