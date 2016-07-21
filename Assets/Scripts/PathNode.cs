using UnityEngine;
using System.Collections;
//using System;
using System.Collections.Generic;

public class PathNode
{
    public PathNode parent = null;
    public bool fullOccupied = false;
    public bool enemyOccupied = false;
    public bool playerOccupied = false;

    public int x;
    public int y;
    public int nodeNumber;
    public int H = 0;// heuristic, simply the distance in terms of movements needed if all tiles were passable
    public int G = 0;//how much it has cost to move to the current node
    public int F = 0; //G + H, in the open, the node with the smallest F value gets added to the closed list

    //This function should print out the entire path
    public void recursivePrint()
    {

        if (parent != null)
        {
            parent.recursivePrint();
        }
        Debug.Log(nodeNumber + "  ");
    }

    public List<int> getMovesToDo()
    {
        List<int> movesToDo = new List<int>();
        PathNode currentNode = this;
        while (currentNode.parent != null)
        {
            if (currentNode.x > currentNode.parent.x)
            {
                movesToDo.Insert(0, 3); //moving right is direction 3
            }
            else if (currentNode.x < currentNode.parent.x) //moving left is direction 4
            {
                movesToDo.Insert(0, 4);
            }
            else if (currentNode.y > currentNode.parent.y) //moving up is direction 2
            {
                movesToDo.Insert(0, 2);
            }
            else if (currentNode.y < currentNode.parent.y) //moving down is direction 1
            {
                movesToDo.Insert(0, 1);
            }
            currentNode = currentNode.parent;
        }
        return movesToDo;
    }

}
