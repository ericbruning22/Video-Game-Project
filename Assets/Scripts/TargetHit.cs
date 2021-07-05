using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public GameObject ball;

    public int num = 0;

    public GameObject newTarget;

    public GameObject background1;
    public GameObject background2;
    public GameObject background3;

    public AudioSource cheers;

    // Start with all goals except the first one to be invisible
    void Start()
    {
        if (gameObject.name != "target1")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == ball)
        {

            //get random number for background
            num = Random.Range(1, 4);

            //change background
            if (num == 1)
            {
                background1.SetActive(true);
                background2.SetActive(false);
                background3.SetActive(false);
            }
            else if (num == 2)
            {
                background1.SetActive(false);
                background2.SetActive(true);
                background3.SetActive(false);
            }
            else if (num == 3)
            {
                background1.SetActive(false);
                background2.SetActive(false);
                background3.SetActive(true);
            }

            //play audio of cheers
            cheers.Play();

            //destroy target that was hit
            gameObject.SetActive(false);

            //activate new target new target
            newTarget.SetActive(true);

            //add to score
            TargetsLeftAndAccuracy.hit++;

            //snap ball back to player
            Ball.HoldingBall = true;
        }
    }
}
