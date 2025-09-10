using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PIDSim : MonoBehaviour
{
    // TODO - add button for dont get rid of integral windup or derivative kick?
    // TODO - add button that comes before ki for on off gravity
    // TODO switch button broke

    public Rigidbody rb;
    public GameObject Platform;
    public GameObject RightPlatform;
    public GameObject LeftPlatform;
    public GameObject CameraPOS;
    public bool onYAxis = false;
    public Toggle IToggle;
    public PIDSimController controller = new PIDSimController();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller.Kp = 0f;
        controller.Ki = 0f;
        controller.Kd = 0f;
        IToggle.isOn = false;
        IToggle.gameObject.SetActive(false);
    }

    //Update is called once per frame
    void FixedUpdate()
    {
        if((controller.Kp != 0f || controller.Ki != 0f || controller.Kd != 0f) && !onYAxis)
        {
            Vector3 force = new Vector3(controller.Update(Time.fixedDeltaTime, rb.transform.position.x, 0f), 0f, 0f);
            rb.AddForce(force, ForceMode.Force);
        }
        else if((controller.Kp != 0f || controller.Ki != 0f || controller.Kd != 0f) && onYAxis)
        {
            Vector3 force = new Vector3(0f, controller.Update(Time.fixedDeltaTime, rb.transform.position.y, 8.5f), 0f);
            rb.AddForce(force, ForceMode.Force);
        }
    }

    public void enableP(bool enable)
    {   
        Debug.Log("P: " + enable);
        if(enable)
        {
            controller.Kp = 1f;
        }
        else
        {
            controller.Kp = 0f;
        }
    }

    public void enableI(bool enable)
    {
        if(enable)
        {
            controller.Ki = 0.55f;
            controller.integrationStored = 0f;
        }
        else
        {
            controller.Ki = 0f;
            controller.integrationStored = 0f;
        }
    }

    public void enableD(bool enable)
    {
        if(enable)
        {
            controller.Kd = 0.33f;
        }
        else
        {
            controller.Kd = 0f;
        }
    }

    public void enableYAxis()
    {
        onYAxis = !onYAxis;
        if(onYAxis)
        {
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
            rb.transform.position = new Vector3(0f, 0.5f, 0f);
            Platform.transform.position = new Vector3(2f, 8.5f, 0f);
            rb.useGravity = true;
            LeftPlatform.transform.position = new Vector3(2f, 0.5f, 0f);
            RightPlatform.transform.position = new Vector3(2f, 16.5f, 0f);
            CameraPOS.transform.position = new Vector3(0f, 8f, -20f);
            IToggle.isOn = false;
            IToggle.gameObject.SetActive(true);
            controller.Reset();

        }
        else
        {
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
            rb.transform.position = new Vector3(-8f, 2.5f, 0f);
            rb.useGravity = false;
            Platform.transform.position = new Vector3(0f, 0.5f, 0f);
            LeftPlatform.transform.position = new Vector3(-8f, 0.5f, 0f);
            RightPlatform.transform.position = new Vector3(8f, 0.5f, 0f);
            CameraPOS.transform.position = new Vector3(0f, 3f, -12f);
            IToggle.isOn = false;
            IToggle.gameObject.SetActive(false);
            controller.Reset();
        }
    }

    public List<float> returnValues()
    {
        List<float> values = new List<float>
        {
            rb.linearVelocity.x/2,
            controller.LastP/2,
            controller.LastI/2,
            controller.LastD/2
        };
        return values;
    }
}

// a class for initializing the PID controller
public class PIDSimController
{
    //note - the wording value just mean distance in this context

    public float LastP;
    public float LastI;
    public float LastD;

    //proportional gain
    public float Kp;
    
    // integral gain
    public float Ki;

    // derivative gain
    public float Kd;

    public float integrationStored;

    // the error from the last time step
    private float lastValue;
    private bool notFirstUpdate = false;

    // the maximum value the integral term can take, to prevent intergral windup
    private float integralSaturation = 8f;


    // dt - time step, in unity we use Time.fixedDeltaTime for this
    // currentValue - the current value of the system (current position)
    // targetValue - the desired value of the system (target position)
    public float Update(float dt, float currentValue, float targetValue)
    {
        float result;

        float error = targetValue - currentValue;

        // proportional term
        float P = Kp * error;

        // integral term
        integrationStored = Mathf.Clamp(integrationStored + (error * dt), -integralSaturation, integralSaturation);
        float I = Ki * integrationStored;

        float D = 0;

        // This check to make sure that the derivative term is not calculated on the first update, 
        // as it causes a large spike in the output since the error is very large and the lastValue is 0
        if(notFirstUpdate)
        {
            //derivative term
            float valueRateOfChange = (currentValue - lastValue) / dt;

            D = Kd * (-valueRateOfChange);
        }
        else
        {
            notFirstUpdate = true;
            D = 0;
        }

        result = P + I + D;

        LastP = P;
        LastI = I;
        LastD = D;

        lastValue = currentValue;

        return result;
    }

    // reset the controller when the pid is not in use
    public void Reset()
    {
        lastValue = 0;
        notFirstUpdate = false;
    }
}