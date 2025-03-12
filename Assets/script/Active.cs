using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    Collision2D col;

    public bool isPlayerEnter; // Player�� ���� �ȿ� �Դ����� �Ǻ��� bool Ÿ�� ����

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<Collision2D>();

        isPlayerEnter = false;
    }

    void Update()
    {
        // �÷��̾ ���� �ȿ� �ְ� E Ű�� �����ٸ�
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
