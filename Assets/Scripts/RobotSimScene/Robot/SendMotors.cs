using System;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class SendMotors : MonoBehaviour
{
    //TODO - manually enter IP and port
    //TODO - it might be better to just convert the powers to a text and give to this script instead of the whole file thing
    //TODO - do both things above
    [SerializeField] private string ip = "";
    [SerializeField] private int port = 0;

    private int count;
    private string text;
    [SerializeField] private float timeBetweenSends = 0.5f;

    [SerializeField] private bool wantSendMotors = false;
    private IPEndPoint ep;
    private Socket sock;
    
    private bool hasSent = true;

    public MotorScript motorScript;

    void Start()
    {
        ep = new IPEndPoint(IPAddress.Parse(ip), port);
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    }

    void Update()
    {
        if(hasSent && wantSendMotors)
        {
            Invoke("sendPackage", timeBetweenSends);
            hasSent = false;
        }

    }

    void sendPackage()
    {
        if(wantSendMotors)
        {
            text = motorScript.getMotorValues();

            byte [] packetdata = Encoding.ASCII.GetBytes(text);

            try
            {
                sock.SendTo(packetdata, ep);
                count++;
                Debug.Log($"Packets Sent: <color=green>{count}</color>");
            }
            catch(Exception e)
            {
                Debug.Log($"<color=red>ohno</color> {e}");
            }

            hasSent = true;
        }

        if(!hasSent)
        {
            Debug.Log("<color=red>failed to send</color>");
            hasSent = true;
        }
    }
}