using UnityEngine;
using System.IO;

public class ROSCommunicator : MonoBehaviour
{
    [SerializeField] Rigidbody subRigidbody;
    [SerializeField] Transform subTransform;

    Vector3 lastVelocity;
    Vector3 lastAngularVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastVelocity = new Vector3(0, 0, 0);
        lastAngularVelocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 acceleration = subRigidbody.linearVelocity - lastVelocity;

        Vector3 angularAcceleration = subRigidbody.angularVelocity - lastAngularVelocity;

        lastVelocity = subRigidbody.linearVelocity;

        lastAngularVelocity = subRigidbody.angularVelocity;

        File.WriteAllLines("D:\\Unity\\Games\\SURF Advanced Robot Simulation\\robotdata.txt", new string[] { $"{acceleration.x}, {acceleration.y}, {acceleration.z}, {angularAcceleration.x}, {angularAcceleration.y}, {angularAcceleration.z}" });
    }
}