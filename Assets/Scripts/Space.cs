using UnityEngine;
using System.Collections;

public class Space{

    //public Space parent; actualgrid shouldn't have parents for their nodes, pathgrid does.
    public bool walkable = true;
    public bool hasPermaWall = false;
    public bool hasWall = false;
    public bool hasPot = false;
    public bool hasEnemy = false;
    public bool hasPlayer = false;
    public int nodeNumber;
    //Have four different fields for movement costs for each of the four directions
    //by default all 4 are 10
    public int upMovementCost = 10; //the standard walkable tile takes a movement cost of 10
    public int downMovementCost = 10; //the standard walkable tile takes a movement cost of 10
    public int rightMovementCost = 10; //the standard walkable tile takes a movement cost of 10
    public int leftMovementCost = 10; //the standard walkable tile takes a movement cost of 10
    public int x;
    public int y;
}
