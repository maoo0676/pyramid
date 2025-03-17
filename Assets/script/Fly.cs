using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public int speed;
    public Rigidbody2D Target;

    bool isLive = true;
    public bool TargetIn = false;

    Rigidbody2D rigid;
    SpriteRenderer rend;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!TargetIn||!isLive)
            return;

        Vector2 dirVec = Target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!TargetIn || !isLive)
            return;

        rend.flipX = Target.position.x < rigid.position.x;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == Target.gameObject) TargetIn = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Target.gameObject) TargetIn = false;
    }
}
