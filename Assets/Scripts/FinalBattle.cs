using UnityEngine;
using System.Collections;

public class FinalBattle : MonoBehaviour {

    public GameObject facingRightSpawn;
    public GameObject facingLeftSpawn;
    public GameObject leftTopSpawn;
    public GameObject leftBottomSpawn;
    public GameObject rightTopSpawn;
    public GameObject rightBottomSpawn;
    public int turnDifference; //how many turns it takes until you spawn
    public int leftTopX;
    public int currentEnemyDifficulty = 0;
    // Use this for initialization
    public void CheckIfSpawn(int currentTurn) {
        
        if (GameManager.instance.difficultyLevel <= 0) {
            CheckIfSpawnEasy(currentTurn);

        }
        if (GameManager.instance.difficultyLevel == 1)
        {
            CheckIfSpawnNormal(currentTurn);

        }
        if (GameManager.instance.difficultyLevel >= 2)
        {
            CheckIfSpawnHard(currentTurn);
        }
    }
    public void CheckIfSpawnEasy(int currentTurn) {
        if (currentTurn == 17)
        {
            GameManager.instance.showMessage("More guards have come from the left");
            SpawnLeft();
        }
        if (currentTurn == 20)
        {
            GameManager.instance.showMessage("More guards have come from the right");
            SpawnRight();
        }
        if (currentTurn == 29)
        {
            GameManager.instance.showMessage("More guards have come from the left");
            currentEnemyDifficulty++;
            SpawnLeft();

        }
        if (currentTurn == 35)
        {
            GameManager.instance.showMessage("More enemies have come from the right");

            SpawnRight();
        }
        if (currentTurn == 39)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            currentEnemyDifficulty++;
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 52)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            currentEnemyDifficulty++;
            SpawnLeft();
        }

        if (currentTurn == 59)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
            currentEnemyDifficulty++;
            SpawnRight();
        }

        if (currentTurn == 65)
        {
            GameManager.instance.showMessage("More enemies have come from the left");
            currentEnemyDifficulty++;
            SpawnLeft();
        }
        if (currentTurn == 69)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
            currentEnemyDifficulty++;
            SpawnRight();
        }
        if (currentTurn == 79)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 87)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 94)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 100)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
    }
    public void CheckIfSpawnNormal(int currentTurn)
    {
        if (currentTurn == 10)
        {
            GameManager.instance.showMessage("More guards have come from the left");
            SpawnLeft();
        }
        if (currentTurn == 12)
        {
            GameManager.instance.showMessage("More guards have come from the right");
            SpawnRight();
        }
        if (currentTurn == 16)
        {
            GameManager.instance.showMessage("More guards have come from the left");
            currentEnemyDifficulty++;
            SpawnLeft();

        }
        if (currentTurn == 18)
        {
            GameManager.instance.showMessage("More enemies have come from the right");

            SpawnRight();
        }
        if (currentTurn == 24)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            currentEnemyDifficulty++;
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 28)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            currentEnemyDifficulty++;
            SpawnLeft();
        }

        if (currentTurn == 30)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
            currentEnemyDifficulty++;
            SpawnRight();
        }

        if (currentTurn == 32)
        {
            GameManager.instance.showMessage("More enemies have come from the left");
            currentEnemyDifficulty++;
            SpawnLeft();
        }
        if (currentTurn == 35)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
            currentEnemyDifficulty++;
            SpawnRight();
        }
        if (currentTurn == 36)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 40)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 45)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 50)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
    }
    public void CheckIfSpawnHard(int currentTurn)
    {
        Debug.Log(currentTurn);
        if (currentTurn == 7) {
            GameManager.instance.showMessage("More guards have come from the left");
            SpawnLeft();
        }
        if (currentTurn == 8)
        {
            GameManager.instance.showMessage("More guards have come from the right");
            SpawnRight();
        }
        if (currentTurn == 13)
        {
            GameManager.instance.showMessage("More guards have come from the left");
            currentEnemyDifficulty++;
            SpawnLeft();

        }
        if (currentTurn == 15)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
 
            SpawnRight();
        }
        if (currentTurn == 19)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            currentEnemyDifficulty++;
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 22)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            currentEnemyDifficulty++;
            SpawnLeft();
        }
        
        if (currentTurn == 23)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
            currentEnemyDifficulty++;
            SpawnRight();
        }

        if (currentTurn == 25)
        {
            GameManager.instance.showMessage("More enemies have come from the left");
            currentEnemyDifficulty++;
            SpawnLeft();
        }
        if (currentTurn == 26)
        {
            GameManager.instance.showMessage("More enemies have come from the right");
            currentEnemyDifficulty++;
            SpawnRight();
        }
        if (currentTurn == 29)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 32)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 35)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
        if (currentTurn == 40)
        {
            GameManager.instance.showMessage("More enemies have come from the left and right");
            SpawnLeft();
            SpawnRight();
        }
    }
    public void SpawnLeft() {
        Debug.Log("spawning");
        if(GameManager.instance.gameCalculation.actualGrid[7,0].walkable == true
            && GameManager.instance.gameCalculation.actualGrid[7, 0].hasPlayer == false
            && GameManager.instance.gameCalculation.actualGrid[7, 0].hasEnemy == false) {

            GameObject leftTop = 
            (GameObject)Instantiate(facingRightSpawn, new Vector3(0, 7, 0), Quaternion.identity);

            leftTop.SetActive(true);
            leftTop.GetComponent<Enemy>().initializeLevel12Stats(currentEnemyDifficulty);
        }

        if (GameManager.instance.gameCalculation.actualGrid[8, 0].walkable == true
            && GameManager.instance.gameCalculation.actualGrid[8, 0].hasPlayer == false
            && GameManager.instance.gameCalculation.actualGrid[8, 0].hasEnemy == false)
        {
            GameObject leftBottom =
            (GameObject)Instantiate(facingRightSpawn, new Vector3(0, 8, 0), Quaternion.identity);
            leftBottom.SetActive(true);
            leftBottom.GetComponent<Enemy>().initializeLevel12Stats(currentEnemyDifficulty);
        }
    }
    public void SpawnRight() {
        if (GameManager.instance.gameCalculation.actualGrid[7, 14].walkable == true
            && GameManager.instance.gameCalculation.actualGrid[7, 14].hasPlayer == false
            && GameManager.instance.gameCalculation.actualGrid[7, 14].hasEnemy == false)
        {

            GameObject rightTop =
            (GameObject)Instantiate(facingLeftSpawn, new Vector3(14, 7, 0), Quaternion.identity);
            rightTop.SetActive(true);
            rightTop.GetComponent<Enemy>().initializeLevel12Stats(currentEnemyDifficulty);
        }

        if (GameManager.instance.gameCalculation.actualGrid[8, 14].walkable == true
            && GameManager.instance.gameCalculation.actualGrid[8, 14].hasPlayer == false
            && GameManager.instance.gameCalculation.actualGrid[8, 14].hasEnemy == false)
        {
            GameObject rightBottom =
            (GameObject)Instantiate(facingLeftSpawn, new Vector3(14, 8, 0), Quaternion.identity);
            rightBottom.SetActive(true);
            rightBottom.GetComponent<Enemy>().initializeLevel12Stats(currentEnemyDifficulty);
        }
    }

}
