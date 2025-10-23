using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;

public class help : MonoBehaviour
{

    IPAddress ipAddress = IPAddress.Parse("127.0.0.1"); // Loopback address
    const int port = 12345; // Choose an available port (use resource manager??? idk we ball)

    Socket handler;
    TcpListener tcpListener;

    bool tryit = true;

    void Update()
    {
        if (tryit && Input.GetKey(KeyCode.A))
        {
            tryit = false;
            DoIt();
        }
    }

    public async void DoIt()
    {
        Debug.Log("5");
        Debug.Log("6");

        tcpListener = new TcpListener(ipAddress, port);
        tcpListener.Start();
        Debug.Log("7");

        try
    {
        handler = await tcpListener.AcceptSocketAsync();
        Debug.Log("8");
        Tryit();
    }
    catch (System.ObjectDisposedException ex)
    {
        Debug.LogError("Listener was stopped or disposed: " + ex);
    }
    catch (SocketException ex)
    {
        Debug.LogError("Network/socket error: " + ex);
    }
    catch (System.InvalidOperationException ex)
    {
        Debug.LogError("Listener not started: " + ex);
    }
    catch (System.OperationCanceledException ex)
    {
        Debug.LogError("Operation was cancelled: " + ex);
    }
    catch (System.Exception ex)
    {
        Debug.LogError("Unknown error: " + ex);
    }
    }

    void Tryit()
    {
        SendAsync("helpppppppppppppppppp");
    }

   async void RecieveAsync()
{
    Debug.Log("1");
    byte[] bytes = new byte[1024];
    int bytesRec = await handler.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
    Debug.Log("2");
    string receivedData = Encoding.ASCII.GetString(bytes, 0, bytesRec);

    Debug.Log(receivedData);
}

async void SendAsync(string toSend)
{
    Debug.Log("3");
    byte[] msg = Encoding.ASCII.GetBytes(toSend);
    await handler.SendAsync(new ArraySegment<byte>(msg), SocketFlags.None);
        Debug.Log("4");

        RecieveAsync();
}

    void OnDestroy()
    {
        if (handler != null)
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        if (tcpListener != null)
        {
            tcpListener.Stop();
        }
    }
}
