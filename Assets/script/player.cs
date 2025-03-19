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
    bool isJumping = false;
    bool isAttack = false;
    bool Freeze = false;

    public GameObject[] Speech;
    public GameObject[] AttackEffect;

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
        if (GameManager.Instance.Hp <= 0||Freeze)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (Input.GetButtonDown("Attack"))
        {
            isAttack = true;
        }

        Move();

        Attack();

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
        if (!isJumping||GameManager.Instance.jumplimt >= 1)
        {
            isJumping = false;
            return;
        }
        GameManager.Instance.jumplimt++;

        //Prevent Velocity amplification.
        rigid.velocity = Vector2.zero;

        rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);

        isJumping = false;
    }

    public void Attack()
    {
        if (!isAttack||GameManager.Instance.AttackDelay > 0)
        {
            isAttack = false;
            return;
        }

        int LR = (rend.flipX)? 0: 1;
        AttackEffect[LR].SetActive(true);

        StartCoroutine(delete(0.2f, LR));

        GameManager.Instance.AttackDelay = 2f;
        isAttack = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("floors"))
        {
            GameManager.Instance.jumplimt = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Monster")&&GameManager.Instance.HitTime <= 0)
        {
            GameManager.Instance.Hp--;
            Freeze = true;

            if (GameManager.Instance.Hp <= 0)
            {
                Dead();
            }
            else
            {
                anim.SetTrigger("isHit");
                StartCoroutine(Freezecancel(0.6f));
                GameManager.Instance.HitTime = 2;
            }
        }
        else if (other.gameObject.name.Equals("floors"))
        {
            GameManager.Instance.jumplimt = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Doors"))
        {
            Active(true, 0);
        }
        else if (other.gameObject.CompareTag("Chests"))
        {
            Active(true, 0);
        }
        else if (other.gameObject.CompareTag("Items"))
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

    IEnumerator Freezecancel(float time)
    {
        yield return new WaitForSeconds(time);
        Freeze = false;
    }

    public void Dead()
    {
        Freeze = true;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        anim.SetBool("isDead", true);
    }

    IEnumerator delete(float time, int i)
    {
        yield return new WaitForSeconds(time);

        AttackEffect[i].SetActive(false);
    }
}
