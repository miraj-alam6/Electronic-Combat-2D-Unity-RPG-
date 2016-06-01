using UnityEngine;
using System; //so we can use serializable which lets us control how variables will appear in editor
using System.Collections.Generic; //so we can use lists
using Random = UnityEngine.Random; // we need to specify this because there is a Random in both 
//UnityEngine namespace and the System namespace.

public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count{
        public int minimum;
        public int maximum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    //can change these dimensions as we want to, but gonna begin with an 8 by 8 game board.
    public Count wallCount = new Count(5,9); //min and max represent the range of the random amount
    public Count foodCount = new Count(1,5);
    public GameObject exit; //just need one exit, for the rest of game objects use an array.
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    //all of these are public variables so we will fill their values in the inspector.
    
    private Transform boardHolder; //this is just a parent that will hold all the board objects
    //as children so that they don't fill up the inspector, because there will be A LOT of board objects
    //so it's better if they are in a wrapper so that inspector is more clean.
    private List<Vector3> gridPositions = new List<Vector3>(); //keeps track of all positions and if an object has been spawned there.

    void InitializeList() {
        gridPositions.Clear();
        for (int x = 1; x < columns-1; x++) {
            for (int y = 1; y < rows-1; y++) {
                gridPositions.Add(new Vector3(x,y,0f)); //this is a 2d game so we don't need the third position
            }
        }
        //we did -1 for columns and rows limit because we're gonna have a board of impassable outer wall
    }

    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;
        //-1 for start and +1 for end is because we want to instantiate the outer walls as well
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                //gonna instantiate a random floor tile
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //now check if we are in the border regions, and if so instantiate an outer wall
                //note to self: wouldn't it be better to check this first?
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                //now the actual instantiation will occur
                GameObject instance = Instantiate(
                    toInstantiate, //the object to Instantiate
                    new Vector3(x,y,0f), 
                    Quaternion.identity
                    ) as GameObject; //as GameObject casts it to a GameObject
                // Quaternion.identity means instatiate with no rotation
                //now make it a child of boardHolder
                instance.transform.SetParent(boardHolder);
            }
        }

        

   
    }

    //set a randomposition using the number of different possible positions in the grid which we store
    //using our Count class. Get a random number in the range of all the positions in the array for
    //all the positions
    Vector3 RandomPosition() {
        int randomIndex = Random.Range(0,gridPositions.Count); //I have a feeling this isn't the Count class
                                                //that we ourselves defined but i'm not sure.
                                                //now make the actual Vector3 for that randomPosition
        Vector3 randomPosition = gridPositions[randomIndex];
        //this removes this grid position from ever being used again.
        gridPositions.RemoveAt(randomIndex);
        return randomPosition; 
    }

    //now a function to actually spawn something at a random position and make sure it's not a duplicate
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum+1);// objectCount will be for how many objects and
        //stuff we'll spawn for let's say example how many inner walls.
        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPosition = RandomPosition(); //get a usable random position by using our function
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)]; //use a tile
            Instantiate(tileChoice,randomPosition,Quaternion.identity); //Instantiate the object at the randomPosition
        }
    }

    //notice that our following function is  the single public function in our class, because it is
    //what will be called by the GameManager or something
    public void SetupScene(int level) {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles,wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, wallCount.maximum);
        //we're gonna make a logarithmic difficulty progression using enemyCount
        int enemyCount = (int)Mathf.Log(level, 2.0f); //means 1 enemie at level 1, 2 enemies at level 4, 3 enemies at level 8, and so on.
        LayoutObjectAtRandom(enemyTiles, enemyCount,enemyCount); //min max are same because we already generated how many enemies, no randomness.
        Instantiate(exit,new Vector3(columns -1, rows -1,0f),Quaternion.identity);//exit always in same position, which is why we use -1,-1.
    }


}
