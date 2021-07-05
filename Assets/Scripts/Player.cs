using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool holdingBall = false;
    private float shotSpeed = 300f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (holdingBall)
        {
            //get direction of player
            float horz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horz, 0f, vert);
            print(player.transform.position);

            //place ball in front of player
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.position = player.transform.position + transform.forward;
            //draw line where player is looking
            Debug.DrawRay(transform.position, direction * 10f, Color.red);

            //shoot
            if (Input.GetKey("c"))
            {
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.GetComponent<Rigidbody>().AddForce(direction * shotSpeed);
                holdingBall = false;

            }

        }
    }
}
