using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Test : NetworkBehaviour
{
    public Text accuracy_text;
    public GameObject winPanel;

    public void Win()
    {
        //Display accuracy
        winPanel.SetActive(true);
        accuracy_text.text = "Shooting Accuracy: " + TargetsLeftAndAccuracy.accuracy + "%";
    }

    //exit game
    public void Exit()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopServer();
        Application.Quit();
    }

    //new level
    public void NewLevel()
    {
        NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("Menu");
    }
}
