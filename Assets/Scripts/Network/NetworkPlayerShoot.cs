using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/*==========================================
This script controls player shooting over
the network.
==========================================*/
public class NetworkPlayerShoot : NetworkBehaviour {

    //==================================
    // All declarations are local
    //==================================
    public NetworkPlayerWeapon RayGun;
	private bool dirRight = true;
    private bool canShoot = true;
	[SerializeField]
	private LayerMask mask;
	private string _id;
    
    [SerializeField]
    LineRenderer laserBeam;

    public GameObject laser1;
    public GameObject laser2;

    Animator animator;
    SpriteRenderer sr;
    Rigidbody rb;

    //=================================
    // Register the player to UNET
    //=================================
    void Start()
	{
		RegisterPlayer ();
	}

	void RegisterPlayer()
	{
        //What is our player going to be named
		_id = "Player " + GetComponent<NetworkIdentity> ().netId;
		transform.name = _id;

        //Find both lasers, then determine which one we use
        laser1 = GameObject.Find("Laser1");
        laser2 = GameObject.Find("Laser2");

        //Laserbeam will be our line renderer depending on which game object we are using!
        if (NetworkGameManager.GetPlayers().Length == 1)
        {
            laserBeam = laser1.GetComponent<LineRenderer>();
        }
        else
        {
            laserBeam = laser2.GetComponent<LineRenderer>();
        }

        //Sprite Stuff, if we need it!
        rb = GetComponent<Rigidbody>();
        animator = rb.GetComponentInChildren<Animator>();
        sr = rb.GetComponentInChildren<SpriteRenderer>();
    }

	void Update()
	{
		if (isLocalPlayer) 
		{
            //Only show if the escape menu is NOT on
            if (!NetworkDCMenu.isOn)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    dirRight = true;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    dirRight = false;
                }

                if (Input.GetKeyDown("space"))
                {
                    if(canShoot)
                        FireTheLaser();
                }
            }
		}
	}

    //========================================
    // OK! So, here we fire our gun on the client
    // side.  I am using Vector3.right and left
    // because they are relavent to the x acis and
    // do not care what way the cube or capsule 
    // is faceing.  I use a boolean dirRigh to determine
    // which we we faced last
    //========================================
    [Client]
	void FireTheLaser()
    {
        //Set up our raycast
        Ray ray;
        RaycastHit _hit;

        //And vectors
        Vector3 start;
        Vector3 end;

        //Determine the start position to be in front of the player, depending on direction!
        if (dirRight)
            start = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
        else
            start = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
        
        //More direction stuff!
        if (dirRight)
        {
            ray = new Ray(transform.position, new Vector3(1, 0, 0));
        }
        else
        {
            ray = new Ray(transform.position, new Vector3(-1, 0, 0));
        }

        if (Physics.Raycast(ray, out _hit, RayGun.range, mask))
        {
            //Tag the prefab of player to Player! (we check to see if it is our own player in NetworkPlayer
            if (_hit.collider.tag == "Player")
            {
                //Control the shooting by the server
                CmdShootHim(_hit.collider.name, RayGun.damage, transform.name);
            }

            //laserBeam.SetPosition(1, _hit.point);
            end = _hit.point;
        }
        else
        {
            if (dirRight)
            {
                end = new Vector3(this.transform.position.x + RayGun.range, this.transform.position.y, this.transform.position.z);
            }
            else
            {
                end = new Vector3(this.transform.position.x - RayGun.range, this.transform.position.y, this.transform.position.z);
            }
        }

        //Let's draw the laser over the network
        //We pass the positions so they are rendered in the same place on either client
        CmdDrawLaser(start, end);

        //After we fire, put the laser on cool down briefly so a player can't rapid fire and absolutely shit stomp the other guy
        StartCoroutine(DisableLaser());
    }

    //========================================
    // Send the firing command to the server
    //========================================
    [Command]
    void CmdDrawLaser(Vector3 _1, Vector3 _2)
    {
        RpcBroadCastLaser(_1, _2);
    }

    //========================================
    // Display Line depending on Laser
    // (Client specific)
    //========================================
    [ClientRpc]
    void RpcBroadCastLaser(Vector3 _1, Vector3 _2)
    {
        laserBeam.SetPosition(0, _1);
        laserBeam.SetPosition(1, _2);
        laserBeam.SetWidth(.5f, .5f);

        StartCoroutine(DisableLaser());
    }

    IEnumerator DisableLaser()
    {
        canShoot = false;
        animator.SetBool("isShooting", true);
        laserBeam.enabled = true;
        yield return new WaitForSeconds(.2f);
        laserBeam.enabled = false;
        yield return new WaitForSeconds(.2f);
        animator.SetBool("isShooting", false);
        canShoot = true;
    }

    [Command]
    void CmdShootHim(string _pid, float _dmg, string _source)
    {
        Debug.Log(_pid + " has been shot");

        NetworkPlayer _nPlayer = NetworkGameManager.GetPlayer(_pid);
        _nPlayer.RpcTakeDamage(_dmg, _source);
    }
}

