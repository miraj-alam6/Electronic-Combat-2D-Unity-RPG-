using UnityEngine;
using System.Collections;
using System.Collections.Generic; //so we can use lists to keep track of enemies
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public float levelStartDelay = 2f; //in seconds
    public float levelDoneDelay = 1f;
    public float turnDelay = 0.1f; //how long game waits between turns
    public float ATBDelay = 2.0f; //how long game waits between turns
    public bool hideVitals = false;
    public static GameManager instance = null; //public let's us modify from inspector, and static
    //let's us make it that this is a variable for the class rather than an object. 
    public BoardManager boardScript;
    // public int playerFoodPoints = 20; no more scavenger game, time to let go of the arkham origins
    public bool singlePlayerMove = false; //won't see in inspector but is still a public variable
    public bool playersTurn = false;
    private Text levelText;
    private GameObject levelImage; //gonna activate and deactivate as need
    public bool doingSetup; //to check if we're setting up the board to prevent player from moving

    public int level = 8; //starting at 7 for testing because that's where enemy appears.
    private List<Enemy> enemies;
    public List<Player> players; //list of all the player units
    public List<Unit> units; // list of all units: both players and enemies
    public Unit currentUnit;
    public bool enemiesMoving = false;
    public bool inputHelper;// I use this boolean to let player move again
    //part of Unity API, is called every time a scene is loaded
    public int currentUnitPoints;
    public bool stopAll = false;
    public bool debuggingBrah = false;
    public bool readyToDestroyPlayerTurn = false;
    public GameCalculation gameCalculation; //this has the code to do A star algorithm
    public Level currentLevel;
    public Data gameData; //not set in inspector
    public GameObject LeftUI; //set in inspector
    public GameObject RightUI;//set in inspector
    public MessageUI messageUI;
    private void OnLevelWasLoaded(int index)
    {

        InitGame();//initgame will manage our UI and manage the stuff

    }
    //use this in future when you move away the stuff from OnLevelWasLoaded cause apprantly that's
    //a problem in mobile device
    public void RestartStuff() {
        level++;
        InitGame();//initgame will manage our UI and manage the stuff
    }
    // Use this for initialization
    //changed start to awake
    void Awake() {
        gameData = new Data();
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
        players = new List<Player>();
        units = new List<Unit>();

        boardScript = GetComponent<BoardManager>();//get the BoardManager script
        //get the gamecalculation script
        gameCalculation = GetComponent<GameCalculation>();
        boardScript.gameCalculation = gameCalculation;
        InitGame();
    }

    //gonna use BoardManager's setup scene function
    void InitGame() {
        enemies = new List<Enemy>();
        players = new List<Player>();
        units = new List<Unit>();
        playersTurn = false;
        singlePlayerMove = false;
        messageUI = RightUI.GetComponent<MessageUI>();
        doingSetup = true;
        if (Application.loadedLevelName.Equals("_Scenes/Tutorial1") || Application.loadedLevelName.Equals("Tutorial1"))
        {
            currentLevel = new TutorialLevel1();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Break all\npots.");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial2") || Application.loadedLevelName.Equals("Tutorial2"))
        {
            currentLevel = new TutorialLevel2();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Defeat the\nenemy");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial3") || Application.loadedLevelName.Equals("Tutorial3"))
        {
            currentLevel = new TutorialLevel3();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Defeat both\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial4") || Application.loadedLevelName.Equals("Tutorial4"))
        {
            currentLevel = new TutorialLevel4();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial5") || Application.loadedLevelName.Equals("Tutorial5"))
        {
            currentLevel = new TutorialLevel5();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial6") || Application.loadedLevelName.Equals("Tutorial6"))
        {
            currentLevel = new TutorialLevel6();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial7") || Application.loadedLevelName.Equals("Tutorial7"))
        {
            currentLevel = new TutorialLevel7();
            Debug.Log(currentLevel);
            messageUI.SetActualObjective("Defeat all\nenemies");
        }
        Debug.Log("Do we ever Init again?");
        Debug.Log(Application.loadedLevelName);


        //NOTE: comment/uncomment this if you don't want/want level image again
        levelImage = GameObject.Find("LevelImage");//here we are finding by name, make sure same name in editor
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);

        Invoke("HideLevelImage", levelStartDelay); //Use Invoke to wait before invoking a function
        //making stopAll = false here doesn't have the effect of the delay, it does it too fast,
        //so instead i do it in HideLevelImage, because it will be delayed
        //        stopAll = false;
        enemies.Clear(); //API function, need to do this when starting a new level
        boardScript.SetupScene(level);
        //Without the next line game doesn't let you move anymore and is frozen if you end the
        //level with your last move
        // playersTurn = true;
        // singlePlayerMove = true;
    }


    //This actually begins the level, DO NOT take it out, it is a good point to realizing that
    //all things have been loaded.
    //hides level image once level actually starts
    private void HideLevelImage() {

        //NOTE: comment/uncomment this if you don't want/want level image again
        levelImage.SetActive(false);
        doingSetup = false; //player can now move
        stopAll = false;
        inputHelper = false;

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Display")) {

            if (hideVitals)
            {


                for (int i = 0; i < units.Count; i++) {
                    // #MIRAJ #IMPORTANT #NOTE : The only child of Player that has 
                    units[i].VitalsWrapper.SetActive(true);
                }
                hideVitals = false;
            }
            else {

                for (int i = 0; i < units.Count; i++)
                {
                    // #MIRAJ #IMPORTANT #NOTE : The only child of Player that has 
                    units[i].VitalsWrapper.SetActive(false);
                }
                hideVitals = true;
            }

        }
        if (currentUnit == null) {
            ApplySpeed();
        }
        if (singlePlayerMove || currentUnit == null || inputHelper || enemiesMoving || doingSetup || stopAll) { //added doingSetup here to prevent player moving while board is being set up
            return;
        }

        StartCoroutine(MoveUnits());


    }

    //This function will be used by enemies to register themselves into the game manager
    public void AddEnemyToList(Enemy script) {
        enemies.Add(script); // put it into the list of enemies
        units.Add(script); //as well as all the units that are in the game
    }

    public void RemoveEnemyFromList(Enemy script)
    {
        enemies.Remove(script); // remove from list of enemies
        units.Remove(script); //remove as well from all the units that are in the game
    }

    //This function will be used by enemies to register themselves into the game manager
    public void AddPlayerToList(Player script)
    {
        players.Add(script); // put it into the list of players
        units.Add(script); // as well as all the units that are in the game
    }

    public void GameOver() {
        levelText.text = "Survived for " + level + " days.";
        levelImage.SetActive(true);
        enabled = false; //disables the game manager
                         //Invoke("RestartLevel",1);
                         // GameManager.instance.currentLevel.updateLevel("restart");

    }


    //Now a coroutine to move our the current active unit
    //Will be used in update
    public IEnumerator MoveUnits() {
        debuggingBrah = true;

        inputHelper = true;
        yield return new WaitForSeconds(turnDelay);
        if (readyToDestroyPlayerTurn) {


            currentUnit.ATB = 0;
            currentUnit = null; // REMINDER: this where player unit becaomes null
            playersTurn = false;
            singlePlayerMove = false;
            readyToDestroyPlayerTurn = false;
            inputHelper = false;

            yield break;
        }
        if (playersTurn)
        {
            yield return new WaitForSeconds(turnDelay);
            yield return new WaitForSeconds(turnDelay);
            singlePlayerMove = true;
            inputHelper = false;
            if (currentUnit.movePoints == -200 || currentUnit.ATB == -200)
            {
                yield return new WaitForSeconds(turnDelay); //one more delay between turns so triggers don't get messed up by player input
            }
            yield break;

        }

        //if you reached this point, it means its the enemy's turn
        if (enemies.Count <= 0) { //this is true for first level
            //enemyMoves = 0;
            yield return new WaitForSeconds(turnDelay); //will wait even tho no enemy to wait for
        }

        if (doingSetup) {
            yield break;
        }

        while (currentUnit.ATB > 0) { //Removed checking currentMovePoints > 0
            if (doingSetup || stopAll) {
                yield break;
            }
            ((Enemy)currentUnit).MoveEnemy();
            yield return new WaitForSeconds(currentUnit.moveTime);
            yield return new WaitForSeconds(turnDelay);
        }
        //reset movepoints so that it can move again later
        currentUnit.ATB = 0;
        currentUnit.movePoints = currentUnit.maxMovePoints;
        // else {
        //       enemiesMoving = true;
        // }
        //do this only if enemy is done moving
        //  enemyMoves--;
        // if (enemyMoves <= 0)
        //  {
        //     singlePlayerMove = true;
        // }

        inputHelper = false;
        currentUnit = null;
    }
    public void ApplySpeed()
    {
        if (doingSetup || stopAll) {
            return;
        }
        //First check if there is already a currentunit, if so don't apply speed
        if (currentUnit != null)
        {
            if (!currentUnit.isPlayer) {
                playersTurn = false;
                singlePlayerMove = false;
            }
            return;
        }

        //Then check if anyone is at a 100, if so do not apply any speed until that unit's
        //move is done

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].ATB >= 100 /*|| (units[i].isPlayer &&((Player)units[i]).isActivePlayer )*/) {
                //if it's someone turn now, just wait before making him active to escape any user
                //input glitch
              
                currentUnit = units[i];
                currentUnit.flashWhite(0.1f);
               // currentUnit.ATB = 100; //moved this to ApplySpeed
                if (currentUnit.isPlayer)
                {
                    RightUI.GetComponent<MessageUI>().SetTurnMessage("Player Turn");
                    playersTurn = true;
                    // singlePlayerMove = true; //might need this
                    ((Player)currentUnit).isActivePlayer = true;
                    ((Player)currentUnit).movePoints = ((Player)currentUnit).maxMovePoints;
                    ((Player)currentUnit).updateText();
                }
                else {
                    RightUI.GetComponent<MessageUI>().SetTurnMessage("Enemy Turn");
                    ((Enemy)currentUnit).Think();
                }
                return;
            }
        }
        for (int i = 0; i < units.Count; i++) {
            if (!units[i].dead)
            {
                units[i].applySpeed();
            }
        }
        
    }

    public void playerDone() {

    }

    public void GonnaRestartLevel()
    {
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, 0);
        Invoke("RestartLevel", levelDoneDelay);
    }

    public void DoneWithLevel() {
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, 0);
        level++;
        Invoke("LoadNextLevel", levelDoneDelay);
    }

    public void RestartLevel()
    {
       
        if (currentLevel is TutorialLevel1)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial1");
        }
        else if (currentLevel is TutorialLevel2)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial2");
        }
        else if (currentLevel is TutorialLevel3)
        {
            //Destroying the SoundManager.instance.gameObject is how you make song stop repeating
            //and actually use the soundmanager that is in the level

            //Destroy(SoundManager.instance.gameObject);
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial3");
        }
        else if (currentLevel is TutorialLevel4)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial4");
        }
        else if (currentLevel is TutorialLevel5)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial5");
        }
        else if (currentLevel is TutorialLevel6)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial6");
        }
        else if (currentLevel is TutorialLevel7)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial7");
        }
    }


    //Once the level is complete go to the next level
    public void LoadNextLevel()
    {
        
        if (currentLevel is TutorialLevel1) {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial2");
        }
        else if (currentLevel is TutorialLevel2)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial3");
        }
        else if (currentLevel is TutorialLevel3)
        {
            //Destroying the SoundManager.instance.gameObject is how you make song stop repeating
            //and actually use the soundmanager that is in the level
            Destroy(SoundManager.instance.gameObject);
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial4");
        }
        else if (currentLevel is TutorialLevel4)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial5");
        }
        else if (currentLevel is TutorialLevel5)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial6");
        }
        else if (currentLevel is TutorialLevel6)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial7");
        }
        else if (currentLevel is TutorialLevel7)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial7");
        }
    }
}
