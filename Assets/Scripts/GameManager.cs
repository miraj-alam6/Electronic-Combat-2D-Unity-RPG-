using UnityEngine;
using System.Collections;
using System.Collections.Generic; //so we can use lists to keep track of enemies
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
    public bool levelMusicContinue;
    public AudioClip levels1to3Music;
    public AudioClip levels4to8Music;
    public AudioClip levels9to11Music;
    public AudioClip level12Music;
    public bool gonnaQuitToTitle = false;
    public bool storyMusicPaused = false;
    public bool levelMusicPaused = false;
    public bool inMenuScreen = false;
    public bool noMoreMenuOnNextLoad = false;
    public bool noMoreLevelOnNextLoad = false;
    public InfoUI actualInfo;
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
    public bool hintsOn = true;
    public int currentDeathsForLevel = 0;
    public int levelNumber = 8; //starting at 7 for testing because that's where enemy appears.
    private List<Enemy> enemies;
    public List<Player> players; //list of all the player units
    public List<Unit> units; // list of all units: both players and enemies
    public Unit currentUnit;
    public bool enemiesMoving = false;
    public bool inputHelper;// I use this boolean to let player move again
    //part of Unity API, is called every time a scene is loaded
    public int currentUnitPoints;
    public bool stopAll = false;
    public bool enemyDying = false;
    public bool debuggingBrah = false;
    public bool readyToDestroyPlayerTurn = false;
    public GameCalculation gameCalculation; //this has the code to do A star algorithm
    public Level currentLevel;
    public Data gameData; //not set in inspector
    public GameObject LeftUI; //set in inspector
    public GameObject RightUI;//set in inspector
    public GameObject MiddleUI;
    public GameObject PopUpMenu;
    public InfoUI infoUI;
    public CameraController mainCamera; // set in inspector for each map
    public bool messageBeingShown;
    public int difficultyLevel; //0 is easy, 1 is normal, 2 is hard
    public string whichMenu = "level_done";
    public bool popUpMenuBeingShown = false;
    public FinalBattle finalBattleStuff;
    private void OnLevelWasLoaded(int index)
    {
        //Debug.Log("Kuch kuch");
        if (noMoreLevelOnNextLoad)
        {
            //Debug.Log("Why do we");
            inMenuScreen = true;
            PopUpMenu.SetActive(false);
            LeftUI.SetActive(false);
            RightUI.SetActive(false);
            MiddleUI.SetActive(false);
            noMoreLevelOnNextLoad = false;
            
        }
        else if (noMoreMenuOnNextLoad) {
          //  Debug.Log("Hota Hai");
            resetVitalsUI();
            inMenuScreen = false;
            PopUpMenu.SetActive(false);
            LeftUI.SetActive(true);
            RightUI.SetActive(true);
            noMoreMenuOnNextLoad = false;
            FindCorrectMusic();
        }
        InitGame();//initgame will manage our UI and manage the stuff

    }
    public void FindCorrectMusic() {
        SoundManager.instance.storyMusicSource.Pause();
        storyMusicPaused = true;
        if (levelNumber >= 1 && levelNumber <= 3)
        {
            if (levelMusicPaused)
            {
                SoundManager.instance.musicSource.UnPause();
                levelMusicPaused = false;
            }
            else if (levelMusicContinue)
            {
                return;
            }
            else {
                SoundManager.instance.musicSource.clip = levels1to3Music;
                SoundManager.instance.musicSource.Play();
            }
        }

        if (levelNumber>= 4 && levelNumber <= 8) {
            if (levelMusicPaused)
            {
                SoundManager.instance.musicSource.UnPause();
                levelMusicPaused = false;
            }
            else if (levelMusicContinue)
            {
                return;
            }
            else {
                SoundManager.instance.musicSource.clip = levels4to8Music;
                SoundManager.instance.musicSource.Play();
            }
        }

        if (levelNumber >= 9 && levelNumber <= 11)
        {
            if (levelMusicPaused)
            {
                SoundManager.instance.musicSource.UnPause();
                levelMusicPaused = false;
            }
            else if (levelMusicContinue)
            {
                return;
            }
            else {
                SoundManager.instance.musicSource.clip = levels9to11Music;
                SoundManager.instance.musicSource.Play();
            }
        }
        if (levelNumber >= 12)
        {
            
            if (levelMusicContinue)
            {
                return;
            }
            else {
                SoundManager.instance.musicSource.clip = level12Music;
                SoundManager.instance.musicSource.Play();
            }
        }
    }
    //This is called whenever a player steps onto an Exit thingy
    public void ExitLogic(Player player) {
        
        Debug.Log("Yes here");
        if (currentLevel is TutorialLevel9) {
            if (currentLevel.updateLevel("remove_player"))
            {
                RemovePlayer(player);
            }
            else {
                showMessage("You have not collected enough chicken flowers to end the level yet.");
            }
        }
        if (currentLevel is TutorialLevel10)
        {
            if (currentLevel.updateLevel("remove_player"))
            {
                RemovePlayer(player);
            }
            else {
                showMessage("You have not collected enough battery boxes to end the level yet");
            }
        }
        if (currentLevel is TutorialLevel11)
        {
            Debug.Log("Why not here");
            if (currentLevel.updateLevel("remove_player"))
            {
                RemovePlayer(player);
            }
           
        }
        if (currentLevel is TutorialLevel12)
        {
            if (currentLevel.updateLevel("remove_player"))
            {
                RemovePlayer(player);
            }
            else {
                showMessage("You cannot escape until you defeat Roxanne");
            }
        }
    }
    public void RemovePlayer(Player player) {
        units.Remove(player);
        players.Remove(player);
        switch (player.name) {
            case "Kali":
                currentLevel.kaliHere = false;
            break;
            case "Winoa":
                currentLevel.winoaHere = false;
                break;
            case "Hugo":
                currentLevel.hugoHere = false;
                break;
            case "Alejandra":
                currentLevel.alejandraHere = false;
                break;
        }

        currentUnit = null;
        playersTurn = false;
        singlePlayerMove = false;
        GameManager.instance.gameCalculation.actualGrid[player.y, player.x].hasPlayer = false;
        GameManager.instance.gameCalculation.actualGrid[player.y, player.x].walkable = true;
        player.gameObject.SetActive(false);
    }
    //use this in future when you move away the stuff from OnLevelWasLoaded cause apprantly that's
    //a problem in mobile device
    public void RestartStuff() {
        //level++;
        InitGame();//initgame will manage our UI and manage the stuff
    }
    // Use this for initialization
    //changed start to awake
    void Awake() {
        PopUpMenu.SetActive(false);
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


    public void UpdateCamera() {
        if (mainCamera) {
            if (currentUnit) { //forgot about this check
                if (currentUnit is Player && ((Player)currentUnit).shooting)
                {
                    GridSelector gridSelector =
                    GameObject.FindGameObjectWithTag("GridSelector").GetComponent<GridSelector>();
                    mainCamera.Move(gridSelector.x, gridSelector.y);
                }
                else
                {  
                    //Debug.Log("Reached eventually");
                    //Debug.Log(currentUnit.x +"  "+currentUnit.y);
                    mainCamera.Move(currentUnit.x, currentUnit.y);
                }
            }
        }
    }

    public void showMessage(string message) {
        if (noMoreLevelOnNextLoad || inMenuScreen) {
            return;
        }
        stopAll = true;
        messageBeingShown = true;
        MiddleUI.SetActive(true);
        MiddleUI.GetComponent<MessageUI>().SetMessage(message);
    }

    //side effect of this function is that stopAll will become false, may not always intend this.
    public void hideMessage() {
        
        stopAll = false;
        messageBeingShown = false;
        MiddleUI.SetActive(false);
        if (gonnaQuitToTitle)
        {
            goBackToTitleScreen();
        }
    }
    //gonna use BoardManager's setup scene function
    void InitGame() {
        if (inMenuScreen) {
            MiddleUI.SetActive(false);
            LeftUI.SetActive(false);
            RightUI.SetActive(false);
            return;
        }
        LeftUI.GetComponent<VitalsUI>().refreshUI();
        MiddleUI.SetActive(false);
        actualInfo = GameManager.instance.RightUI.GetComponent<InfoUI>();
        enemies = new List<Enemy>();
        players = new List<Player>();
        units = new List<Unit>();
        playersTurn = false;
        singlePlayerMove = false;
        infoUI = RightUI.GetComponent<InfoUI>();
        doingSetup = true;
        if (Application.loadedLevelName.Equals("_Scenes/Tutorial1") || Application.loadedLevelName.Equals("Tutorial1"))
        {
            levelNumber = 1;
            currentLevel = new TutorialLevel1(currentDeathsForLevel);
            //Debug.Log(currentLevel);
            infoUI.SetActualObjective("Break all\npots.");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial2") || Application.loadedLevelName.Equals("Tutorial2"))
        {
            levelNumber = 2;
            currentLevel = new TutorialLevel2(currentDeathsForLevel);
            //Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat the\nenemy");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial3") || Application.loadedLevelName.Equals("Tutorial3"))
        {
            levelNumber = 3;
            currentLevel = new TutorialLevel3(currentDeathsForLevel);
            //Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat both\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial4") || Application.loadedLevelName.Equals("Tutorial4"))
        {
            levelNumber = 4;
            currentLevel = new TutorialLevel4(currentDeathsForLevel);
            //Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial5") || Application.loadedLevelName.Equals("Tutorial5"))
        {
            levelNumber = 5;
            currentLevel = new TutorialLevel5(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial6") || Application.loadedLevelName.Equals("Tutorial6"))
        {
            levelNumber = 6;
            currentLevel = new TutorialLevel6(currentDeathsForLevel);
            Debug.Log(currentLevel);
            Debug.Log(infoUI);
            infoUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial7") || Application.loadedLevelName.Equals("Tutorial7"))
        {
            levelNumber = 7;
            currentLevel = new TutorialLevel7(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat all\nenemies");
        }

        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial8") || Application.loadedLevelName.Equals("Tutorial8"))
        {
            levelNumber = 8;
            currentLevel = new TutorialLevel8(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat all\nenemies");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial9") || Application.loadedLevelName.Equals("Tutorial9"))
        {
            levelNumber = 9;
            levelMusicContinue = true;
           
            currentLevel = new TutorialLevel9(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Collect "+ ((TutorialLevel9)currentLevel).goalChickens+ "\nChicken Flowers,\nand escape.");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial10") || Application.loadedLevelName.Equals("Tutorial10"))
        {
            levelNumber = 10;
            levelMusicContinue = true;
            currentLevel = new TutorialLevel10(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Collect " + ((TutorialLevel10)currentLevel).goalElectric + "\nBatter Boxes,\nand escape.");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial11") || Application.loadedLevelName.Equals("Tutorial11"))
        {
            levelNumber = 11;
            levelMusicContinue = true;
            currentLevel = new TutorialLevel11(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Escape the stage.");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/Tutorial12") || Application.loadedLevelName.Equals("Tutorial12"))
        {
            levelNumber = 12;
            levelMusicContinue = true;
            currentLevel = new TutorialLevel12(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("Defeat Roxanne\nand\nescape the stage.");
        }
        else if (Application.loadedLevelName.Equals("_Scenes/HasBugOnlyKeepAlejandra") || Application.loadedLevelName.Equals("HasBugOnlyKeepAlejandra"))
        {
            levelNumber = 11;
            levelMusicContinue = true;
            currentLevel = new TutorialLevel11(currentDeathsForLevel);
            Debug.Log(currentLevel);
            infoUI.SetActualObjective("tHIS WILL HAVE A BUG");
            Debug.Log("Here here dsklajdla ");
        }

        if (currentLevel !=  null) { 
        currentLevel.retainHintsSetting(hintsOn);
        }
        // Debug.Log("Do we ever Init again?");
        // Debug.Log(Application.loadedLevelName);


        //NOTE: comment/uncomment this if you don't want/want level image again
        levelImage = GameObject.Find("LevelImage");//here we are finding by name, make sure same name in editor
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Mission " + levelNumber + "\n" +GetLevelName();
        levelImage.SetActive(true);
        if (currentDeathsForLevel > 0) {
            levelImage.GetComponent<Image>().color = Color.black;
        }

        if (currentDeathsForLevel == 0)
        {
            Invoke("HideLevelImage", 2 * levelStartDelay);
        }
        else { 
            Invoke("HideLevelImage", levelStartDelay); //Use Invoke to wait before invoking a function
        }
        //making stopAll = false here doesn't have the effect of the delay, it does it too fast,
        //so instead i do it in HideLevelImage, because it will be delayed
        //        stopAll = false;
        enemies.Clear(); //API function, need to do this when starting a new level
        boardScript.SetupScene(levelNumber);
        //Without the next line game doesn't let you move anymore and is frozen if you end the
        //level with your last move
        // playersTurn = true;
        // singlePlayerMove = true;
    }

    public string GetLevelName() {
        string s = "";
        switch (levelNumber) {
            case 1:
                s = "Breaker";
                break;
            case 2:
                s = "First Battle";
                break;
            case 3:
                s = "Against The Odds";
                break;
            case 4:
                s = "Impending Fury";
                break;
            case 5:
                s = "Vicious Babies";
                break;
            case 6:
                s = "The Fixer";
                break;
            case 7:
                s = "Intelligent Power";
                break;
            case 8:
                s = "Wall of Steel";
                break;
            case 9:
                s = "Get Chicken";
                break;
            case 10:
                s = "Too Crowded";
                break;

            case 11:
                s = "Path Creator";
                break;
            case 12:
                s = "True Combat";
                break;
        }
        return s;
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
        stuffAfterLoading();
    }

    public void stuffAfterLoading() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        //Debug.Log("F do you know about my pain");
        //Debug.Log(GameObject.FindGameObjectWithTag("MainCamera"));
        //Debug.Log(mainCamera);
        UpdateCamera();

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
        if (currentLevel is TutorialLevel6) {
            script.weInTutorialLevel6 = true;
        }
        players.Add(script); // put it into the list of players
        units.Add(script); // as well as all the units that are in the game
    }

    public void GameOver() {
        levelText.text = "Survived for " + levelNumber + " days.";
        levelImage.SetActive(true);
        enabled = false; //disables the game manager
                         //Invoke("RestartLevel",1);
                         // GameManager.instance.currentLevel.updateLevel("restart");

    }
    public void toggleHints() {
        
        if (hintsOn)
        {
            hintsOn = false;
            currentLevel.hintsOn = false;
            showMessage("Hints have been turned OFF");
        }
        else
        {

            hintsOn = true;
            currentLevel.hintsOn = true;
            showMessage("Hints have been turned ON");

        }
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
        if (doingSetup || stopAll || noMoreLevelOnNextLoad || inMenuScreen) {
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
                addATurn();      
                currentUnit = units[i];
                UpdateCamera();
                currentUnit.flashWhite(0.1f);
               // currentUnit.ATB = 100; //moved this to ApplySpeed
                if (currentUnit.isPlayer)
                {
                    RightUI.GetComponent<InfoUI>().SetTurnMessage("Player Turn");
                    playersTurn = true;
                    // singlePlayerMove = true; //might need this
                    ((Player)currentUnit).isActivePlayer = true;
                    ((Player)currentUnit).movePoints = ((Player)currentUnit).maxMovePoints;
                    ((Player)currentUnit).updateText();
                }
                else {
                    RightUI.GetComponent<InfoUI>().SetTurnMessage("Enemy Turn");
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

    public void addATurn() {
        currentLevel.incrementTurnCount();
    }
    public void playerDone() {

    }

    public void GonnaRestartLevel()
    {
        noMoreMenuOnNextLoad = true;
        currentDeathsForLevel++;
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, 0);
        Invoke("RestartLevel", levelDoneDelay);
    }
    public void resetVitalsUI() {
        Debug.Log("This happens");
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().KaliSetSpecial(25);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateKaliATB(100, 0);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().winoaSetSpecial(25);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateWinoaHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateWinoaATB(100, 0);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().hugoSetSpecial(25);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateHugoHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateHugoATB(100, 0);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().alejandraSetSpecial(25);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateAlejandraHP(1, 1);
        GameManager.instance.LeftUI.GetComponent<VitalsUI>().UpdateAlejandraATB(100, 0);
    }

    public void DoneWithLevel() {
        if (levelNumber == 1 || levelNumber == 2 || (levelNumber >= 4 && levelNumber <= 8)
            || (levelNumber >= 9 && levelNumber <=10))
        {
            levelMusicContinue = true;
        }

        else {
            levelMusicContinue = false;
        }
        currentDeathsForLevel = 0;
        resetVitalsUI();
        
        
//        Invoke("LoadNextLevel", levelDoneDelay);
        Invoke("GoToEndLevelScreen",  levelDoneDelay);
    }
    public void GoToEndLevelScreen() {
        if (levelNumber == 3)
        {
            SoundManager.instance.musicSource.Stop();

        }
        if (levelNumber == 8)
        {
            SoundManager.instance.musicSource.Stop();
        }
       
        levelNumber++;
        noMoreLevelOnNextLoad = true;
        whichMenu = "level_done";
        if (levelNumber == 12)
        {
            SoundManager.instance.musicSource.Stop(); 
        }
        if (levelNumber == 13)
        {
            PlayerPrefs.SetInt("BeatGame",1);
            SoundManager.instance.musicSource.Stop();
            Application.LoadLevel("_Scenes/MenuEnding");
            return;
        }
        else { 
            Application.LoadLevel("_Scenes/Menu");
        }
    }
    public void RestartLevel()
    {
        GameManager.instance.levelMusicContinue = true;
        noMoreMenuOnNextLoad = true;
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
        else if (currentLevel is TutorialLevel8)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial8");
        }
        else if (currentLevel is TutorialLevel9)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial9");
        }
        else if (currentLevel is TutorialLevel10)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial10");
        }
        else if (currentLevel is TutorialLevel11)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial11");
        }
        else if (currentLevel is TutorialLevel12)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial12");
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
            Application.LoadLevel("_Scenes/Tutorial8");
        }
        else if (currentLevel is TutorialLevel8)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial9");
        }
        else if (currentLevel is TutorialLevel9)
        {
            //Both this line and next do same thing: Application.LoadLevel("Tutorial2");
            Application.LoadLevel("_Scenes/Tutorial9");
        }
    }
    public void setLevelNumber(int n) {
        levelNumber = n;
    }
    public void levelNumberIncrement()
    {
        levelNumber++;
    }

    public void LoadLevelByNumber(int lvlNumber) {
        noMoreMenuOnNextLoad = true;
        switch (lvlNumber) {
            case 1:
                Application.LoadLevel("_Scenes/Tutorial1");
                break;
            case 2:
                Application.LoadLevel("_Scenes/Tutorial2");
                break;
            case 3:
                Application.LoadLevel("_Scenes/Tutorial3");
                break;
            case 4:
                Application.LoadLevel("_Scenes/Tutorial4");
                break;
            case 5:
                Application.LoadLevel("_Scenes/Tutorial5");
                break;
            case 6:
                Application.LoadLevel("_Scenes/Tutorial6");
                break;
            case 7:
                Application.LoadLevel("_Scenes/Tutorial7");
                break;
            case 8:
                Application.LoadLevel("_Scenes/Tutorial8");
                break;
            case 9:
                Application.LoadLevel("_Scenes/Tutorial9");
                break;
            case 10:
                Application.LoadLevel("_Scenes/Tutorial10");
                break;
            case 11:
                Application.LoadLevel("_Scenes/Tutorial11");
                break;
            case 12:
                Application.LoadLevel("_Scenes/Tutorial12");
                break;
            case 13:
                Application.LoadLevel("_Scenes/Story");
                noMoreMenuOnNextLoad = false;
                noMoreLevelOnNextLoad = true;
                break;
            case 14:
                Application.LoadLevel("_Scenes/Credits");
                noMoreMenuOnNextLoad = false;
                noMoreLevelOnNextLoad = true;
                break;
            case 15:
                Application.LoadLevel("_Scenes/Story");
                noMoreMenuOnNextLoad = false;
                noMoreLevelOnNextLoad = true;
                break;
            case 16:
                Application.LoadLevel("_Scenes/ToBeContinued");
                noMoreMenuOnNextLoad = false;
                noMoreLevelOnNextLoad = true;
                break;
        }
    }
    public void RefreshMessage() {
        if (currentLevel is TutorialLevel1)
        {
            infoUI.SetMessage("This space\nain't used\nuntil mission 6");
        }
        else if (currentLevel is TutorialLevel2)
        {
            infoUI.SetMessage("Don't Lose");
        }
        else if (currentLevel is TutorialLevel3)
        {
            infoUI.SetMessage("Obama is\na lizard");
        }
        else if (currentLevel is TutorialLevel4)
        {
            infoUI.SetMessage("Hilary is\na snake");
        }
        else if (currentLevel is TutorialLevel5)
        {
            infoUI.SetMessage("Remember how\nto shoot");
        }
        else if (currentLevel is TutorialLevel6)
        {
            infoUI.SetMessage("Winoa can\n heal.\nPress z with\n 75 special\n and then run\ninto another\na unit to\nheal him.");
        }
        else if (currentLevel is TutorialLevel7)
        {
            infoUI.SetMessage("Intel can\n help a great\ndeal.");
        }
        else if (currentLevel is TutorialLevel8)
        {
            infoUI.SetMessage("Be brave\nlike Caitlyn.");
        }
        else if (currentLevel is TutorialLevel9)
        {
            infoUI.SetMessage("You have\n"+ ((TutorialLevel9)currentLevel).chickens+ "\nchicken flowers");
        }
        else if (currentLevel is TutorialLevel10)
        {
            infoUI.SetMessage("You have\n" + ((TutorialLevel10)currentLevel).currentElectric + "\nbattery boxes");
        }
        else if (currentLevel is TutorialLevel11)
        {
            infoUI.SetMessage("Be brave" + "\nlike Bruce");
        }
        else if (currentLevel is TutorialLevel12)
        {
            infoUI.SetMessage("Prepare" + "\nyour anus.");
        }
    }


    public void goBackToTitleScreen()
    {
        noMoreLevelOnNextLoad = true;
        hintsOn = true;
        difficultyLevel = 1;
        levelMusicContinue = false;
        levelMusicPaused = false;
        storyMusicPaused = false;
        GameManager.instance.whichMenu = "title";
        if (SoundManager.instance) { 
        Destroy(SoundManager.instance.gameObject);
        }
        Application.LoadLevel("_Scenes/TitleScreen");


    }
}
