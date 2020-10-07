using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using UnityEngine;



public struct BoardPOS
{
    public int indexX; //numerical position of ti
    public int indexY;

    public int packetIndex {
        get {
            return (indexY * 8) + (1 + indexX);
        }
    }

    private string _x;
    private string _y;


    public string x {
        get {

            return _x; //////////////////////////////////////////////crashes, don't use itwaitactueally
        
        }
        set {
            switch (value)
            {
                case "0":
                    _x = "A";
                    break;
                case "1":
                    _x = "B";
                    break;
                case "2":
                    _x = "C";
                    break;
                case "3":
                    _x = "D";
                    break;
                case "4":
                    _x = "E";
                    break;
                case "5":
                    _x = "F";
                    break;
                case "6":
                    _x = "G";
                    break;
                case "7":
                    _x = "H";
                    break;
                default:
                    _x = "Error: Invalid Index. x value beyond the scoope of 0-7";
                    break;
            }
        } 

    }
    public string y {
        get {
            return _y;
        }
        set {
            if (value == "0" || value == "1" || value == "2" || value == "3" || value == "4" || value == "5" || value == "6" || value == "7")
            {
                _y = value;

            }
            else {

                _y = "Error: Invalid Index. y value beyond the scoope of 0-7";
                
            }
        }
    }


    public BoardPOS(int X, int Y)
    {
        this.indexX = X;
        this.indexY = Y;
        _x = "";
        _y = "";
        this.x = X.ToString();
        this.y = Y.ToString();
    }

    public override string ToString()
    {
        return $"Grid Position {x}, {y}";
    }

}

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
public class BoardSegment : MonoBehaviour
{

    BoardPOS pos;
    public Transform snapPointPlaced;
    public Transform snapPointHover;
    public GameObject glow;
    public SegmentOccupationState state = SegmentOccupationState.Empty;
    private Material blackMat;
    private Material whiteMat;


    public void init(BoardPOS pos, Material blackMat, Material whiteMat /*, UnityAction callback*/)
    {
        this.pos = pos;
        this.blackMat = blackMat;
        this.whiteMat = whiteMat;
        
        //return PopulateStart();

    }

    public GameObject PopulateStart() {
        GameObject playerPeice = null;
        Material peiceMat;
        int player;
        bool isPlayer1 = false;

        

        if (pos.indexY > 5)
        {
            peiceMat = blackMat;
            player = 2;
            
        }
        else {
            peiceMat = whiteMat;
            player = 1;
            isPlayer1 = true;
        }


        if (pos.indexY == 1 || pos.indexY == 6)
        {
            //print("Spawning Pawn at " + pos.x + ", " + pos.y);
            playerPeice = HandleInstantiatePeice(peiceMat, player, isPlayer1, Peices.Pawn);

        }

        else if (pos.indexY == 0 || pos.indexY == 7) {

            //print("Spawning peice at " + pos.x + ", " + pos.y);
            switch (pos.indexX) {

                case 0:
                case 7:
                    playerPeice = HandleInstantiatePeice(peiceMat, player, isPlayer1, Peices.Rook);

                    break;
                case 1:
                case 6:
                    playerPeice = HandleInstantiatePeice(peiceMat, player, isPlayer1, Peices.Knight);
                    break;

                case 2:
                case 5:
                    playerPeice = HandleInstantiatePeice(peiceMat, player, isPlayer1, Peices.Bishop);
                    break;
                case 4:
                    playerPeice = HandleInstantiatePeice(peiceMat, player, isPlayer1, Peices.Queen);
                    break;

                case 3:
                    playerPeice = HandleInstantiatePeice(peiceMat, player, isPlayer1, Peices.King);
                    break;

                default:
                    //print error
                    break;
                
           
            }


        }



        




       return playerPeice;
    }

    private GameObject HandleInstantiatePeice(Material peiceMat, int player, bool isPlayer1, Peices peice)
    {

        GameObject playerPeice = InstantiatePeice();
       
        playerPeice.GetComponent<PlayerPeice>().init(snapPointPlaced, peice, peiceMat, player);
        
        switch (peice) {
            case Peices.Pawn:
                state = isPlayer1 ? SegmentOccupationState.P1Pawn : SegmentOccupationState.P2Pawn;
                break;
            case Peices.Rook:
                state = isPlayer1 ? SegmentOccupationState.P1Rook : SegmentOccupationState.P2Rook;
                break;
            case Peices.Knight:
                state = isPlayer1 ? SegmentOccupationState.P1Knight : SegmentOccupationState.P2Knight;
                break;
            case Peices.Bishop:
                state = isPlayer1 ? SegmentOccupationState.P1Bishop : SegmentOccupationState.P2Bishop;
                break;
            case Peices.Queen:
                state = isPlayer1 ? SegmentOccupationState.P1Queen : SegmentOccupationState.P2Queen;
                break;
            case Peices.King:
                state = isPlayer1 ? SegmentOccupationState.P1King : SegmentOccupationState.P2King;
                break;
            default:
                state = SegmentOccupationState.Empty;
                break;
        }
        
        return playerPeice;

    }

    public GameObject InstantiatePeice() {


        return Instantiate(Resources.Load<GameObject>("Prefabs/PlayerPeice"), snapPointPlaced.position, Quaternion.identity) as GameObject;


    }


    public void toggleGlow() {

        setGlowActive(!glow.activeSelf);
        
    }

    //,make private?
    public void setGlowActive(bool isActive = true) {

        glow.SetActive(isActive);

    }


   


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
