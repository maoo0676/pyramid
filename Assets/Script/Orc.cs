using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Orc : MonoBehaviour
{
    public int speed;
    public int Start, End;

    public bool turn = true;

    SpriteRenderer rend;

    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (transform.position.x < Start || transform.position.x > End) turn = !turn;

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
}
