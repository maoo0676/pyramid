using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject tutorial;
    public void StartGame()
    {
        SceneManager.LoadScene("pyramid");
    }
    public void Help()
    {
        tutorial.SetActive(true);
    }
    public void Back()
    {
        tutorial.SetActive(false);
    }
    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
