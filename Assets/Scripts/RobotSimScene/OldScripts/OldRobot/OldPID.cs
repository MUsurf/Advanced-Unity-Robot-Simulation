using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class OldPID : MonoBehaviour
{
    public Rigidbody rb;
    private OldPIDHandler LeHandler = new OldPIDHandler();
    public float xSetpoint = 0f;
    public float ySetpoint = 0f;
    public float zSetpoint = 0f;
    public float yawSetpoint = 0f;
    public float rollSetpoint = 0f;
    public float pitchSetpoint = 0f;
    public List<Vector3> forces = new List<Vector3>() {Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
    public List<float> andrewList = new List<float>() {0.5f, 0, 0.1f, 0.5f, 0, 0.1f, 0.5f, 0, 0.1f, 0.05f, 0, 0.1f, 0.05f, 0, 0.1f, 0.05f, 0, 0.1f};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public List<Vector3> getVectors(float[] pows){
        Vector3 force6 = new Vector3(pows[0]*0.707f, pows[4], pows[0]*0.707f);
        Vector3 force5 = new Vector3(pows[1]*0.707f, pows[5], -pows[1]*0.707f);
        Vector3 force7 = new Vector3(pows[2]*0.707f, pows[6], pows[2]*0.707f);
        Vector3 force8 = new Vector3(pows[3]*0.707f, pows[7], -pows[3]*0.707f);
        return new List<Vector3> {force5, force6, force7, force8};
    }

    public List<Vector3> returnGetVectors(){
        LeHandler.xPIDController.kP = andrewList[0];
        LeHandler.xPIDController.kI = andrewList[1];
        LeHandler.xPIDController.kD = andrewList[2];
        LeHandler.yPIDController.kP = andrewList[3];
        LeHandler.yPIDController.kI = andrewList[4];
        LeHandler.yPIDController.kD = andrewList[5];
        LeHandler.zPIDController.kP = andrewList[6];
        LeHandler.zPIDController.kI = andrewList[7];
        LeHandler.zPIDController.kD = andrewList[8];
        LeHandler.rollPIDController.kP = andrewList[9];
        LeHandler.rollPIDController.kI = andrewList[10];
        LeHandler.rollPIDController.kD = andrewList[11];
        LeHandler.pitchPIDController.kP = andrewList[12];
        LeHandler.pitchPIDController.kI = andrewList[13];
        LeHandler.pitchPIDController.kD = andrewList[14];
        LeHandler.yawPIDController.kP = andrewList[15];
        LeHandler.yawPIDController.kI = andrewList[16];
        LeHandler.yawPIDController.kD = andrewList[17];

        float[] powerArray = LeHandler.calculate(
            x_setpoint: xSetpoint,
            x_measurement: rb.position.x,
            y_setpoint: ySetpoint,
            y_measurement: -rb.position.z,
            z_setpoint: zSetpoint,
            z_measurement: rb.position.y,
            roll_setpoint: rollSetpoint,
            roll_measurement: normalize_angle(rb.rotation.eulerAngles.x),
            pitch_setpoint: pitchSetpoint,
            pitch_measurement: normalize_angle(rb.rotation.eulerAngles.z),
            yaw_setpoint: yawSetpoint,
            yaw_measurement: normalize_angle(rb.rotation.eulerAngles.y)
        );
        
        forces = getVectors(PowerControl.calculatePowers(powerArray[0], powerArray[1], powerArray[2], powerArray[3], powerArray[4], -powerArray[5]));
        
        return forces;
    }

    public float normalize_angle(float angle){
        if(angle <= 180){
            return angle;
        }
        return angle - 360;
    }

    public float[] ReturnPosRot(){
        float [] powers = new float[6] {-rb.position.z, rb.position.x, rb.position.y, rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.z, rb.rotation.eulerAngles.y};
        
        return powers;
    }
}

public class OldPIDController{
    public float kP;
    public float kI;
    public float kD;
    private long last_time;
    private float last_error;
    private bool is_first_call = false;
    private float accum_error = 0;
    public OldPIDController(float kP, float kI, float kD){
        this.kP = kP;
        this.kI = kI;
        this.kD = kD;
        last_time = 0;
        last_error = 0;
    }

    private long get_time(){
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }


    public float calculate(float setpoint, float measurement)
    {
        float error = setpoint - measurement;
        if(is_first_call)
        {
            is_first_call = false;
            last_error = error;
            last_time = get_time();
        }
        accum_error += error * (get_time() - last_time);
        float output = kP * error;
        output += kI * accum_error;
        if(get_time() - last_time != 0)
        {
            output += kD * (error - last_error) / (get_time() - last_time);
        }

        last_time = get_time();
        last_error = error;

        return output;
    }
}

public class OldPIDHandler{
    public OldPIDController xPIDController;
    public OldPIDController yPIDController;
    public OldPIDController zPIDController;
    public OldPIDController rollPIDController;
    public OldPIDController pitchPIDController;
    public OldPIDController yawPIDController;

    public OldPIDHandler(){
        //PID parameters are set inside here instead of constant variables
        xPIDController = new OldPIDController(
            0.5f,
            0,
            0.1f
        );
        yPIDController = new OldPIDController(
            0.5f,
            0,
            0.1f
        );
        zPIDController = new OldPIDController(
            0.5f,
            0,
            0.1f
        );
        rollPIDController = new OldPIDController(
            0.05f,
            0,
            0.1f
        );
        pitchPIDController = new OldPIDController(
            0.05f,
            0,
            0.1f
        );
        yawPIDController = new OldPIDController(
            0.05f,
            0,
            0.1f
        );
    }
    
    public float[] calculate(
        float? x_setpoint = null,
        float? x_measurement = null,
        float? y_setpoint = null,
        float? y_measurement = null,
        float? z_setpoint = null,
        float? z_measurement = null,
        float? roll_setpoint = null,
        float? roll_measurement = null,
        float? pitch_setpoint = null,
        float? pitch_measurement = null,
        float? yaw_setpoint = null,
        float? yaw_measurement = null
    ){
        return new float[6]{
            (x_setpoint == null || x_measurement == null) ? 0 : xPIDController.calculate((float)x_setpoint, (float)x_measurement),
            (y_setpoint == null || y_measurement == null) ? 0 : yPIDController.calculate((float)y_setpoint, (float)y_measurement),
            (z_setpoint == null || z_measurement == null) ? 0 : zPIDController.calculate((float)z_setpoint, (float)z_measurement),
            (roll_setpoint == null || roll_measurement == null) ? 0 : rollPIDController.calculate((float)roll_setpoint, (float)roll_measurement),
            (pitch_setpoint == null || pitch_measurement == null) ? 0 : pitchPIDController.calculate((float)pitch_setpoint, (float)pitch_measurement),
            (yaw_setpoint == null || yaw_measurement == null) ? 0 : yawPIDController.calculate((float)yaw_setpoint, (float)yaw_measurement)
        };
    }
}

public static class PowerControl{
    public static float max_abs_power = 100;

    public static float[] calculatePowers(
        float x = 0f,
        float y = 0f,
        float z = 0f,
        float roll = 0f,
        float pitch = 0f,
        float yaw = 0f
    ){
        //Power definitions
        float[] out_pows = new float[8]; //Is this syntax right for all 0s?

        float[] x_pows = new float[4]{x,x,x,x};
        float[] y_pows = new float[4]{-y,y,-y,y};
        float[] z_pows = new float[4]{z,z,z,z};
        float[] roll_pows = new float[4]{roll,-roll,-roll,roll};
        float[] pitch_pows = new float[4]{pitch,pitch,-pitch,-pitch};
        float[] yaw_pows = new float[4]{yaw,-yaw,-yaw,yaw};

        //First 4 motors
        float[] cumulative_powers = new float[4]{
            x_pows[0] + y_pows[0] + yaw_pows[0],
            x_pows[1] + y_pows[1] + yaw_pows[1],
            x_pows[2] + y_pows[2] + yaw_pows[2],
            x_pows[3] + y_pows[3] + yaw_pows[3],
        };

        float max = cumulative_powers.Max();
        float min = cumulative_powers.Min();
        float actual_max = (Math.Abs(max) > Math.Abs(min)) ? Math.Abs(max) : Math.Abs(min);
        float scale = 1;
        if(actual_max > max_abs_power){scale = actual_max;}
        out_pows[0] = cumulative_powers[0] / scale;
        out_pows[1] = cumulative_powers[1] / scale;
        out_pows[2] = cumulative_powers[2] / scale;
        out_pows[3] = cumulative_powers[3] / scale;

        cumulative_powers[0] = roll_pows[0] + pitch_pows[0] + z_pows[0];
        cumulative_powers[1] = roll_pows[1] + pitch_pows[1] + z_pows[1];
        cumulative_powers[2] = roll_pows[2] + pitch_pows[2] + z_pows[2];
        cumulative_powers[3] = roll_pows[3] + pitch_pows[3] + z_pows[3];
        max = cumulative_powers.Max();
        min = cumulative_powers.Min();
        actual_max = (Math.Abs(max) > Math.Abs(min)) ? Math.Abs(max) : Math.Abs(min);
        scale = 1;
        if(actual_max > max_abs_power){scale = actual_max;}
        out_pows[4] = cumulative_powers[0] / scale;
        out_pows[5] = cumulative_powers[1] / scale;
        out_pows[6] = cumulative_powers[2] / scale;
        out_pows[7] = cumulative_powers[3] / scale;

        //Debug.Log($"{out_pows[0]} {out_pows[1]} {out_pows[2]} {out_pows[3]} {out_pows[4]} {out_pows[5]} {out_pows[6]} {out_pows[7]} ");


        return out_pows;
    }
}