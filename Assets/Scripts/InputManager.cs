using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    #region Movement Variables
    private Vector2 initMousePos;
    private Vector2 lastMousePos;

    private Vector3 moveDirection;

    private Stack<Vector2> previousTile;

    private bool isMoving;

    private bool holdMovement;

    private bool mouseUp;

    private bool startedLevel;

    public bool StartedLevel
    {
        get
        {
            return startedLevel;
        }
        set
        {
            startedLevel = value;
        }
    }
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
        }
    }
   


    public float timeToMove;

    public GameObject gameBoard;

    #endregion




    

    public static InputManager Instance;

   

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

        previousTile = new Stack<Vector2>();

        holdMovement = false;

        mouseUp = false;

        startedLevel = false;

    }




    private void Update()
    {
        if (startedLevel)
        {
            if (Input.GetMouseButtonDown(0) && !isMoving)
            {
                mouseUp = false;

                initMousePos = Input.mousePosition;
                StartCoroutine(DirectionDelay());



            }
            else if (!isMoving && holdMovement)
            {

                CheckMovement(false);


                holdMovement = false;


            }
            if (Input.GetMouseButtonUp(0))
            {

                mouseUp = true;
                moveDirection = Vector3.zero;



            }
        }
    }





    #region Movement

    private IEnumerator DirectionDelay()
    {

        
        

        float time = 0f;

        while(time < 0.15f)
        {

            lastMousePos = Input.mousePosition;

            time += Time.deltaTime;

            yield return null;
        }

        if(Vector2.Distance(initMousePos, lastMousePos) > 0.5f)
        {
            CheckMovement(true);

        }



    }

    

    private void CheckMovement(bool firstMove)
    {

        if (firstMove)
        {
            float xDif = initMousePos.x - lastMousePos.x;
            float yDif = initMousePos.y - lastMousePos.y;

            if (Mathf.Abs(xDif) > Mathf.Abs(yDif))
            {

                if (initMousePos.x > lastMousePos.x)
                {
                    moveDirection = Vector3.left;
                }
                else
                {
                    moveDirection = Vector3.right;
                }


            }

            else
            {
                if (initMousePos.y > lastMousePos.y)
                {
                    moveDirection = Vector3.back;
                }
                else
                {
                    moveDirection = Vector3.forward;
                }
            }



            //moveDirection *= gameBoard.transform.localScale.x;
        }
        

        Ray moveRay = new Ray(PlayerBehaviour.Instance.gameObject.transform.position + moveDirection, Vector3.down);
        RaycastHit hitInfo = new RaycastHit();


        if (Physics.Raycast(moveRay, out hitInfo))
        {


            CheckMovementType(hitInfo.transform.gameObject);





            

        }
        

    }

    


    private void CheckMovementType(GameObject targetObject)
    {

        Vector2 targetPos = new Vector2((float)Math.Round(targetObject.transform.position.x), (float)Math.Round(targetObject.transform.position.z));



        Vector2 currPos = new Vector2((float)Math.Round(PlayerBehaviour.Instance.transform.position.x), (float)Math.Round(PlayerBehaviour.Instance.transform.position.z));

        


        if(previousTile.Count == 0 || targetPos != previousTile.Peek())
        {
            if(targetObject.tag == "ColouredTile" && targetObject.GetComponent<Renderer>().material.color != PlayerBehaviour.Instance.GetColor())
            {
                //Don't Move, tiles colour is different


            }
            else
            {
                


                if(targetObject.tag == "ColouredTile")
                {
                    //Move to same coloured tile empty stack



                    /*int count = previousTile.Count;
                    for (int i = 0; i < count; i++)
                    {

                        


                        previousTile.Pop();
                    }*/

                    IsMoving = true;
                    Move();


                }
                else if (targetObject.tag == "SourceTile")
                {
                    //Move to source tile empty stack


                    /*int count = previousTile.Count;

                    for(int i = 0; i < count; i++)
                    {

                        

                        previousTile.Pop();
                    }*/

                    IsMoving = true;
                    Move();


                }
                else if(targetObject.tag == "EmptyTile")
                {

                    if(PlayerBehaviour.Instance.MoveAmount > 0)
                    {

                        previousTile.Push(currPos);

                        

                        

                       

                        isMoving = true;
                        Move();

                    }


                }


            }


        }
        else
        {
            //It's Rewind Time


            /*previousTile.Pop();

            PlayerBehaviour.Instance.Undo();*/


            isMoving = true;
            Move();




        }



    }



    private void Move()
    {

        Vector3 targetPos = PlayerBehaviour.Instance.gameObject.transform.position + moveDirection;

        StartCoroutine(MoveAnim(targetPos));

    }


    private IEnumerator MoveAnim(Vector3 targetPos)
    {
        



        float elapsedTime = 0f;

        while(elapsedTime < timeToMove)
        {
            

            PlayerBehaviour.Instance.gameObject.transform.position = Vector3.Lerp(PlayerBehaviour.Instance.gameObject.transform.position, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        PlayerBehaviour.Instance.gameObject.transform.position = targetPos;

        

        


        yield return new WaitForSeconds(.05f);
        
        isMoving = false;



        while (PlayerBehaviour.Instance.OnAnimation)
        {
            yield return null;
        }

        if (Input.GetMouseButton(0) && !mouseUp)
        {

            yield return new WaitForSeconds(0.2f);
            holdMovement = true;
        }

        

    }
    #endregion

}
