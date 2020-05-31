using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    public string levelName;
    public Text rank;
    public Text time;
    public Image pawprint;
    public GameObject levelCard;

    // private float time;
    // private string rank;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        pawprint.enabled = false;
        var complete = PlayerPrefs.GetString(levelName + "Complete", "false");
        
        if(bool.Parse(complete))
        {
            pawprint.enabled = true;
        }

        time.text = "Time: " + PlayerPrefs.GetString(levelName + "Time", "");
        rank.text = "Rank: " + PlayerPrefs.GetString(levelName + "Rank", "");
        // PlayerPrefs.DeleteAll();

        if(levelName == "BossLevel")
        {
            var complete0 = PlayerPrefs.GetString("Level0Complete", "false");
            var complete1 = PlayerPrefs.GetString("Level1Complete", "false");
            var complete2 = PlayerPrefs.GetString("Level2Complete", "false");
        
            if(bool.Parse(complete0) && bool.Parse(complete1) && bool.Parse(complete2))
            {
                levelCard.SetActive(true);
            } else {
                levelCard.SetActive(false);
            }
            
        }

        if(levelName == "FinalLevel")
        {
            var rank0 = PlayerPrefs.GetString("Level0Rank", "");
            var rank1 = PlayerPrefs.GetString("Level1Rank", "");
            var rank2 = PlayerPrefs.GetString("Level2Rank", "");
            var rankBoss = PlayerPrefs.GetString("BossLevelRank", "");
            if(rank0 == "S" && rank1 == "S" && rank2 == "S" && rankBoss == "S")
            {
                levelCard.SetActive(true);
            } else {
                levelCard.SetActive(false);
            }
        }
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
