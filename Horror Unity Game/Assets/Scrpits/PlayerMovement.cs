using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed, jumpForce;

    public bool Jumping;

    public bool m_FacingRight;

    public Rigidbody2D RG2D;

    private Animator Anim;

    private SpriteRenderer Renderer;

    [SerializeField] BoxCollider2D feet;


    // Start is called before the first frame update
    void Start()
    {
        RG2D = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        feet = GetComponent<BoxCollider2D>();
        Renderer = GetComponent<SpriteRenderer>();

        moveSpeed = 5f;
        jumpForce = 5f;

        Jumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Crouch();
        Jump();

    }

    void Walk()
    {
        float MovX = Input.GetAxisRaw("Horizontal"); //get horizontal player input (A or D)

        //Horizontal Movement
        if (MovX != 0)
        {
            RG2D.velocity = new Vector2(moveSpeed * MovX, RG2D.velocity.y);
            if (MovX < 0)
            {
                Renderer.flipX = true;
            }
            else
            {
                Renderer.flipX = false;
            }
            bool playerHasHorizontalSpeed = Mathf.Abs(RG2D.velocity.x) > Mathf.Epsilon; //set a bool to determine oif the player has horizontal speed
            Anim.SetBool("Walking", playerHasHorizontalSpeed); //if the player has horizontal speed the walking animation plays
        }
        else
        {
            RG2D.velocity = new Vector2(0, RG2D.velocity.y);    //stop moving if A or D is not pushed
            Anim.SetBool("Walking", false);                     //Turn off walking animation when movement stops
        }

    }

    void Jump()
    {
        float MovY = Input.GetAxisRaw("Vertical"); //Get vertical user input (W or S)


        if (!feet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }    //if the player isn't touching the ground they are unable to jump (stops double jump)

        if (Input.GetKeyDown("space"))
        {
            RG2D.velocity = new Vector2(RG2D.velocity.x, jumpForce);
        }

        /*
        if (MovY == 1) //if user input is W
        {
            RG2D.velocity = new Vector2(RG2D.velocity.x, jumpForce); //player jumps
        }
        */
    }

    void Crouch()
    {
        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftShift))         //if user input is Shift
        {
            Anim.SetBool("Crunching", true);

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Anim.SetBool("Crunching", false);

        }

    }

    void OnTriggerEnter2D(Collider other)
    {
        if (other.gameObject.CompareTag("Next"))
        {
            LoadScene(2);
        }
    }
}
