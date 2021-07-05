using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNetwork : NetworkPacketController {

    [SerializeField]
    float moveSpeed = 0.2f;

    [SerializeField]
    [Range(0.1f, 1)]
    float networkSendRate = 0.1f;

    [SerializeField]
    bool isPredictionEnabled;

    [SerializeField]
    float correctionThreshold = 1.5f;

    CharacterController controller;

    List<ReceivePackage> predictedPackages;
    Vector3 lastPosition;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-88, 0.1f, -13);
        //get player controller
        controller = GetComponent<CharacterController>();

        //set send speed
        PacketManager.sendSpeed = networkSendRate;
        ServerPackageManager.sendSpeed = networkSendRate;

        predictedPackages = new List<ReceivePackage>();
    }

    // Update is called once per frame
    void Update()
    {
        //client update
        localClientUpdate();

        //server update
        serverUpdate();

        //remote client update
        RemoteClientUpdate();

        if(this.transform.position.y > 1.5f)
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            transform.Rotate(0, 0, 0);
        }
    }

    public override void OnStartLocalPlayer()
    {
        //camera movement
        Camera.main.transform.position = this.transform.position - this.transform.forward*25 + this.transform.up*8;
        Camera.main.transform.LookAt(this.transform.position);
        Camera.main.transform.parent = this.transform;

    }

    //player movement
    void Move(float horizontal, float vertical)
    {
        controller.Move(new Vector3(horizontal, 0, vertical));
    }

    //client update method
    void localClientUpdate()
    {
        if(!isLocalPlayer)
        {
            return;
        }
      
        //movement package
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //get current time
            float timeStep = Time.time;

            //add package to packet manager
            PacketManager.addPackage(new Package
            {
                horizontal = Input.GetAxis("Horizontal"),
                vertical = Input.GetAxis("Vertical"),
                timeStamp = timeStep
            });

            if (isPredictionEnabled)
            {
                Move(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);

                //predicted package
                predictedPackages.Add(new ReceivePackage
                {
                    timeStamp = timeStep,
                    x = transform.position.x,
                    y = transform.position.y,
                    z = transform.position.z
                });
            }
        }
        //Move(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
    }


    //server update method
    void serverUpdate()
    {
        if (!isServer || isLocalPlayer)
            return;

        //receive package data from server
        Package packageData = PacketManager.GetNextDataReceived();

        //check if we have data
        if (packageData == null)
            return;

        Move(packageData.horizontal * moveSpeed, packageData.vertical * moveSpeed);

        //check if we have moved
        if (transform.position == lastPosition)
        {
            return;
        }

        lastPosition = transform.position;

        //send package to server
        ServerPackageManager.addPackage(new ReceivePackage
        {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z,
            timeStamp = packageData.timeStamp
        });
    }

    //remote client update method
    public void RemoteClientUpdate()
    {
        if (isServer)
            return;

        //get data from server
        var data = ServerPackageManager.GetNextDataReceived();

        //check if data was received
        if (data == null)
            return;

        if(isLocalPlayer && isPredictionEnabled)
        {
            //get predicted package where data and predicted packages on the same time stamp
            var transmittedPackage = predictedPackages.Where(x => x.timeStamp == data.timeStamp).FirstOrDefault();

            if (transmittedPackage == null)
                return;

            //if prediction and data exceed threshold
            if (Vector3.Distance(new Vector3(transmittedPackage.x, transmittedPackage.y, transmittedPackage.z), new Vector3(data.x, data.y, data.z)) > correctionThreshold)
            {
                //snap player to server location
                transform.position = new Vector3(data.x, data.y,data.z);
                transform.Rotate(0, 0, 0);
            }

            //clear old predictions
            predictedPackages.RemoveAll(x => x.timeStamp <= data.timeStamp);
        }
        else
        {
            //move player to correct position if prediction is not right
            transform.position = new Vector3(data.x, data.y, data.z);
            transform.Rotate(0, 0, 0);
        }
    }
}
