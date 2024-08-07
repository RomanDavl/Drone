using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseText;
    public Button pauseButton;
    public Button resumeButton;
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
        if (pauseButton != null && resumeButton != null)
        {
            pauseButton.onClick.AddListener(Pause);
            resumeButton.onClick.AddListener(Resume);
        }
        else
        {
            Debug.LogError("PauseButton or ResumeButton is not assigned.");
        }

        
    }

    

    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P key was pressed");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (pauseButton != null && resumeButton != null)
        {
            
            UpdateButtonVisibility();
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
    private void UpdateButtonVisibility()
    {
        if (pauseButton != null && resumeButton != null)
        {
            pauseButton.gameObject.SetActive(!isPaused);
            resumeButton.gameObject.SetActive(isPaused);
        }
    }
}
