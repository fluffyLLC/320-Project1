using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    // Start is called before the first frame update
    //public Transform chatPanel;
    bool chatActive = true;
    public RectTransform chatPanelRect;
    public float chatAnimTime = 1;
    public Text chatButtonText;

    //public LTRect chatPanelLTRect;
    bool ppActive = true;
    public RectTransform playOptionsPanel;
    public float ppAnimTime = 1;
    public Text ppButtonText;

    public ControllerGameplay game;

    public TMP_InputField message;
    public TextMeshProUGUI chatDisplay;

    public Text whiteUserDisplay; //My need to change type
    public Text blackUserDisplay;


    

    
    void Start()
    {
        ToggleChat();
        TogglePlayOptions();
       // chatPanelLTRect = (LTRect)chatPanelRect;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */

        //ToDo:Add Smoothing
    public void ToggleChat() {

        if (chatActive)
        {
            LeanTween.move(chatPanelRect, new Vector3(-Screen.width * 0.3f, 0, 0), chatAnimTime); //moveX(chatPanel.gameObject, 608, 5);
            chatButtonText.text = ">";

        }
        else {

            LeanTween.move(chatPanelRect, Vector3.zero, chatAnimTime);
            chatButtonText.text = "<";

        }

        chatActive = !chatActive;

    }
    public void TogglePlayOptions()
    {

        if (ppActive)
        {
            LeanTween.move(playOptionsPanel, new Vector3(-180, -75, 0), ppAnimTime); //moveX(chatPanel.gameObject, 608, 5);
            ppButtonText.text = ">";

        }
        else
        {

            LeanTween.move(playOptionsPanel, new Vector3(-180, 75, 0), ppAnimTime);
            ppButtonText.text = "<";

        }

        ppActive = !ppActive;

    }

    public void SendChat() {

        game.SendChat(message.text);

        message.text = "";
        message.Select();
    }

    public void AddMessageToChat(string chatMessage) {
        chatDisplay.text += chatMessage;
    
    }
}
