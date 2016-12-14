using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject body;
    public GameObject head;
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
    SpriteRenderer sr;
    GameObject player;
    Rigidbody rb;
    Animator animator;
    private int health = 2; //how many shots it takes to kill the enemy
    public bool attacking;

    bool wasHit;
    bool dead;
    public bool frozen = false;

    float fireRate = 1.0f;
    float nextFire = 0.0f;


    // Use this for initialization
    void Start()
    {
        animator = body.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        wasHit = false;
        dead = false;
        attacking = false;
        frozen = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!frozen)
        {
            if (!attacking) { 
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
            else //ATTACKING
            {
                Debug.Log("ayo");
                StartCoroutine(Shoot());

            }
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "bullet")
        {
            Destroy(col.gameObject);
            wasHit = true;
            health--;
        }
    }

    private IEnumerator Die()
    {
        Destroy(this.gameObject.GetComponent<Collider>());
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
    }

    public void decrementHealth()
    { // when player jumps on this enemy this function gets called
        wasHit = true;
        health--;
    }

    private IEnumerator Shoot()
    {
        Debug.Log(Time.time);
        if (Time.time > nextFire)
        {
            
            nextFire = Time.time + fireRate;
            var bullet1 = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            Physics.IgnoreCollision(bullet1.GetComponent<Collider>(), this.GetComponent<Collider>());

            bullet1.GetComponent<Rigidbody>().AddForce(new Vector3(-200, 200, 0));
            Destroy(bullet1, 5.0f);
            yield return new WaitForSeconds(5.0f);
            attacking = false;
        }
    }
}
