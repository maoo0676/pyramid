using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public int speed;
    public Rigidbody2D Target;

    bool isLive = true;
    public bool isHit = false;
    int Hp = 2;
    float distance;

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

    private void Update()
    {
        distance = Vector2.Distance(Target.position, rigid.velocity);
    }

    void FixedUpdate()
    {
        if (distance < 5f||!isLive||isHit)
            return;

        Vector2 dirVec = Target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (distance < 5f || !isLive||isHit)
            return;

        rend.flipX = Target.position.x < rigid.position.x;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == GameObject.FindGameObjectWithTag("Bullet")&&!isHit)
        {
            Hp--;
            isHit = true;
            if (Hp == 0)
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
        rigid.gravityScale = 1;
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    IEnumerator Hit(float time)
    {
        anim.SetTrigger("isHit");
        gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
        yield return new WaitForSeconds(time);
        isHit = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
