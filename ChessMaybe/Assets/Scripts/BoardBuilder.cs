using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;




public class BoardBuilder : MonoBehaviour
{

    public float offset = 0.2f;

    public Material blackMat;
    public Material whiteMat;

    private int boardSize = 8;
    //public GameObject boardSegment;

    public static GameObject[,]  board;
    public static GameObject[,] peices;
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



    public GameObject[,] BuildBoard() {

        if (board != null)
        {
            DeconstructBoard();
        }


        board = new GameObject[boardSize, boardSize];
        peices = new GameObject[boardSize, boardSize];

        int totalIterations = 0;
        float segmentSpaceing = 1 + offset;

        for (int y = 0; y < boardSize; y++) {

            for (int x = 0; x < boardSize; x++)
            {
                GameObject b = InstantiateSegmant(new Vector3((segmentSpaceing) *x, 0, (segmentSpaceing) * y));//spawn board peices
                board[x, y] = b; //store a refrence to the peice
                BoardSegment bs = b.GetComponent<BoardSegment>(); //
                bs.init(new BoardPOS(x, y),blackMat,whiteMat);
                GameObject peice = bs.PopulateStart();
                if (peice) {
                    peices[x, y] = peice;
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

        return board;
 
    }

    private void SwapToBlack(GameObject b)
    {
       //print("trying to colorchange");
        MeshRenderer m = b.GetComponent<MeshRenderer>();
        m.material = blackMat;
        //m.sharedMaterial.color = Color.black;
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
                GameObject p = peices[x, y];

                if (Application.isPlaying)
                {
                    Destroy(b);
                    if (p) {
                        Destroy(p);
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

        board = null;

    }

    public GameObject InstantiateSegmant(Vector3 pos) {

        //boardSegment = Resources.Load<GameObject>("Prefabs/BoardSegment") as GameObject; <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Success!!!!!!!!!

        return Instantiate(Resources.Load<GameObject>("Prefabs/BoardSegment"), pos, Quaternion.identity) as GameObject;

    }

}
