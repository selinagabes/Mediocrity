using UnityEngine;
using System.Collections;

public class ChronosMove : MonoBehaviour {

	public GameObject body;
	public GameObject head;
	Rigidbody rb;
	Animator animator;
	private int health = 2; //how many shots it takes to kill the enemy
	bool wasHit;

	public Vector3 attackPos;
	public bool explode;

	float attackRate = 1.0f;
	float nextAttack = 0.0f;


	// Use this for initialization
	void Start () {
		animator = body.GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		wasHit = false;
		explode = false;
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (!explode) {
			if (wasHit) {
				animator.SetTrigger ("isHit");
				wasHit = false;
			} else {
				this.transform.position += Vector3.left * 0.05f;
			}

			if (health <= 0) {
				this.transform.position -= Vector3.left * 0.05f;
				animator.SetBool ("isDead", true);
				StartCoroutine (Die ());
			}
		} else {
			if (Time.time > nextAttack) {
				StartCoroutine (Attack ());
			} else {

			}

		}
		
		
	}

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "bullet") {
			Debug.Log ("bullet hit");
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

	private IEnumerator Attack(){
		yield return new WaitForSeconds(1.0f);
		this.transform.position = attackPos;
		explode = false;
	}

	public void decrementHealth(){ // when player jumps on this enemy this function gets called
		wasHit = true;
		health--;
	}

	public void Explosion(){

	}


}
