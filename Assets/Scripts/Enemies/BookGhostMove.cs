using UnityEngine;
using System.Collections;

public class BookGhostMove : MonoBehaviour {

	public GameObject body;
	public GameObject head;
	Rigidbody rb;
	Animator animator;
	private int health = 1; //how many shots it takes to kill the enemy
	bool wasHit;
    public bool frozen = false;

	// Use this for initialization
	void Start () {
		animator = body.GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		wasHit = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!frozen)
        {
            if (wasHit)
            {
                animator.SetBool("isHit", true);
                wasHit = false;
            }
            else
            {
                this.transform.position += Vector3.left * 0.05f;
            }

            if (health <= 0)
            {
                this.transform.position -= Vector3.left * 0.05f;
                animator.SetBool("isDead", true);
                StartCoroutine(Die());
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
		animator.SetBool ("isHit", true);
		wasHit = false;
		health--;
	}
}
