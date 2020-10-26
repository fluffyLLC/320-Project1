using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Button joinAsWhite;
    public Button joinAsBlack;
    public Button joinAsSpectator;

    public Text joinAsWhiteText;
    public Text joinAsBlackText;
    public Text joinAsSpectatorText;

    public ControllerGameplay game;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateState(string p1Username, string p2Username, int playerRole) {
       // print("we got" + p1Username + ", " + p2Username + ", " + playerRole);
        if (p1Username == "none" && p2Username == "none" && playerRole < 3) return;
        int quedPlayers = 0;


        if (p1Username != "none") {
            //joinAsWhite.interactable = false;
            joinAsWhite.enabled = false;
            joinAsWhiteText.text = $"White is {p1Username}";
            quedPlayers++;
            if (playerRole == 1) {

                joinAsBlack.interactable = false;
                joinAsBlackText.text = "Waiting For Player";
                joinAsSpectator.interactable = false;
                joinAsSpectatorText.text = "";

            }
        }

        if (p2Username != "none")
        {
            joinAsBlack.interactable = false;
            joinAsBlackText.text = $"Black is {p2Username}";
            quedPlayers++;

            if (playerRole == 2)
            {
                joinAsWhite.interactable = false;
                joinAsWhiteText.text = "Waiting For Player";
                joinAsSpectator.interactable = false;
                joinAsSpectatorText.text = "";

            }

        }

        if (playerRole == 3) {
            if (joinAsBlack.interactable) {
                joinAsBlackText.text = "Waiting For Player";
            }

            if (joinAsWhite.interactable) {
                joinAsWhiteText.text = "Waiting For Player";
            }

            //print("running on 3");

            joinAsBlack.interactable = false;
            joinAsWhite.interactable = false;
            joinAsSpectatorText.text = "You are a Spectator!";
            joinAsSpectator.interactable = false;

        }

        if (quedPlayers == 2 && playerRole > 0) {
            if (game.screenState == ScreenState.Lobby)
            {
                game.SwitchScreenState(ScreenState.Game);
            }
        }

    

    }





}
