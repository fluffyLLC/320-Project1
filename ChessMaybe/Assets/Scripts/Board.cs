using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Board : MonoBehaviour
{

    public float offset = 0.2f;

    public Material blackMat;
    public Material whiteMat;

    private int boardSize = 8;
    //public GameObject boardSegment;

    public static GameObject[,] boardSpaces; //TODO: store these as the relevant scripts rather than gameObjects
    public static GameObject[,] peices;

    //TODO: Generate an *x8 board
    //TODO: Chanvge the color of the board segements
    //TODO: Populate the game board with player peices
    //TODO: Impliment singelton


    /*
    // Start is called before the first frame update
    void Start()
    {
       BuildBoard();

    }


    void Update()
    {

    }
    */


    public GameObject[,] BuildBoard(bool rebuildBoard = false) {

        if (boardSpaces != null)
        {
            if (rebuildBoard)
            {
                DeconstructBoard();
            }
            else {
                return boardSpaces;
            }
        }


        boardSpaces = new GameObject[boardSize, boardSize];
        peices = new GameObject[boardSize, boardSize];

        int totalIterations = 0;
        float segmentSpaceing = 1 + offset;

        for (int y = 0; y < boardSize; y++) {

            for (int x = 0; x < boardSize; x++)
            {
                GameObject b = InstantiateSegmant(new Vector3((segmentSpaceing) *x, 0, (segmentSpaceing) * y));//spawn board peices
                boardSpaces[x, y] = b; //store a refrence to the peice
                BoardSegment bs = b.GetComponent<BoardSegment>(); //
                bs.init(new BoardPOS(x, y),blackMat,whiteMat);

                GameObject peice = bs.PopulateStart();

                if (peice) {
                    peices[x, y] = peice;
                    PlayerPeice peiceScript = peice.GetComponent<PlayerPeice>();
                    peiceScript.acessIndex = new Vector2(x, y);
                }

                if (y % 2 != 0 && y != 0)//every other row
                {
                    if (totalIterations % 2 == 0)//if we are even
                    {
                        SwapToBlack(b);
                    }
                }
                else if (totalIterations % 2 != 0 && totalIterations != 0)//if we are odd
                {
                        SwapToBlack(b);
                }
                totalIterations++;


            }

        }

        return boardSpaces;
 
    }

    public void DeconstructBoard()
    {
        if (boardSpaces == null)
        {
            return;
        }


        for (int y = 0; y < boardSize; y++)
        {

            for (int x = 0; x < boardSize; x++)
            {
                //GameObject b = InstantiateSegmant(new Vector3((1 + offset) * x, 0, (1 + offset) * y));
                GameObject b = boardSpaces[x, y];
                GameObject p = peices[x, y];

                if (Application.isPlaying)
                {
                    Destroy(b);
                    if (p)
                    {
                        Destroy(p); //TODO: create a "clear board" function that just clears the play peices

                    }
                }
                else
                {
                    DestroyImmediate(b);
                    if (p)
                    {
                        DestroyImmediate(p);

                    }
                }

            }

        }

        boardSpaces = null;

    }


    private void SwapToBlack(GameObject b)
    {
        MeshRenderer m = b.GetComponent<MeshRenderer>();
        m.material = blackMat;
        //m.sharedMaterial.color = Color.black;
    }

    public bool HandleMove(byte[] updatedState) {
        Vector2 currentIndex = new Vector2(-1, -1);
        Vector2 targetIndex = new Vector2(-1,-1);
        

        //determine which spaces can need to be updated
        for (int i = 0; i < updatedState.Length; i++) {
            int y = i / boardSize;
            int x = i % boardSize;

            if (updatedState[i] == 0)
            {
                if (updatedState[i] != (int)boardSpaces[x, y].GetComponent<BoardSegment>().state)
                {

                    currentIndex = new Vector2(x, y);

                }
            }
            else {
                if (updatedState[i] != (int)boardSpaces[x, y].GetComponent<BoardSegment>().state) {

                    targetIndex = new Vector2(x, y);   
                
                }
            }
        
        
        }

        if (targetIndex.x < 0 || targetIndex.y < 0 || currentIndex.x < 0 || currentIndex.y < 0) return false; //there was no change in board state

        BoardSegment targetSegmant = boardSpaces[(int)targetIndex.x, (int)targetIndex.y].GetComponent<BoardSegment>();
        //BoardSegment currentSegmant = boardSpaces[(int)currentIndex.x, (int)currentIndex.y].GetComponent<BoardSegment>();

        //remove "taken peices"
        SegmentOccupationState futureState = (SegmentOccupationState)updatedState[targetSegmant.pos.packetIndex];

        if (OccupiedByEnemey(targetSegmant.state, futureState)) {

            
            GameObject p = peices[(int)targetIndex.x, (int)targetIndex.y];
            if (p != null)
            {
                peices[(int)targetIndex.x, (int)targetIndex.y] = null;
                //print("DESTROYING");
                Destroy(p);
            }
            else {
                //print("alwats null");
                return false;
            }
        }

        //move peices on gameboard
        peices[(int)currentIndex.x, (int)currentIndex.y].GetComponent<PlayerPeice>().MovePeice(targetSegmant.snapPointHover.position, targetSegmant.snapPointPlaced.position);


        //update gamestate
        peices[(int)targetIndex.x, (int)targetIndex.y] = peices[(int)currentIndex.x, (int)currentIndex.y];
        peices[(int)currentIndex.x, (int)currentIndex.y] = null;

        peices[(int)targetIndex.x, (int)targetIndex.y].GetComponent<PlayerPeice>().acessIndex = targetIndex;

        boardSpaces[(int)targetIndex.x, (int)targetIndex.y].GetComponent<BoardSegment>().state = boardSpaces[(int)currentIndex.x, (int)currentIndex.y].GetComponent<BoardSegment>().state;
        boardSpaces[(int)currentIndex.x, (int)currentIndex.y].GetComponent<BoardSegment>().state = SegmentOccupationState.Empty;


        return true;
    }

    private bool OccupiedByEnemey(SegmentOccupationState currentState, SegmentOccupationState futureState) {
        if (currentState == SegmentOccupationState.Empty) {
            return false;
        }

        if ((int)futureState < 7 && (int)currentState >= 7) {
            return true;
        }else if ((int)futureState >= 7 && (int)currentState < 7)
        {
            return true;
        }

        return false;


    }

    

    public GameObject InstantiateSegmant(Vector3 pos) {

        //boardSegment = Resources.Load<GameObject>("Prefabs/BoardSegment") as GameObject; <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Success!!!!!!!!!

        return Instantiate(Resources.Load<GameObject>("Prefabs/BoardSegment"), pos, Quaternion.identity,transform) as GameObject;

    }

}
