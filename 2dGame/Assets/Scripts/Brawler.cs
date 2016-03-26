using UnityEngine;
using System.Collections;

public class Brawler : MonoBehaviour {

    private static Brawler instance;

    public static Brawler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Brawler>();
            }
            return instance;
        }
    }

    private Animator myAnimator;

    [SerializeField] //ability to access in unity window
    private float mvmtSpeed;

    private bool facingRight;

    [SerializeField] //ability to access in unity window
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce; //power to the jump

    public Rigidbody2D MyRigidbody { get; set; }

    public bool Attack { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }



    // Use this for initialization
    void Start () {
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        OnGround = IsGrounded();

        HandleInput();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleLayers();

	}

    private void HandleMovement(float horizontal)
    {   
        if (MyRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if (!Attack && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * mvmtSpeed, MyRigidbody.velocity.y);
        }
        if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }



    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            myAnimator.SetTrigger("attack");
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
    
    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0) //falling or stopped moving down
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject) //if grounded
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            myAnimator.SetLayerWeight(1, 1); //if in air, give airLayer animations Priority
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0); //revert to GroundLayer
        }
    }
}
