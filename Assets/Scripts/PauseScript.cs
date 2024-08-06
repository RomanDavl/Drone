using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseText;
    private bool isPaused = false;

    void Start()
    {
        if (pauseText != null)
        {
            pauseText.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseText GameObject is not assigned.");
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Escape key was pressed");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Debug.Log("Pausing the game");
        Time.timeScale = 0.0f;
        if (pauseText != null)
        {
            pauseText.SetActive(true);
        }
        isPaused = true;
    }

    public void Resume()
    {
        Debug.Log("Resuming the game");
        Time.timeScale = 1.0f;
        if (pauseText != null)
        {
            pauseText.SetActive(false);
        }
        isPaused = false;
    }
}
