using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotSceneController : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("PIDSimScene");
    }
}
