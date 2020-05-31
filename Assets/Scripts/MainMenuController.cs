using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    // Start is called before the first frame update
    void Start()
    {
        // myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
        // scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playGame() 
    {
        Debug.Log("in playgame button");
        SceneManager.LoadScene("WelcomeMenu");
    }
}
