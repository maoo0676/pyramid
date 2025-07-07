using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bat : MonoBehaviour
{
    public int speed;
    public float range = 5f;
    float distance;

    public Transform Target;


    Rigidbody2D rigid;
    SpriteRenderer rend;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        if (!gameObject.GetComponent<Monster>().isLive || gameObject.GetComponent<Monster>().isHit)
            return;

        // 대상과의 거리 계산
        distance = Vector3.Distance(transform.position, Target.position);

        // 범위 안에 들어오면 따라감
        if (distance <= range)
        {
            // 대상 방향으로 이동
            Vector3 direction = (Target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        if (distance >= 4f || !gameObject.GetComponent<Monster>().isLive || gameObject.GetComponent<Monster>().isHit)
            return;

        rend.flipX = Target.position.x < rigid.position.x;
    }
}
