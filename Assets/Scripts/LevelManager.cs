using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public Scene[] levels;
    public int curScene = 0;
    public GameObject LevelScroll;
    public Scene nextLevel;
    

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("WelcomeMenu");
    }

    public void GoToLevelMenu()
    {
        //SceneManager.LoadScene("LevelMenu");
        LevelScroll.SetActive(true);
    }

    public void GoToWelcome()
    {
        LevelScroll.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoLevel0()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level0");
    }

    public void GoLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void GoLevel2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level2");
    }

    public void GoBossLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("BossLevel");
    }

    public void GoFinalLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("FinalLevel");
    }

    public void NextLevel()
    {
        curScene += 1;
        //SceneManager.LoadScene(nextLevel.name);
    }
}
