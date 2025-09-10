using System.Collections.Generic;
using UnityEngine;

public class OldMovementController : MonoBehaviour
{
    public float keyboardForceMultiplier = 10;
    public float mouseForceMultiplier = 20f;
    public float yawForceMultiplier = 4f;
    public float shiftMultiplier = 4f;
    private int averageSpeedsForward = 0;
    private int averageSpeedsSideways = 0;
    public bool invertMouse = false;
    private int invertMouseMultiplier = 1;
    public float mouseRotMultiplier = 0.1f;
    public bool calledFor = false;
    private float timeSinceLastToggle = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if(calledFor)
        {
            timeSinceLastToggle += Time.deltaTime;

            if(Input.GetKey(KeyCode.M) && timeSinceLastToggle > 0.25f)
            {
                if(Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                timeSinceLastToggle = 0f;
            }
        }
    }

    public void invertMouseToggle(bool invertMouseBool)
    {
        invertMouse = invertMouseBool;
    }

    public List<Vector3> MovementOverride()
    {
        Vector3 force5 = Vector3.zero;
        Vector3 force6 = Vector3.zero;
        Vector3 force7 = Vector3.zero;
        Vector3 force8 = Vector3.zero;
        if(invertMouse)
        {
            invertMouseMultiplier = -1;
        }
        else
        {
            invertMouseMultiplier = 1;
        }

        //Debug.Log(invertMouseMultiplier);

        if(Input.GetKey(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        force5 = Vector3.zero;
        force6 = Vector3.zero;
        force7 = Vector3.zero;
        force8 = Vector3.zero;
        averageSpeedsForward = 0;
        averageSpeedsSideways = 0;
        if(Input.GetKey(KeyCode.W))
        {
            force5 += Vector3.right * keyboardForceMultiplier;
            force6 += Vector3.right * keyboardForceMultiplier;
            force7 += Vector3.right * keyboardForceMultiplier;
            force8 += Vector3.right * keyboardForceMultiplier;
            averageSpeedsForward++;
        }
        if
        (Input.GetKey(KeyCode.S))
        {
            force5 += Vector3.left * keyboardForceMultiplier;
            force6 += Vector3.left * keyboardForceMultiplier;
            force7 += Vector3.left * keyboardForceMultiplier;
            force8 += Vector3.left * keyboardForceMultiplier;
            averageSpeedsForward++;
        }
        if(Input.GetKey(KeyCode.A))
        {
            force5 += Vector3.forward * keyboardForceMultiplier;
            force6 += Vector3.forward * keyboardForceMultiplier;
            force7 += Vector3.forward * keyboardForceMultiplier;
            force8 += Vector3.forward * keyboardForceMultiplier;
            averageSpeedsSideways++;
        }
        if(Input.GetKey(KeyCode.D))
        {
            force5 += Vector3.back * keyboardForceMultiplier;
            force6 += Vector3.back * keyboardForceMultiplier;
            force7 += Vector3.back * keyboardForceMultiplier;
            force8 += Vector3.back * keyboardForceMultiplier;
            averageSpeedsSideways++;
        }
        if((averageSpeedsForward == 1) && (averageSpeedsSideways == 1))
        {
            force5 *= 0.7071f;
            force6 *= 0.7071f;
            force7 *= 0.7071f;
            force8 *= 0.7071f;
        }
        if(Input.GetKey(KeyCode.Space))
        {
            force5 += Vector3.up * keyboardForceMultiplier;
            force6 += Vector3.up * keyboardForceMultiplier;
            force7 += Vector3.up * keyboardForceMultiplier;
            force8 += Vector3.up * keyboardForceMultiplier;
        }
        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            force5 += Vector3.down * keyboardForceMultiplier;
            force6 += Vector3.down * keyboardForceMultiplier;
            force7 += Vector3.down * keyboardForceMultiplier;
            force8 += Vector3.down * keyboardForceMultiplier;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            force5 *= shiftMultiplier;
            force6 *= shiftMultiplier;
            force7 *= shiftMultiplier;
            force8 *= shiftMultiplier;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 mouseDelta = new Vector3(mouseX, mouseY, 0);

        force5 += new Vector3(0, mouseDelta.y, 0) * mouseForceMultiplier * invertMouseMultiplier;
        force6 += new Vector3(0, mouseDelta.y, 0) * mouseForceMultiplier * invertMouseMultiplier;
        force7 -= new Vector3(0, mouseDelta.y, 0) * mouseForceMultiplier * invertMouseMultiplier;
        force8 -= new Vector3(0, mouseDelta.y, 0) * mouseForceMultiplier * invertMouseMultiplier;

        force5 += new Vector3(0, mouseDelta.x, 0) * mouseForceMultiplier;
        force6 -= new Vector3(0, mouseDelta.x, 0) * mouseForceMultiplier;
        force7 += new Vector3(0, mouseDelta.x, 0) * mouseForceMultiplier;
        force8 -= new Vector3(0, mouseDelta.x, 0) * mouseForceMultiplier;

        if(Input.GetKey(KeyCode.Q))
        {
            force5 += Vector3.forward * yawForceMultiplier;
            force6 += Vector3.forward * yawForceMultiplier;
            force7 += Vector3.back * yawForceMultiplier;
            force8 += Vector3.back * yawForceMultiplier;
        }
        if(Input.GetKey(KeyCode.E))
        {
            force5 += Vector3.back * yawForceMultiplier;
            force6 += Vector3.back * yawForceMultiplier;
            force7 += Vector3.forward * yawForceMultiplier;
            force8 += Vector3.forward * yawForceMultiplier;
        }
        
        return new List<Vector3> {force5, force6, force7, force8};
    }
}
