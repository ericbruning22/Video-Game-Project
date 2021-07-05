using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetsLeftAndAccuracy : NetworkBehaviour
{
    public int targetsLeft;
    public Text targets_text;
    public Text accuracy_text;
    public static float hit;
    public static float miss;
    public static float targetsHit;
    public static float targetsMissed;
    public float hit_temp;
    public float miss_temp;
    public static float accuracy;

    public Test test;

    void Start()
    {
        hit = 0f;
        miss = 0f;
        hit_temp = 0f;
        miss_temp = 0f;
        targetsHit = 0f;
        targetsMissed = 0f;
        targetsLeft = int.Parse(NetworkManagerCustom.Max_Targets);

        //Display time left
        targets_text.text = ("Targets Left: " + targetsLeft);

        //Display accuracy
        accuracy_text.text = ("Shooting Accuracy: " + accuracy + "%");
    }

    void Update()
    {
        //check to see if score has gone up
        if (hit > hit_temp)
        {
            hit_temp = hit;

            targetsHit++;
            targetsLeft--;

            //Display time left
            targets_text.text = ("Targets Left: " + targetsLeft);

            accuracy = targetsHit / (targetsHit + targetsMissed);
            accuracy = accuracy * 100;

            //Display accuracy
            accuracy_text.text = ("Shooting Accuracy: " + accuracy + "%");

            if (targetsLeft == 0 && NetworkClient.active)
            {
                test.Win();
                accuracy = 0;
            }
        }

        //check to see if score has gone up
        if (miss > miss_temp)
        {
            miss_temp = miss;

            targetsMissed++;

            accuracy = targetsHit / (targetsHit + targetsMissed);
            accuracy = accuracy * 100;

            //Display accuracy
            accuracy_text.text = ("Shooting Accuracy: " + accuracy + "%");
        }
    }  
}