using UnityEngine;
using System.Collections;

//=======================
// Move the player!
//=======================
public class NetworkPlayerMove : MonoBehaviour {
	
	private Rigidbody rb;
	private Vector3 velocity = Vector3.zero;
    private Vector3 jumpVelocity = Vector3.zero;
    private GameObject groundCheck;

    bool isJumping = false;
    bool isGrounded = false;
    bool doubleJump = false;

    void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

    void Update()
    {
        //Check to see if input is accepted
        if (!NetworkDCMenu.isOn)
        {
            RaycastHit _hit;

            //Grounded?
            Debug.DrawRay(transform.position, Vector3.down, Color.red, 3f);
            if (Physics.Raycast(transform.position, Vector3.down, out _hit, 3f))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            //Jump and Double Jump! yay!
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(isGrounded)
                {
                    Debug.Log("Jump!");
                    isJumping = true;
                }

                if (isJumping)
                {
                    rb.AddForce(new Vector3(0, 500, 0));
                    isJumping = false;
                    doubleJump = true;
                }
                else
                {
                    if (doubleJump)
                    {
                        rb.AddForce(new Vector3(0, 500, 0));
                        doubleJump = false;
                    }
                }
            }

            velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        }
    }

	void FixedUpdate()
	{
        //Check to see if input is accepted
        if (!NetworkDCMenu.isOn)
        {
            rb.MovePosition(transform.position + velocity * Time.deltaTime * 15f);
        }
    }
}
