using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    Rigidbody2D rigid;
    public int movePower = 1;
    public float jumpPower = 1;
    public int keymount = 0;
    int jumplimt = 0;
    Vector3 movement;
    bool isJumping = false;

    public GameObject F;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
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
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping||jumplimt >= 2)
            return;

        //Prevent Velocity amplification.
        rigid.velocity = Vector2.zero;

        rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);

        isJumping = false;
        jumplimt++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Tilemap"))
        {
            jumplimt = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Door"))
        {
            F_Active(true);
            if (Input.GetButtonDown("Trigger"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.name.Equals("Chest"))
        {
            F_Active(true);
            if (Input.GetButtonDown("Trigger"))
            {
                collision.gameObject.SetActive(false);
            }
        }
        else
        {
            F_Active(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Key"))
        {
            F_Active(true);
            if (Input.GetButtonDown("Trigger"))
            {
                collision.gameObject.SetActive(false);
                keymount++;
            }
        }
        else
        {
            F_Active(false);
        }
    }

    void F_Active(bool turning)
    {
        F.SetActive(turning);
    }
}
