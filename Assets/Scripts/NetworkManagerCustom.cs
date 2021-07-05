using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager
{
    public static string TimeLeft = "0";
    public static string Max_Targets = "0";
    public bool reload = false;

    //start server
    public void StartupServer()
    {
        SetPort();
        NetworkManager.singleton.StartServer();
    }

    //set port
    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    //client
    public void Join()
    {
        if(NetworkClient.active == false)
        {
            //set up game mode
            SetTime();
            SetMaxTargets();
            //set up connection
            SetIP();
            SetPort();
            NetworkManager.singleton.StartClient();
        }

    }

    //set time length for game
    void SetTime()
    {
        TimeLeft = GameObject.Find("InputTime").transform.Find("Text").GetComponent<Text>().text;
    }

    //set IP address
    void SetMaxTargets()
    {
        Max_Targets = GameObject.Find("InputMaxTargets").transform.Find("Text").GetComponent<Text>().text;
    }
    //set IP address
    void SetIP()
    {
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            SetupMenuSceneButtons();
        }
    }

    void SetupMenuSceneButtons()
    {
            //GameObject.Find("Host").GetComponent<Button>()..RemoveAllListeners();
            GameObject.Find("Host").GetComponent<Button>().onClick.AddListener(StartupServer);

           // GameObject.Find("Join").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("Join").GetComponent<Button>().onClick.AddListener(Join);
    }
}
