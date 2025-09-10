using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Rigidbody targetRigidbody;
    public GameObject ReallyMotionSicknessToggle;
    public GameObject firstPersonToggle;
    public GameObject fcameraPos;
    public GameObject tcameraPos;
    private bool trackPosition = false;
    private bool trackDistance = false;
    public bool trackRotation = false;
    private bool motionSickness = false;
    private bool reallyMotionSickness = false;
    public bool firstPerson = false;
    private Vector3 offset = new Vector3(-10, 5, 0);

    void Start()
    {
        if (ReallyMotionSicknessToggle != null)
        {
            ReallyMotionSicknessToggle.SetActive(false);
        }
        firstPersonToggle.SetActive(false);
    }

    void Update()
    {
        if (trackPosition)
        {
            transform.LookAt(targetRigidbody.transform);
        }
        if (trackDistance)
        {
            transform.position = targetRigidbody.transform.position + offset;
        }
        if (trackRotation)
        {
            if(firstPerson)
            {
                transform.position = fcameraPos.transform.position;
                transform.rotation = fcameraPos.transform.rotation;
            }
            else
            {
                transform.position = tcameraPos.transform.position;
                transform.rotation = tcameraPos.transform.rotation;
            }
        }
        if(motionSickness)
        {
            transform.position = targetRigidbody.transform.TransformPoint(offset);
            transform.LookAt(targetRigidbody.transform);
            if(reallyMotionSickness)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetRigidbody.rotation.eulerAngles.z);
            }
        }
        
    }

    public void changeTrackPosition(bool wantTrackPosition)
    {
        trackPosition = wantTrackPosition;
    }

    public void changeTrackDistance(bool wantTrackDistance)
    {
        trackDistance = wantTrackDistance;
    }

    public void changeTrackRotation(bool wantTrackRotation)
    {
        trackRotation = wantTrackRotation;
        firstPersonToggle.SetActive(wantTrackRotation);
        firstPerson = false;
    }

    public void changeFirstPerson(bool wantFirstPerson)
    {
        firstPerson = wantFirstPerson;
    }

    public void changeMotionSickness(bool wantMotionSickness)
    {
        motionSickness = wantMotionSickness;
        ReallyMotionSicknessToggle?.SetActive(wantMotionSickness);
        reallyMotionSickness = false;
    }    

    public void changeReallyMotionSickness(bool wantReallyMotionSickness)
    {
        reallyMotionSickness = wantReallyMotionSickness;
    }
}