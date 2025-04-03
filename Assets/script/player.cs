using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;
    Vector3 movement;

    public int Speed;
    public float jumpPower;
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
        if (GameManager.Instance.Hp <= 0||Freeze||GameManager.Instance.Pause.isOn)
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

        transform.position += moveVelocity * Speed * Time.deltaTime;
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
        anim.SetTrigger("isAttack");

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
                StartCoroutine(Dead());
            }
            else
            {
                anim.SetTrigger("isHit");
                StartCoroutine(Freezecancel(0.6f));
                gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
                GameManager.Instance.HitTime = 2;
            }
        }
        else if (other.gameObject.name.Equals("floors"))
        {
            GameManager.Instance.jumplimt = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Equals("lava"))
        {
            GameManager.Instance.Hp--;

            if (GameManager.Instance.Hp <= 0)
            {
                StartCoroutine(Dead());
            }
            else
            {
                isJumping = true;
                GameManager.Instance.jumplimt = 0;
                gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
                GameManager.Instance.HitTime = 0.5f;
                Jump();
            }
        }
        else if (other.gameObject.CompareTag("Bullet")) return;
        else if (other.gameObject.name.Equals("hidden")) other.gameObject.GetComponent<Tilemap>().color = new Color(1, 1, 1, 1/2f);
        else Active(true, 0);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Active(false, 0);
        if(other.gameObject.name.Equals("hidden")) other.gameObject.GetComponent<Tilemap>().color = Color.white;
    }

    public void Active(bool turning, int i)
    {
        Speech[i].SetActive(turning);
    }

    IEnumerator Freezecancel(float time)
    {
        yield return new WaitForSeconds(time);
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        Freeze = false;
        Debug.Log("¶¯");
    }

    public IEnumerator Dead()
    {
        Freeze = true;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        //anim.SetBool("isDead", true);
        GameManager.Instance.selling(false);
        Debug.Log("¾óÀ½");
        yield return new WaitForSeconds(5f);

        GameManager.Instance.StageLoad(GameManager.Instance.mapId);

        StartCoroutine(Freezecancel(2f));
        yield return new WaitForSeconds(2f);
        //anim.SetBool("isDead", false);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    IEnumerator delete(float time, int i)
    {
        yield return new WaitForSeconds(time);

        AttackEffect[i].SetActive(false);
    }
}
