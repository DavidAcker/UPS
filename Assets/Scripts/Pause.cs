using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;

    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape)) 
        {
            Debug.Log("Key pressed");
            if (!pausePanel.activeInHierarchy) 
            {
                Debug.Log("pause");
                PauseGame();
            }
            // if (pausePanel.activeInHierarchy) 
            // {
            //     Debug.Log("resume");
            //      ContinueGame();   
            // }
        } 
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    } 

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
}
