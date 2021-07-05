using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class NetworkPacketManager<T> where T : class
{
    public event System.Action<byte[]> OnRequirePackageTransmit;

    //send speed
    private float m_sendSpeed = .2f;
    public float sendSpeed
    {
        get
        {
            if (m_sendSpeed < .1f)
                return m_sendSpeed - .1f;
            return m_sendSpeed;
        }
        set
        {
            m_sendSpeed = value;
        }
    }

    float nextTick;

    //list (package) that holds the data being sent
    private List<T> m_Packages;
    public List<T> Packages
    {
        get
        {
            if (m_Packages == null)
                m_Packages = new List<T>();
            return m_Packages;
        }
    }

    //add new package to the Packages list
    public void addPackage(T package)
    {
        Packages.Add(package);
    }

    //create a queue for the packages
    public Queue<T> receivedPackages;

    //process all data received
    public void ReceivedData(byte[] bytes)
    {
        if (receivedPackages == null)
        {
            receivedPackages = new Queue<T>();
        }

        //read bytes to an array
        T[] packages = readBytes(bytes).ToArray();

        //add packages to queue
        for (int i = 0; i < packages.Length; i++)
        {
            receivedPackages.Enqueue(packages[i]);
        }
    }

    //tick rate (send speed?)
    public void Tick()
    {
        nextTick += 1 / this.sendSpeed * Time.fixedDeltaTime;

        if (nextTick > 1 && Packages.Count > 0)
        {
            nextTick = 0;

            //transmit package when the time permits
            if(OnRequirePackageTransmit != null)
            {
                byte[] bytes = CreateBytes();
                Packages.Clear();
                OnRequirePackageTransmit(bytes);
            }
        }
    }

    //gets the next data in the queue
    public T GetNextDataReceived()
    {
        if(receivedPackages == null || receivedPackages.Count == 0)
        {
            return default(T);
        }

        return receivedPackages.Dequeue();
    }

    //bytes that will be sent back
    byte[] CreateBytes()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            formatter.Serialize(ms, this.Packages);
            return ms.ToArray();
        }
    }

    //read bytes being sent
    List<T> readBytes(byte[] bytes)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(bytes, 0, bytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            return (List<T>)formatter.Deserialize(ms);
        }
    }
}
