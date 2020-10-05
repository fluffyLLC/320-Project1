using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public enum SegmentOccupationState
{
    Empty,

    //Typically "black"
    P1Pawn,
    P1Rook,
    P1Knight,
    P1Bishop,
    P1Queen,
    P1King,

    //Typically "white"
    P2Pawn,
    P2Rook,
    P2Knight,
    P2Bishop,
    P2Queen,
    P2King,
}


public class BoardBuilder : MonoBehaviour
{

    public float offset = 0.2f;

    public Texture black;

    private int boardSize = 8;
    //public GameObject boardSegment;

    GameObject[,]  board;
    //ToDo: Generate an *x8 board
    //ToDo: Chanvge the color of the board segements
    //ToDo: Populate the game board with player peices


    // Start is called before the first frame update
    void Start()
    {
        BuildBoard();

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void BuildBoard() {
        if (board != null)
        {
            DeconstructBoard();
        }

        board = new GameObject[boardSize, boardSize];

        int totalIterations = 0;

        for (int y = 0; y < boardSize; y++) {

            for (int x = 0; x < boardSize; x++)
            {
                GameObject b = InstantiateSegmant(new Vector3((1+offset)*x, 0, (1 + offset) * y));
                board[x, y] = b;
                BoardSegment bs = b.GetComponent<BoardSegment>();
                bs.init(new BoardPOS(x, y));
                //print(bs);

                if (y % 2 != 0 && y != 0)
                {
                    if (totalIterations % 2 == 0)
                    {
                        SwapToBlack(b);
                    }
                }
                else {
                    if (totalIterations % 2 != 0 && totalIterations != 0)
                    {
                        SwapToBlack(b);
                    }
                }


                totalIterations++;


            }

        }
 
    }

    private static void SwapToBlack(GameObject b)
    {
        print("trying to colorchange");
        MeshRenderer m = b.GetComponent<MeshRenderer>();
        m.material.color = Color.black;
    }

    public void DeconstructBoard() {
        if (board == null) {
            return;
        }


        for (int y = 0; y < boardSize; y++)
        {

            for (int x = 0; x < boardSize; x++)
            {
                //GameObject b = InstantiateSegmant(new Vector3((1 + offset) * x, 0, (1 + offset) * y));
                GameObject b = board[x, y];
                if (Application.isPlaying)
                {
                    Destroy(b);
                }
                else {
                    DestroyImmediate(b);
                }

            }

        }

        board = null;

    }

    public GameObject InstantiateSegmant(Vector3 pos) {

        //boardSegment = Resources.Load<GameObject>("Prefabs/BoardSegment") as GameObject; <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Success!!!!!!!!!

        return Instantiate(Resources.Load<GameObject>("Prefabs/BoardSegment"), pos, Quaternion.identity) as GameObject;

    }

}
