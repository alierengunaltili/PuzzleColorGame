using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public RectTransform mainMenu;

    public RectTransform inGameUI;
    public RectTransform nextLevelUI;
    public RectTransform endLevelUI;

    public RectTransform stars1;
    public RectTransform stars2;
    public RectTransform stars3;


    public Transform confetti1Pos;
    public Transform confetti2Pos;

    public GameObject confettiPrefab;



    [SerializeField] private CanvasScaler mainCanvasScaler;

    private float canvasX;
    private float canvasY;

    public static UIManager Instance;


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
        SetCanvasScale();
    }

    private void Start()
    {
        InitializeVariables();

    }

    private void InitializeVariables()
    {

        inGameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + GameManager.Instance.LastLevel;


    }

    private void SetCanvasScale()
    {
        Vector2 mainRes = mainCanvasScaler.referenceResolution;
        canvasX = mainRes.x;
        canvasY = mainRes.y;
        if (mainMenu != null)
        {
            mainMenu.DOAnchorPos(Vector2.zero, 0);
        }
    }



    public void GetNextLevelUI()
    {
        nextLevelUI.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void RetryButton()
    {

        GameManager.Instance.RetryLevel();

    }


    public void EndLevelButton()
    {
        GameManager.Instance.GetEndLevel();

        inGameUI.DOAnchorPos(canvasX * Vector2.left, 0.25f);
        nextLevelUI.DOAnchorPos(2 * canvasX * Vector2.left, 0.25f);

        //TODO End Level UI and Scoring






        endLevelUI.DOAnchorPos(Vector2.zero, 0.25f);

        if(TileManager.Instance.TotalScore < 85)
        {
            stars1.DOAnchorPos(Vector2.zero, 0.2f);
        }
        else if(TileManager.Instance.TotalScore < 100)
        {
            stars2.DOAnchorPos(Vector2.zero, 0.2f);

        }
        else if(TileManager.Instance.TotalScore == 100)
        {
            stars3.DOAnchorPos(Vector2.zero, 0.2f);

        }



        StartCoroutine(Confetti());


    }

    private IEnumerator Confetti()
    {
        yield return new WaitForSeconds(0.4f);
        Instantiate(confettiPrefab, confetti1Pos.position, Quaternion.identity);
        Instantiate(confettiPrefab, confetti2Pos.position, Quaternion.identity);
    }

    public void NextLevelButton()
    {
        GameManager.Instance.NextLevel();

    }


    public void PlayButton()
    {

        mainMenu.DOAnchorPos(canvasX * Vector2.left, 0.25f);



        inGameUI.DOAnchorPos(Vector2.zero, 0.25f);


        InputManager.Instance.StartedLevel = true;

    }


}
