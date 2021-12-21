using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject playerCam;
    public GameObject endLevelCam;

    public static GameManager Instance;

    public GameObject gameBoard;
    public float widthRatio;

    private int currentLevel;

    private int lastLevel;

    public int LastLevel
    {
        get
        {
            return lastLevel;
        }
        set
        {
            lastLevel = value;
        }
    }

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
        }

    }

    private void MakeSingleton()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Awake()
    {
        MakeSingleton();

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        

        

        NextLevel();
    }

    public void InitializeVariables()
    {

        /*float height = Screen.height;
        float width = Screen.width;

        if(width / height > .5f)
        {
            width = height * .5f;
        }



        




        if (gameBoard != null)
        {
            gameBoard.transform.localScale = new Vector3(width * widthRatio, 1, width * widthRatio);

        }*/



    }

    

    

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {

        LoadlLastLevel();

        SceneManager.LoadScene("Level" + lastLevel);


        
    }

    public void GetEndLevel()
    {

        playerCam.SetActive(false);
        endLevelCam.SetActive(true);


        TileManager.Instance.DisplayPixelArt();


        lastLevel++;
        SaveLastLevel();

    }


    public void SaveLastLevel()
    {



        PlayerPrefs.SetInt("LastLevel", lastLevel);
        PlayerPrefs.Save();
    }


    public void LoadlLastLevel()
    {
        if (PlayerPrefs.HasKey("LastLevel"))
        {

            lastLevel = PlayerPrefs.GetInt("LastLevel");


            if (lastLevel > SceneManager.sceneCountInBuildSettings - 1)
            {

                lastLevel = SceneManager.sceneCountInBuildSettings - 1;

                

            }
            

            

        }
        else
        {
            lastLevel = 1;
        }

    }

}
