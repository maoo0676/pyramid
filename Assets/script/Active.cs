using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    GameObject player;
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
        if (Player.Instance.Freeze|| GameManager.Instance.Pause.isOn) return;

        // 플레이어가 범위 안에 있고 E 키를 누른다면
        if (isPlayerEnter && Input.GetButtonDown("Trigger"))
        {
            if (gameObject.CompareTag("Items"))
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Get);
                switch (Id)
                {
                    case 0:
                        gameObject.SetActive(false);
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
                                gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            StartCoroutine(GameManager.Instance.P_active(4));
                        }
                        break;
                }


            }
            else if (gameObject.CompareTag("Doors"))
            {
                if (GameManager.Instance.Keymount > 0)
                {
                    GameManager.Instance.Keymount--;
                    gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(GameManager.Instance.P_active(2));
                }
            }
            else if (gameObject.CompareTag("Doorway"))
            {
                if (GameManager.Instance.mapId != 0) GameManager.Instance.selling(true);
                GameManager.Instance.StageLoad(GameManager.Instance.mapId);
            }
            else if (gameObject.name.Equals("enter_shop"))
            {
                Debug.Log("enter_shop");
                GameManager.Instance.Pause.isOn = true;
                GameManager.Instance.Store.SetActive(true);
                GameManager.Instance.StageText.SetActive(false);
            }
            else if (gameObject.name.Equals("tutorial"))
            {
                GameManager.Instance.Pause.isOn = true;
                GameManager.Instance.Tutorial.SetActive(true);
                GameManager.Instance.StageText.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
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
