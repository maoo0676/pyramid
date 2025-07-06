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
        if (Player.Instance.Freeze) return;

        // 플레이어가 범위 안에 있고 E 키를 누른다면
        if (isPlayerEnter && Input.GetButtonDown("Trigger"))
        {
            if (gameObject.CompareTag("Items"))
            {
                switch (Id)
                {
                    case -2:
                        gameObject.SetActive(false);
                        GameManager.Instance.KeyAmount++;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.Get);
                        break;
                    default:
                        if (GameManager.Instance.Casual&&0 <= Id&&Id <= 7)
                        {
                            GameManager.Instance.Slotactive(Id, true);

                            gameObject.SetActive(false);
                            AudioManager.instance.PlaySfx(AudioManager.Sfx.Get);
                            break;
                        }

                        if (GameManager.Instance.SlotAmount < 8)
                        {
                            if (GameManager.Instance.Weight + weight > GameManager.Instance.MaxWeight)
                            {
                                Debug.Log("가방이 무겁습니다.");
                                StartCoroutine(Player.Instance.Speak(1));
                            }
                            if (GameManager.Instance.SlotAmount >= GameManager.Instance.SlotLimt)
                            {
                                Debug.Log("가방이  포화 상태입니다.");
                                StartCoroutine(Player.Instance.Speak(0));
                            }
                            GameManager.Instance.SlotId[GameManager.Instance.SlotAmount] = Id;
                            GameManager.Instance.SlotAmount++;
                            GameManager.Instance.Weight += weight;
                            gameObject.SetActive(false);
                            GameManager.Instance.SlotSetting();
                            AudioManager.instance.PlaySfx(AudioManager.Sfx.Get);
                        }
                        else
                        {
                            Debug.Log("가방이 가득 찼습니다.");
                            StartCoroutine(Player.Instance.Speak(2));
                        }
                        break;
                }
            }
            else if (gameObject.CompareTag("Coin"))
            {
                GameManager.Instance.Gold += weight;
                gameObject.SetActive(false);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Get);
            }

            else if (gameObject.name.Equals("door"))
            {
                if (GameManager.Instance.KeyAmount > 0)
                {
                    GameManager.Instance.KeyAmount--;
                    gameObject.SetActive(false);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Active);
                }
                else
                {
                    StartCoroutine(Player.Instance.Speak(5));
                    //열쇠가 없습니다.
                }
            }
            else if (gameObject.name.Equals("exit"))
            {
                GameManager.Instance.selling(true);
                if (GameManager.Instance.Casual) return;

                GameManager.Instance.StageLoad(GameManager.Instance.mapId);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Active);
            }
            else if (gameObject.name.Equals("enter"))
            {
                GameManager.Instance.StageLoad(GameManager.Instance.mapId);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Active);
            }
            else if (gameObject.name.Equals("Store"))
            {
                Debug.Log("enter_shop");
                GameManager.Instance.Pause.isOn = true;
                GameManager.Instance.Store.SetActive(true);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Active);
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
