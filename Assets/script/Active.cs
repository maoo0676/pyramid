using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    public GameObject col;

    public bool isPlayerEnter; // Player�� ���� �ȿ� �Դ����� �Ǻ��� bool Ÿ�� ����

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        isPlayerEnter = false;
    }

    void Update()
    {
        // �÷��̾ ���� �ȿ� �ְ� E Ű�� �����ٸ�
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
