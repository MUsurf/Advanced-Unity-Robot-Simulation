using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class quarterback : MonoBehaviour
{
    bool tryit = true;

    void Update()
    {
        if (tryit && Input.GetKey(KeyCode.Z))
        {
            tryit = false;
            Debug.Log("WORKGIND");
            Henry();
        }
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Henry()
    {
        string serverIp = "127.0.0.1";
        int port = 12345;

        try
        {
            Debug.Log("WORKGIND2");
            TcpClient client = new TcpClient();
            await client.ConnectAsync(serverIp, port);

            NetworkStream stream = client.GetStream();

            // Receive data from server
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string received = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Received from server: " + received);

            // Optionally, send a response back
            string response = "Hello from client!";
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            stream.Close();
            client.Close();
        }
        catch (Exception ex)
        {
            Debug.Log("Client error: " + ex);
        }
    }
}
