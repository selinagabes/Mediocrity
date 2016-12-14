using UnityEngine;
using System.Collections;

public class TuitionSpikes : MonoBehaviour {

	Animator animator;
    public bool frozen = false;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
        if (!frozen)
        {
            if (col.collider.tag == "Player")
            {
                animator.SetTrigger("Activated");
                col.gameObject.GetComponent<PlayerController>().Dead();
            }
        }
	}
}
