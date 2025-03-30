using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject items;
    public GameObject[] Jem;

    public bool[] isGotten;
    public bool[] get;

    public bool isClear = false;

    int findertarget;

    void Update()
    {
        if (GameManager.Instance.isMapStart) Startroutine();
        if (GameManager.Instance.Hp <= 0) Loss();
        if (GameManager.Instance.FoundTresure == null || Jem[findertarget].activeSelf) GameManager.Instance.InDungeon.transform.GetChild(6).gameObject.SetActive(false);

        if (isClear && !GameManager.Instance.isClear)
        {
            Debug.Log("isClear");
            GameManager.Instance.isClear = true;
            GameManager.Instance.StageText.transform.GetChild(1).gameObject.SetActive(true);
            return;
        }

        isClear = true;
        for(int i = 0; i < Jem.Length; i++)
        {
            if (!Jem[i].activeSelf && !isGotten[i])
            {
                get[i] = true;
                isGotten[i] = true;
            }

            if(!isGotten[i]) isClear = false;
        }
    }
    void Startroutine()
    {
        GameManager.Instance.isMapStart = false;

        for (int i = 0; i < Jem.Length; i++)
        {
            get[i] = false;
        }

        foreach (Transform child in items.transform)
        {
            if (child.gameObject.name.Equals("Key")) continue;

            Debug.Log(child.gameObject.name);
            child.gameObject.SetActive(true);
        }

        for (int i = 0; i < Jem.Length; i++)
        {
            if (!isGotten[i])
            {
                Debug.Log("target : " + Jem[i].gameObject.name);
                findertarget = i;
                if (GameManager.Instance.FoundTresure == null) GameManager.Instance.FoundTresure = Jem[i].gameObject;
                return;
            }

            Debug.Log("-" + Jem[i].gameObject.name);
            Jem[i].SetActive(false);
        }
    }
    void Loss()
    {
        Debug.Log("loss");
        isClear = false;
        GameManager.Instance.isClear = false;
        for (int i = 0;i < Jem.Length;i++)
        {
            if (get[i])
            {
                get[i] = false;
                isGotten[i] = false;
            }
        }
    }
}
