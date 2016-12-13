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
	[SerializeField]
	private LayerMask mask;
	private string _id;
    
    [SerializeField]
    LineRenderer laserBeam;

    public GameObject laser1;
    public GameObject laser2;

    void Start()
	{
		RegisterPlayer ();
	}

	void RegisterPlayer()
	{
        //What is our player going to be named
		_id = "Player " + GetComponent<NetworkIdentity> ().netId;
		transform.name = _id;

        laser1 = GameObject.Find("Laser1");
        laser2 = GameObject.Find("Laser2");

        if (NetworkGameManager.GetPlayers().Length == 1)
        {
            laserBeam = laser1.GetComponent<LineRenderer>();
        }
        else
        {
            laserBeam = laser2.GetComponent<LineRenderer>();
        }
    }

	void Update()
	{
		if (isLocalPlayer) 
		{
            //Only show if the escape menu is NOT on
            if (!NetworkDCMenu.isOn)
            {
                if (Input.GetKeyDown("space"))
                {
                    if (Input.GetAxis("Horizontal") > 0)
                        dirRight = true;
                    else if (Input.GetAxis("Horizontal") < 0)
                        dirRight = false;
                    
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
		Ray ray;
		RaycastHit _hit;
        Vector3 start = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        Vector3 end;
        //laserBeam.SetPosition(0, new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z));

        if (dirRight) 
		{	
			ray = new Ray (transform.position, new Vector3(1,0,0));
		} 
		else 
		{
			ray = new Ray (transform.position, new Vector3(-1,0,0));
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
                //laserBeam.SetPosition(1, new Vector3(this.transform.position.x + RayGun.range, this.transform.position.y, this.transform.position.z));
            }
            else
            {
                end = new Vector3(this.transform.position.x - RayGun.range, this.transform.position.y, this.transform.position.z);
                //laserBeam.SetPosition(1, new Vector3(this.transform.position.x  - RayGun.range, this.transform.position.y, this.transform.position.z));
            }
        }

        //Let's draw the laser over the network
        laserBeam.SetPosition(0, start);
        laserBeam.SetPosition(1, end);
        laserBeam.SetWidth(.2f, .2f);
        CmdDrawLaser();
        StartCoroutine(DisableLaser());
    }

    [Command]
    void CmdDrawLaser()
    {
        RpcBroadCastLaser();
    }

    [ClientRpc]
    void RpcBroadCastLaser()
    {
        StartCoroutine(DisableLaser());
    }

    IEnumerator DisableLaser()
    {
        laserBeam.enabled = true;
        yield return new WaitForSeconds(.2f);
        laserBeam.enabled = false;
    }

	[Command]
	void CmdShootHim(string _pid, float _dmg, string _source)
	{
		Debug.Log (_pid + " has been shot");

        NetworkPlayer _nPlayer = NetworkGameManager.GetPlayer(_pid);
        _nPlayer.RpcTakeDamage(_dmg,_source);
	}
}
