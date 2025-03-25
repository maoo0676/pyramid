using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject items;
    public GameObject[] Jem;

    public bool[] isGotten;
    public bool[] get;

    bool startsetting = true;
    public bool isClear = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startsetting) Startroutine();
        if (GameManager.Instance.Hp <= 0) Loss();
        if (isClear)
        {
            GameManager.Instance.isClear = true;
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
        startsetting = false;
        for (int i = 0; i < Jem.Length; i++)
        {
            get[i] = false;
        }

        foreach (Transform child in items.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    void Loss()
    {
        isClear = false;
        for(int i = 0;i < Jem.Length;i++)
        {
            if (get[i])
            {
                get[i] = false;
                isGotten[i] = false;
            }
        }
    }
}
