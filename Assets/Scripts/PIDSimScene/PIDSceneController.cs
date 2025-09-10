using UnityEngine;
using UnityEngine.SceneManagement;

public class PIDSceneController : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("RobotSimScene");
    }
}
