using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int speed;
    public GameObject Return;

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
