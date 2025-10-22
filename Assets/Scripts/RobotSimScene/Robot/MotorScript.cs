using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

// TODO - max thrust is - 51.4 N and 40 N
// TODO - invert mouse button
// TODO - could rainbow be more efficient?
// TODO - remove gravity, cause the sub cant fight it

public class MotorScript : MonoBehaviour
{
    private static string fileName = "motorIO.txt";
    float[] motorPowers = new float[8];
    
    private string inputFilePath = Path.Combine(Application.dataPath, "Assets", fileName);
    public Rigidbody rb;
    public float maxSpeed;
    private Vector3[] force = new Vector3[8];
    //front left, front right, back left, back right XY motors
    //then front left, front right, back left, back right Z motors
    private Vector3[] position =
    {
        new Vector3(1.95f, 0f, 4.9f),
        new Vector3(-1.95f, 0f, 4.9f),
        new Vector3(1.95f, 0f, -4.9f),
        new Vector3(-1.95f, 0f, -4.9f),
        new Vector3(2.72f, 0f, 5.66f),
        new Vector3(-2.72f, 0f, 5.66f),
        new Vector3(2.72f, 0f, -5.66f),
        new Vector3(-2.72f, 0f, -5.66f),
    };
    private List<Vector3> MovementOverrideList = new List<Vector3>();
    public MovementController movementControllerScript;
    public PID PIDScript;
    [SerializeField] private simTestScript testScript;
    public bool overrideMovement = false;
    public GameObject InvertMouseButton;
    private float timeSinceLastIToggle = 0f;

    public GameObject ThirdPersonCamera;
    public GameObject FirstPersonCamera;
    public Material robotMaterial;

    public bool collisionBool = false;
    public bool collisionBoomBool = false;
    public Explosion explosionScript;
    public AudioClip collisionSound;
    public GameObject explodeQuad;
    public GameObject mainCamera;

    IEnumerator updateMotorPowers()
    {
        //TODO: PARSE THE LINE!
        char delimiter = ',';
        string[] line = ReadTextFile(inputFilePath).Split(delimiter);
        motorPowers = Array.ConvertAll(line, s => float.Parse(s, CultureInfo.InvariantCulture));

        yield return new WaitForSeconds(1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(updateMotorPowers());

        rb = GetComponent<Rigidbody>();
        if (!overrideMovement)
        {
            InvertMouseButton.SetActive(false);
        }
        else
        {
            InvertMouseButton.SetActive(true);
        }
        rb.useGravity = false;
        maxSpeed = 320f;
        explodeQuad.SetActive(false);
    }
    
    /*
        Read the next line of the I/O motor powers file.
        Takes:
            string filePath = path to the I/O file
        Returns:
            string containing line read
    */
    private string ReadTextFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            string line = reader.ReadLine();
            if (line != null)
            {
                Debug.LogWarning("Input: " + line); // Should be a 8-element list of motor powers
                return line;
            }
            else
            {
                Debug.LogWarning("The file is empty or the first line is null.");
            }
        }
        else
        {
            Debug.LogError("File not found at: " + filePath);
        }
        return null;
    }

    // FixedUpdate is called once per fixed frame (physics engine)
    void FixedUpdate()
    {

        // TODO - here is where we get the motor powers

        // TODO - you can probably do the 40 and 51.4f better using a custom line

        for (int i = 0; i < force.Length / 2; i++)
        {
            force[i] = new Vector3((testScript.powerList[i] < 0 ? testScript.powerList[i] * 40 / 100 : testScript.powerList[i] * 51.4f / 100) * (Mathf.Sqrt(2)/2), 0, (testScript.powerList[i] < 0 ? testScript.powerList[i] * 40 / 100 : testScript.powerList[i] * 51.4f / 100) * (Mathf.Sqrt(2)/2));
        }
        
        for(int i = force.Length / 2; i < force.Length; i++)
        {
            force[i] = new Vector3(0, testScript.powerList[i] < 0 ? testScript.powerList[i] * 40 / 100 : testScript.powerList[i] * 51.4f / 100, 0);
        }

        // Debug.Log($"force[0]: {force[0]}, force[1]: {force[1]}, force[2]: {force[2]}, force[3]: {force[3]}, force[4]: {force[4]}, force[5]: {force[5]}, force[6]: {force[6]}, force[7]: {force[7]}");

        for(int i = 0; i < force.Length; i++)
        {
            rb.AddForceAtPosition(transform.TransformDirection(force[i]), transform.TransformPoint(position[i]), ForceMode.Force);
        }

        // NYC Skyline (post 9/11)        
    }

    public string getMotorValues()
    {
        return $"{Mathf.Clamp(force[0].z / maxSpeed / .707f, -1, 1)}, {Mathf.Clamp(force[1].z / maxSpeed / .707f, -1, 1)}, {Mathf.Clamp(force[2].z / maxSpeed / .707f, -1, 1)}, {Mathf.Clamp(force[3].z / maxSpeed / .707f, -1, 1)}, {force[4].y / maxSpeed}, {force[5].y / maxSpeed}, {force[6].y / maxSpeed}, {force[7].y / maxSpeed}";
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collisionBool == true)
        {
            AudioSource.PlayClipAtPoint(collisionSound, transform.position);

            Vector3 point = collision.GetContact(0).point;

            explodeQuad.transform.position = point;

            Vector3 lookPoint = explodeQuad.transform.position - mainCamera.transform.position;
            lookPoint.y = mainCamera.transform.position.y;
            explodeQuad.transform.LookAt(lookPoint);

            explodeQuad.SetActive(true);

            Invoke("disableExplosion", 0.5f);

            if(collisionBoomBool)
            {
                explosionScript.ExplodePoint(point);
            }
        }    
    }

    void disableExplosion()
    {
        explodeQuad.SetActive(false);
    }

    public void enableOverrideMovement(bool enable)
    {
        overrideMovement = enable;
        if(enable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        movementControllerScript.invertMouse = false;
        movementControllerScript.calledFor = enable;
        InvertMouseButton.SetActive(enable);
    }

    public void realisticMode(bool enable)
    {
        if(enable)
        {
            maxSpeed = 40f;
        }
        else
        {
            maxSpeed = 320f;
        }
    }

    private string targetWord = "rainbow";
    private int currentIndex = 0;
    private bool rainbowAchieved = false;

    private float duration = 1f; // Duration to transition between each color
    private List<Color> rainbowColors = new List<Color>
        {
            Color.red,
            new Color(1f, 0.5f, 0f), // Orange
            Color.yellow,
            Color.green,
            Color.blue,
            new Color(0.29f, 0f, 0.51f), // Indigo
            new Color(0.56f, 0f, 1f) // Violet
        };
    private int currentColorIndex = 0;
    private float timeElapsed = 0f;
    public Renderer objectRenderer;

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.R))
        {
            rb.position = new Vector3(0, 4.5f, 0);
            rb.rotation = Quaternion.identity;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            PIDScript.resetAll();
        }

        timeSinceLastIToggle += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.I) && overrideMovement && timeSinceLastIToggle > 0.2f)
        {
            timeSinceLastIToggle = 0f;
            //movementControllerScript.invertMouse = !movementControllerScript.invertMouse;
            InvertMouseButton.GetComponent<Toggle>().isOn = !InvertMouseButton.GetComponent<Toggle>().isOn;
        }

        if (currentIndex < targetWord.Length)
        {
            foreach (char c in Input.inputString)
            {
                if (c == targetWord[currentIndex])
                {
                    currentIndex++;
                    
                    if (currentIndex == targetWord.Length)
                    {
                        rainbowAchieved = !rainbowAchieved;
                        currentIndex = 0;
                        if (!rainbowAchieved)
                        {
                            objectRenderer.material = robotMaterial;

                            currentColorIndex = 0;
                            timeElapsed = 0f;
                        }
                    }
                }
                else
                {
                    currentIndex = 0;
                }
            }
        }
        
        if (rainbowAchieved)
        {
            timeElapsed += Time.deltaTime;

            // lerp value is a value between 0 and 1 that represents the progress of the interpolation
            float lerpValue = timeElapsed / duration;

            // Interpolate the color
            Color startColor = rainbowColors[currentColorIndex];
            Color endColor = rainbowColors[(currentColorIndex + 1) % rainbowColors.Count];
            objectRenderer.material.color = Color.Lerp(startColor, endColor, lerpValue);

            // Move to the next color if the duration is exceeded
            if (timeElapsed >= duration)
            {
                timeElapsed = 0f;
                currentColorIndex = (currentColorIndex + 1) % rainbowColors.Count;
            }
        }
    }
}