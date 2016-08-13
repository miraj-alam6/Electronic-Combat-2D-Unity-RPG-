using UnityEngine;
using System.Collections;

//All input for menus will be in Update for this class
public class InGameMenu : MonoBehaviour
{
    //   public bool inputCoolDown = false;
    //   public int inputCoolDownGoal = 30;
    //   public int inputCoolDownCurrent = 0;

    public ChoiceScreen currentChoiceScreen;
    public ChoiceScreen primaryScreen; // this retains what your actual screen is when the confirm yes or
    //or no screen shows up
    public ChoiceScreen confirmScreen;
    public ChoiceScreen inGameMenu;
    public float timeLastPresed = -1;
    public int primaryChoice = -1; //this is the actual choice
    public int secondaryChoice = -1; //this is if you pick yes or no
                                     // Use this for initialization

    //The following are for reading a text file.
    public string txtFile = "Save.txt";
    public string textContents;
    void Start()
    {
        currentChoiceScreen = inGameMenu;

    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.messageBeingShown) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
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
        
        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            if (timeLastPresed == -1 || (Time.time - timeLastPresed > 0.1))
            {
                mainstick(horizontal, vertical);
            }
            timeLastPresed = Time.time;
            //     inputCoolDown = true;

        }
    }

    public void activateConfirmScreen()
    {
        if (confirmScreen)
        {
            confirmScreen.gameObject.SetActive(true);
            currentChoiceScreen = confirmScreen;
        }

    }
    public void deactivateConfirmScreen()
    {
        currentChoiceScreen = null;
        confirmScreen.gameObject.SetActive(false);

    }

    
    public void activateInGameMenu()
    {
        deactivateAllScreens();
        inGameMenu.gameObject.SetActive(true);
        currentChoiceScreen = inGameMenu;
    }
    public void deactivateAllScreens()
    {
        currentChoiceScreen = null;
        if (confirmScreen)
        {
            confirmScreen.gameObject.SetActive(false);
        }
        if (inGameMenu) {
            inGameMenu.gameObject.SetActive(false);
        }

    }
    void mainstick(int xDir, int yDir)
    {
        if (currentChoiceScreen == null)
        {
            return;
        }
        if (yDir != 0)
        {
            xDir = 0;
        }
        if (yDir == 1)
        {
            currentChoiceScreen.moveUp();
        }
        if (yDir == -1)
        {
            currentChoiceScreen.moveDown();
        }
    }

    void submit()
    {
        if (currentChoiceScreen == null)
        {
            return;
        }
        int theChoice = currentChoiceScreen.chooseChoice();
        processChoice(theChoice);

    }

    //Look at the choice that was picked and based on which choice screen you're on, process the request
    public void processChoice(int choice)
    {
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

        else if (currentChoiceScreen == inGameMenu)
        {
            if (choice == -1)
            {
                // Debug.Log("No choice picked yet");
                return;

            }

            if (choice == 0)
            {
                Debug.Log("This will immediately resume game again.");
                GameManager.instance.popUpMenuBeingShown = false;
                if (GameManager.instance.currentUnit is Player) {
                    ((Player)GameManager.instance.currentUnit).popUpMenuBeingShown = false;
                }
                inGameMenu.currentIndex = -1;
                inGameMenu.updateChoiceScreen();
                gameObject.SetActive(false);

            }
            else if (choice == 1)
            {
                //Debug.Log("This will toggle hints");
                GameManager.instance.popUpMenuBeingShown = false;
                if (GameManager.instance.currentUnit is Player)
                {
                    ((Player)GameManager.instance.currentUnit).popUpMenuBeingShown = false;
                }
                inGameMenu.setToggledChoice(1,true);
                inGameMenu.currentIndex = -1;
                inGameMenu.updateChoiceScreen();
                GameManager.instance.toggleHints();
                gameObject.SetActive(false);
            }
            else if (choice == 2)
            {
                primaryScreen = inGameMenu;
                primaryChoice = choice;
                activateConfirmScreen();
            }
            else if (choice == 3)
            {
                primaryScreen = inGameMenu;
                primaryChoice = choice;
                activateConfirmScreen();
            }


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
    public void confirmPrimaryChoice()
    {
        if (primaryScreen == inGameMenu)
        {
            if (primaryChoice == 0)
            {
                Debug.Log("Shouldn't reach here");
            }
            else if (primaryChoice == 1)
            {

                Debug.Log("Shouldn't reach here too");

            }
            else if (primaryChoice == 2)
            {
                Debug.Log("Confirmed you want to restart level");
                //deactivateAllScreens();
                deactivateConfirmScreen();
                currentChoiceScreen = inGameMenu;
                inGameMenu.currentIndex = -1;
                inGameMenu.updateChoiceScreen();
                GameManager.instance.popUpMenuBeingShown = false;
                if (GameManager.instance.currentUnit is Player)
                {
                    ((Player)GameManager.instance.currentUnit).popUpMenuBeingShown = false;
                }
                GameManager.instance.LoadLevelByNumber(GameManager.instance.levelNumber);
                gameObject.SetActive(false);
                // GameManager.instance.toggleHints();
                //gameObject.SetActive(false);
            }
            else if (primaryChoice == 3)
            {
                if (GameManager.instance.levelNumber == 1) {
                    deactivateConfirmScreen();
                    currentChoiceScreen = inGameMenu;
                    inGameMenu.currentIndex = -1;
                    inGameMenu.updateChoiceScreen();
                    GameManager.instance.popUpMenuBeingShown = false;
                    if (GameManager.instance.currentUnit is Player)
                    {
                        ((Player)GameManager.instance.currentUnit).popUpMenuBeingShown = false;
                    }
                    gameObject.SetActive(false);
                    GameManager.instance.showMessage("You cannot quit the first level punk.");
                }
                else { 
                    Debug.Log("Confirmed you want to quit level");
                    deactivateConfirmScreen();
                    currentChoiceScreen = inGameMenu;
                    inGameMenu.currentIndex = -1;
                    inGameMenu.updateChoiceScreen();
                    GameManager.instance.popUpMenuBeingShown = false;
                    if (GameManager.instance.currentUnit is Player)
                    {
                        ((Player)GameManager.instance.currentUnit).popUpMenuBeingShown = false;
                    }
                   
                    GameManager.instance.noMoreLevelOnNextLoad = true;
                    Application.LoadLevel("_Scenes/Menu");
                    gameObject.SetActive(false);
                }
            }

        }

    }

}
