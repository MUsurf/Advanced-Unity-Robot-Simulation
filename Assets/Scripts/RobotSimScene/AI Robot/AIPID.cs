using UnityEngine;
using System.Collections.Generic;
public class AIPID : MonoBehaviour
{
    public Rigidbody rb;
    public float xSetpoint = 0f;
    public float ySetpoint = 0f;
    public float zSetpoint = 0f;
    public float rollSetpoint = 0f;
    public float pitchSetpoint = 0f;
    public float yawSetpoint = 0f;
    public float xSetpointRelative = 0f;
    public float ySetpointRelative = 0f;
    public float zSetpointRelative = 0f;
    public List<float> kValues = new List<float>() {0.5f, 0, 0.1f, 0.5f, 0, 0.1f, 0.5f, 0, 0.1f, 0.05f, 0, 0.1f, 0.05f, 0, 0.1f, 0.05f, 0, 0.1f};

    public AIMotorScript motorScript;
    private List<float> location;
    private PIDHandler pidHandler = new PIDHandler();

    public bool yawTrack = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        location = new List<float>() {rb.transform.position.x, rb.transform.position.y, rb.transform.position.z, rb.rotation.eulerAngles.z, rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y};
        pidHandler.getMaxSpeed(motorScript.maxSpeed);
        pidHandler.getLocation(location, yawTrack);
        pidHandler.UpdateKValues(kValues);
        UpdateSetpoint(xSetpoint, ySetpoint, zSetpoint, rollSetpoint, pitchSetpoint, yawSetpoint);
    }

    public List<Vector3> getVectors()
    {
        location = new List<float>() {rb.transform.position.x, rb.transform.position.y, rb.transform.position.z, rb.rotation.eulerAngles.z, rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y};
        pidHandler.getMaxSpeed(motorScript.maxSpeed);
        pidHandler.getLocation(location, yawTrack);
        pidHandler.UpdateKValues(kValues);
        UpdateSetpoint(xSetpoint, ySetpoint, zSetpoint, rollSetpoint, pitchSetpoint, yawSetpoint);
        sendSetpoints();
        return pidHandler.Update();
    }

    public void resetAll()
    {
        pidHandler.resetAll();
    }

    public void UpdateSetpoint(float x, float y, float z, float roll, float pitch, float yaw)
    {
        if(yawTrack)
        {
            Vector3 vector = new Vector3(x, y, z);
            vector = transform.InverseTransformPoint(vector);
            xSetpointRelative = vector.x;
            ySetpointRelative = vector.y;
            zSetpointRelative = vector.z;
            rollSetpoint = roll;
            pitchSetpoint = pitch;
            yawSetpoint = yaw;
        }
        else
        {
            xSetpointRelative = x;
            ySetpointRelative = y;
            zSetpointRelative = z;
            rollSetpoint = roll;
            pitchSetpoint = pitch;
            yawSetpoint = yaw;
        }
    }

    public void sendSetpoints()
    {
        List<float> setpoints = new List<float>() {xSetpointRelative, ySetpointRelative, zSetpointRelative, rollSetpoint, pitchSetpoint, yawSetpoint};
        pidHandler.recieveSetpoints(setpoints);
    }
}