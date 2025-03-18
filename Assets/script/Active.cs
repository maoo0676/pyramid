using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    public GameObject col;
    public int Id;

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
            if(col.CompareTag("Items"))
            {
                switch(Id)
                {
                    case 0:
                        col.SetActive(false);
                        GameManager.Instance.Keymount++;
                        break;
                }
            }
            else if(col.name.Equals("Door"))
            {
                if(GameManager.Instance.Keymount > 0) {
                    col.SetActive(false);
                }
                else {
                    Player.Instance.Active(true, 2);
                    StartCoroutine(Wait(2));
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
    IEnumerator Wait(int i)
    {
        yield return new WaitForSeconds(2f);
        Player.Instance.Active(false, i);
    }
}
