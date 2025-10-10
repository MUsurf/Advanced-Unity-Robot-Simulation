using System.Collections;
using UnityEngine;

public class simTestScript : MonoBehaviour
{
    public float[] powerList = new float[8];
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        StartCoroutine(newPowers());
    }
    
    IEnumerator newPowers()
    {
        while(true)
        {
            for(int i = 0; i < powerList.Length; i++)
            {
                powerList[i] = Random.Range(-100f, 100f);
            }
            yield return new WaitForSeconds(8f);
        }
    }
}
