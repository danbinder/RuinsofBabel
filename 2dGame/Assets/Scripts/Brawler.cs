using UnityEngine;
using System.Collections;

public class Brawler : MonoBehaviour {

    private Rigidbody2D myRigidbody;
	
	private Animator myAnimator;

    [SerializeField] //ability to access in unity window
    private float mvmtSpeed;

    private bool attack;

    private bool facingRight;

    [SerializeField] //ability to access in unity window
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce; //power to the jump



	// Use this for initialization
	void Start () {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();

        HandleInput();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleAttacks();

        HandleLayers();



        ResetValues();
	}

    private void HandleMovement(float horizontal)
    {   
        if (myRigidbody.velocity.y < 0) //begin falling
        {
            myAnimator.SetBool("landing", true);
        }
        if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) //layer 0 (only layer so far)
        {   //move player only if not attacking
            myRigidbody.velocity = new Vector2(horizontal * mvmtSpeed * Time.deltaTime, myRigidbody.velocity.y); //x val of -1, y val of 0;
           
        }
        if (isGrounded && jump) //if we press jump button & are on ground -- make player jump
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if (attack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            attack = true;
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 playerScale = transform.localScale;

            playerScale.x *= -1;

            transform.localScale = playerScale;
        }
    }

    private void ResetValues()
    {
        attack = false;
        jump = false;
    }

    private bool IsGrounded()
    {
        if (myRigidbody.velocity.y <= 0) //falling or stopped moving down
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject) //if grounded
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("landing", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1); //if in air, give airLayer Priority
            Debug.Log("airLayer");
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0); //revert to GroundLayer
        }
    }
}
