using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 playerDirection;
    private Vector3 ballDirection;
    public static bool HoldingBall = false;
    private float shotSpeed = 70f;
    public float timer = 0;
    public float ball_timer = 0;
    public float timePressed = 0;
    public GameObject player;

    public Vector3 ball_start;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //this.GetComponent<Rigidbody>().useGravity = true;
            //this.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1000);
            player = collision.gameObject;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().detectCollisions = true;
            HoldingBall = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HoldingBall = false;
        //HoldingBall = true;
        ball_start = new Vector3(-84f,1.5f,-12f);

        this.transform.position = ball_start;
        Debug.Log("Ball = " + ball_start);
    }

    // Update is called once per frame
    void Update()
    {
        //if player is holding ball
        if (HoldingBall)
        {
            //get direction of player
            float horz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

            playerDirection = new Vector3(horz, 0, vert);
            //Debug.Log(playerDirection);

            //place ball in front of player
            this.transform.Rotate(playerDirection *10);
            this.transform.position = player.transform.position + new Vector3(0,1.25f,3);
            //draw line where player is looking
            Debug.DrawRay(transform.position, playerDirection * 10f, Color.red);
        }
        else
        {
            ball_timer += Time.deltaTime;

            if(ball_timer > 5)
            {
                this.transform.position = ball_start;
                ball_timer = 0;
            }
        }

        //shoot
        if (Input.GetButtonDown("Shoot") && HoldingBall)
        {
            timer = Time.time;
        }
        if (Input.GetButtonUp("Shoot") && HoldingBall)
        {
            timePressed = Time.time - timer;
            timer = 0;
            Debug.Log(timePressed.ToString("00:00.00"));

            //StartCoroutine(WaitOneSec());
            //get direction of player
            float horz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

            //fix vertical direction of the ball so it doesnt go in a weird direction
            if (vert < 0.7f)
            {
                vert = 1.0f;
            }

            //fix horizontal direction of the ball
            if (horz > 0.7f)
            {
                horz = 0f;
            }

            ballDirection = new Vector3(horz, timePressed, 1.0f);
            Debug.Log(ballDirection);

            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().AddForce(ballDirection * shotSpeed, ForceMode.Impulse);
            HoldingBall = false;
        }
    }

    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(.5f);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            this.transform.position = ball_start;
            Debug.Log("Level:" + level);
        }
    }
}
