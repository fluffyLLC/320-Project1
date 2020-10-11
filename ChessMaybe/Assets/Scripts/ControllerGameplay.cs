using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenState
{
    Conection,
    Username,
    Game,

}

public class ControllerGameplay : MonoBehaviour
{
    public CameraController cameraController;
    public Board gameBoard;
    public ScreenState screenState = ScreenState.Conection;
    public Transform paneHostDetails;
    public Transform panelUsername;
    // Start is called before the first frame update
    void Start()
    {
        SwitchScreenState(ScreenState.Conection);
    }

    // Update is called once per frame
    void Update()
    {
        if (screenState == ScreenState.Game) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                //Debug.Log(hit.collider.gameObject.name);

                PlayerPeice peice = hit.collider.gameObject.GetComponentInParent<PlayerPeice>();

                Debug.Log(peice.peiceType + " " + peice.owner);
            
            
            
            
            }


        
        
        
        
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
