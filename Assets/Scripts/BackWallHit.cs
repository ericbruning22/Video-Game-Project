using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallHit : MonoBehaviour
{
    public GameObject ball;

    public GameObject background1;
    public GameObject background2;
    public GameObject background3;
    public GameObject background4;
    public GameObject background5;

    public AudioSource boos;

    public int num = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball)
        {
            //get random number for background
            num = Random.Range(1, 3);

            //change background
            if (num == 1)
            {
                background1.SetActive(false);
                background2.SetActive(false);
                background3.SetActive(false);
                background4.SetActive(true);
                background5.SetActive(false);
            }
            else if (num == 2)
            {
                background1.SetActive(false);
                background2.SetActive(false);
                background3.SetActive(false);
                background4.SetActive(false);
                background5.SetActive(true);
            }

            //play audio of boos
            boos.Play();

            //add miss
            TargetsLeftAndAccuracy.miss++;

            //snap ball back to player
            Ball.HoldingBall = true;

        }
    }
}
