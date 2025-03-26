using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
    public GameObject col;
    public int Id;
    public int weight;

    public bool isPlayerEnter; // Player가 범위 안에 왔는지를 판별할 bool 타입 변수
    public bool isSolved;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        isPlayerEnter = false;
    }

    void Update()
    {
        if (GameManager.Instance.Hp <= 0 || GameManager.Instance.Pause.isOn)
        {
            return;
        }
        // 플레이어가 범위 안에 있고 E 키를 누른다면
        if (isPlayerEnter && Input.GetButtonDown("Trigger"))
        {
            if (col.CompareTag("Items"))
            {
                switch (Id)
                {
                    case 0:
                        col.SetActive(false);
                        GameManager.Instance.Keymount++;
                        break;
                    default:
                        if (GameManager.Instance.SlotAmount < GameManager.Instance.SlotLimt)
                        {
                            if (GameManager.Instance.Weight + weight > GameManager.Instance.MaxWeight)
                            {
                                StartCoroutine(GameManager.Instance.P_active(4));
                            }
                            else
                            {
                                GameManager.Instance.SlotId[GameManager.Instance.SlotAmount] = Id;
                                GameManager.Instance.SlotAmount++;
                                GameManager.Instance.Slotsetting();
                                GameManager.Instance.Weight += weight;
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
            else if (col.CompareTag("Doors"))
            {
                if (GameManager.Instance.Keymount > 0)
                {
                    col.SetActive(false);
                }
                else
                {
                    StartCoroutine(GameManager.Instance.P_active(2));
                }
            }
            else if (col.CompareTag("Doorway"))
            {
                GameManager.Instance.StageLoad(GameManager.Instance.mapId);
                GameManager.Instance.selling(true);
            }
            else if (col.name.Equals("Peddler"))
            {
                Debug.Log("Peddler");
                GameManager.Instance.Pause.isOn = true;
                GameManager.Instance.Store.SetActive(true);
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
