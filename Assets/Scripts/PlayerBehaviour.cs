using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Material playerColor;

    private int moveAmount = 1;

    private GameObject lastTile;

    public float stackLength = .2f;

    public Material defaultMaterial;

    private GameObject moveAmountText;

    private bool onAnimation;


    public GameObject MoveAmountText
    {
        get
        {
            return moveAmountText;
        }
    }

    public bool OnAnimation
    {
        get
        {
            return onAnimation;
        }
        
    }

    public int MoveAmount
    {
        get
        {
            return moveAmount;
        }
        set
        {
            moveAmount = value;
        }
    }

    public static PlayerBehaviour Instance;


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

        
    }

    private void Start()
    {
        playerColor = GetComponent<Renderer>().material;
        moveAmountText = transform.GetChild(0).transform.GetChild(0).gameObject;
        transform.GetChild(0).SetParent(null, false);

        onAnimation = false;
    }




    private void Update()
    {

        float yPos = GetComponent<Renderer>().bounds.max.y;


        moveAmountText.transform.position = new Vector3(transform.position.x, yPos + .4f, transform.position.z);
        moveAmountText.GetComponent<TextMeshProUGUI>().SetText(moveAmount.ToString());
        moveAmountText.GetComponent<TextMeshProUGUI>().color = playerColor.color;


        
    }

    public Color GetColor()
    {
        return GetComponent<Renderer>().material.color;
    }


    public void Undo()
    {

        moveAmount++;


        if(lastTile.tag == "ColouredTile")
        {
            lastTile.tag = "EmptyTile";
            lastTile.GetComponent<Renderer>().material = defaultMaterial;
            TileManager.Instance.CheckTilesToColor();



            

        }

        StartCoroutine(UpdateScale(1));


    }




    private void OnCollisionEnter(Collision collision)
    {


        lastTile = collision.gameObject;

        

        if(collision.gameObject.tag == "EmptyTile")
        {
            collision.gameObject.GetComponent<Renderer>().material.color = playerColor.color;
            collision.gameObject.tag = "ColouredTile";

            if(collision.gameObject.transform.parent.childCount < 2)
            {

                moveAmount--;

            }

            TileManager.Instance.CalculatePoints();

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - stackLength, transform.localScale.z);


        }
        else if(collision.gameObject.tag == "SourceTile")
        {
            playerColor.color = collision.gameObject.GetComponent<Renderer>().material.color;

            TileManager.Instance.CheckTilesToColor();


        }
        
    }

    

    

    private void OnTriggerEnter(Collider other)
    {



        if(other.tag == "ColorStack")
        {
            string stackText = other.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;

            int stackAmount = Parse(stackText);

            if (stackText[0] == '+')
            {
                moveAmount += stackAmount;
            }
            else if (stackText[0] == 'x')
            {

                int prevMoveAmount = moveAmount;

                moveAmount *= stackAmount;

                stackAmount = moveAmount - prevMoveAmount;
            }

            

            StartCoroutine(UpdateScale(stackAmount + 1));

            

            Destroy(other.gameObject);
        }


    }


    private IEnumerator UpdateScale(int stackAmount)
    {
        onAnimation = true;


        while (InputManager.Instance.IsMoving)
        {
            yield return null;
        }



        GetComponent<Rigidbody>().isKinematic = true;

        

        transform.position += Vector3.up * (stackAmount) * stackLength / 2;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + stackAmount * stackLength, transform.localScale.z);



        yield return new WaitForSeconds(0.05f);

        GetComponent<Rigidbody>().isKinematic = false;

        onAnimation = false;


    }



    private int Parse(string text)
    {
        int num = 0;

        for(int i = 1; i < text.Length; i++)
        {


            int digit = text[i] - 48;



            num = num * 10 + digit;

        }


        return num;

    }

    

    

}
