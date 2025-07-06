using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public int speed;
    public Rigidbody2D Target;

    float distance = 10f;

    Rigidbody2D rigid;
    SpriteRenderer rend;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        distance = 10f;
    }

    private void Update()
    {
        distance = Vector2.Distance(Target.position, rigid.velocity);
    }

    void FixedUpdate()
    {
        Debug.Log(distance);
        if (distance >= 5f || !gameObject.GetComponent<Monster>().isLive || gameObject.GetComponent<Monster>().isHit)
            return;


        Vector2 dirVec = Target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (distance >= 5f || !gameObject.GetComponent<Monster>().isLive || gameObject.GetComponent<Monster>().isHit)
            return;

        rend.flipX = Target.position.x < rigid.position.x;
    }
}
