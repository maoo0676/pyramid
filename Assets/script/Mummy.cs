using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonoBehaviour
{
    public int speed;
    public Rigidbody2D Return;

    bool isLive = true;
    public bool turn = true;

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
        if (!isLive)
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
    }
}
