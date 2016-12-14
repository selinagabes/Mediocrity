using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized * 10;
    }
    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Tooth"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("TimeFreeze"))
        {
            Debug.Log("TimeFreeze");
            other.gameObject.SetActive(false);

        }
        if (other.gameObject.CompareTag("Death"))
        {
            Debug.Log("You dead");
            //other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("EndZone"))
        {
            gameObject.SetActive(false);
            var currentZone = GameObject.FindGameObjectWithTag("Zone");
            float nextZoneX = currentZone.GetComponentInChildren<MapGenerator>().width - currentZone.GetComponentInChildren<MeshGenerator>().wallHeight + currentZone.transform.position.x;
            Vector3 nextZonePos = new Vector3(nextZoneX, 0, 0);
            var nextZone = GameObject.FindGameObjectWithTag("ZoneManager");
            nextZone.GetComponent<ZoneGenerator>().RestartZone(nextZonePos);
            MeshGenerator nextMesh = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
            Vector3 nextMin = nextMesh.GetMinimumVertex();
            transform.position = new Vector3(nextMin.x + 5f, nextMin.z + 1f, 2.5f);
            gameObject.SetActive(true);
            //ONCE INSTANTIATED, TELEPORT TO THE MINIMUM VERTEX AND STUFF.......


            Debug.Log("New Zone");
        }

    }

}


//using UnityEngine;
//using System.Collections;

//public class PlayerController : MonoBehaviour
//{
//    Rigidbody rb;
//    Vector3 velocity;

//	Animator animator;
//	SpriteRenderer sr;
//	bool isRunning;
//	float speed;

//	bool dontTakeDamage; //this is if the player jumps on an enemies head, they shouldn't take damage

//	int direction = 1;

//	int numTeeth;

//	IEnumerator powerupTimer;

//	public GameObject body; //player body
//	public GameObject playerStats; //Counter in top right corner
//	public GameObject bulletPrefab;
//	Transform bulletSpawn;
//	public Transform leftShooter;
//	public Transform rightShooter;
//	private int health;

//	public bool grounded; //used for jumping

//	bool currentlyShooting;
//	float fireRate = 1.0f;
//	float nextFire = 0.0f;

//	float damageRate = 1.0f;
//	float nextDamage = 0.0f;

//	// Use this for initialization
//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();   
//		animator = body.GetComponent<Animator> ();
//		sr = body.GetComponent<SpriteRenderer> ();

//		grounded = true;
//		isRunning = false;
//		speed = 0.2f;

//		bulletSpawn = leftShooter;
//		numTeeth = 32;
//		currentlyShooting = false;
//		health = 10;
//    }

//	void FixedUpdate () {

//		if (health != 0) {

//			//RUNNING SPEED

//			if (isRunning) { //first check what the characters speed should be
//				speed = 0.4f;
//			} else {
//				speed = 0.2f;
//			}

//			//DIRECTION

//			KeyMovement ();

//			//POWERUPS

//			//PLAYER STATS
//			playerStats.GetComponent<GUIText> ().text = "HP: " + health + "\nTeeth: " + numTeeth; 
//		} else {
//			//you died son
//			playerStats.GetComponent<GUIText> ().text = "DEAD x_x"; 
//		}
//	}


//    void OnTriggerEnter(Collider other)
//    {

//        if (other.gameObject.CompareTag("Tooth"))
//        {
//			numTeeth++;
//            other.gameObject.SetActive(false);
//        }
//        if (other.gameObject.CompareTag("Death"))
//        {
//            Debug.Log("You dead");
//            //other.gameObject.SetActive(false);
//        }
//        if (other.gameObject.CompareTag("EndZone"))
//        {
//            gameObject.SetActive(false);
//            var currentZone = GameObject.FindGameObjectWithTag("Zone");
//            float nextZoneX = currentZone.GetComponentInChildren<MapGenerator>().width - currentZone.GetComponentInChildren<MeshGenerator>().wallHeight + currentZone.transform.position.x;
//            Vector3 nextZonePos = new Vector3(nextZoneX, 0, 0);
//            var nextZone = GameObject.FindGameObjectWithTag("ZoneManager");
//            nextZone.GetComponent<ZoneGenerator>().RestartZone(nextZonePos);
//            MeshGenerator nextMesh = GameObject.FindGameObjectWithTag("LevelPath").GetComponent<MeshGenerator>();
//            Vector3 nextMin = nextMesh.GetMinimumVertex();
//            transform.position = new Vector3(nextMin.x + 5f, nextMin.z + 1f, 2.5f);
//            gameObject.SetActive(true);
//            //ONCE INSTANTIATED, TELEPORT TO THE MINIMUM VERTEX AND STUFF.......


//            Debug.Log("New Zone");
//        }

//		if (other.GetComponent<Collider>().tag == "EnemyHead") {
//			if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "BookGhost") {
//				other.transform.parent.gameObject.GetComponent<BookGhostMove>().decrementHealth();

//			} else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "Chronos"){
//				other.transform.parent.gameObject.GetComponent<ChronosMove>().decrementHealth();

//			}
//			dontTakeDamage = true;
//		}

//		if (other.tag == "ExplodeZone") {
//			other.transform.parent.gameObject.GetComponent<ChronosMove> ().explode = true;
//			other.transform.parent.gameObject.GetComponent<ChronosMove> ().attackPos = this.transform.position;
//		}

//		if (other.GetComponent<Collider>().tag == "HitBox") {
//			if (Time.time > nextDamage && !dontTakeDamage) {
//				decrementHealth ();
//				nextDamage = Time.time + damageRate;
//			}
//		}
//		dontTakeDamage = false;

//    }

//	void OnCollisionEnter(Collision col){

//		if (col.collider.tag == "powerup") {
//			animator.SetBool ("hasGun", true);
//			StartCoroutine (PowerupTimeout());
//		}


//	}


//	//TODO: check if the char has collided with anthing tagged "ground"
//	void KeyMovement(){

//		if (currentlyShooting == false) { //if not currently shooting the player can walk
//			if (Input.GetKey (KeyCode.LeftArrow)) { 
//				direction = -1;
//				bulletSpawn = leftShooter;
//				this.transform.position += Vector3.left * speed;
//				animator.SetBool ("isWalking", true);
//				sr.flipX = false;
//			}
//			if (Input.GetKey (KeyCode.RightArrow)) {
//				direction = 1;
//				bulletSpawn = rightShooter;
//				this.transform.position += Vector3.right * speed;
//				animator.SetBool ("isWalking", true);
//				sr.flipX = true;
//			}
//			if (!Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow)) {
//				animator.SetBool ("isWalking", false);	
//			}

//			//JUMP

//			if (Input.GetKeyDown (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow)) {
//				rb.velocity = new Vector3 (0, 20, 0);
//			}
//		}

//		//SHOOT

//		if (Input.GetKey (KeyCode.LeftAlt) || Input.GetKey (KeyCode.RightAlt)) {
//			if (!currentlyShooting && Time.time > nextFire) {
//				animator.SetBool ("isShooting", true);
//				if (numTeeth > 0) {
//					StartCoroutine (Fire ());
//				} else {
//					animator.SetBool ("isShooting", false);
//					currentlyShooting = false;
//				}
//				nextFire = Time.time + fireRate;
//			} 
//		} else {
//			currentlyShooting = false;
//		}

//		if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
//			isRunning = true;
//		} else {
//			isRunning = false;
//		}
//	}

//	private IEnumerator Fire(){
//		currentlyShooting = true;
//		yield return new WaitForSeconds (0.5f);

//		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, Quaternion.identity);
//		Physics.IgnoreCollision (bullet.GetComponent<Collider> (), this.GetComponent<Collider> ());
//		bullet.GetComponent<Rigidbody> ().AddForce(new Vector3(1200 * direction, 0, 0));
//		Destroy (bullet, 4.0f);

//		currentlyShooting = false;
//		numTeeth--;
//		animator.SetBool ("isShooting", false);

//	}

//	private IEnumerator PowerupTimeout(){
//		while (true) {
//			yield return new WaitForSeconds (30f);
//			animator.SetBool ("hasGun", false);
//		}
//	}

//	public void decrementHealth(){ // when player gets hit
//		animator.SetBool ("isHit", true);
//		health--;
//	}

//}
