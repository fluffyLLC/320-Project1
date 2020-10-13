using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;




public class Board : MonoBehaviour
{

    public float offset = 0.2f;

    public Material blackMat;
    public Material whiteMat;

    private int boardSize = 8;
    //public GameObject boardSegment;

    public static GameObject[,] boardSpaces;
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


    private void SwapToBlack(GameObject b)
    {
        MeshRenderer m = b.GetComponent<MeshRenderer>();
        m.material = blackMat;
        //m.sharedMaterial.color = Color.black;
    }

    /*
    private bool MovePeice(Vector2 currentPos, Vector2 targetPos) {
        BoardSegment currentSegmant = boardSpaces[(int)currentPos.x, (int)currentPos.y].GetComponent<BoardSegment>();
        BoardSegment targetSegmant = boardSpaces[(int)targetPos.x, (int)targetPos.y].GetComponent<BoardSegment>();
        GameObject peice  = peices[(int)currentPos.x, (int)currentPos.y];

        peice.transform.position = Vector3.Lerp(currentSegmant.snapPointPlaced.position, targetSegmant.snapPointPlaced.position, 1);

        if (peice.transform.position == targetSegmant.snapPointPlaced.position) {
            
            return true;

        }
        else {

            return false;

        }

    }
    
    private bool HoverPeice(Vector2 currentPos, Vector2 peiceIndex)
    {
        BoardSegment currentSegmant = boardSpaces[(int)currentPos.x, (int)currentPos.y].GetComponent<BoardSegment>();
        //BoardSegment targetSegmant = board[(int)targetPos.x, (int)targetPos.y].GetComponent<BoardSegment>();
        GameObject peice = peices[(int)currentPos.x, (int)currentPos.y];

        peice.transform.position = Vector3.Lerp(currentSegmant.snapPointPlaced.position, currentSegmant.snapPointHover.position, 1);

        if (peice.transform.position == currentSegmant.snapPointHover.position)
        {

            return true;

        }
        else
        {

            return false;

        }


    }
    private bool EndHoverPeice(Vector2 currentPos, Vector2 peiceIndex)
    {
        BoardSegment currentSegmant = boardSpaces[(int)currentPos.x, (int)currentPos.y].GetComponent<BoardSegment>();
        //BoardSegment targetSegmant = board[(int)targetPos.x, (int)targetPos.y].GetComponent<BoardSegment>();
        GameObject peice = peices[(int)currentPos.x, (int)currentPos.y];

        peice.transform.position = Vector3.Lerp(currentSegmant.snapPointHover.position, currentSegmant.snapPointPlaced.position, 1);

        if (peice.transform.position == currentSegmant.snapPointHover.position)
        {

            return true;

        }
        else
        {

            return false;

        }


    }
    */

    /*private Vector3 LerpPeice(Vector3 currentPos, Vector3 targetPos)
    { 
    
    
    
    }*/

    public void DeconstructBoard() {
        if (boardSpaces == null) {
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
                    if (p) {
                        Destroy(p); //TODO: create a "clear board" function that just clears the play peices

                    }
                }
                else {
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

    public GameObject InstantiateSegmant(Vector3 pos) {

        //boardSegment = Resources.Load<GameObject>("Prefabs/BoardSegment") as GameObject; <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Success!!!!!!!!!

        return Instantiate(Resources.Load<GameObject>("Prefabs/BoardSegment"), pos, Quaternion.identity,transform) as GameObject;

    }

}
