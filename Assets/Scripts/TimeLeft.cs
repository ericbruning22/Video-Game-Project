using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeLeft : NetworkBehaviour
{ 
    public int timeLeft = 0;
    public Text countdown;

    public LoseManager loseManager;

    void Start()
    {
        timeLeft = int.Parse(NetworkManagerCustom.TimeLeft);
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }

    void Update()
    {
         //Display time left
        countdown.text = ("Time Left: " + timeLeft);
        
        if(timeLeft < 1 && NetworkClient.active)
        {
            loseManager.Lose();
        }
    }

    //exit game
    public void Exit()
    {
        NetworkManager.singleton.StopClient();
        Application.Quit();
    }

    //new level
    public void NewLevel()
    {
        NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("Menu");
    }

    //countdown one second at a time
    IEnumerator LoseTime()
    {
        while (true)
        {
             yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}
