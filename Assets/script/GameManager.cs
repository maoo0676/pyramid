using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# Object")]
    public Image slot;
    public Image HpGauge;
    public Slider slider;
    public GameObject InDungeon;
    public GameObject Store;
    public Toggle Pause;
    public Transform player;

    [Header("# Timer Info")]
    public float curTime = 0; //* ÇöÀç ½Ã°£
    public float maxTime = 45;
    public float O2level = 1;
    public float O2tic = 1;

    [Header("# Map Info")]
    public GameObject[] maps;
    public int mapId;
    public int Stage = 1;
    public bool isClear = false;
    float time = 0;

    [Header("# Player Info")]
    public int Hp = 8;
    public float HitTime = 0.1f;
    public float AttackDelay = 0f;
    public int jumplimt = 0;
    public GameObject[] Dark;
    public int Sight = 0;

    [Header("# Bag Info")]
    public Image[] itemsimage;
    public Image[] slotitems;
    public int Gold = 0;
    public int SlotLimt = 4;
    public int SlotAmount = 0;
    public int Weight = 0;
    public int MaxWeight = 150;
    public int[] SlotId = { -1 };
    public int Keymount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;


        StageLoad(mapId);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))// Ä¡Æ®Å°--------------------------
        {
            curTime = 0;
            slider.value = curTime;
            Hp = 8;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            message("F2");
            if (mapId != 0)
            {
                message("F2");
                StageLoad(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            message("F3");
            if (Stage != 5 && mapId != 0)
            {
                message("F3");
                Stage++;
                StageLoad(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            message("f4");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if(mapId != 0)
            {
                Time.timeScale = 0;
                Pause.isOn = true;
                Pause.gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Player.Instance.Speed = 10;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }// Ä¡Æ®Å°------------------------------------------------------------

        if (!Pause.isOn) {
            Time.timeScale = 1;
            Pause.gameObject.SetActive(false);
        }

        if (Hp <= 0 || maxTime < curTime || mapId == 0 || Pause.isOn)
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
        if (time >= O2tic && maxTime > curTime)
        {
            curTime++;
            time = 0;
        }
        else if (maxTime == curTime)
        {
            StartCoroutine(Player.Instance.Dead());
            curTime++;
            message("dead");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))// ½½·ÔÅ°--------------------------
        {
            Slotactive(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Slotactive(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Slotactive(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Slotactive(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && SlotLimt >= 6)
        {
            Slotactive(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && SlotLimt >= 6)
        {
            Slotactive(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && SlotLimt >= 8)
        {
            Slotactive(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && SlotLimt >= 8)
        {
            Slotactive(8);
        }// ½½·ÔÅ°----------------------------------------------------------

    }
    public IEnumerator P_active(int i)
    {
        Player.Instance.Active(true, i);
        yield return new WaitForSeconds(2f);
        Player.Instance.Active(false, i);
    }

    public void message(string str)
    {
        Debug.Log(str);
    }

    void Slotactive(int i) //-------------------------------------½½·Ô
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

            for (int j = i; j < SlotLimt && SlotId[j] != -1; j++)
            {
                SlotId[j - 1] = SlotId[j];
                SlotId[j] = -1;
            }
            SlotAmount--;

            Slotsetting();
        }
    }

    public void Slotsetting()
    {
        foreach (Transform child in slot.transform)
        {
            Destroy(child.gameObject);
        }
        for (int j = 0; j < SlotAmount; j++)
        {
            slotitems[j] = Instantiate(itemsimage[SlotId[j] - 1], slot.transform.position + new Vector3(-412.8f + j * 120, 0, 0), Quaternion.identity, slot.transform);
            Debug.Log(slot.transform.position);
        }
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

    public void StageLoad(int i)//-------------------------------------¸Ê
    {
        message("" + i);
        slot.fillAmount = 0.125f * SlotLimt;
        curTime = 0;
        slider.value = curTime;
        if (i == 0)
        {
            mapId = Stage;
        }
        else if (i != 0)
        {
            mapId = 0;
            if (isClear)
            {
                maps[Stage].SetActive(false);
                Stage++;
            }
        }
        switch (mapId)
        {
            case 0:
                player.position = new Vector3(1, -0.5f, 0);
                break;
            case 1:
                player.position = new Vector3(-10.5f, 2.5f, 0);
                break;
            case 2:
                player.position = new Vector3(-12, 16, 0);
                break;
            case 3:
            case 4:
            case 5:
                player.position = new Vector3(-8, 16, 0);
                break;
        }

        switch (mapId)
        {
            case 1:
            case 2:
                O2tic = 1 * O2level;
                break;
            case 3:
            case 4:
                O2tic = 1f / 2 * O2level;
                break;
            case 5:
                O2tic = 1f / 4 * O2level;
                break;
        }

        if(i == 0)
        {
            message("" + Stage);
            maps[0].SetActive(false);
            maps[Stage - 1].SetActive(false);
            maps[Stage].SetActive(true);
        }
        else
        {
            maps[Stage].SetActive(false);
            maps[0].SetActive(true);
        }

        if (i != 0) InDungeon.SetActive(false);
        else InDungeon.SetActive(true);

        isClear = false;
    }

    public void selling(bool isLive)
    {
        if (isLive)
        {
            for(int i = 0; i < SlotAmount; i++)
            {
                switch(SlotId[i])
                {
                    case 7:
                        Gold += 100;
                        break;
                    case 8:
                        Gold += 500;
                        break;
                    case 9:
                        Gold += 1000;
                        break;
                    case 10:
                        Gold += 2000;
                        break;
                    case 11:
                        Gold += 3000;
                        break;
                    case 12:
                        Gold += 5000;
                        break;
                    case 13:
                        Gold += 10000;
                        break;
                }
                SlotId[i] = -1;
            }
        }
        else
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                SlotId[i] = -1;
            }
        }
        Slotsetting();
    }

    public void closeStore()//-------------------------------------»óÁ¡
    {
        GameManager.Instance.Pause.isOn = false;
        Store.SetActive(false);
    }

    public void O2GasUpgrade()
    {
        if (O2level == 1)
        {
            O2level++;
            GameObject.Find("O2Gas_0").SetActive(false);
        }
        else if (O2level == 2)
        {
            O2level++;
            GameObject.Find("O2Gas_1").SetActive(false);
        }
        else if (O2level == 3)
        {
            O2level++;
            GameObject.Find("O2Gas_2").SetActive(false);
        }
    }
    public void BagUpgrade()
    {
        if (SlotLimt == 4)
        {
            SlotLimt += 2;
            GameObject.Find("Bag_0").SetActive(false);
        }
        else if (SlotLimt == 6)
        {
            SlotLimt += 2;
            GameObject.Find("Bag_1").SetActive(false);
        }
    }
    public void LightUpgrade()
    {
        if (Sight == 0)
        {
            Sight++;
            GameObject.Find("Light_0").SetActive(false);
            Dark[0].SetActive(false);
        }
        else if (Sight == 1)
        {
            GameObject.Find("Light_1").SetActive(false);
            Dark[1].SetActive(false);
        }
    }
}
