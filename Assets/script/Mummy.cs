using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonoBehaviour
{
    public int speed;
    public Rigidbody2D Return;

    bool isLive = true;
    public bool turn = true, isHit = false;
    int Hp = 4;

    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isLive||isHit)
            return;

        Vector3 moveVelocity = Vector3.zero;

        if (turn)
        {
            moveVelocity = Vector3.right;
            rend.flipX = false;
        }
        else
        {
            moveVelocity = Vector3.left;
            rend.flipX = true;
        }

        transform.position += moveVelocity * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Return.gameObject) turn = !turn;

        if (other.gameObject == GameObject.FindGameObjectWithTag("Bullet") && !isHit) {
            Hp--;
            isHit = true;
            if(Hp == 0)
            {
                StartCoroutine(Dead(2f));
            }
            else
            {
                StartCoroutine(Hit(1f));
            }
        }
    }

    IEnumerator Dead(float time)
    {
        anim.SetBool("isDead", true);
        isLive = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(time);
        isLive = true;
        isHit = false;
        gameObject.SetActive(false);

        Hp = 4;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    IEnumerator Hit(float time)
    {
        gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
        yield return new WaitForSeconds(time);
        isHit = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
