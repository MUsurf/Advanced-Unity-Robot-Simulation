using UnityEngine;
using UnityEngine.UIElements;

public class AIBehaviour : MonoBehaviour
{
    // TODO - make thing where shots a cone of rays in 90 degree (or whatever degree we want) from a point
    // TODO - make raycasts into sphere?
    public GameObject PlayerBot;
    public GameObject AIBot;
    public int distance = 10;
    public int rayDistance = 5;
    public int rayConeDistance = 5;
    public int rayConeAngle = 90;
    public bool aiEnabled = true;
    public bool followPlayer = true;
    public AIPID pidScript;
    public GameObject movePoint;
    public bool avoidObstacles = true;
    private Vector3 frontVector = new Vector3(-3.4f, 1.5f, 6.2f);
    private Vector3 rightVector = new Vector3(3.4f, 1.5f, 6.2f);
    private Vector3 upVector = new Vector3(-3.4f, 1.5f, 6.2f);

    // Update is called once per frame
    void FixedUpdate()
    {
        if(aiEnabled)
        {
            Vector3 directionVector = AIBot.transform.position - PlayerBot.transform.position;

            if(!followPlayer && directionVector.magnitude > distance)
            {
                return;
            }

            Vector3 wantedPosition;

            wantedPosition = PlayerBot.transform.position + directionVector.normalized * distance;

            //Debug.Log($"wantedPosition: {wantedPosition:F10}, directionVector: {directionVector.normalized:F10}");

            if(avoidObstacles)
            {

                float closestHit = rayDistance + 1;

                bool hit;

                int flip = 0;

                bool closestHitBack = false;

                while(flip != -1)
                {
                    Vector3 modifiedVector = frontVector;

                    if(flip == 0)
                    {
                        flip = 1;
                    }
                    else
                    {
                        flip = -1;
                    }

                    
                    for(int i = 0; i < 5; i++)
                    {
                        modifiedVector.y = frontVector.y - i * 1.5f * 2 / 5;
                        //2 is always, but change 9 to whatever i < than
                        for(int j = 0; j < 9; j++)
                        {
                            modifiedVector.x = frontVector.x + j * 3.4f * 2 / 9;
                            //2 is always, but change 9 to whatever i < than
                            hit = Physics.Raycast(flip * modifiedVector + AIBot.transform.position, Vector3.forward * flip, out RaycastHit hitInfo, rayDistance, 1);

                            // Debug.Log(hit);

                            Debug.DrawRay(flip * (modifiedVector) + AIBot.transform.position, Vector3.forward * rayDistance * flip, Color.red, 0);

                            if(hit && (hitInfo.distance < closestHit))
                            {
                                closestHit = hitInfo.distance;
                                if(flip == -1)
                                {
                                    closestHitBack = true;
                                }
                            }
                        }
                    }
                }
                
                if(closestHit != rayDistance + 1)
                {
                    float toAdd;
                    toAdd = closestHit - rayDistance;
                    if(closestHitBack)
                    {
                        toAdd = -toAdd;
                    }
                    wantedPosition.z = toAdd + AIBot.transform.position.z;

                    //Debug.Log($"toAdd: {toAdd}, wantedPosition: {wantedPosition:F10}, closestHit: {closestHit}");
                }

                flip = 0;

                closestHitBack = false;

                hit = false;

                while(flip != -1)
                {
                    Vector3 modifiedVector = rightVector;

                    if(flip == 0)
                    {
                        flip = 1;
                    }
                    else
                    {
                        flip = -1;
                    }

                    
                    for(int i = 0; i < 5; i++)
                    {
                        modifiedVector.y = rightVector.y - i * 1.5f * 2 / 5;
                        for(int j = 0; j < 10; j++)
                        {
                            modifiedVector.z = rightVector.z - j * 6.2f * 2 / 10;
                            hit = Physics.Raycast(flip * modifiedVector + AIBot.transform.position, Vector3.right * flip, out RaycastHit hitInfo, rayDistance, 1);

                            // Debug.Log(hit);

                            Debug.DrawRay(flip * (modifiedVector) + AIBot.transform.position, Vector3.right * rayDistance * flip, Color.red, 0);

                            if(hit && (hitInfo.distance < closestHit))
                            {
                                closestHit = hitInfo.distance;
                                if(flip == -1)
                                {
                                    closestHitBack = true;
                                }
                            }
                        }
                    }
                }
                
                if(closestHit != rayDistance + 1)
                {
                    float toAdd;
                    toAdd = closestHit - rayDistance;
                    if(closestHitBack)
                    {
                        toAdd = -toAdd;
                    }
                    wantedPosition.x = toAdd + AIBot.transform.position.x;

                    //Debug.Log($"toAdd: {toAdd}, wantedPosition: {wantedPosition:F10}, closestHit: {closestHit}");
                }

                flip = 0;

                closestHitBack = false;

                hit = false;

                while(flip != -1)
                {
                    Vector3 modifiedVector = upVector;

                    if(flip == 0)
                    {
                        flip = 1;
                    }
                    else
                    {
                        flip = -1;
                    }

                    
                    for(int i = 0; i < 9; i++)
                    {
                        modifiedVector.x = upVector.x + i * 3.4f * 2 / 9;
                        for(int j = 0; j < 10; j++)
                        {
                            modifiedVector.z = upVector.z - j * 6.2f * 2 / 10;
                            hit = Physics.Raycast(flip * modifiedVector + AIBot.transform.position, Vector3.up * flip, out RaycastHit hitInfo, rayDistance, 1);

                            // Debug.Log(hit);

                            Debug.DrawRay(flip * (modifiedVector) + AIBot.transform.position, Vector3.up * rayDistance * flip, Color.red, 0);

                            // Debug.Log("j:" + j);

                            if(hit && (hitInfo.distance < closestHit))
                            {
                                closestHit = hitInfo.distance;
                                if(flip == -1)
                                {
                                    closestHitBack = true;
                                }
                            }
                        }
                    }
                }
                
                if(closestHit != rayDistance + 1)
                {
                    float toAdd;
                    toAdd = closestHit - rayDistance;
                    if(closestHitBack)
                    {
                        toAdd = -toAdd;
                    }
                    wantedPosition.y = toAdd + AIBot.transform.position.y;

                    //Debug.Log($"toAdd: {toAdd}, wantedPosition: {wantedPosition:F10}, closestHit: {closestHit}");
                }

                //-1.5 to 1.5 y

                //-6.2 to 6.2 z

                //-3.4 to 3.4 x
            }

            fireCone(transform.position);

            pidScript.xSetpoint = wantedPosition.x;
            pidScript.ySetpoint = wantedPosition.y;
            pidScript.zSetpoint = wantedPosition.z;

            movePoint.transform.position = wantedPosition;
        }
    }

    private void fireCone(Vector3 point)
    {
        // TODO - make thing where shoots a cone of rays in 90 degree (or whatever degree we want) from a point

        float startAngle = -rayConeAngle / 2;

        for(int i = 0; i < rayConeAngle; i++)
        {
            Vector3 direction = new Vector3(Mathf.Sin((startAngle + i) * Mathf.Deg2Rad), 0, Mathf.Cos((startAngle + i) * Mathf.Deg2Rad));
            bool hit = Physics.Raycast(point, direction, out RaycastHit hitConeInfo, rayConeDistance, 1);

            Debug.DrawRay(point, direction * rayConeDistance, Color.red, 0);
        }

        // Debug.Log(hit);

        // Debug.DrawRay(flip * (modifiedVector) + AIBot.transform.position, Vector3.up * rayDistance * flip, Color.red, 0);
    }
}
