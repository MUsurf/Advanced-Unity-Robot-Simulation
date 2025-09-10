using UnityEngine;

public class FloorDisable : MonoBehaviour
{
    public GameObject floor;
    // Update is called once per frame
    void Start()
    {
        floor.SetActive(true);
    }
    public void changeFloor(bool wantFloor)
    {
        floor.SetActive(wantFloor);
    }
}
