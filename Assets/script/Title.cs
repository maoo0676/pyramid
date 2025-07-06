using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject scoreboard;
    public GameObject mode;

    public int tutorialpaze = 0;

    Transform Panel;

    public Text[] Scores;

    private void Awake()
    {
        DataManager.Instance.StartGame();
        Panel = tutorial.transform.GetChild(0);
    }

    public void Classic()
    {
        SceneManager.LoadScene("pyramid");
    }
    public void Casual()
    {
        SceneManager.LoadScene("Pyramid(short)");
    }
    public void Help()
    {
        tutorial.SetActive(true);
    }
    public void ModeSelect()
    {
        mode.SetActive(true);
    }
    public void Score()
    {
        for (int i = 0; i < Scores.Length; i++) {
            if (DataManager.Instance.data.TopScore[i] == 0) Scores[i].text = $"{i + 1}. (Empty)";
            else Scores[i].text = $"{i + 1}. {DataManager.Instance.data.TopScore[i]}";
        }
        scoreboard.SetActive(true);
    }
    public void Back()
    {
        tutorial.SetActive(false);
        scoreboard.SetActive(false);
        mode.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void Right()
    {
        if (tutorialpaze == 3)
        {
            Panel.transform.GetChild(3).gameObject.SetActive(false);
            Panel.transform.GetChild(0).gameObject.SetActive(true);
            tutorialpaze = 0;
            return;
        }
        Panel.transform.GetChild(tutorialpaze).gameObject.SetActive(false);
        tutorialpaze++;
        Panel.transform.GetChild(tutorialpaze).gameObject.SetActive(true);
    }
    public void Left()
    {
        if (tutorialpaze == 0)
        {
            Panel.transform.GetChild(0).gameObject.SetActive(false);
            Panel.transform.GetChild(3).gameObject.SetActive(true);
            tutorialpaze = 3;
            return;
        }
        Panel.transform.GetChild(tutorialpaze).gameObject.SetActive(false);
        tutorialpaze--;
        Panel.transform.GetChild(tutorialpaze).gameObject.SetActive(true);
    }
}
