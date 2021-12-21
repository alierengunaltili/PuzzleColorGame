using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public GameObject tiles;

    public Material defaultMat;

    public Material finalMat;

    public static TileManager Instance;

    private int totalScore;



    private int emptyTileCount;

    public int TotalScore
    {

        get
        {
            return totalScore;
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

        InitializeTiles();

        CinemachineCore.GetInputAxis = DisableCinemachine;        
    }

    private float DisableCinemachine(string axisName)
    {
        return 0;
    }


    private void Start()
    {
        GetVariables();
        GameManager.Instance.InitializeVariables();

    }

    private void InitializeTiles()
    {

        


        for (int i = 0; i < tiles.transform.childCount; i++)
        {
            Transform tile = tiles.transform.GetChild(i);

            if(tile.tag == "Untagged")
            {

                if (tile.GetChild(0).GetComponent<MeshRenderer>().material.color == defaultMat.color)
                {

                    tile.GetChild(0).tag = "EmptyTile";
                    emptyTileCount++;

                }
                else
                {
                    tile.GetChild(0).tag = "SourceTile";
                }
            }




        }


        





    }




    public void CheckTilesToColor()
    {
        
        for(int i = 0; i < tiles.transform.childCount; i++)
        {

            GameObject tile = tiles.transform.GetChild(i).gameObject;


            if(tile.transform.GetChild(0).tag == "EmptyTile" || tile.transform.GetChild(0).tag == "DifferentColor" || tile.transform.GetChild(0).tag == "ColouredTile")
            {

                

                if(tile.GetComponent<TileScript>().targetColorSO.targetColor.color == PlayerBehaviour.Instance.GetColor())
                {
                    if(tile.transform.GetChild(0).tag != "ColouredTile")
                    {
                        tile.transform.GetChild(0).tag = "EmptyTile";
                        tile.transform.GetChild(0).GetComponent<Renderer>().material = defaultMat;

                    }

                    tile.transform.position = new Vector3(tile.transform.position.x, 0, tile.transform.position.z);

                }
                else
                {


                    
                    tile.transform.position = new Vector3(tile.transform.position.x, 0.4f, tile.transform.position.z);

                    
                    if(tile.transform.GetChild(0).tag != "ColouredTile")
                    {

                        tile.transform.GetChild(0).tag = "DifferentColor";
                        tile.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black; 


                    }
                }



            }





        }



    }


    public void DisplayPixelArt()
    {

        PlayerBehaviour.Instance.gameObject.SetActive(false);
        PlayerBehaviour.Instance.MoveAmountText.SetActive(false);

        for(int i = 0; i < tiles.transform.childCount; i++)
        {
            GameObject tile = tiles.transform.GetChild(i).gameObject;

            tile.transform.position = new Vector3(tile.transform.position.x, 0f, tile.transform.position.z);


            if(tile.transform.GetChild(0).tag == "EmptyTile" || tile.transform.GetChild(0).tag == "DifferentColor")
            {
                tile.transform.GetChild(0).GetComponent<Renderer>().material = finalMat;
                tile.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;

            }
            if (tile.transform.childCount == 2)
            {
                Destroy(tile.transform.GetChild(1).gameObject);
            }

            if(tile.transform.GetChild(0).tag == "ColouredTile")
            {
                tile.transform.GetChild(0).GetComponent<Renderer>().material = finalMat;

                tile.transform.GetChild(0).GetComponent<Renderer>().material.color = tile.GetComponent<TileScript>().targetColorSO.targetColor.color;



            }


        }




    }

    public void CalculatePoints()
    {

        

        int colouredTileCount = 0;

        for (int i = 0; i < tiles.transform.childCount; i++)
        {

            GameObject tile = tiles.transform.GetChild(i).gameObject;


            if (tile.transform.GetChild(0).tag == "ColouredTile")
            {

               


                colouredTileCount++;

            }





        }

        totalScore = (int)(((float)colouredTileCount / emptyTileCount) * 100);

        

        if(totalScore == 100)
        {
            UIManager.Instance.EndLevelButton();
        }
        else if(totalScore > 70f)
        {

            UIManager.Instance.GetNextLevelUI();

        }


    }


    public void GetVariables()
    {

        GameManager.Instance.playerCam = GameObject.Find("PlayerCam");
        GameManager.Instance.gameBoard = GameObject.Find("GameBoard");
        GameManager.Instance.endLevelCam = GameObject.Find("EndLevelCam");



    }


}
