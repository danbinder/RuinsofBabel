using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    private Rigidbody2D myRigidbody2;

    private Animator myAnimator2;

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

    private bool death;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce; //power to the jump



    // Use this for initialization
    void Start()
    {
        facingRight = false;
        myRigidbody2 = GetComponent<Rigidbody2D>();
        myAnimator2 = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        HandleInput();

        HandleMovement(horizontal);

        Flip(horizontal);

        HandleAttacks();

        ResetValues();
    }

    private void HandleMovement(float horizontal)
    {
        if (!death && !this.myAnimator2.GetCurrentAnimatorStateInfo(0).IsTag("eAttack")) //layer 0 (only layer so far)
        {   //move player only if not attacking
            myRigidbody2.position += new Vector2(-1 * Time.deltaTime, myRigidbody2.velocity.y); //x val of -1, y val of 0;

        }
        

        myAnimator2.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if (attack) //if we press jump button & are on ground -- make player jump
        {
            myAnimator2.SetTrigger("attack");
        }
        if (death)
        {
            myAnimator2.SetTrigger("death");

        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            myAnimator2.SetTrigger("death");
            death = true;

        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            attack = true;
        }
    }

    private void Flip(float horizontal)
    {

    }

    private void ResetValues()
    {
        attack = false;
    }

    private bool IsGrounded()
    {
        if (myRigidbody2.velocity.y <= 0) //falling or stopped moving down
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject) //if grounded
                    {
                        myAnimator2.ResetTrigger("jump");
                        myAnimator2.SetBool("landing", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }


}
