using UnityEngine;
using System.Collections;

public class ElderBookGhost : MonoBehaviour {

	public GameObject body;
	public GameObject head;
    SpriteRenderer sr;
	GameObject player;
	Rigidbody rb;
	Animator animator;
	private int health = 6; //how many shots it takes to kill the enemy
	bool wasHit;
	bool dead ;
    public bool frozen = false;


	// Use this for initialization
	void Start () {
		animator = body.GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
        sr = GetComponentInChildren<SpriteRenderer>();
		wasHit = false;
		dead = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
        if (!frozen)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }

            if (!dead)
            {
                if (wasHit)
                { //if hit play animation and reset bool
                    animator.SetTrigger("isHit");
                    wasHit = false;
                }
                else
                { //if enemy has not been hit, keep him moving

                    if (transform.position.x > player.transform.position.x)
                    {
                        this.transform.position += Vector3.left * 0.05f;
                        sr.flipX = false;
                    }
                    else
                    {
                        this.transform.position += Vector3.right * 0.05f;
                        sr.flipX = true;

                    }
                    this.transform.position -= new Vector3(0.0f, Mathf.Sin(Time.time) * 0.05f, 0.0f);
                }

                if (health <= 0)
                { //if dead, stop moving, go to dead animation
                    this.transform.position -= Vector3.left * 0.05f; //once the enemy is dead make sure he doesn't move anymore
                    this.transform.position += new Vector3(0.0f, Mathf.Sin(Time.time) * 0.05f, 0.0f);
                    animator.SetBool("isDead", true);
                    dead = true;
                    StartCoroutine(Die());
                }
            }
        }
	}

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "bullet") {
			Destroy (col.gameObject);
			wasHit = true;
			health--;
		}
	}

	private IEnumerator Die(){
		Destroy(this.gameObject.GetComponent<Collider>());
		yield return new WaitForSeconds(3.0f);
		this.gameObject.SetActive (false);
	}

	public void decrementHealth(){ // when player jumps on this enemy this function gets called
		wasHit = true;
		health--;
	}
}
