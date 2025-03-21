using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    public GameObject col;
    public int Id;
    public int weight;

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
                    default:
                        if(GameManager.Instance.SlotAmount < GameManager.Instance.SlotLimt)
                        {
                            if(GameManager.Instance.Weight + weight > GameManager.Instance.MaxWeight)
                            {
                                StartCoroutine(GameManager.Instance.P_active(4));
                            }
                            else
                            {
                                GameManager.Instance.SlotId[GameManager.Instance.SlotAmount] = Id;
                                GameManager.Instance.SlotAmount++;
                                GameManager.Instance.Slotsetting();
                                col.SetActive(false);
                            }
                        }
                        else
                        {
                            StartCoroutine(GameManager.Instance.P_active(4));
                        }
                            break;
                }

                
            }
            else if(col.CompareTag("Doors"))
            {
                if(GameManager.Instance.Keymount > 0) {
                    col.SetActive(false);
                }
                else {
                    StartCoroutine(GameManager.Instance.P_active(2));
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
