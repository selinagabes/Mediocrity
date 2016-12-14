using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 velocity;

    public GameObject PlayerUIPrefab;
    private PlayerUI playerUI;
    private GameObject playerUIInstance;

    Animator animator;
	SpriteRenderer sr;
	bool isRunning;
    bool isJumping = false;
    bool isGrounded = false;
    bool doubleJump = false;

    float speed;

	bool dontTakeDamage; //this is if the player jumps on an enemies head, they shouldn't take damage

	int direction = 1;

	int teeth;

	IEnumerator powerupTimer;

	public GameObject body; //player body
	public GameObject bulletPrefab;
	Transform bulletSpawn;
	public Transform leftShooter;
	public Transform rightShooter;
	private float lucidPoints = 5;

	public bool grounded; //used for jumping

	bool currentlyShooting;
	float fireRate = 1.0f;
	float nextFire = 0.0f;

	float damageRate = 1.0f;
	float nextDamage = 0.0f;

	// Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();   
		animator = body.GetComponent<Animator> ();
		sr = body.GetComponent<SpriteRenderer> ();

		grounded = true;
		isRunning = false;
		speed = 0.2f;

		bulletSpawn = leftShooter;
		teeth = 32;
		currentlyShooting = false;
		lucidPoints = 5;

        //Spawn UI
        playerUIInstance = Instantiate(PlayerUIPrefab);
        playerUIInstance.name = PlayerUIPrefab.name;

        //Configure UI
        playerUI = playerUIInstance.GetComponent<PlayerUI>();
        if (playerUI == null)
            Debug.Log("No UI!");
    }

	void FixedUpdate () {

        playerUI.SetLucidity((lucidPoints * 20 / 200));
        playerUI.SetTeeth(teeth);

		if (lucidPoints > 0) {

			//RUNNING SPEED

			if (isRunning) { //first check what the characters speed should be
				speed = 0.4f;
			} else {
				speed = 0.2f;
			}

			//DIRECTION

			KeyMovement ();


		} 
	}


    void OnTriggerEnter(Collider other)
    {

		if (other.GetComponent<Collider>().tag == "EnemyHead") { //check which enemy it is, decrement lucidPoints
			if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "BookGhost") {
				other.transform.parent.gameObject.GetComponent<BookGhostMove>().decrementHealth();

			} else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "Chronos"){
				other.transform.parent.gameObject.GetComponent<ChronosMove>().decrementHealth();
			} else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "ElderBookGhost"){
				other.transform.parent.gameObject.GetComponent<ElderBookGhost>().decrementHealth();
			}
			dontTakeDamage = true;
		}


		if (other.tag == "ExplodeZone") {
			//sets the explode flag in the chronos script to true so that the enemy knows to attack
			//also updates the players coordinates in the attack script, this is where cronos will teleport
			other.transform.parent.gameObject.GetComponent<ChronosMove> ().explode = true;
			other.transform.parent.gameObject.GetComponent<ChronosMove> ().attackPos = this.transform.position;
		}

        if (other.tag == "ShooterZone")
        {
            other.transform.parent.gameObject.GetComponent<Shooter>().attacking = true;
        }

		if (other.tag == "HitBox") {
			if (Time.time > nextDamage && !dontTakeDamage) {
				decrementlucidPoints ();
				nextDamage = Time.time + damageRate;
			}
		}
		dontTakeDamage = false;




		if (other.gameObject.CompareTag("Tooth"))
        {
            //TODO:: ADD 1 TOOTH
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Denture"))
        {
            //TODO:: ADD 32 TEETH
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("TimeZone"))
        {
            //TODO:: FREEZE ALL ENEMIES
            Debug.Log("TimeFreeze");
            other.gameObject.SetActive(false);

        }
        if (other.gameObject.CompareTag("Shadow"))
        {
            //TODO:CLOAK OF INVISIBILITY
            Debug.Log("You can see't see me");
            other.gameObject.SetActive(false);
            //other.gameObject.SetActive(false);
        }
      
        if (other.gameObject.CompareTag("EndZone"))
        {
            //TODO:: YOU GET A LUCID POINT YEEAHHH
            Debug.Log("Portal");
        }
        if (other.gameObject.CompareTag("Gun"))
        {
            //TODO:: PEW PEW
            Debug.Log("Say hello to my little friend");
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("TopShelf"))
        {

            //TODO:: CRUMBLE THOSE STEPS YEAS
            GameObject Crumbler = (GameObject)Resources.Load("Prefabs/Crumbler");
            Instantiate(Crumbler, other.transform.position, new Quaternion());
            other.gameObject.SetActive(false);
            //Debug.Log("You dead");
            //other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Death"))
        {
            //TODO:L YOU DEAD
            Debug.Log("You dead");
            //other.gameObject.SetActive(false);
        }

    }

	void OnCollisionEnter(Collision col){

		if (col.collider.tag == "powerup") {
			animator.SetBool ("hasGun", true);
			StartCoroutine (PowerupTimeout());
		}

		if (col.collider.tag == "ExplodeDmgZone1") {
			decrementlucidPoints (4);
		} 
		if (col.collider.tag == "ExplodeDmgZone2") {
			decrementlucidPoints (2);
		}



	}


	//TODO: check if the char has collided with anthing tagged "ground"
	void KeyMovement(){
		
		if (currentlyShooting == false) { //if not currently shooting the player can walk
			if (Input.GetKey (KeyCode.LeftArrow)) { 
				direction = -1;
				bulletSpawn = leftShooter;
				this.transform.position += Vector3.left * speed;
				animator.SetBool ("isWalking", true);
				sr.flipX = false;
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				direction = 1;
				bulletSpawn = rightShooter;
				this.transform.position += Vector3.right * speed;
				animator.SetBool ("isWalking", true);
				sr.flipX = true;
			}
			if (!Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
				animator.SetBool ("isWalking", false);	
			}

            //JUMP
            RaycastHit _hit;
            Debug.DrawRay(transform.position, Vector3.down, Color.red, 3f);
            if (Physics.Raycast(transform.position, Vector3.down, out _hit, 3f))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (isGrounded)
                {
                    Debug.Log("Jump!");
                    isJumping = true;
                }

                if (isJumping)
                {
                    rigidbody.AddForce(new Vector3(0, 5000, 0));
                    isJumping = false;
                    doubleJump = true;
                }
                else
                {
                    if (doubleJump)
                    {
                        rigidbody.AddForce(new Vector3(0, 5000, 0));
                        doubleJump = false;
                    }
                }
            }
        }

		//SHOOT

		if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) {
			if (!currentlyShooting && Time.time > nextFire) {
				animator.SetBool ("isShooting", true);
				if (teeth > 0) {
					StartCoroutine (Fire ());
				} else {
					animator.SetBool ("isShooting", false);
					currentlyShooting = false;
				}
				nextFire = Time.time + fireRate;
			} 
		} else {
			currentlyShooting = false;
		}

		if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
			isRunning = true;
		} else {
			isRunning = false;
		}
	}

	private IEnumerator Fire(){
		currentlyShooting = true;
		yield return new WaitForSeconds (0.5f);

		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, Quaternion.identity);
		Physics.IgnoreCollision (bullet.GetComponent<Collider> (), this.GetComponent<Collider> ());
		bullet.GetComponent<Rigidbody> ().AddForce(new Vector3(1200 * direction, 0, 0));
		Destroy (bullet, 4.0f);

		currentlyShooting = false;
		teeth--;
		animator.SetBool ("isShooting", false);

	}

    public float GetLPs()
    {
        return lucidPoints;
    }

    public int GetTeeth()
    {
        return teeth;
    }

    private IEnumerator PowerupTimeout(){
		while (true) {
			yield return new WaitForSeconds (30f);
			animator.SetBool ("hasGun", false);
		}
	}

	public void decrementlucidPoints(){ // when player gets hit
		animator.SetBool ("isHit", true);
		lucidPoints--;
	}

	public void decrementlucidPoints(int n){ // when player gets hit
		animator.SetBool ("isHit", true);
		lucidPoints -= n;
	}

	public void Dead(){
		lucidPoints = 0;
	}

}

