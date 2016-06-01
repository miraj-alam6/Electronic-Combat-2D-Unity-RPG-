using UnityEngine;
using System.Collections;
using System.Collections.Generic; //so we can use lists to keep track of enemies
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f; //in seconds

    public float turnDelay = 0.1f; //how long game waits between turns

    public static GameManager instance = null; //public let's us modify from inspector, and static
    //let's us make it that this is a variable for the class rather than an object. 
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]public bool playersTurn = true; //won't see in inspector but is still a public variable

    private Text levelText;
    private GameObject levelImage; //gonna activate and deactivate as need
    private bool doingSetup; //to check if we're setting up the board to prevent player from moving

    private int level = 1; //starting at 3 for testing because that's where enemy appears.
    private List<Enemy> enemies;
    private bool enemiesMoving;

    //part of Unity API, is called every time a scene is loaded
    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();//initgame will manage our UI and manage the stuff
    }
    // Use this for initialization
    //changed start to awake
    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject); //destroy so that we don't have two gamemanagers
        }

        DontDestroyOnLoad(gameObject); //make sure that gamemanager does not automatically get 
        //destroyed when another level is loaded. Everything gets destroyed when a new level is
        //loaded, but having DontDestroyOnLoad makes it such that this thing won't get destroyed
        //we want a common game manager so that the score value can be maintained thruout levels.
        enemies = new List<Enemy>();

        boardScript = GetComponent<BoardManager>();//get the BoardManager script
        InitGame();
    }

    //gonna use BoardManager's setup scene function
    void InitGame() {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");//here we are finding by name, make sure same name in editor
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay); //Use Invoke to wait before invoking a function

        enemies.Clear(); //API function, need to do this when starting a new level
        boardScript.SetupScene(level);
    }

    //hides level image once level actually starts
    private void HideLevelImage() {
        
        levelImage.SetActive(false);
        doingSetup = false; //player can now move
    }

    // Update is called once per frame
    void Update() {
        if (playersTurn || enemiesMoving ||doingSetup) { //added doingSetup here to prevent player moving while board is being set up
            return;
        }
        StartCoroutine(MoveEnemies());
	}

    //This function will be used by enemies to register themselves into the game manager
    public void AddEnemyToList(Enemy script) {
        enemies.Add(script);
    }

    public void GameOver() {
        levelText.text = "Survived for " + level + " days.";
        levelImage.SetActive(true);
        enabled = false; //disables the game manager
    }

    
    //Now a coroutine to move our enemies one at a time in sequence
    //Will be used in update
    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count <= 0) { //this is true for first level
            yield return new WaitForSeconds(turnDelay); //will wait even tho no enemy to wait for
        }
        for (int i = 0; i < enemies.Count; i++) {// move one, then wait, then repeat
            enemies[i].MoveEnemy(); 
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
}
