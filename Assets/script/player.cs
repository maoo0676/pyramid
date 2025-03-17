using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;
    Vector3 movement;

    public int movePower = 1;
    public float jumpPower = 1;
    int jumplimt = 0;
    bool isJumping = false;

    public GameObject[] Speech;
    public Image HpGauge;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        rigid = GetComponent<Rigidbody2D>();

        rend = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Hp <= 0)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        Move();
        Jump();

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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Monster")&&GameManager.Instance.HitTime <= 0)
        {
            GameManager.Instance.Hp--;
            HpGauge.fillAmount -= 0.125f;
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;

            if (GameManager.Instance.Hp <= 0)
            {
                anim.SetBool("isDead", true);
            }
            else
            {
                anim.SetTrigger("isHit");
                StartCoroutine(WaitForIt());
                rigid.constraints = RigidbodyConstraints2D.None;
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                GameManager.Instance.HitTime = 2;
            }
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
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(1f);
    }
}
