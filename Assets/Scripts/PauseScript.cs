using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseText;
    public Button pauseButton;
    public Button resumeButton;

    public Button TimerButton;

    public Button Drohnenshot1;

    public Button Drohnenshot2;

    public Button Drohnenshot3;

    public Button Drohnenshot4;

    public Button Drohnenshot5;

    public Button Drohnenshot6;

    public GameObject shotButtonsPanel;// Parent object von den shot buttons
    private bool isPaused = false;
    private int selectedShot = 1;

    private bool timer = true;

    void Start()
    {
        if (pauseText != null){
            pauseText.SetActive(false);
        }
        
        if (pauseButton != null && resumeButton != null){
            pauseButton.onClick.AddListener(Pause);
            resumeButton.onClick.AddListener(Resume);
        }
        if(  Drohnenshot1 != null &&  Drohnenshot2 != null &&  Drohnenshot3 != null && Drohnenshot4 != null && Drohnenshot5 != null && Drohnenshot6 != null){
            Drohnenshot1.onClick.AddListener(SelectShot1);
            Drohnenshot2.onClick.AddListener(SelectShot2);
            Drohnenshot3.onClick.AddListener(SelectShot3);
            Drohnenshot4.onClick.AddListener(SelectShot4);
            Drohnenshot5.onClick.AddListener(SelectShot5);
            Drohnenshot6.onClick.AddListener(SelectShot6);
            Drohnenshot1.gameObject.SetActive(false);
            Drohnenshot2.gameObject.SetActive(false);
            Drohnenshot3.gameObject.SetActive(false);
            Drohnenshot4.gameObject.SetActive(false);
            Drohnenshot5.gameObject.SetActive(false);
            Drohnenshot6.gameObject.SetActive(false);
        }
        if (shotButtonsPanel != null){
            shotButtonsPanel.SetActive(false);
        }
        if (TimerButton != null){
            TimerButton.gameObject.SetActive(false);
            TimerButton.onClick.AddListener(ChangeColor);
            TimerButton.GetComponent<Image>().color = Color.green;
            
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
        if (shotButtonsPanel != null)
        {
            Drohnenshot1.gameObject.SetActive(true);
            Drohnenshot2.gameObject.SetActive(true);
            Drohnenshot3.gameObject.SetActive(true);
            Drohnenshot4.gameObject.SetActive(true);
            Drohnenshot5.gameObject.SetActive(true);
            Drohnenshot6.gameObject.SetActive(true);
        }
        if(TimerButton!=null){
            TimerButton.gameObject.SetActive(true);
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
         if (shotButtonsPanel != null)
        {
            Drohnenshot1.gameObject.SetActive(false);
            Drohnenshot2.gameObject.SetActive(false);
            Drohnenshot3.gameObject.SetActive(false);
            Drohnenshot4.gameObject.SetActive(false);
            Drohnenshot5.gameObject.SetActive(false);
            Drohnenshot6.gameObject.SetActive(false);
        }
        if(TimerButton!=null){
            TimerButton.gameObject.SetActive(false);
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
         if (Drohnenshot1 != null && Drohnenshot2 != null && Drohnenshot3 != null && Drohnenshot4!= null && Drohnenshot5!= null && Drohnenshot6!= null)
        {
            Drohnenshot1.gameObject.SetActive(isPaused);
            Drohnenshot2.gameObject.SetActive(isPaused);
            Drohnenshot3.gameObject.SetActive(isPaused);
            Drohnenshot4.gameObject.SetActive(isPaused);
            Drohnenshot5.gameObject.SetActive(isPaused);
            Drohnenshot6.gameObject.SetActive(isPaused);
        }
        if (TimerButton!= null){
            TimerButton.gameObject.SetActive(isPaused);
        }
    }
     private void SelectShot(int shotNumber)
    {
        selectedShot = shotNumber;
        FindObjectOfType<DroneMovement>().SetShot(shotNumber);
        UpdateButtonColors();
    }
    private void UpdateButtonColors()
    {
        Drohnenshot1.GetComponent<Image>().color = Color.white;
        Drohnenshot2.GetComponent<Image>().color = Color.white;
        Drohnenshot3.GetComponent<Image>().color = Color.white;
        Drohnenshot4.GetComponent<Image>().color = Color.white;
        Drohnenshot5.GetComponent<Image>().color = Color.white;
        Drohnenshot6.GetComponent<Image>().color = Color.white;
        

        // Highlight the selected button
        switch (selectedShot)
        {
            case 1:
                Drohnenshot1.GetComponent<Image>().color = Color.green;
                break;
            case 2:
                Drohnenshot2.GetComponent<Image>().color = Color.green;
                break;
            case 3:
                Drohnenshot3.GetComponent<Image>().color = Color.green;
                break;
            case 4:
                Drohnenshot4.GetComponent<Image>().color = Color.green;
                break;
            case 5:
                Drohnenshot5.GetComponent<Image>().color = Color.green;
                break;
            case 6:
                Drohnenshot6.GetComponent<Image>().color = Color.green;
                break;
        }
        
        
    }
     private void SelectShot1()
    {
        SelectShot(1);
    }

    private void SelectShot2()
    {
        SelectShot(2);
    }

    private void SelectShot3()
    {
        SelectShot(3);
    }
    private void SelectShot4()
    {
        SelectShot(4);
    }
    private void SelectShot5()
    {
        SelectShot(5);
    }
    private void SelectShot6()
    {
        SelectShot(6);
    }
    private void ChangeColor()
    {
       UpdateButtonColorsTimer();
    }

    private void UpdateButtonColorsTimer()

    {
        
        if(timer==false){
            TimerButton.GetComponent<Image>().color = Color.green;
            timer = true;
            //Setzt den TimerButton in der DrohneMovementklasse auf false
            FindObjectOfType<DroneMovement>().SetTimerButton(true);
            


        }
        else{
           TimerButton.GetComponent<Image>().color = Color.white; 
           timer = false;
           // gleiche nur auf true
           FindObjectOfType<DroneMovement>().SetTimerButton(false);
    }
    }
}
