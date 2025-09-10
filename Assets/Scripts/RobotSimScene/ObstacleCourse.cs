using UnityEngine;

public class ObstacleCourse : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int obstacleDistance = 40;
    public int width = 20;
    public int obstacleAmount = 20;
    private Vector3 obstaclePosition;
    public GameObject hoopPrefab;
    public int hoopDistance = 120;
    public int hoopAmount = 5;
    private Vector3 hoopPosition;
    public int spaceApart = 50;
    public int spaceForward = 60;
    private int flipInt = 1;


    public PID PIDScript;
    public GameObject obstacleCourseSphere;
    public GameObject realRobot;
    private bool runCourse = false;
    private Vector3 spherePosition;
    private bool hoopDone = false;
    private bool courseDone = false;
    private bool hoopReset = false;

    public GameObject ThirdPersonCamera;
    public GameObject FirstPersonCamera;
    public CameraTrack CameraTrackScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obstacleCourseSphere.SetActive(false);
        obstaclePosition = new Vector3(spaceApart - width, 16, spaceForward);
        
        for(int i = 0; i < obstacleAmount; i++)
        {
            Instantiate(obstaclePrefab, obstaclePosition, new Quaternion(0, 0, 0, 0));
            obstaclePosition.z += obstacleDistance;
            obstaclePosition.x += flipInt * width;
            flipInt *= -1;
        }

        hoopPosition = new Vector3(-spaceApart, 0, spaceForward);

        for(int i = 0; i < hoopAmount; i++)
        {
            Instantiate(hoopPrefab, hoopPosition, Quaternion.Euler(0, 90, 0));
            hoopPosition.z += hoopDistance;
        }

        spherePosition = new Vector3(-spaceApart, 35, spaceForward);
    }

    void Update()
    {
        if(runCourse)
        {
            PIDScript.xSetpoint = spherePosition.x;
            PIDScript.ySetpoint = spherePosition.y;
            PIDScript.zSetpoint = spherePosition.z;

            if(PIDScript.yawTrack)
            {
                PIDScript.yawSetpoint = findAngle(realRobot.transform.position, spherePosition);
            }

            if(Vector3.Distance(realRobot.transform.position, spherePosition) < 3)
            {
                if(courseDone)
                {
                    spherePosition = new Vector3(-spaceApart, 35, spaceForward);
                    
                    obstacleCourseSphere.transform.position = spherePosition;
                    courseDone = false;
                }
                else if((spherePosition.z < hoopDistance * (hoopAmount - 1) + spaceForward) && !hoopDone)
                {
                    spherePosition.z += hoopDistance;
                    
                    obstacleCourseSphere.transform.position = spherePosition;
                }
                else if(hoopDone)
                {
                    if(spherePosition.z == 60)
                    {
                        spherePosition = new Vector3(0, 5, 0);
                        
                        obstacleCourseSphere.transform.position = spherePosition;
                        courseDone = true;
                        hoopDone = false;

                        if((CameraTrackScript.firstPerson || CameraTrackScript.trackRotation) && (!PIDScript.yawTrack))
                        {
                            FirstPersonCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
                            FirstPersonCamera.transform.position = new Vector3(0, 0, 4) + realRobot.transform.position;
                            ThirdPersonCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
                            ThirdPersonCamera.transform.position = new Vector3(0, 10, -20) + realRobot.transform.position;
                        }
                    }
                    else
                    {
                        spherePosition.z -= obstacleDistance;
                        if(spherePosition.x == 30 + width)
                        {
                            spherePosition.x -= width;
                        }
                        else
                        {
                            spherePosition.x += width;
                        }
                        
                        obstacleCourseSphere.transform.position = spherePosition;
                    }
                }
                else if(hoopReset)
                {
                    if((CameraTrackScript.firstPerson || CameraTrackScript.trackRotation) && (!PIDScript.yawTrack))
                    {
                        FirstPersonCamera.transform.rotation = Quaternion.Euler(0, 180, 0);
                        FirstPersonCamera.transform.position = new Vector3(0, 0, -4) + realRobot.transform.position;
                        ThirdPersonCamera.transform.rotation = Quaternion.Euler(0, 180, 0);
                        ThirdPersonCamera.transform.position = new Vector3(0, 10, 20) + realRobot.transform.position;
                    }

                    hoopReset = false;
                    hoopDone = true;
                    spherePosition = new Vector3(30 + obstacleAmount % 2 * width, 5, spaceForward + obstacleDistance * (obstacleAmount - 1));
                    
                    obstacleCourseSphere.transform.position = spherePosition;
                }
                else
                {
                    spherePosition.z = spaceForward + obstacleDistance * (obstacleAmount - 1);
                    
                    obstacleCourseSphere.transform.position = spherePosition;
                    hoopReset = true;
                }

            }
        }
    }

    public void RunCourse()
    {
        runCourse = !runCourse;
        obstacleCourseSphere.SetActive(runCourse);
        obstacleCourseSphere.transform.position = spherePosition;
    }

    private float findAngle(Vector3 fromPosition, Vector3 toPosition)
    {
        Vector3 direction = toPosition - fromPosition;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        return -(angle - 90);
    }
}
