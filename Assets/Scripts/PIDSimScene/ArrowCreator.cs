using System.Collections.Generic;
using UnityEngine;

public class ArrowCreator : MonoBehaviour
{
    // TODO - for every scale of 1, add 0.5 to x positon on arrow
    // TODO - every fixed frame set the position of the arrow to the position of the arrowposition thingy
    public GameObject cube;
    public GameObject Accelarrow;

    public GameObject AccelarrowHead;
    public GameObject AccelarrowheadPosition;

    // public GameObject Parrow;
    // public GameObject Iarrow;
    // public GameObject Darrow;
    public List<GameObject> arrows = new List<GameObject>();
    public List<GameObject> arrowHeads = new List<GameObject>();
    public List<GameObject> arrowHeadsPositions = new List<GameObject>();
    public PIDSim pidSimScript;
    private List<float> values = new List<float>();
    private List<float> positions = new List<float>() {0f, 1.5f, 2.5f, 3.5f};
    
    // void Start()
    // {
    //     transform.position = arrowPosition.transform.position;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     transform.position = arrowPosition.transform.position;
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < arrowHeads.Count; i++)
        {
            arrowHeads[i].transform.position = arrowHeadsPositions[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        values = pidSimScript.returnValues();

        // Debug.Log($"Values: {values[0]}");
        updateAll();
    }

    public void updateAll()
    {
        for(int i = 0; i < arrows.Count; i++)
        {
            if(pidSimScript.onYAxis)
            {
                arrows[i].transform.position = cube.transform.position + new Vector3(-positions[i], 0, 0);
                
                arrows[i].transform.position += new Vector3(0, Mathf.Sign(values[i])*0.5f + values[i]/2, 0);

                if(values[i] < 0)
                {
                    arrowHeads[i].transform.localRotation = Quaternion.Euler(180, 90, 90);
                }
                else
                {
                    arrowHeads[i].transform.localRotation = Quaternion.Euler(0, 90, 90);
                }
            
                Vector3 scale = new Vector3(0.1f, 1, 0.1f);
                scale.y = values[i];
                arrows[i].transform.localScale = scale;
            }
            else
            {
                arrows[i].transform.position = cube.transform.position + new Vector3(0f, positions[i], 0);
                
                arrows[i].transform.position += new Vector3(Mathf.Sign(values[i])*0.5f + values[i]/2, 0, 0);

                if(values[i] < 0)
                {
                    arrowHeads[i].transform.localRotation = Quaternion.Euler(90, 180, 0);
                }
                else
                {
                    arrowHeads[i].transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
            
                Vector3 scale = new Vector3(1, 0.1f, 0.1f);
                scale.x = values[i];
                arrows[i].transform.localScale = scale;
            }
            

        }

        for(int i = 0; i < arrowHeads.Count; i++)
        {
            if(pidSimScript.onYAxis)
            {
                Vector3 newPosition = arrowHeadsPositions[i].transform.position;
                newPosition.y += Mathf.Sign(values[i])*0.5f + values[i]/2 + Mathf.Sign(values[i]) * -0.5f;
                arrowHeads[i].transform.position = newPosition;

            }
            else
            {
                arrowHeads[i].transform.position = arrowHeadsPositions[i].transform.position;
            }

            arrowHeads[i].transform.position += new Vector3(-0.05f, 0, 0);
        }
        
    }

    public List<float> getValues()
    {
        return values;
    }
}