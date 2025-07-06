using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Camera;

    [Header("# UI")]
    public GameObject InDungeon;
    public GameObject Store;
    public GameObject Result;
    public GameObject Tutorial;
    public GameObject[] text = new GameObject[5];

    [Header("# System Info")]
    public int Gold = 0;
    public int Score = 0;
    public Text ScoreText;
    public Text GoldText;
    public Text KeyText;
    public GameObject StageText;
    public bool isClear = false;
    public bool Casual; // true: 캐주얼, false: 클래식

    [Header("# Object")]
    public GameObject Slot;
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
    public int StageLimt = 5;
    public bool isMapStart = false;
    public GameObject FoundTresure = null;
    public Image wave;
    public GameObject waveCenter;

    [Header("# Player Info")]
    public int Hp = 8;
    public float HitTime = 0.1f;
    public float AttackDelay = 0f;
    public float Delay = 0f;
    public Image AttackDelayimage;
    public int JumpLimt = 0;
    public int Damage = 1;
    public GameObject Dark;

    [Header("# Bag Info")]
    public Image[] itemsimage;
    public Image[] slotitems;
    public Text WeightText;
    public int SlotLimt = 4;
    public int SlotAmount = 0;
    public int Weight = 0;
    public int MaxWeight = 150;
    public int[] SlotId = { -1 };
    public int KeyAmount = 0;
    bool isFull = false;
    public bool isHard = false;

    [Header("# Store Info")]
    public GameObject[] SText;
    public int[] SPrice;
    public int O2level = 1;
    public int BagLevel = 1;
    public int LightLevel = 1;
    public int KnifeLevel = 1;
    public bool isFree = false;
    bool stat = true;

    [Header("# SomeThing")]
    public Color Hurt = new Color(1, 126 / 255f, 126 / 255f);
    int Fancount = 0;

    void Awake()
    {
        Instance = this;

        DataManager.Instance.StartGame();
    }
    void Update()
    {
        if (stat)
        {
            StageLoad(1);

            DataManager.Instance.StartGame();

            stat = false; 
        }

        Camera.transform.position = player.position + new Vector3(0, 0, -1);

        GoldText.text = Gold.ToString() + "G";
        KeyText.text = KeyAmount.ToString();
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
            isFree = true;
            for (int i = 0; i < 5; i++)
            {
                SText[i].transform.GetChild(0).GetComponent<Text>().text = "0G";
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("F3");
            Debug.Log("reset");

            curTime = -1;
            selling(false);

            if (mapId != 0) StageLoad(0);
            else StageLoad(Stage);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("F4");

            curTime = -1;
            selling(false);

            if (Stage != 5 && mapId != 0)
            {
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
            Time.timeScale = 0;
            Pause.isOn = !Pause.isOn;
            Pause.gameObject.SetActive(Pause.isOn);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Player.Instance.Speed = 10;
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            GameResult();
        }// 치트키------------------------------------------------------------

        if (!Pause.isOn)
        {
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

        if (AttackDelay >= Delay)
        {
            Delay += Time.deltaTime;

            AttackDelayimage.fillAmount = Delay / AttackDelay;
            Debug.Log(Delay);
        }
        else AttackDelayimage.fillAmount = 0;

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
            Slotactive(1, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Slotactive(2, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Slotactive(3, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Slotactive(4, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && SlotLimt >= 6)
        {
            Slotactive(5, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && SlotLimt >= 6)
        {
            Slotactive(6, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) && SlotLimt >= 8)
        {
            Slotactive(7, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) && SlotLimt >= 8)
        {
            Slotactive(8, false);
        }
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
        if (Casual)
        {
            DataManager.Instance.EndCasual();
        }
        else {
            DataManager.Instance.EndGame();
        }
        Result.SetActive(true);
    }

    public void Slotactive(int i, bool immediately) //-------------------------------------슬롯
    {
        int S_id;

        if (immediately) S_id = i;
        else {
            S_id = SlotId[i - 1];
            Weight -= 10;
        }

        if (S_id != -1 || S_id <= 5)
        {
            StartCoroutine(Player.Instance.Speak(S_id + 6));
        }
        switch (S_id)
        {
            case -1:
                Weight += 10;
                StartCoroutine(Player.Instance.Speak(2));
                break;
            case 0:
                Hp += 2;
                break;
            case 1:
                curTime += 9;
                break;
            case 2:
                if (FoundTresure == null) StartCoroutine(Player.Instance.Speak(4));

                else
                {
                    waveCenter.transform.rotation = Quaternion.FromToRotation(Vector3.up, FoundTresure.transform.position - player.position);
                    waveCenter.gameObject.SetActive(true);
                }

                Debug.Log("pathfinder");
                break;
            case 3:
                if (Player.Instance.Speed <= 8)
                {
                    Player.Instance.Speed = 8;
                    Fancount++;
                }
                break;
            case 4:
                Player.Instance.Speed = 10;
                Fancount++;
                break;
            case 5:
                HitTime = 10;
                player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                break;
            case 6:
                Dark.GetComponent<RectTransform>().localScale *= 1.2f;
                break;
            case 7:
                Player.Instance.GetComponent<Rigidbody2D>().gravityScale *= 0.6f;
                break;
            default:
                Weight += 10;
                StartCoroutine(Player.Instance.Speak(3));
                break;
        }

        StartCoroutine(ItemabilityEnd(S_id));

        if (immediately) return;

        if (0 <= S_id && S_id <= 7)
        {
            S_id = -1;

            for (int j = i; j < SlotAmount && SlotId[j] != -1; j++)
            {
                SlotId[j - 1] = SlotId[j];
                SlotId[j] = -1;
            }
            SlotAmount--;

            SlotSetting();
        }
    }

    public void SlotSetting()
    {
        for (int i = 0; i < 8; i++)
        {
            Transform Slotitem = Slot.transform.GetChild(i);
            foreach (Transform child in Slotitem)
            {
                if (child.gameObject.name.Equals("num")) continue;
                child.gameObject.SetActive(false);
            }
            if (i >= SlotAmount) continue;

            slotitems[i] = Instantiate(itemsimage[SlotId[i]], Slotitem.position, Quaternion.identity, Slotitem);
        }
        WeightText.GetComponent<Text>().text = $"{Weight}/{MaxWeight}";

        if(!isFull && SlotAmount > SlotLimt)
        {
            isFull = true;
            Player.Instance.GetComponent<Rigidbody2D>().gravityScale /= 0.6f;
        }
        else if (isFull && SlotAmount <= SlotLimt)
        {
            isFull = false;
            Player.Instance.GetComponent<Rigidbody2D>().gravityScale *= 0.6f;
        }

        if (!isHard && Weight > MaxWeight)
        {
            isHard = true;
            WeightText.GetComponent<Text>().color = Color.red;
        }
        else if (isHard && Weight <= MaxWeight)
        {
            isHard = false;
            WeightText.GetComponent<Text>().color = Color.white;
        }
    }
    IEnumerator ItemabilityEnd(int i)
    {
        int nowfan = Fancount;
        yield return new WaitForSeconds(10f);
        switch (i)
        {
            case 3:
            case 4:
                if (nowfan == Fancount) Player.Instance.Speed = 5;
                break;
            case 5:
                player.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 6:
                Dark.GetComponent<RectTransform>().localScale /= 1.2f;
                break;
            case 7:
                Player.Instance.GetComponent<Rigidbody2D>().gravityScale /= 0.6f;
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
                if (Stage == StageLimt)
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
        //AudioManager.instance.PlayBgm(true);

        player.position = new Vector3(1, -0.5f, 0);

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

        if (mapId != 0)
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

        if (Casual && mapId != 0)
        {
            StartCoroutine(CasualMode.instance.ReadyGo());
            Debug.Log("ReadyGo");
        }
    }

    public void selling(bool isLive)
    {
        if (isLive)
        {
            int j = 0;
            for (int i = 0; i < SlotAmount; i++)
            {
                switch (SlotId[i])
                {
                    case 15:
                        j += 500;
                        break;
                    case 16:
                        j += 1000;
                        break;
                    case 8:
                        j += 2000;
                        break;
                    case 9:
                        j += 3000;
                        break;
                    case 10:
                        j += 3000;
                        break;
                    case 11:
                        j += 4000;
                        break;
                    case 12:
                        j += 7000;
                        break;
                    case 13:
                        j += 10000;
                        break;
                    case 14:
                        j += 100000;
                        break;
                }
                SlotId[i] = -1;
            }

            Gold += j;
            Score += j;
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                SlotId[i] = -1;
            }
        }
        SlotAmount = 0;
        Weight = 0;

        SlotSetting();
    }

    public void StoreClose()
    {
        Pause.isOn = false;
        Store.SetActive(false);
        isFree = false;

        for (int i = 0; i < 5; i++)
        {
            SText[i].transform.GetChild(0).GetComponent<Text>().text = SPrice[i].ToString() + "G";
        }
    }

    public void O2GasUpgrade()
    {
        if (isFree)
        {

            O2level++;

            switch (O2level - 1)
            {
                case 1:
                    SPrice[0] = 1000;
                    SText[0].transform.GetChild(0).GetComponent<Text>().text = "0G";
                    SText[0].GetComponent<Text>().text = "중압용 산소통";
                    break;
                case 2:
                    SPrice[0] = 3000;
                    SText[0].transform.GetChild(0).GetComponent<Text>().text = "0G";
                    SText[0].GetComponent<Text>().text = "고압용 산소통";
                    break;
                case 3:
                    GameObject.Find("O2Gas_Sell").SetActive(false);
                    break;
            }

            return;
        }

        if (Gold - SPrice[0] < 0) return;

        Gold -= SPrice[0];

        O2level++;

        switch (O2level - 1)
        {
            case 1:
                SPrice[0] = 1000;
                SText[0].transform.GetChild(0).GetComponent<Text>().text = SPrice[0].ToString() + "G";
                SText[0].GetComponent<Text>().text = "중압용 산소통";
                break;
            case 2:
                SPrice[0] = 3000;
                SText[0].transform.GetChild(0).GetComponent<Text>().text = SPrice[0].ToString() + "G";
                SText[0].GetComponent<Text>().text = "고압용 산소통";
                break;
            case 3:
                GameObject.Find("O2Gas_Sell").SetActive(false);
                break;
        }
    }
    public void BagUpgrade()
    {
        if (isFree)
        {

            BagLevel++;
            SlotLimt += 2;

            switch (BagLevel - 1)
            {
                case 1:
                    SPrice[1] = 5000;
                    SText[1].transform.GetChild(0).GetComponent<Text>().text = "0G";
                    SText[1].GetComponent<Text>().text = "초대형 가방";
                    break;
                case 2:
                    MaxWeight = 400;
                    GameObject.Find("Bag_Sell").SetActive(false);
                    break;
            }

            int k = 1;
            foreach (Transform child in Slot.transform)
            {
                if (SlotLimt >= k) Slot.transform.GetChild(k).GetComponent<Image>().color = Color.white;
                else break;
                k++;
            }
            return;
        }

        if (Gold - SPrice[1] < 0) return;

        Gold -= SPrice[1];

        BagLevel++;
        SlotLimt += 2;

        switch (BagLevel - 1)
        {
            case 1:
                SPrice[1] = 5000;
                MaxWeight = 250;
                SText[1].transform.GetChild(0).GetComponent<Text>().text = SPrice[1].ToString() + "G";
                SText[1].GetComponent<Text>().text = "초대형 가방";
                break;
            case 2:
                MaxWeight = 400;
                GameObject.Find("Bag_Sell").SetActive(false);
                break;
        }
        int j = 1;
        foreach (Transform child in Slot.transform)
        {
            if (SlotLimt >= j) Slot.transform.GetChild(j).GetComponent<Image>().color = Color.white;
            else break;
            j++;
        }
    }
    public void LightUpgrade()
    {
        if (isFree)
        {

            LightLevel++;
            Dark.transform.localScale *= 1.2f;

            switch (LightLevel - 1)
            {
                case 1:
                    SPrice[2] = 1000;
                    SText[2].transform.GetChild(0).GetComponent<Text>().text = "0G";
                    SText[2].GetComponent<Text>().text = "초고급 손전등";
                    break;
                case 2:
                    GameObject.Find("Light_Sell").SetActive(false);
                    break;
            }

            return;
        }

        if (Gold - SPrice[2] < 0) return;

        Gold -= SPrice[2];

        LightLevel++;
        Dark.transform.localScale *= 1.2f;

        switch (LightLevel - 1)
        {
            case 1:
                SPrice[2] = 2000;
                SText[2].transform.GetChild(0).GetComponent<Text>().text = SPrice[2].ToString() + "G";
                SText[2].GetComponent<Text>().text = "초고급 손전등";
                break;
            case 2:
                GameObject.Find("Light_Sell").SetActive(false);
                break;
        }
    }
    public void KnifeUpgrade()
    {
        if (isFree)
        {

            KnifeLevel++;
            Damage++;
            AttackDelay -= 0.5f;

            switch (KnifeLevel - 1)
            {
                case 1:
                    SPrice[3] = 3000;
                    SText[3].transform.GetChild(0).GetComponent<Text>().text = "0G";
                    SText[3].GetComponent<Text>().text = "군용 단검";
                    break;
                case 2:
                    GameObject.Find("Knife_Sell").SetActive(false);
                    break;
            }

            return;
        }

        if (Gold - SPrice[3] < 0) return;

        Gold -= SPrice[3];

        KnifeLevel++;
        Damage++;
        AttackDelay -= 0.5f;

        switch (KnifeLevel - 1)
        {
            case 1:
                SPrice[3] = 3000;
                SText[3].transform.GetChild(0).GetComponent<Text>().text = SPrice[3].ToString() + "G";
                SText[3].GetComponent<Text>().text = "군용 단검";
                break;
            case 2:
                GameObject.Find("Knife_Sell").SetActive(false);
                break;
        }
    }
    public void ShoseUpgrade()
    {
        if (isFree)
        {
            JumpLimt = 3;
            GameObject.Find("Shose_Sell").SetActive(false);

            return;
        }
        if (Gold - SPrice[4] < 0) return;

        Gold -= SPrice[4];

        JumpLimt = 3;
        GameObject.Find("Shose_Sell").SetActive(false);
    }
}
