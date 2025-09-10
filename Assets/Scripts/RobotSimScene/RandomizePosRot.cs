using UnityEngine;

public class RandomizePosRot : MonoBehaviour
{
    public void RandomizePos()
    {
        transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
    }

    public void RandomizeRot()
    {
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }
}