using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Image slot;
    [Header("# Player Info")]
    public int Hp = 8;
    public float HitTime = 0.1f;
    public float AttackDelay = 0f;
    [Header("# Bag Info")]
    public int Gold = 0;
    public int SlotAmount = 4;
    public int[] SlotId = { -1 };
    public int Keymount = 0;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        slot.fillAmount = 0.125f * SlotAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (HitTime > 0)
        {
            HitTime -= Time.deltaTime;
        }
        if (AttackDelay > 0)
        {
            AttackDelay -= Time.deltaTime;
        }
    }

    public void Sleep(float maxtime)
    {
        float time = 0;
        while(true)
        {
            time += Time.deltaTime;
            if (time >= maxtime) break;
        }
    }
}
