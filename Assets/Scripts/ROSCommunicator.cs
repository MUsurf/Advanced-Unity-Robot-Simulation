using UnityEngine;
using System.IO;

public class ROSCommunicator : MonoBehaviour
{
    [SerializeField] Rigidbody subRigidbody;
    [SerializeField] Transform subTransform;

    Vector3 lastVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastVelocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 acceleration = subRigidbody.linearVelocity - lastVelocity;

        File.WriteAllLines("D:\\Unity\\Games\\SURF Advanced Robot Simulation\\robotdata.txt", new string[] { $"Accelerations: ({acceleration.x}, {acceleration.y}, {acceleration.z}), Rotations: ({subTransform.eulerAngles.x}, {subTransform.eulerAngles.y}, {subTransform.eulerAngles.z})" });
    }
}
