using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum ScreenState
{
    Conection,
    Username,
    Game,

}

public class ControllerGameplay : MonoBehaviour
{

    public LayerMask boardMask;
    public LayerMask peiceMask;

    public CameraController cameraController;
    public Board gameBoard;

    public ScreenState screenState = ScreenState.Conection;

    public Transform paneHostDetails;
    public int playerState; // are we player 1 player 2 or a spectator
    public bool peiceSelected = false;

    public Transform panelUsername;



    public bool isMyTurn = false;
    PlayerPeice peice;
    BoardSegment segment;

    Vector2 selectedPeice = new Vector2(-1,-1);
    //bool preformingMove;

    // Start is called before the first frame update
    void Start()
    {
        SwitchScreenState(ScreenState.Conection);
    }

    // Update is called once per frame
    void Update()
    {
        if (screenState == ScreenState.Game && playerState != 3)
        {
            //print("Looping");
            if (!Input.GetMouseButton(2) || !Input.GetMouseButton(1))//if we're not using the camera
            {
                
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //print("Casting");
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 50000, peiceMask))
                    {
                        //print("castHit");
                        PlayerPeice p = hit.collider.gameObject.GetComponentInParent<PlayerPeice>();

                        if (p.owner == playerState)
                        {

                            if (p == peice)
                            {
                                peiceSelected = false;
                                peice = null;
                            }
                            else
                            {

                                peice = p;
                                ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Hover((int)peice.acessIndex.x, (int)peice.acessIndex.y));
                                peiceSelected = true;
                            }
                        }


                        /*
                        //Debug.Log(hit.collider.gameObject.name);
                        peice = hit.collider.gameObject.GetComponentInParent<PlayerPeice>();
                        segment = hit.collider.gameObject.GetComponent<BoardSegment>();


                        if (IsPeice(peice, segment)) {

                            ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Hover((int)peice.acessIndex.x, (int)peice.acessIndex.y));
                        }
                        //Debug.Log(peice.peiceType + " " + peice.owner);
                        */


                    }
                    else if (Physics.Raycast(ray, out hit, 50000, boardMask) && peiceSelected) { 
                        
                        segment = hit.collider.gameObject.GetComponent<BoardSegment>();

                        ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Play((int)peice.acessIndex.x, (int)peice.acessIndex.y, segment.pos.indexX, segment.pos.indexY));



                    }//raycasthit


                }
            }//aren't using camera controlls
        }//isGameState

        if (peiceSelected) {
            ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Hover((int)peice.acessIndex.x, (int)peice.acessIndex.y));
            HoverPeice((int)peice.acessIndex.x, (int)peice.acessIndex.y);
        }
    }//update

    private bool IsPeice(PlayerPeice peice, BoardSegment segment)
    {
        if (peice)
        {

            return true;

        }
        else
        {
            
            if (segment)
            {

                if (segment.state == SegmentOccupationState.Empty)
                {
                    this.peice = Board.peices[segment.pos.indexX, segment.pos.indexY].GetComponent<PlayerPeice>();
                }
               

                if (this.peice)
                {
                    return false;

                }
                else
                {
                    return true;

                }
            }
            
            //return false;
             
        }

        return false;
    }

    public void HoverPeice(int x, int y) {
        //Vector3 hoverTarget = segment.snapPointHover.position;//Board.boardSpaces[(int)peice.acessIndex.x, (int)peice.acessIndex.y].GetComponent<BoardSegment>().snapPointHover.position
        //BoardSegment segment = 

        PlayerPeice peice = Board.peices[x, y].GetComponent<PlayerPeice>();
        Vector3 hoverTarget = Board.boardSpaces[x, y].GetComponent<BoardSegment>().snapPointHover.position;
        
        peice.HoverUp(hoverTarget);
        
    }

    public void ProcessUpdate(int gameStatus, int whoseTurn, byte[] spaces) {

        if (gameBoard.HandleMove(spaces)) {
            peiceSelected = false;
            peice = null;
            segment = null;
        }


        
        
    
        
    }

    public void SwitchScreenState(ScreenState state)
    {
        switch (state)
        {
            case ScreenState.Conection:
                screenState = ScreenState.Conection;
                paneHostDetails.gameObject.SetActive(true);
                panelUsername.gameObject.SetActive(false);
                gameBoard.DeconstructBoard();
                cameraController.pauseCamRig = true;
                break;
            case ScreenState.Username:
                screenState = ScreenState.Username;
                paneHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(true);
                gameBoard.DeconstructBoard();
                cameraController.pauseCamRig = true;
                break;
            case ScreenState.Game:
                screenState = ScreenState.Game;
                paneHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                gameBoard.BuildBoard();
                cameraController.pauseCamRig = false;
                break;
            default:
                print("ERROR: Screen state not available");
                break;
        }
    }
}
