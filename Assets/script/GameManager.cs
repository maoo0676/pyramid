using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# UI")]
    public GameObject InDungeon;
    public GameObject Store;
    public GameObject Result;
    public GameObject Tutorial;
    public GameObject[] text = new GameObject[5];

    [Header("# System Info")]
    public int Gold = 0;
    public int Score = 0;
    public Text GoldText;
    public Text KeyText;
    public Text ScoreText;
    public GameObject StageText;
    public bool isClear = false;

    [Header("# Object")]
    public GameObject slot;
    public Image HpGauge;
    public Image O2Gauge;
    public Toggle Pause;
    public Transform player;

    [Header("# Timer Info")]
    public int curTime = 45; //* 현재 시간
    public float O2tic = 1;
    float time = 0;

    [Header("# Map Info")]
    public GameObject[] maps;
    public int mapId;
    public int Stage = 1;
    public bool isMapStart = false;
    public GameObject FoundTresure = null;
    public Image wave;
    public GameObject waveCenter;

    [Header("# Player Info")]
    public int Hp = 8;
    public float HitTime = 0.1f;
    public float AttackDelay = 0f;
    public int jumplimt = 0;
    public GameObject Dark;

    [Header("# Bag Info")]
    public Image[] itemsimage;
    public Image[] slotitems;
    public int SlotLimt = 4;
    public int SlotAmount = 0;
    public int Weight = 0;
    public int MaxWeight = 150;
    public int[] SlotId = { -1 };
    public int Keymount = 0;
    public float itempadding;

    [Header("# Store Info")]
    public GameObject[] SText;
    public int[] SPrice;
    public int O2level = 1;
    public int BagLevel = 1;
    public int LightLevel = 1;

    [Header("# SomeThing")]
    public Color Hurt = new Color(1, 126 / 255f, 126 / 255f);


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        StageLoad(mapId);

        DataManager.Instance.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        GoldText.text = Gold.ToString();
        KeyText.text = Keymount.ToString();
        ScoreText.text = Score.ToString();

        HpGauge.fillAmount = 0.125f * Hp;
        O2Gauge.fillAmount = 0.022222f * curTime;

        if (Input.GetKeyDown(KeyCode.F1))// 치트키--------------------------
        {
            curTime = 45;
            Hp = 8;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            for (int i = 0; i < SPrice.Length; i++)
            {
                SPrice[i] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                SText[i].transform.GetChild(0).GetComponent<Text>().text = "0G";
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("F3");
            if (mapId != 0)
            {
                Debug.Log("reset");
                StageLoad(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (Stage != 5 && mapId != 0)
            {
                Debug.Log("F4");
                Stage++;
                StageLoad(0);
            }
            else if (mapId == 0)
            {
                Stage = 1;
                StageLoad(0);
            }
            else Debug.Log("is none");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if(mapId != 0)
            {
                Time.timeScale = 0;
                Pause.isOn = !Pause.isOn;
                Pause.gameObject.SetActive(Pause.isOn);
            }
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Player.Instance.Speed = 10;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            GameResult();
        }// 치트키------------------------------------------------------------

        if (!Pause.isOn) {
            Time.timeScale = 1;
            Pause.gameObject.SetActive(false);
        }

        if (Hp <= 0 || 0 > curTime || mapId == 0 || Pause.isOn)
            return;

        if (Hp > 8) Hp = 8;
        if (curTime > 45) curTime = 45;

        if (HitTime > 0)
        {
            HitTime -= Time.deltaTime;
        }
        else player.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);

        if (AttackDelay > 0)
        {
            AttackDelay -= Time.deltaTime;
        }

        time += Time.deltaTime;
        if (time >= O2tic && 0 < curTime)
        {
            curTime--;
            time = 0;
        }
        else if (0 == curTime)
        {
            StartCoroutine(Player.Instance.Dead());
            curTime--;
            Debug.Log("dead");
        }

        if (waveCenter.gameObject.activeSelf)
        {
            float distance = Vector2.Distance(player.position, FoundTresure.transform.position);
            Debug.Log($"{distance}");

            if (distance < 3) wave.fillAmount = 0;
            else wave.fillAmount = 1;

            waveCenter.transform.rotation = Quaternion.FromToRotation(Vector3.up, FoundTresure.transform.position - player.position);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))// 슬롯키--------------------------
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
        }// 슬롯키----------------------------------------------------------

    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void GameResult()
    {
        DataManager.Instance.EndGame();
        Result.SetActive(true);
    }

    public IEnumerator P_active(int i)
    {
        Player.Instance.Active(true, i);
        yield return new WaitForSeconds(2f);
        Player.Instance.Active(false, i);
    }

    void Slotactive(int i) //-------------------------------------슬롯
    {
        Weight -= 10;
        switch(SlotId[i - 1])
        {
            case 1:
                Hp += 2;
                break;
            case 2:
                curTime += 9;
                break;
            case 3:
                if(FoundTresure == null) StartCoroutine(P_active(4));

                else waveCenter.gameObject.SetActive(true);

                Debug.Log("pathfinder");
                break;
            case 4:
                if(Player.Instance.Speed <= 8)
                    Player.Instance.Speed = 8;
                break;
            case 5:
                Player.Instance.Speed = 10;
                break;
            case 6:
                HitTime = 10;
                player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                break;
            default:
                Weight += 10;
                StartCoroutine(P_active(4));
                break;
        }

        StartCoroutine(ItemabilityEnd(SlotId[i - 1]));

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
        for (int i = 0; i < SlotLimt; i++)
        {
            Transform items = slot.transform.GetChild(i);
            foreach (Transform child in items)
            {
                Destroy(child.gameObject);
            }

            if (SlotId[i] == -1) continue;

            slotitems[i] = Instantiate(itemsimage[SlotId[i] - 1], items.position, Quaternion.identity, items);
            Debug.Log(SlotId[i] - 1);    
        }
    }
    IEnumerator ItemabilityEnd(int i)
    {
        yield return new WaitForSeconds(10f);
        switch(i)
        {
            case 4:
            case 5:
                Player.Instance.Speed = 5;
                break;
            case 6:
                player.GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }
    }

    public void StageLoad(int i)//-------------------------------------맵
    {
        curTime = 45;
        Hp = 8;
        Weight = 0;
        FoundTresure = null;

        if (i == 0)
        {
            mapId = Stage;
            isMapStart = true;
        }
        else if (i != 0)
        {
            mapId = 0;
            if (isClear)
            {
                if (Stage == 4)
                {
                    GameResult();
                }
                maps[Stage].SetActive(false);
                Stage++;
                Score += Gold;
            }
        }

        StageText.transform.GetChild(0).GetComponent<Text>().text = Stage.ToString();

        Debug.Log("" + mapId);
        
        player.position = new Vector3(1, -0.5f, 0);

        switch (mapId)
        {
            case 1:
                O2tic = 1 * O2level;
                break;
            case 2:
            case 3:
                O2tic = 1f / 2 * O2level;
                break;
            case 4:
                O2tic = 1f / 4 * O2level;
                break;
        }

        if(mapId != 0)
        {
            maps[0].SetActive(false);
            maps[Stage - 1].SetActive(false);
            maps[Stage].SetActive(true);
        }
        else
        {
            maps[Stage].SetActive(false);
            maps[0].SetActive(true);
        }

        if (mapId == 0) InDungeon.SetActive(false);
        else InDungeon.SetActive(true);

        isClear = false;
    }

    public void selling(bool isLive)
    {
        if (isLive)
        {
            int j = 0;
            for(int i = 0; i < SlotAmount; i++)
            {
                switch(SlotId[i])
                {
                    case 7:
                        j += 500;
                        break;
                    case 8:
                        j += 1000;
                        break;
                    case 9:
                        j += 2000;
                        break;
                    case 10:
                        j += 3000;
                        break;
                    case 11:
                        j += 4000;
                        break;
                    case 12:
                        j += 6000;
                        break;
                    case 13:
                        j += 10000;
                        break;
                }
                SlotId[i] = -1;
            }

            Gold += j;
            Score += j;
        }
        else
        {
            for (int i = 0; i < SlotAmount; i++)
            {
                SlotId[i] = -1;
            }
        }
        SlotAmount = 0;
        Slotsetting();
    }
    public void closeTutorial()//-------------------------------------상점
    {
        Pause.isOn = false;
        Tutorial.SetActive(false);
        StageText.SetActive(true);
    }


    public void closeStore()//-------------------------------------상점
    {
        Pause.isOn = false;
        Store.SetActive(false);
        StageText.SetActive(true);
    }

    public void O2GasUpgrade()
    {
        if (Gold - SPrice[O2level - 1] < 0) return;

        Gold -= SPrice[O2level - 1];

        O2level++;

        switch (O2level - 1)
        {
            case 1:
                SText[0].transform.GetChild(0).GetComponent<Text>().text = SPrice[O2level - 1].ToString() + "G";
                SText[0].transform.GetChild(1).GetComponent<Text>().text = "중압용 산소통";
                break;
            case 2:
                SText[0].transform.GetChild(0).GetComponent<Text>().text = SPrice[O2level - 1].ToString() + "G";
                SText[0].transform.GetChild(1).GetComponent<Text>().text = "고압용 산소통";
                break;
            case 3:
                GameObject.Find("O2Gas_Sell").SetActive(false);
                break;
        }
    }
    public void BagUpgrade()
    {
        if (Gold - SPrice[2 + BagLevel] < 0) return;

        Gold -= SPrice[2 + BagLevel];

        BagLevel++;

        switch (BagLevel)
        {
            case 2:
                SlotLimt = 6;
                MaxWeight = 250;
                SText[1].transform.GetChild(0).GetComponent<Text>().text = SPrice[2 + BagLevel].ToString() + "G";
                SText[1].transform.GetChild(1).GetComponent<Text>().text = "초대형 가방";
                break;
            case 3:
                SlotLimt = 8;
                MaxWeight = 400;
                GameObject.Find("Bag_Sell").SetActive(false);
                break;
        }
        int j = 0;
        foreach (Transform child in slot.transform)
        {
            if (SlotLimt >= j + 1) slot.transform.GetChild(j).gameObject.SetActive(true);
            else break;
            j++;
        }
    }
    public void LightUpgrade()
    {
        if (Gold - SPrice[5 + LightLevel] < 0) return;

        Gold -= SPrice[5 + LightLevel];

        LightLevel++;

        switch (LightLevel)
        {
            case 1:
                SText[2].transform.GetChild(0).GetComponent<Text>().text = SPrice[4 + LightLevel].ToString() + "G";
                SText[2].transform.GetChild(1).GetComponent<Text>().text = "초고급 손전등";
                Dark.transform.localScale = new Vector3(1.75f, 2f);
                break;
            case 2:
                GameObject.Find("Light_Sell").SetActive(false);
                Dark.transform.localScale = new Vector3(2f, 2.5f);
                break;
        }
    }
}
