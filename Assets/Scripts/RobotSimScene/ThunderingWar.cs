using UnityEngine;

public class ThunderingWar : MonoBehaviour
{
    private string targetWord = "war thunder";
    private int currentIndex = 0;
    private bool warthunderAchieved = false;
    public AudioClip audioClip; // List of audio clips to play
    public AudioSource audioSource;
    // public MovementController MovementControllerScript;
    public MovementController movementControllerScript;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(movementControllerScript.calledFor)
        {
            if (currentIndex < targetWord.Length)
            {
                foreach (char c in Input.inputString)
                {
                    if (c == targetWord[currentIndex])
                    {
                        currentIndex++;
                        
                        if (currentIndex == targetWord.Length)
                        {
                            warthunderAchieved = !warthunderAchieved;
                            currentIndex = 0;
                            if (warthunderAchieved)
                            {                            
                                audioSource.clip = audioClip;

                                audioSource.loop = false;

                                audioSource.Play();

                            }
                            else
                            {
                                audioSource.Stop();
                            }
                        }
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
            }
        }
    }
}

