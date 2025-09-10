using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Ragtime : MonoBehaviour
{
    private string targetWord = "ragtime";
    private int currentIndex = 0;
    private bool ragtimeAchieved = false;
    private string targetWord2 = "next";
    private int currentIndex2 = 0;
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    public List<AudioClip> audioClips; // List of audio clips to play
    public AudioSource audioSource;
    private int currentClipIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShuffleAudioClips();
    }

    void Update()
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
                        ragtimeAchieved = !ragtimeAchieved;
                        currentIndex = 0;
                        if (ragtimeAchieved)
                        {                            
                            audioSource.clip = audioClips[currentClipIndex];

                            // Set the AudioSource to loop
                            audioSource.loop = false;

                            // Play the audio
                            audioSource.Play();

                            if (globalVolume.profile.TryGet(out colorAdjustments))
                            {
                                colorAdjustments.saturation.value = -100f;
                            }                           
                        }
                        else
                        {
                            audioSource.Stop();

                            if (globalVolume.profile.TryGet(out colorAdjustments))
                            {
                                colorAdjustments.saturation.value = 0f;
                            }
                        }
                    }
                }
                else
                {
                    currentIndex = 0;
                }
            }
        }

        if(ragtimeAchieved)
        {
            if (!audioSource.isPlaying)
            {
                // Move to the next clip
                currentClipIndex++;

                // If all clips have been played, shuffle the list and start over
                if (currentClipIndex >= audioClips.Count)
                {
                    currentClipIndex = 0;
                }

                // Assign the next clip to the AudioSource
                audioSource.clip = audioClips[currentClipIndex];

                // Play the next clip
                audioSource.Play();
            }

            if (currentIndex2 < targetWord2.Length)
            {
                foreach (char c in Input.inputString)
                {
                    if (c == targetWord2[currentIndex2])
                    {
                        currentIndex2++;
                        
                        if (currentIndex2 == targetWord2.Length)
                        {
                            // Move to the next clip
                            currentClipIndex++;

                            // If all clips have been played, shuffle the list and start over
                            if (currentClipIndex >= audioClips.Count)
                            {
                                currentClipIndex = 0;
                            }

                            // Assign the next clip to the AudioSource
                            audioSource.clip = audioClips[currentClipIndex];

                            // Play the next clip
                            audioSource.Play();

                            currentIndex2 = 0;
                        }
                    }
                    else
                    {
                    currentIndex2 = 0;
                    }
                }
            }
        }
    }

    void ShuffleAudioClips()
    {
        System.Random rng = new System.Random();
        int n = audioClips.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            AudioClip value = audioClips[k];
            audioClips[k] = audioClips[n];
            audioClips[n] = value;
        }
    }
}