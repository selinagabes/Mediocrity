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
    
    public void Setup()
    {
        wasEnabled = new bool[disableWhenDead.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableWhenDead[i].enabled;
        }

        SetStatsAndStuff();
    }

    public void SetStatsAndStuff()
    {
        isDead = false;

        for (int i = 0; i < disableWhenDead.Length; i++)
        {
            disableWhenDead[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        Rigidbody _rb = GetComponent<Rigidbody>();
        if (_rb != null)
            _rb.useGravity = true;

        if(isLocalPlayer)
        {
            GetComponent<NetworkPlayerMove>().enabled = true;
        }

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

        //Disable Stuff!
        Debug.Log(transform.name + " is dead");

        for (int i = 0; i < disableWhenDead.Length; i++)
        {
            disableWhenDead[i].enabled = false;
        }


        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Rigidbody _rb = GetComponent<Rigidbody>();
        if (_rb != null)
            _rb.useGravity = false;

        if(isLocalPlayer)
        {
            GetComponent<NetworkPlayerMove>().enabled = false;
        }

        //Respawn
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(NetworkGameManager.instance.matchSettings.respawnTime);

        SetStatsAndStuff();

        //Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        //transform.position = _spawnPoint.position;
        //transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " Respawned!");
    }
}
        
