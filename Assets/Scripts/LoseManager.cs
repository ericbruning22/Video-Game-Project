using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseManager : NetworkBehaviour
{
    public GameObject losePanel;
    public GameObject winPanel;

    public void Lose()
    {
        if(winPanel.active == false)
        {
            //Display accuracy
            losePanel.SetActive(true);
            TargetsLeftAndAccuracy.accuracy = 0;
        }
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
