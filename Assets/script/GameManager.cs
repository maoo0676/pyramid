using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# System Info")]
    public Image slot;
    public Image HpGauge;
    public Slider slider;

    public float curTime = 0; //* 현재 시간
    public float maxTime = 45;
    public float O2level = 1;

    public int mapId;
    public GameObject[] maps;
    public GameObject InDungeon;
    public int Stage = -1;
    float time = 0;

    [Header("# Player Info")]
    public int Hp = 8;
    public float HitTime = 0.1f;
    public float AttackDelay = 0f;
    public int jumplimt = 0;
    [Header("# Bag Info")]
    public int Gold = 0;
    public int SlotAmount = 4;
    public int[] SlotId = { -1 };
    public int Keymount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        StageLoad(mapId);
        StageLoad(mapId);
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0 || maxTime < curTime || mapId == 0)
            return;

        if (Hp > 8) Hp = 8;
        if (curTime < 0) curTime = 0;

        HpGauge.fillAmount = 0.125f * Hp;
        slider.value = curTime;

        if (HitTime > 0)
        {
            HitTime -= Time.deltaTime;
        }
        if (AttackDelay > 0)
        {
            AttackDelay -= Time.deltaTime;
        }

        time += Time.deltaTime;
        if (time >= O2level && maxTime > curTime)
        {
            curTime++;
            time = 0;
        }
        else if (maxTime == curTime)
        {
            Player.Instance.Dead();
            curTime++;
            message("dead");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))// 슬롯키--------------------------
        {
            Slot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Slot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Slot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Slot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Slot(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Slot(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Slot(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Slot(8);
        }// 슬롯키----------------------------------------------------------

        if (Input.GetKeyDown(KeyCode.F1))// 치트키--------------------------
        {
            curTime = 0;
            slider.value = curTime;
            Hp = 8;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            StageLoad(Stage);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if(Stage != 5)
                StageLoad(Stage + 1);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            message("f4");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            message("f5");
        }// 치트키------------------------------------------------------------
    }
    void message(string str)
    {
        Debug.Log(str);
    }

    void Slot(int i)
    {
        switch(SlotId[i - 1])
        {
            case 1:
                Hp += 2;
                break;
            case 2:
                curTime -= 9;
                break;
            case 3:
                break;
            case 4:
                if(Player.Instance.Speed <= 8)
                    Player.Instance.Speed = 8;
                break;
            case 5:
                Player.Instance.Speed = 10;
                break;
            case 6:
                break;
            default:
                StartCoroutine(P_active(4));
                break;
        }

        StartCoroutine(Itemability(SlotId[i - 1]));

        if (1 <= SlotId[i - 1]&&SlotId[i - 1] <= 6)
        {
            SlotId[i - 1] = -1;

            for (int j = i; j < SlotAmount && SlotId[j] != -1; j++)
            {
                SlotId[j - 1] = SlotId[j];
                SlotId[j] = -1;
            }

        }
    }
    public IEnumerator P_active(int i)
    {
        Player.Instance.Active(true, i);
        yield return new WaitForSeconds(2f);
        Player.Instance.Active(false, i);
    }
    IEnumerator Itemability(int i)
    {
        yield return new WaitForSeconds(10f);
        switch(i)
        {
            case 4:
            case 5:
                Player.Instance.Speed = 5;
                break;
        }
        Player.Instance.Active(false, i);
    }

    void StageLoad(int i)
    {
        message("i" + i);
        slot.fillAmount = 0.125f * SlotAmount;
        slider.maxValue = maxTime;

        if (i == 0) InDungeon.SetActive(false);
        else InDungeon.SetActive(true);
    }
}
