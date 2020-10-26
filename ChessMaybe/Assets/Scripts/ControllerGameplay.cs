using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum ScreenState
{
    Conection,
    Username,
    Game,
    Lobby,

}

public class ControllerGameplay : MonoBehaviour
{

    //public LayerMask boardMask;
    //public LayerMask peiceMask;

    public CameraController cameraController;
    public Board gameBoard;

    public ScreenState screenState = ScreenState.Conection;

    public Transform paneHostDetails;
    public Transform panelUsername;
    public Transform panelLobby;
    public Transform playUITransform;


    public PlayUI playUI;
    public LobbyUI lobbyUI;

    public int playerState = 0; // are we player 1 player 2 or a spectator (1, 2, and 3 respectivly)
    public bool peiceSelected = false;
    public bool isMyTurn = false;

    PlayerPeice peice;

    public string p1Username = "none";
    public string p2Username = "none";
    //BoardSegment segment;

    Vector2 selectedPeice = new Vector2(-1, -1);
    //bool preformingMove;

    // Start is called before the first frame update
    void Start()
    {
        SwitchScreenState(ScreenState.Conection);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState == 0) return;
        //print(playerState);
        if (screenState == ScreenState.Game && playerState != 3)
        {
            //print("Looping");
            if (!Input.GetMouseButton(2) || !Input.GetMouseButton(1))//if we're not using the camera
            {

                if (Input.GetMouseButtonDown(0))
                {
                    HandleSelection();

                }
            }//aren't using camera controlls
        }//isGameState

        if (peiceSelected) {
            ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Hover((int)peice.acessIndex.x, (int)peice.acessIndex.y));
            HoverPeice((int)peice.acessIndex.x, (int)peice.acessIndex.y);
        }

    }//update

    private void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //print("Casting");
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))//, 50000, peiceMask))
        {
            PlayerPeice p;
            BoardSegment b = null;

            //bool isPeice = true;

            //print("castHit");
            GameObject obj = hit.collider.gameObject;
            if (obj != null)
            {
                p = obj.GetComponentInParent<PlayerPeice>();

                b = obj.GetComponentInParent<BoardSegment>();

                if (p == null && b == null)//if it's not a player peice
                {

                    return;

                }



            }
            else {
                return;
            }



            if (p)
            {
                if (p.owner == playerState)
                {

                    if (p == peice) //we have clicked on our selected peice again
                    {
                        peiceSelected = false;
                        peice = null;
                    }
                    else //we have clicked on one of our peices
                    {
                        peice = p;
                        ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Hover((int)peice.acessIndex.x, (int)peice.acessIndex.y));
                        peiceSelected = true;

                    }

                }
                else if (peiceSelected) { //we have a piece selected and we are clicking on the opponents peice

                    SendMoveToServer((int)p.acessIndex.x, (int)p.acessIndex.y);

                }


            }
            else if (peiceSelected && b) { //we have selected a segmant and we have a piece selected

                SendMoveToServer(b.pos.indexX, b.pos.indexY);

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
        /*
                   else if (Physics.Raycast(ray, out hit, 50000, boardMask) && peiceSelected) { 

                       segment = hit.collider.gameObject.GetComponent<BoardSegment>();

                       



                   }//raycasthit
                   */
    }

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

    public void SendMoveToServer(int targetX, int targetY) {
        //print("Moving to: " + targetX + ", " + targetY);
        if (peice)
        {

            ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Play((int)peice.acessIndex.x, (int)peice.acessIndex.y, targetX, targetY));

        }

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
            // segment = null;
        }


    }


    public void SendChat(string message) {

        ControllerGameClient.singleton.SendPacketToServer(PacketBuilder.Chat(message));

    }

    public void ReturnToConnection() {
        SwitchScreenState(ScreenState.Conection);
        ControllerGameClient.singleton.Disconnect();

    }

    public void AddMessageToChatDisplay(string message) {
        playUI.AddMessageToChat(message);
    }

    public void ProcessInit() {
        playUI.whiteUserDisplay.text = $"White is {p1Username}";
        playUI.blackUserDisplay.text = $"Black is {p2Username}";
        lobbyUI.UpdateState(p1Username, p2Username, playerState);
    
    }


    public void SwitchScreenState(ScreenState state)
    {
        switch (state)
        {
            case ScreenState.Conection:
                screenState = ScreenState.Conection;
                paneHostDetails.gameObject.SetActive(true);
                panelUsername.gameObject.SetActive(false);
                panelLobby.gameObject.SetActive(false);
                playUITransform.gameObject.SetActive(false);
                gameBoard.DeconstructBoard();
                cameraController.pauseCamRig = true;
                break;
            case ScreenState.Username:
                screenState = ScreenState.Username;
                paneHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(true);
                panelLobby.gameObject.SetActive(false);
                playUITransform.gameObject.SetActive(false);
                gameBoard.DeconstructBoard();
                cameraController.pauseCamRig = true;
                break;
            case ScreenState.Lobby:
                screenState = ScreenState.Username;
                paneHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelLobby.gameObject.SetActive(true);
                //ProcessInit();
                playUITransform.gameObject.SetActive(false);
                gameBoard.DeconstructBoard();
                cameraController.pauseCamRig = true;
                break;
            case ScreenState.Game:
                screenState = ScreenState.Game;
                paneHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelLobby.gameObject.SetActive(false);
                playUITransform.gameObject.SetActive(true);
                //gameBoard.BuildBoard();
                if (playerState == 2)
                {
                    cameraController.ChangeRotation(30, 180);
                }
                else if (playerState == 3)
                {
                    cameraController.ChangeRotation(30, 90);
                }
                cameraController.pauseCamRig = false;
                break;
            default:
                print("ERROR: Screen state not available");
                break;
        }
    }
}
