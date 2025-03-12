using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    Collision2D col;

    public bool isPlayerEnter; // Player가 범위 안에 왔는지를 판별할 bool 타입 변수

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<Collision2D>();

        isPlayerEnter = false;
    }

    void Update()
    {
        // 플레이어가 범위 안에 있고 E 키를 누른다면
        if (isPlayerEnter && Input.GetButtonDown("Trigger"))
        {
            col.gameObject.SetActive(false);
        }

    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerEnter = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isPlayerEnter = false;
        }
    }
}
