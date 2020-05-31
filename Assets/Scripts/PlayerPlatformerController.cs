using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPlatformerController : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float platformBreakTime = 1;
    public float rechargeRate = 10.0f;
    public float freezeRate = 3.0f;
    public float timeLeft = 40.0f;
    public float warmth = 108.0f;
    public static float maxWarmth = 108.0f;
    public int packagesFound = 0;
    public bool levelOver = false;
    public GameObject victoryScreen;
    public GameObject remindText;
    public GameObject levelTitle;
    public GameObject bossButton;
    public GameObject finalButton;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private TempController timer;
    private GameObject spawnPoint;
    private string[] ranks = new string[] {"S", "A", "B", "C"};
    private List<GameObject> brokenPlatforms = new List<GameObject> {};
    private AudioSource audioSource;
    

    // Start is called before the first frame update
    void Awake()
    {  
        Time.timeScale = 1;
        timer = GameObject.Find("Main Camera").GetComponent<TempController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spawnPoint =  GameObject.Find("SpawnPoint");
        StartCoroutine(TitleDisplay());
        audioSource = GetComponent<AudioSource>();
    }

    // called by Update which is called once per frame by base class
    protected override void ComputeVelocity()
    {
        //CheckGameStatus();
        UpdateMaxSpeed();
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        //change for double jump
        if(Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if(Input.GetButtonUp("Jump"))
        {
          if(velocity.y > 0)
          {
              velocity.y = velocity.y * .5f;
          }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if(flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            iceMove.x = -iceMove.x;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        //Ice stuff
        // if(onIce)
        // {
        //     move.x = previousHorizontalMove.x;
        // }

        targetVelocity = move * maxSpeed;

    }

    void UpdateMaxSpeed()
    {
        //Freeze a bit
        warmth = warmth - freezeRate * Time.deltaTime;
        
        //Change max speed dependent on warmth value
        maxSpeed = 0.07f * warmth;

        if(1.0f >= warmth){
            warmth = 5.0f;
            maxSpeed = 1.0f;
        } 
    }

    void CheckGameStatus()
    {
        if(packagesFound == 3 && !levelOver)
        {
            Time.timeScale = 0;
            levelOver = true;

            // Determine Rank
            var rank = "C";
            var currentRank = PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "Rank", "C");
            if(timer.timePassedInt < (.3 * timer.countDownSeconds))
            {
                rank = "S";
            } else if(timer.timePassedInt < (.4 * timer.countDownSeconds))
            {
                rank = "A";
            } else if(timer.timePassedInt < (.5 * timer.countDownSeconds))
            {
                rank = "B";
            }
            if(Array.IndexOf(ranks, currentRank) >= Array.IndexOf(ranks, rank))
            {
                currentRank = rank;
            }
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "Rank", currentRank);

            // Determine to save time or not
            var currentTime = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "TimeInt", 1000);
            if(timer.timePassedInt < currentTime)
            {
                PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "Time", timer.timePassed);
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TimeInt", timer.timePassedInt);
            }

            // Mark as complete and save
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "Complete", "true");
            PlayerPrefs.Save();

            Debug.Log(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "Time"));

            // PlayerPrefs.DeleteAll();
            victoryScreen.SetActive(true);
            Text rankTimeText = GameObject.Find("RankTimeDisplay").GetComponent<Text>();
            rankTimeText.text = "Rank: " + rank + " Time: " + timer.timePassed;

            var bossComplete = PlayerPrefs.GetString("BossLevelComplete", "false");
            // If all levels completed, prompt for boss level
            if(SceneManager.GetActiveScene().name != "BossLevel" && SceneManager.GetActiveScene().name != "FinalLevel" && !bool.Parse(bossComplete))
            {
                var complete0 = PlayerPrefs.GetString("Level0Complete", "false");
                var complete1 = PlayerPrefs.GetString("Level1Complete", "false");
                var complete2 = PlayerPrefs.GetString("Level2Complete", "false");
                if(bool.Parse(complete0) && bool.Parse(complete1) && bool.Parse(complete2))
                {
                    bossButton.SetActive(true);
                } else {
                    bossButton.SetActive(false);
                }
            } else {
                var rank0 = PlayerPrefs.GetString("Level0Rank", "");
                var rank1 = PlayerPrefs.GetString("Level1Rank", "");
                var rank2 = PlayerPrefs.GetString("Level2Rank", "");
                var rankBoss = PlayerPrefs.GetString("BossLevelRank", "");
                if(rank0 == "S" && rank1 == "S" && rank2 == "S" && rankBoss == "S" && SceneManager.GetActiveScene().name != "FinalLevel")
                {
                    finalButton.SetActive(true);
                } else {
                    finalButton.SetActive(false);
                }
            }

        } else if(packagesFound != 3 && !levelOver) {
            StartCoroutine(PackageReminder());
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
	{
        if(other.tag == "Campfire")
		{
            spawnPoint.transform.position = this.transform.position;
        }
		if(other.tag == "Package")
		{
            audioSource.Play(0);

			other.gameObject.SetActive (false);
            packagesFound += 1;

			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			//Invoke ("Restart", restartLevelDelay);

			//Disable the player object since level is over.
			//enabled = false;
		}
        if(other.tag == "Boundary")
        {
            // SceneManager.LoadScene("Main");
            foreach (GameObject plat in brokenPlatforms)
            {
                plat.SetActive(true);
            }
            this.transform.position = spawnPoint.transform.position;
            this.warmth = 106.0f;    
        }
        if(other.tag ==  "Finish")
        {
            CheckGameStatus();
        }
	}

    private void OnTriggerStay2D (Collider2D other)
	{
		//Check if the tag of the trigger collided with is Campfire.
		if(other.tag == "Campfire")
		{
            if(warmth < maxWarmth)
            {
                //warmth = Mathf.Min(warmth + rechargeRate * Time.deltaTime, 100.0f);
                warmth = warmth + rechargeRate * Time.deltaTime;
		    }
        }
                
	}

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ice")
        {
            this.onIce = true;
        }
        if (collision.gameObject.tag == "Weak")
        {
            StartCoroutine(BreakWeakPlatform(collision));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ice")
        {
            this.onIce = false;
        }
        if (collision.gameObject.tag == "Weak")
        {
            //collision.gameObject.SetActive (false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weak")
        {  
            //StartCoroutine(BreakWeakPlatform(collision));
            
        }     
    }

    IEnumerator BreakWeakPlatform(Collision2D collision)
    {
        Animator platformAnimator = collision.gameObject.GetComponent<Animator>();
        platformAnimator.SetBool("touched", true);
        yield return new WaitForSeconds(platformBreakTime);
        if (collision.gameObject.tag == "Weak")
        {  
            collision.gameObject.SetActive (false);
            brokenPlatforms.Add(collision.gameObject);
        }
    }

    IEnumerator PackageReminder()
    {
        remindText.SetActive(true);
        yield return new WaitForSeconds(3);
        remindText.SetActive(false);
    }

    IEnumerator TitleDisplay()
    {
        levelTitle.SetActive(true);
        yield return new WaitForSeconds(3);
        levelTitle.SetActive(false);
    }
}
