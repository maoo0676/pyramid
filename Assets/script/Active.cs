using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    public GameObject col;

    public bool isPlayerEnter; // Player가 범위 안에 왔는지를 판별할 bool 타입 변수

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        isPlayerEnter = false;
    }

    void Update()
    {
        // 플레이어가 범위 안에 있고 E 키를 누른다면
        if (isPlayerEnter && Input.GetButtonDown("Trigger"))
        {
            if(col.name.Equals("Key"))
            {
                col.SetActive(false);
                Bag.Instance.Keymount++;
            }
            else if(col.name.Equals("Door"))
            {
                if(Bag.Instance.Keymount > 0) {
                    col.SetActive(false);
                }
            }
            else
            {
                col.SetActive(false);
            }
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
