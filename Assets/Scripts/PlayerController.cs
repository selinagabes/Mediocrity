using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //======================================
    // Animations and Sprite Stuff
    //======================================
    Rigidbody rb;
	Animator animator;
	SpriteRenderer sr;

    //======================================
    // Player Stuff
    //======================================
	public GameObject bulletPrefab;
	Transform bulletSpawn;
	public Transform leftShooter;
	public Transform rightShooter;
	private float health;

    //======================================
    // Jump Stuff
    //======================================
    private Vector3 velocity = Vector3.zero;
    private Vector3 jumpVelocity = Vector3.zero;
    bool isJumping = false;
    bool isGrounded = false;
    bool doubleJump = false;

    //======================================
    // Health / Damage Stuff
    //======================================
    float fireRate = 1.0f;
	float nextFire = 0.0f;
	float damageRate = 1.0f;
	float nextDamage = 0.0f;
    int direction = 1;
    bool isAlive;
    bool isPlaying; //Use for menus and such
    bool currentlyShooting;

    //======================================
    // Start and Set up the Player!
    //======================================
    void Start()
    {
        //Sprite Stuff
        rb = GetComponent<Rigidbody>();   
		animator = body.GetComponent<Animator> ();
		sr = body.GetComponent<SpriteRenderer> ();

        isPlaying = true;

        PlayerSpawn();
    }

    public void PlayerSpawn()
    {
        //Bools
        isGrounded = false;
        isRunning = false;
        currentlyShooting = false;
        isAlive = true;

        //Stats and Stuff
        bulletSpawn = leftShooter;
        numTeeth = 32;
        health = 10;
    }

    //=======================================================
    // Calculate Everything for Positioning Here
    //=======================================================
    void Update()
    {
        if (isPlaying)
        {
            if (isAlive)
            {
                // ============== Fire! ====================
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                {
                    if (!currentlyShooting)
                    {
                        animator.SetBool("isShooting", true);
                        if (numTeeth > 0)
                        {
                            StartCoroutine(SingleFire());
                        }
                        else
                        {
                            animator.SetBool("isShooting", false);
                            currentlyShooting = false;
                        }
                    }
                }

                // ============== Move & Jump ====================
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    direction = -1;
                    bulletSpawn = leftShooter;
                    animator.SetBool("isWalking", true);
                    sr.flipX = false;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    direction = 1;
                    bulletSpawn = rightShooter;
                    animator.SetBool("isWalking", true);
                    sr.flipX = true;
                }
                if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    animator.SetBool("isWalking", false);
                }

                RaycastHit _hit;

                //Grounded?
                Debug.DrawRay(transform.position, Vector3.down, Color.red, 6f);
                if (Physics.Raycast(transform.position, Vector3.down, out _hit, 3f))
                {
                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }

                //Jump and Double Jump! yay!
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (isGrounded)
                    {
                        Debug.Log("Jump!");
                        isJumping = true;
                    }

                    if (isJumping)
                    {
                        rb.AddForce(new Vector3(0, 5000f, 0));
                        isJumping = false;
                        doubleJump = true;
                    }
                    else
                    {
                        if (doubleJump)
                        {
                            rb.AddForce(new Vector3(0, 5000f, 0));
                            doubleJump = false;
                        }
                    }
                }
                velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            }
        }
    }

    void FixedUpdate()
    {
        if (isPlaying)
        { 
            if (isAlive)
            {
                rb.MovePosition(transform.position + velocity * Time.deltaTime * 15f);
            }
        }
	}

    private IEnumerator SingleFire()
    {
        currentlyShooting = true;

        yield return new WaitForSeconds(0.5f);

        numTeeth--;
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.GetComponent<Collider>());
        bullet.GetComponent<Rigidbody>().AddForce(new Vector3(1200 * direction, 0, 0));
        Destroy(bullet, 4.0f);
        animator.SetBool("isShooting", false);

        yield return new WaitForSeconds(2f);

        currentlyShooting = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tooth"))
        {
			numTeeth++;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Death"))
        {
            Debug.Log("You dead");
        }
        if (other.gameObject.CompareTag("EndZone"))
        {
            
        }
        if (other.GetComponent<Collider>().tag == "EnemyHead")
        { //check which enemy it is, decrement health
            if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "BookGhost")
            {
                other.transform.parent.gameObject.GetComponent<BookGhostMove>().decrementHealth();

            }
            else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "Chronos")
            {
                //other.transform.parent.gameObject.GetComponent<ChronosMove>().decrementHealth();
            }
            else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "ElderBookGhost")
            {
                //other.transform.parent.gameObject.GetComponent<ElderBookGhost>().decrementHealth();
            }
            dontTakeDamage = true;
        }
        if (other.GetComponent<Collider>().tag == "EnemyHead") { //check which enemy it is, decrement health
			if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "BookGhost") {
				other.transform.parent.gameObject.GetComponent<BookGhostMove>().decrementHealth();

			} else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "Chronos"){
				//other.transform.parent.gameObject.GetComponent<ChronosMove>().decrementHealth();
			} else if (other.transform.parent.gameObject.GetComponent<GUIText>().text == "ElderBookGhost"){
				//other.transform.parent.gameObject.GetComponent<ElderBookGhost>().decrementHealth();
			}
			dontTakeDamage = true;
		}
		if (other.tag == "ExplodeZone") {
			other.transform.parent.gameObject.GetComponent<ChronosMove> ().explode = true;
			other.transform.parent.gameObject.GetComponent<ChronosMove> ().attackPos = this.transform.position;
		}

		if (other.GetComponent<Collider>().tag == "HitBox") {
			if (Time.time > nextDamage && !dontTakeDamage) {
				decrementHealth ();
				nextDamage = Time.time + damageRate;
			}
		}
		dontTakeDamage = false;

    }

	void OnCollisionEnter(Collision col)
    {
		if (col.collider.tag == "powerup")
        {
			animator.SetBool ("hasGun", true);
			StartCoroutine (PowerupTimeout());
		}
	}

    private IEnumerator PowerupTimeout()
    {
        //Only keep gun for 30 seconds
        yield return new WaitForSeconds(30f);
        animator.SetBool("hasGun", false); 
    }
    
	public void decrementHealth(){ // when player gets hit
		animator.SetBool ("isHit", true);
		health--;
	}

}
