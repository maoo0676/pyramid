using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasualMode : MonoBehaviour
{
    public static CasualMode instance;

    public Text timer;
    public Text ready;
    public int time;
    public int maxTime;
    float s = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int min = time / 60, sec = time % 60;

        timer.text = min.ToString("00") + ":" + sec.ToString("00");

        s += Time.deltaTime;

        if (s >= 1&&time > 0)
        {
            s = 0;
            time--;
        }

        if (time == 0)
        {
            time = -1;
            GameManager.Instance.GameResult();
        }
    }

    public IEnumerator ReadyGo()
    {
        Player.Instance.Freeze = true;

        ready.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            ready.text = i.ToString();
            Debug.Log(i);

            yield return new WaitForSeconds(1f);

            GameManager.Instance.curTime = 45;
        }
        Player.Instance.Freeze = false;
        ready.text = "GO!".ToString();
        Debug.Log("go");

        s = 0;
        time = maxTime;
        timer.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        ready.gameObject.SetActive(false);
    }
}
