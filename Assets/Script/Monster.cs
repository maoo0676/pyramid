using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isLive = true;
    public bool isHit = false;
    public int Hp;
    int hp;
    public int Id; // 0 유령
                   // 1 박쥐
                   // 3 기사
    Vector2 respawn;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Awake()
    {
        respawn = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        hp = Hp;
    }

    private void Update()
    {
        if (GameManager.Instance.isMapStart) ReStart();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Id == 0) return;

        if (other.gameObject == GameObject.FindGameObjectWithTag("Bullet"))
        {

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Attack);
            Hp -= GameManager.Instance.Damage;
            isHit = true;
            if (Hp <= 0)
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
        isLive = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if (Id == 1) rigid.gravityScale = 1;

        yield return new WaitForSeconds(time);

        isLive = true;
        isHit = false;
        gameObject.SetActive(false);

        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        if (Id == 1) rigid.gravityScale = 0;
        transform.position = respawn;
    }
    IEnumerator Hit(float time)
    {
        gameObject.GetComponent<SpriteRenderer>().color = GameManager.Instance.Hurt;
        yield return new WaitForSeconds(time);
        isHit = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void ReStart()
    {
        Hp = hp;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        if (Id == 1) rigid.gravityScale = 0;
        transform.position = respawn;
    }
}
