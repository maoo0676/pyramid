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

    public GameObject[] Speech;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("floors"))
        {
            jumplimt = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("Door"))
        {
            Active(true, 0);
            if (Input.GetButtonDown("Trigger"))
            {
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.name.Equals("Chest"))
        {
            Active(true, 0);
            if (Input.GetButtonDown("Trigger"))
            {
                other.gameObject.SetActive(false);
            }
        }
        else
        {
            Active(false,0);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Key"))
        {
            Active(true, 1);
            if (Input.GetButtonDown("Receive"))
            {
                other.gameObject.SetActive(false);
                keymount++;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("Key"))
        {
            Active(false, 1);
        }
    }

    void Active(bool turning, int i)
    {
        Speech[i].SetActive(turning);
    }
}
