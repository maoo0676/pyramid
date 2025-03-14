using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    Rigidbody2D rigid;
    SpriteRenderer rend;
    Vector3 movement;

    public int movePower = 1;
    public float jumpPower = 1;
    int jumplimt = 0;
    bool isJumping = false;

    public GameObject[] Speech;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        rigid = gameObject.GetComponent<Rigidbody2D>();

        rend = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        Move();
        Jump();

    }

    //Physics engine Updates
    void FixedUpdate()
    {
    }

    //---------------------------------------------------[Movement Function]

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            rend.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            rend.flipX = false;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    public void Jump()
    {
        if (!isJumping||jumplimt >= 2)
            return;

        //Prevent Velocity amplification.
        rigid.velocity = Vector2.zero;

        rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);

        isJumping = false;
        jumplimt++;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("floors"))
        {
            jumplimt = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Door"))
        {
            Active(true, 0);
        }
        else if (other.gameObject.name.Equals("Chest"))
        {
            Active(true, 0);
        }
        else if (other.gameObject.name.Equals("Key"))
        {
            Active(true, 0);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Active(false, 0);
    }

    public void Active(bool turning, int i)
    {
        Speech[i].SetActive(turning);
    }
}
