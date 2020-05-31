using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempController : MonoBehaviour
{

    public Sprite[] thermSprites;
    public Image curThemometer;
    public Image firstPackage;
    public Image secondPackage;
    public Image thirdPackage;
    public Text timer;
    public string timePassed = "";
    public int timePassedInt;
    public int countDownSeconds = 90;
    public GameObject gameOverScreen;

    private float startTime;
    private PlayerPlatformerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerPlatformerController>();
    }

    // Update is called once per frame
    void Update()
    {
        curThemometer.sprite = thermSprites[(int) playerController.warmth/10];

        switch (playerController.packagesFound)
        {
            case 1: 
                firstPackage.enabled = true;
                break;
            case 2: 
                secondPackage.enabled = true;
                break;
            case 3: 
                thirdPackage.enabled = true;
                break;
            default:
                break;
        }

    }

    void Awake() 
    {
        startTime = Time.time;
    }

    void OnGUI() 
    {
        var guiTime = Time.time - startTime;
        var restSeconds = countDownSeconds - (guiTime);

        var roundedRestSeconds = Mathf.CeilToInt(restSeconds);
        var displaySeconds = roundedRestSeconds % 60;
        var displayMinutes = roundedRestSeconds / 60; 
        if(roundedRestSeconds <= 0){
            roundedRestSeconds = 0;
            displaySeconds = 0;
            displayMinutes = 0;

            GameOver();
        }
        timer.text = string.Format("{0:00}:{1:00}", displayMinutes, displaySeconds); 

        timePassedInt = countDownSeconds - roundedRestSeconds;
        var displaySecondsPassed = timePassedInt % 60;
        var displayMinutesPassed = timePassedInt / 60;
        timePassed =  string.Format("{0:00}:{1:00}", displayMinutesPassed, displaySecondsPassed);
    }

    void GameOver()
    {
        if(!playerController.levelOver)
        {
            Time.timeScale = 0;
            playerController.levelOver = true;
            gameOverScreen.SetActive(true);
        }    
    }
}
