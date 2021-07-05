using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPacketController : NetworkBehaviour
{
    //Package to send to server
    [System.Serializable]
    public class Package
    {
        public float horizontal;
        public float vertical;
        public float timeStamp;
    }

    //Package received from server
    [System.Serializable]
    public class ReceivePackage
    {
        public float x;
        public float y;
        public float z;
        public float timeStamp;
    }

    //client packet manager
    public NetworkPacketManager<Package> m_PacketManager;
    public NetworkPacketManager<Package> PacketManager
    {
        get
        {
            if (m_PacketManager == null)
            {
                m_PacketManager = new NetworkPacketManager<Package>();

                if (isLocalPlayer)
                {
                    m_PacketManager.OnRequirePackageTransmit += TransmitPackageToServer;
                }
            }

            return m_PacketManager;
        }
    }

    //server package manager
    public NetworkPacketManager<ReceivePackage> m_ServerPackageManager;
    public NetworkPacketManager<ReceivePackage> ServerPackageManager
    {
        get
        {
            if (m_ServerPackageManager == null)
            {
                m_ServerPackageManager = new NetworkPacketManager<ReceivePackage>();

                if (isServer)
                {
                    m_ServerPackageManager.OnRequirePackageTransmit += TransmitToClients;
                }
            }

            return m_ServerPackageManager;
        }
    }

    private void TransmitPackageToServer(byte[] bytes)
    {
        CmdTransmitPackages(bytes);
    }

    private void TransmitToClients(byte[] bytes)
    {
        RpcReceiveDataOnClient(bytes);
    }

    //commands that only work on the server
    //have to start with Cmd
    [Command]
    //transmit data to server
    void CmdTransmitPackages(byte[] data)
    {
        PacketManager.ReceivedData(data);
    }

    //Rpc is for clients
    //have to start with Rpc
    [ClientRpc]
    //receive data from server
    void RpcReceiveDataOnClient(byte[] data)
    {
        ServerPackageManager.ReceivedData(data);
    }

    //tick rate for packages
    public virtual void FixedUpdate()
    {
        PacketManager.Tick();
        ServerPackageManager.Tick();
    }
}
