using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;

//my real way to save and load just does not fucking work in Unity builds, there is no way
//to write to a fucking file in your folder, so i'm just
//gonna use PlayerPrefs
public class FileLoader {
    public int level;
    public int difficulty;
    public int hintsON; //0 means hints off, 1 means hints on
    public string path;


    public void crappyOpenFile() {
        level = PlayerPrefs.GetInt("LevelNumber");
        hintsON = PlayerPrefs.GetInt("HintStatus");

        difficulty = PlayerPrefs.GetInt("Difficulty");
    }
    public void crappySaveFile() {
        PlayerPrefs.SetInt("LevelNumber",GameManager.instance.levelNumber);
        if (GameManager.instance.hintsOn)
        {
            PlayerPrefs.SetInt("HintStatus", 1);
        }
        else {
            PlayerPrefs.SetInt("HintStatus", 0);
        }
        PlayerPrefs.SetInt("Difficulty", GameManager.instance.difficultyLevel);
    } 
    public void openFile() {
        StreamReader sr = new StreamReader("SaveDat.txt");
        string line = "";
        int count = 0;
        while ((line = sr.ReadLine()) != null) {
            if (count == 0)
            {
                level = Int32.Parse(line);
            }
            else {
                hintsON = Int32.Parse(line);
            }
            count++;
        }
    }

    public void saveFile() {


        System.IO.Directory.CreateDirectory(Application.dataPath + "Saves");

        int count = 0;
        level = GameManager.instance.levelNumber;
        if (GameManager.instance.hintsOn)
        {
            hintsON = 1;
        }
        else {
            hintsON = 0;
        } 
        Debug.Log("Reached 1");
        
        using (StreamWriter sw = new StreamWriter("SaveDat.txt")) {
            sw.Write(level);
            sw.WriteLine();
            sw.Write(hintsON);
        } 
    }
    public int getLevel() {
        Debug.Log("Level is" + level);
        return level;
    }

    public int getHintsOn()
    {
        Debug.Log("Hints On is" + hintsON);
        return hintsON;
    }
    public int getDifficulty()
    {
        return difficulty;
    }
}
