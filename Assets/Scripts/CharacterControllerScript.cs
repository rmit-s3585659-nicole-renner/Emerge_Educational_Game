using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterControllerScript : MonoBehaviour {

    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool jump = false;

    public float moveForce = 365;
    public float maxSpeed = 5;
    public float jumpForce = 1000f;
    public Transform groundCheck;

    private bool grounded = false;
    private Animator anim;
    public GameObject player;
    private Rigidbody2D rb2d;

    private Vector2 touchOrigin = -Vector2.one;
    float h = 0;

    // Use this for initialization
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void FixedUpdate()
    {
            
    #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
            h = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
    #else

    #endif

            if (anim != null)
            {
                anim.SetFloat("Speed", Mathf.Abs(h));
            }
            if (h * rb2d.velocity.x < maxSpeed)
            {
                rb2d.AddForce(Vector2.right * h * moveForce);
            }
            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            {
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);
            }
        if (h > 0 && facingRight)
        {
            Flip();
        }
        else if (h < 0 && !facingRight)
        {
            Flip();
        }
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (jump && grounded)
        {
            if (anim != null)
            {
                anim.SetTrigger("Jump");
            }
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
        h = 0;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Collectible Triggered");
             if (coll.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Collectible"))
        {
            if (CollectibleController._cc == null)
            {
                Debug.Log("CC is Null");
            }
            CollectibleController._cc.
                HitCollectible();
            DestroyObject(coll.gameObject);
        }
    }

    public void GoingRight()
    {
        h = 5;
    }

    public void GoingLeft()
    {
        h = -5;
    }

    public void Jump()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (grounded == true)
        {
            jump = true;
        }
    }

    public void ReturnToStart()
    {
        Debug.Log("Returning to Start");
        GameController._gc.Setup();
        player.transform.position = new Vector3(0, 0, 0);
    }
}
