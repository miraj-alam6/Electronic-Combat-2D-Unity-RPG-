using UnityEngine;
using System.Collections;

//All input for menus will be in Update for this class
public class MenuManager : MonoBehaviour {
    //   public bool inputCoolDown = false;
    //   public int inputCoolDownGoal = 30;
    //   public int inputCoolDownCurrent = 0;

    public ChoiceScreen currentChoiceScreen;
    public ChoiceScreen primaryScreen; // this retains what your actual screen is when the confirm yes or
    //or no screen shows up
    public ChoiceScreen confirmScreen;
    public ChoiceScreen difficultyScreen;
    public ChoiceScreen titleScreen;
    public ChoiceScreen levelDoneScreen;
    public ChoiceScreen inGameMenu;
    public GameObject difficultyScreenObject;
    public GameObject confirmScreenObject;
    public GameObject titleScreenObject;
    public GameObject messageBox;
    public bool showingMessage;
    public float timeLastPresed = -1;
    public int primaryChoice = -1; //this is the actual choice
    public int secondaryChoice = -1; //this is if you pick yes or no
                                     // Use this for initialization

    //The following are for reading a text file.
    public string txtFile = "Save.txt";
    public string textContents;
    void Start() {

        if (GameManager.instance.whichMenu.Equals("level_done")) {
            activateLevelDoneScreen();
        }
    }
    public void loadFile() {

    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            activateConfirmScreen();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activateDiffcultySelectionScreen();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activateTitleScreen();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activateLevelDoneScreen();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activateInGameMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            deactivateAllScreens();
        }
        // if (inputCoolDownCurrent >= inputCoolDownGoal) {
        //     inputCoolDown = false;
        //     inputCoolDownCurrent = 0;
        // }
        //  if (inputCoolDown) {
        //      inputCoolDownCurrent++;
        //      return;
        //  }
        else if (Input.GetButtonDown("Submit"))
        {
            submit();
        }
        if (showingMessage) {
            return;
        }
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            if (timeLastPresed == -1 || (Time.time - timeLastPresed > 0.1)) {
                mainstick(horizontal, vertical);
            }
            timeLastPresed = Time.time;
            //     inputCoolDown = true;

        }
    }

    public void activateConfirmScreen() {
        if (confirmScreen) {
            confirmScreen.gameObject.SetActive(true);
            currentChoiceScreen = confirmScreen;
        }

    }
    public void deactivateConfirmScreen()
    {
        currentChoiceScreen = null;
        confirmScreen.gameObject.SetActive(false);

    }
    public void activateDiffcultySelectionScreen()
    {
        deactivateAllScreens();
        if (difficultyScreen) {
            difficultyScreen.gameObject.SetActive(true);
            currentChoiceScreen = difficultyScreen;
            currentChoiceScreen.setToggledChoice(GameManager.instance.difficultyLevel, false);
            currentChoiceScreen.updateChoiceScreen();
            currentChoiceScreen.resetChoiceMemory(); //this is so that your difficulty doesn't stay highlighted red.
        }

    }
    public void activateTitleScreen()
    {
        deactivateAllScreens();
        if (titleScreen) {
            titleScreen.gameObject.SetActive(true);
            currentChoiceScreen = titleScreen;
        }

    }
    public void activateLevelDoneScreen()
    {
        deactivateAllScreens();
        levelDoneScreen.gameObject.SetActive(true);
        currentChoiceScreen = levelDoneScreen;
    }

    public void activateInGameMenu() {
        deactivateAllScreens();
        inGameMenu.gameObject.SetActive(true);
        currentChoiceScreen = inGameMenu;
    }
    public void deactivateAllScreens() {
        currentChoiceScreen = null;
        if (messageBox) {
            messageBox.SetActive(false);
        }
        if (titleScreen) {
            titleScreen.gameObject.SetActive(false);
        }
        if (levelDoneScreen) {
            levelDoneScreen.gameObject.SetActive(false);
        }
        if (difficultyScreen) {
            difficultyScreen.gameObject.SetActive(false);
        }
        if (confirmScreen) {
            confirmScreen.gameObject.SetActive(false);
        }

    }
    void mainstick(int xDir, int yDir) {
        if (currentChoiceScreen == null)
        {
            return;
        }
        if (yDir != 0) {
            xDir = 0;
        }
        if (yDir == 1) {
            currentChoiceScreen.moveUp();
        }
        if (yDir == -1)
        {
            currentChoiceScreen.moveDown();
        }
    }
    void popUpMessage()
    {
        messageBox.SetActive(true);
        showingMessage = true;

    }
    void submit() {
        if (showingMessage) {
            messageBox.SetActive(false);
            showingMessage = false;
            return;
        }
        if (currentChoiceScreen == null) {
            return;
        }
        int theChoice = currentChoiceScreen.chooseChoice();
        processChoice(theChoice);
       
    }

    //Look at the choice that was picked and based on which choice screen you're on, process the request
    public void processChoice(int choice) {
        if (currentChoiceScreen == confirmScreen)
        {
            if (choice == -1)
            {
               // Debug.Log("No choice picked yet");
                return;

            }
            if (choice == 0)
            {
               // Debug.Log("Kabul HO GAYA");
                confirmPrimaryChoice();
            }
            if (choice == 1)
            {
               // Debug.Log("Denied punk!");
                denyPrimaryChoice();
            }
        }

        else if (currentChoiceScreen == difficultyScreen)
        {
            if (choice == -1)
            {
               // Debug.Log("No choice picked yet");
                return;

            }
            
            if (choice == 0)
            {
             //   Debug.Log("Want it easy, eh?");
                

            }
            if (choice == 1)
            {
              //  Debug.Log("Very uncreative");

            }
            if (choice == 2) {
              //  Debug.Log("You crazy fam?");
            }
            primaryScreen = difficultyScreen;
            primaryChoice = choice;
            activateConfirmScreen();
        }

        else if (currentChoiceScreen == titleScreen)
        {
            if (choice == -1) {
                return;
            }
            else if(choice == 1){
                if (
                    !(PlayerPrefs.HasKey("LevelNumber") && PlayerPrefs.HasKey("HintStatus"))
                    ) {
                    Debug.Log("There is no save file");
                    return;
                }
            }
            primaryScreen = titleScreen;
            primaryChoice = choice;
            activateConfirmScreen();
        }

        else if (currentChoiceScreen == levelDoneScreen)
        {
            if (choice == -1)
            {
                return;
            }
            else if (choice == 2) {
                activateDiffcultySelectionScreen();
                return;
            }

            else if (choice == 3)
            {
                Debug.Log("thee hell");
                if (levelDoneScreen.setToggledChoice(3, true)) {
                    GameManager.instance.hintsOn = true;
                    Debug.Log("Turned on hints");
                }
                else
                {
                    GameManager.instance.hintsOn = false;
                    Debug.Log("Turned off hints");
                    //   Debug.Log("No more hints");
                }
                return;
            }
            primaryScreen = levelDoneScreen;
            primaryChoice = choice;
            activateConfirmScreen();
        }
    }

    //This will be called if you say no to the confirmation message, thus it has to go back to the previous scene
    public void denyPrimaryChoice()
    {

        deactivateConfirmScreen();
        currentChoiceScreen = primaryScreen;
    }

    //this function is called if player confirms yes, and thus it will act according to what
    //the original question before the yes or no was.
    public void confirmPrimaryChoice() {
        if (primaryScreen == difficultyScreen)
        {
            if(primaryChoice == 0)
            {
                Debug.Log("You have confirmed that you want to punk out");
                GameManager.instance.difficultyLevel = 0;
            }
            else if (primaryChoice == 1)
            {
                Debug.Log("You have confirmed your sensibility");
                GameManager.instance.difficultyLevel = 1;
            }
            else if (primaryChoice == 2)
            {
                Debug.Log("You have confirmed that you are a brave fool");
                GameManager.instance.difficultyLevel = 2;
            }
            activateLevelDoneScreen();
        }
        else if (primaryScreen == titleScreen)
        {
            if (primaryChoice == 0)
            {
              //  Debug.Log("A new journey begins");
                GameManager.instance.noMoreMenuOnNextLoad = true;
                GameManager.instance.levelNumber = 1;
                LoadNextLevel();
            }
            //Load game here
            else if (primaryChoice == 1)
            {
               // Debug.Log("You want to continue fam? ");
                CrappyLoadFile();
            }
            else if (primaryChoice == 2)
            {
               // Debug.Log("You will now quit, doesn't work in editor but works in actual game");
                Application.Quit();
            }
        }
        else if (primaryScreen == levelDoneScreen)
        {
            if (primaryChoice == 0)
            {
                Debug.Log("Time to start fighting again.");
                GameManager.instance.noMoreMenuOnNextLoad = true;
                LoadNextLevel();
            }
            else if (primaryChoice == 1)
            {
                Debug.Log("You have now saved the game. Need a confirmation message to pop up.");
                CrappySaveFile();
                popUpMessage();
                deactivateConfirmScreen();
                currentChoiceScreen = primaryScreen;
               
            }
            else if (primaryChoice == 2)
            {
                Debug.Log("You shouldn't be able to reach here");
                
            }

            else if (primaryChoice == 3)
            {
                Debug.Log("You shoudn't be able to reach here");
            }
            else if (primaryChoice == 4)
            {
                Debug.Log("You will now quit, doesn't work in editor but works in actual game");
                GameManager.instance.goBackToTitleScreen();
            }
        }
        //Invoke("deactivateConfirmScreen",0.1f);
    }

    public void LoadNextLevel() {
        //The game will have already went to the next level in Game Manager, so just load
        //whatever the current level is, because it is actually the next level.
        GameManager.instance.LoadLevelByNumber(GameManager.instance.levelNumber);
    }
     public void CrappyLoadFile()
    {
        FileLoader fl = new FileLoader();
        fl.crappyOpenFile();
        GameManager.instance.levelNumber = fl.getLevel();

        if (fl.getHintsOn() == 0)
        {
            GameManager.instance.hintsOn = false;
        }
        else
        {
            GameManager.instance.hintsOn = true;
        }
        GameManager.instance.difficultyLevel = fl.getDifficulty();
        GameManager.instance.noMoreLevelOnNextLoad = true;
        GameManager.instance.whichMenu = "level_done";
        Application.LoadLevel("_Scenes/Menu");
    }
    public void CrappySaveFile()
    {
        FileLoader fl = new FileLoader();
        fl.crappySaveFile();
    }

    public void LoadFile()
    {
        FileLoader fl = new FileLoader();
        fl.openFile();
        GameManager.instance.levelNumber = fl.getLevel();

        if (fl.getHintsOn() == 0)
        {
            GameManager.instance.hintsOn = false;
        }
        else
        {
            GameManager.instance.hintsOn = true;
        }
        GameManager.instance.noMoreLevelOnNextLoad = true;
        GameManager.instance.whichMenu = "level_done";
        Application.LoadLevel("_Scenes/Menu");
    }

    public void SaveFile()
    {
        FileLoader fl = new FileLoader();
        fl.saveFile();
    }
}
