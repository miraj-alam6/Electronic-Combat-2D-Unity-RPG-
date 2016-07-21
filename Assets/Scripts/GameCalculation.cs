using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameCalculation : MonoBehaviour
{

    public Space[,] actualGrid;
    public int maxNodeNumber;
    public int columns;
    public int rows;
    public void printGrid() {
        for (int i = 0; i < actualGrid.GetLength(0); i++) {
            for (int j = 0; j < actualGrid.GetLength(1); j++) {
                Debug.Log(actualGrid[i,j].x + "," + actualGrid[i, j].y + ":"+ actualGrid[i, j].nodeNumber +  " walkable: " +
                        actualGrid[i, j].walkable + " soft wall: " + actualGrid[i, j].hasWall + " enemy: " + actualGrid[i, j].hasEnemy + " player: " + actualGrid[i, j].hasPlayer);
            }
        }

    }
    public void initializeGameCalculation(int rows, int columns, int levelID)
    {
        this.columns = columns;
        this.rows = rows;
        actualGrid = new Space[rows, columns];
        for (int i = 0; i < actualGrid.GetLength(0); i++)
        {
            for (int j = 0; j < actualGrid.GetLength(1); j++)
            {
                //Console.WriteLine(i + "," + j);
                actualGrid[i, j] = new Space();
                actualGrid[i, j].x = j;
                actualGrid[i, j].y = i;
                actualGrid[i, j].nodeNumber = j + (i * actualGrid.GetLength(1));
            }

        }
        maxNodeNumber = ((columns * rows) - 1);
//        loadLevel(1);
    }

    //this function is commented out because we can just do all the loading in BoardManager
  /*  public void loadLevel(int level)
    {
        if (level == 1)
        {
            //this entire loop may be unneccesary 
            for (int i = 0; i < actualGrid.GetLength(0); i++)
            {
                for (int j = 0; j < actualGrid.GetLength(1); j++)
                {
                    //x and y being 0 is actually acceptable
                    if (i == 0 || j == 0 || i == actualGrid.GetLength(0) - 1 || j == actualGrid.GetLength(1) - 1)
                    {
                        actualGrid[i, j].hasHardWall = true;
                        actualGrid[i, j].walkable = false;
                    }
                }

            }
        }
        //this is X coordinate 1, and Y coordinate 2
        actualGrid[2, 1].hasEnemy = true;
        actualGrid[2, 1].walkable = false;
        actualGrid[2, 2].hasHardWall = true;
        actualGrid[2, 2].walkable = false;
        actualGrid[2, 3].hasPlayer = true;
        actualGrid[2, 3].walkable = false;
    }
    */

    //all the nodes are numbered. This will get you the exact node you want
    public Space getNode(int index)
    {
        int i = index / actualGrid.GetLength(1);
        int j = index % actualGrid.GetLength(1);
        return actualGrid[i, j];
    }

    //overload method for PathGrid. Pathgrid isn't a global, so pass in the actual grid itself
    public PathNode getNode(PathNode[,] grid, int index)
    {
        int i = index / grid.GetLength(1);
        int j = index % grid.GetLength(1);
        return grid[i, j];
    }

    //Shortest path is found using A star algorithm
    public List<int> getShortestPath(int srcX, int srcY, int destX, int destY)
    {
        //This will hold a list of all the movements to do to get to destination
        List<int> movementsToDo = new List<int>();
        List<int> openList = new List<int>();
        List<int> closedList = new List<int>();
        PathNode[,] pathGrid = new PathNode[actualGrid.GetLength(0), actualGrid.GetLength(1)];
        //initializee the array for the grid that we will use to calculate path
        //get the information from the actualgrid into your path grid so that you know which
        //nodes you cannot move to
        for (int i = 0; i < pathGrid.GetLength(0); i++)
        {
            for (int j = 0; j < pathGrid.GetLength(1); j++)
            {
                pathGrid[i, j] = new PathNode();
                if (actualGrid[i, j].hasWall || actualGrid[i, j].hasPot)
                {
                    pathGrid[i, j].fullOccupied = true;
                }
                else if (actualGrid[i, j].hasPlayer)
                {
                    pathGrid[i, j].playerOccupied = true;
                }
                else if (actualGrid[i, j].hasEnemy)
                {
                    pathGrid[i, j].enemyOccupied = true;
                }

                pathGrid[i, j].x = j;
                pathGrid[i, j].y = i;
                pathGrid[i, j].nodeNumber = actualGrid[i, j].nodeNumber;
            }

        }



        //Console.WriteLine("The state looks down");
        int sourceNodeNumber = srcX + (srcY * actualGrid.GetLength(1));
        int destinationNodeNumber = destX + (destY * pathGrid.GetLength(1));
        PathNode sourceNode = getNode(pathGrid, sourceNodeNumber);
        PathNode destinationNode = getNode(pathGrid, destinationNodeNumber);
        //Console.WriteLine(maxNodeNumber);
        // Console.WriteLine(sourceNodeNumber);
        // Console.WriteLine(destinationNodeNumber);
        //Loop through all PathNodes to give H values to all the nodes in pathGrid that aren't occupied
        for (int i = 0; i < pathGrid.GetLength(0); i++)
        {
            for (int j = 0; j < pathGrid.GetLength(1); j++)
            {
                //only calculate H value if it is a valid space to go in
                //the second requirement of no enemy should be fixed later to let enemies move
                //through each other, but do this later. Not necessary now
                if (!pathGrid[i, j].fullOccupied && !pathGrid[i, j].enemyOccupied)
                {
                    pathGrid[i, j].H = Math.Abs(pathGrid[i, j].x - destinationNode.x) + Math.Abs(pathGrid[i, j].y - destinationNode.y);
                }
            }
        }
        //the if condition would have excluded the currentNode is currentnode is enemy, so set H
        //value here:
        sourceNode.H = Math.Abs(sourceNode.x - destinationNode.x) + Math.Abs(sourceNode.y - destinationNode.y);
        //cool, the above actually worked at changing the value in the array. The variable is
        //a reference to it then rather than a copy.

        //comment this out, gonna just print everything to make sure
        /*
        for (int i = 0; i < pathGrid.GetLength(0); i++)
        {
            for (int j = 0; j < pathGrid.GetLength(1); j++)
            {
                Console.WriteLine(pathGrid[i, j].x + "," + pathGrid[i, j].y + ": " + pathGrid[i, j].nodeNumber +
                    " fulloccupied: " + pathGrid[i, j].fullOccupied + " enemy here: " +
                    pathGrid[i, j].enemyOccupied + " player here: "
                    + pathGrid[i, j].playerOccupied + " H: " + pathGrid[i, j].H);

            }
        }
        */
        bool findingPath = true; //this will be set to false once path is found
        closedList.Add(sourceNodeNumber);
        while (findingPath)
        {
            int currentNodeNumber = closedList[closedList.Count - 1];
            //Console.WriteLine(currentNodeNumber);
            if (currentNodeNumber == destinationNodeNumber)
            {
                findingPath = false;
            }
            PathNode currentNode = getNode(pathGrid, currentNodeNumber);
            //we will add a max of four new nodenumbers into our open list and then we will find the least value
            int x = currentNode.x; //this is our j in the loops
            int y = currentNode.y; //this is our i in the loops
            int upNodeNumber = -200;
            int downNodeNumber = -200;
            int rightNodeNumber = -200;
            int leftNodeNumber = -200;
            if ((y + 1) < pathGrid.GetLength(0))
            {
                upNodeNumber = x + ((y + 1) * pathGrid.GetLength(1));
            }
            if ((y - 1) >= 0)
            {
                downNodeNumber = x + ((y - 1) * pathGrid.GetLength(1));
            }
            if ((x + 1) < pathGrid.GetLength(1))
            {
                rightNodeNumber = x + 1 + (y * pathGrid.GetLength(1));
            }
            if ((x - 1) >= 0)
            {
                leftNodeNumber = x - 1 + (y * pathGrid.GetLength(1));
            }
            //Console.WriteLine(upNodeNumber);
            //Console.WriteLine(downNodeNumber);
            //Console.WriteLine(leftNodeNumber);
            //Console.WriteLine(rightNodeNumber);
            //if any of your nodes are already in closed list, DO NOT COUNT IT
            //or you will have infinite loop between parent and child relationship
            if (closedList.Contains(upNodeNumber))
            {
                upNodeNumber = -200;
            }
            if (closedList.Contains(downNodeNumber))
            {
                downNodeNumber = -200;
            }
            if (closedList.Contains(rightNodeNumber))
            {
                rightNodeNumber = -200;
            }
            if (closedList.Contains(leftNodeNumber))
            {
                leftNodeNumber = -200;
            }
            //we only add each of the four nodes if they are a valid node. negative nodes are invalid
            //nodes bigger than the maxnode size is invalid
            //occupied nodes are invalid
            //need to check the -200 thing first because short circuting will prevent us from
            //using an invalid index for getNode
            if (upNodeNumber != -200 && !getNode(pathGrid, upNodeNumber).fullOccupied && !getNode(pathGrid, upNodeNumber).enemyOccupied)
            {
                //this is reparenting logic
                if (openList.Contains(upNodeNumber))
                {
                    //FOLLOWING CODE IS NOT TESTED YET
                    PathNode node = getNode(pathGrid, upNodeNumber);
                    if (node.G > (currentNode.G + getNode(currentNodeNumber).upMovementCost))
                    {
                        node.G = getNode(currentNodeNumber).upMovementCost + currentNode.G;
                        node.F = node.G + node.H;
                        openList.Add(upNodeNumber);
                        getNode(pathGrid, upNodeNumber).parent = currentNode;
                    }

                    //have reparenting logic here, have to copy and paste for all four cases
                    //so it's probably better to write a function
                }
                //this is the normal casse i.e. not reparenting i.e. if the node 
                //is already in open list
                else {
                    //reminder, the function getNode is overloaded to
                    //get from actualgrid when only 1 parameter.
                    PathNode node = getNode(pathGrid, upNodeNumber);
                    node.G = getNode(currentNodeNumber).upMovementCost + currentNode.G;
                    node.F = node.G + node.H;
                    openList.Add(upNodeNumber);
                    getNode(pathGrid, upNodeNumber).parent = currentNode;
                }
            }
            //downNode
            if (downNodeNumber != -200 && !getNode(pathGrid, downNodeNumber).fullOccupied && !getNode(pathGrid, downNodeNumber).enemyOccupied)
            {
                if (openList.Contains(downNodeNumber))
                {
                    //FOLLOWING CODE IS NOT TESTED YET
                    PathNode node = getNode(pathGrid, downNodeNumber);
                    if (node.G > (currentNode.G + getNode(currentNodeNumber).downMovementCost))
                    {
                        node.G = getNode(currentNodeNumber).downMovementCost + currentNode.G;
                        node.F = node.G + node.H;
                        openList.Add(downNodeNumber);
                        getNode(pathGrid, downNodeNumber).parent = currentNode;
                    }

                }
                else {
                    PathNode node = getNode(pathGrid, downNodeNumber);
                    node.G = getNode(currentNodeNumber).downMovementCost + currentNode.G;
                    node.F = node.G + node.H;
                    openList.Add(downNodeNumber);
                    getNode(pathGrid, downNodeNumber).parent = currentNode;
                }
            }
            //rightNode
            if (rightNodeNumber != -200 && !getNode(pathGrid, rightNodeNumber).fullOccupied && !getNode(pathGrid, rightNodeNumber).enemyOccupied)
            {
                if (openList.Contains(rightNodeNumber))
                {
                    //FOLLOWING CODE IS NOT TESTED YET
                    PathNode node = getNode(pathGrid, rightNodeNumber);
                    if (node.G > (currentNode.G + getNode(currentNodeNumber).rightMovementCost))
                    {
                        node.G = getNode(currentNodeNumber).rightMovementCost + currentNode.G;
                        node.F = node.G + node.H;
                        openList.Add(rightNodeNumber);
                        getNode(pathGrid, rightNodeNumber).parent = currentNode;
                    }

                }
                else {
                    PathNode node = getNode(pathGrid, rightNodeNumber);
                    node.G = getNode(currentNodeNumber).rightMovementCost + currentNode.G;
                    node.F = node.G + node.H;
                    openList.Add(rightNodeNumber);
                    getNode(pathGrid, rightNodeNumber).parent = currentNode;
                }
            }

            //leftNode
            if (leftNodeNumber != -200 && !getNode(pathGrid, leftNodeNumber).fullOccupied && !getNode(pathGrid, leftNodeNumber).enemyOccupied)
            {
                if (openList.Contains(leftNodeNumber))
                {
                    PathNode node = getNode(pathGrid, leftNodeNumber);
                    if (node.G > (currentNode.G + getNode(currentNodeNumber).leftMovementCost))
                    {
                        node.G = getNode(currentNodeNumber).leftMovementCost + currentNode.G;
                        node.F = node.G + node.H;
                        openList.Add(leftNodeNumber);
                        getNode(pathGrid, leftNodeNumber).parent = currentNode;
                    }
                    //have reparenting logic here, have to copy and paste for all four cases
                    //so it's probably better to write a function
                }
                else {
                    PathNode node = getNode(pathGrid, leftNodeNumber);
                    node.G = getNode(currentNodeNumber).leftMovementCost + currentNode.G;
                    node.F = node.G + node.H;
                    openList.Add(leftNodeNumber);
                    getNode(pathGrid, leftNodeNumber).parent = currentNode;
                }
            }
            //To do: if open list or closed list or something like that
            //did not become bigger between this iteration and last one, that means there is no path to
            //your target. Work out the kinks more later, and deal with this case and break out of loop.  
            //Console.Write("open list:");
            //printList(openList);
            //Console.Write("closed list:");
            //printList(closedList);

             if (openList.Count <= 0)
            {
                //probably also put findingPath = false; to be consistent logically
                break; // when there is no path, this is where you reach.
            }

            //now we need to end this iteration by removing the node with the smallest F value in  the
            //open list and add it to the closed list so that in next iteration that will become
            //the current node.

            int minFNodeNumber = openList[0];
            int minF = getNode(pathGrid, openList[0]).F;
            for (int i = 1; i < openList.Count; i++)
            {
                if (getNode(pathGrid, openList[i]).F < minF)
                {
                    minF = getNode(pathGrid, openList[i]).F;
                    minFNodeNumber = openList[i];
                }
            }
            openList.Remove(minFNodeNumber);
            closedList.Add(minFNodeNumber);
            //Console.ReadKey(true); //take this out later, testing to see each iteration
        }


        //Console.WriteLine("");
        //destinationNode.recursivePrint();
        //Console.WriteLine("");
        movementsToDo = destinationNode.getMovesToDo();
        //printList(destinationNode.getMovesToDo());
        return movementsToDo;
    }


    //removes the smallest element from a list
    //use this on the open list, and add what this function returns into the closed list
    public int removeSmallest(List<int> list)
    {
        int minIndex = 0;
        int min = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
                minIndex = i;
            }
        }
        list.Remove(min);
        return min;
    }

    public void printList(List<int> list)
    {
        Debug.Log("This is a list:");
        foreach (int x in list)
        {
            Debug.Log(x);
        }
        //Console.WriteLine("");
    }

}


