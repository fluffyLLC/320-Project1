using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

using TMPro;




public class ControllerGameClient : MonoBehaviour
{

    static public ControllerGameClient singleton;
    // Start is called before the first frame update

    TcpClient socket = new TcpClient();

    Buffer buffer = Buffer.Alloc(0);

    public TMP_InputField inputHost;
    public TMP_InputField inputPort;
    public TMP_InputField inputUsername;

    public ControllerGameplay controllerGameplay;

    
    //public GameplayController panelGameplay;

    void Start()
    {
        buffer.validPacketIdentifyers = new string[] {"JOIN", "UPDT", "CHAT"};
        
        //implimented singleton design pattern
        if (singleton)
        {
            Destroy(gameObject); //there's already one out there.....
        }
        else {
            singleton = this;
            DontDestroyOnLoad(gameObject); //don't destroy when you load a new scene

            //controllerGameplay.SwitchScreenState(ScreenState.Conection);
            
        }

        
        

    }

    public void onButtonConnect() {
        string host = inputHost.text;

        UInt16.TryParse(inputPort.text, out ushort port);

        TryToConnect(host, port);
        

    }

    async public void TryToConnect(string host, int port) {

        if (socket.Connected) return;
        try
        {

            await socket.ConnectAsync(host, port); // neeed async to use the await keyword
            StartRecevingPackets();
            controllerGameplay.SwitchScreenState(ScreenState.Username);

        }
        catch (Exception e) {

            print("FAILED TO CONNECT... " + e);

        }

    }

    public void OnNameSubmit() { //come back to this

        string user = inputUsername.text;

        Buffer packet = PacketBuilder.Join(user);

        SendPacketToServer(packet);
    
    }

    async private void StartRecevingPackets()
    {
        int maxPacketSize = 4096;


        while (socket.Connected) {

            byte[] data = new byte[maxPacketSize];

            try
            {
                print("reading");
                int bytesRead = await socket.GetStream().ReadAsync(data, 0, maxPacketSize);

                buffer.Concat(data,bytesRead);

                ProcessPackets();
            }
            catch (Exception e) {

                print(e);
            }
        
        
        }
    }

    void ProcessPackets() {
        if (buffer.Length < 4) return; // Not enough data in buffer
        //print("Buffer Contents: " + buffer);


        string packetIdentifier = buffer.ReadString(0, 4);

       // print("packet Identifyer: " + packetIdentifier);
        

        switch (packetIdentifier) {
            case "JOIN":
                if (buffer.Length < 5) return;
                byte joinResponse = buffer.ReadUInt8(4);

                if (joinResponse == 1 || joinResponse == 2 || joinResponse == 3)
                {

                    controllerGameplay.playerState = joinResponse;
                    controllerGameplay.SwitchScreenState(ScreenState.Game);
                }
                else if (joinResponse == 9) {
                    controllerGameplay.SwitchScreenState(ScreenState.Conection);
                } else {
                    //TODO: show error
                    controllerGameplay.SwitchScreenState(ScreenState.Username);
                    inputUsername.text = "";
                    print(joinResponse);
                }

                buffer.Consume(5);
                break;
            case "UPDT":
                if (buffer.Length < 70) return; //TODO: give players the ability to pass

                print("update recieved");

                byte whoseTurn = buffer.ReadUInt8(4);
                byte gameStatus = buffer.ReadUInt8(5);

                byte[] spaces = new byte[64];

                for (int i = 0; i < 64; i++) {

                    spaces[i] = buffer.ReadUInt8(6 + i);
                
                }

                print(spaces);


                controllerGameplay.ProcessUpdate(gameStatus, whoseTurn, spaces);

                if (controllerGameplay.screenState != ScreenState.Game) {
                    controllerGameplay.SwitchScreenState(ScreenState.Game);
                }
               


               // panelGameplay.UpdateFromServer(gameStatus, whoseTurn, spaces);

                buffer.Consume(70);

                break;
            case "MOVE":
                if (buffer.Length < 5) return;

                byte errorCode = buffer.ReadUInt8(4);

                switch (errorCode) {
                    case 0:
                        print("Move Is Legal");
                        break;

                    case 1:
                        print("Move invalid, Move is off of the board");
                        break;
                    case 2:
                        print("Move invalid, Player does not own peice");
                        break;

                    case 3:
                        print("Move invalid, Board space Is Empty");
                        break;

                    case 4:
                        print("Move invalid, Player is stepping on themselves");
                        break;

                    case 5:
                        print("Move invalid, It is not the current players turn");
                        break;

                    case 6:
                        print("Move invalid, Move is not valid for the type of peice");
                        break;

                    case 7:
                        print("Move invalid, Path is blocked by one of the player's peices");
                        break;
                    case 8:
                        print("Move invalid, Move is not valid for an unspecified reason");
                        break;

                }

                
                buffer.Consume(5);

                break;
            case "CHAT":
                print("chat recived");
                if (buffer.Length <= 6) return;
                
                byte usernameLength = buffer.ReadUInt8(4);

                ushort messageLength = buffer.ReadUInt8(5);

                int fullPacketLength = 6 + usernameLength + messageLength;

                if (buffer.Length < fullPacketLength) return;

                string username = buffer.ReadString(6, usernameLength);
                string chatMessage = buffer.ReadString(6 + usernameLength, messageLength);

                //print(chatMessage);

                string message = $"{username}: {chatMessage}\n";

                controllerGameplay.AddMessageToChatDisplay(message);

                //panelGameplay.gameObject.SetActive(true);

                //TODO: consume data
                buffer.Consume(fullPacketLength);
                break;
            case "HOVR":

                if (buffer.Length <= 5) return;

                HandleHover();

                break;
            default:
                print("unknown packet identifyer...");

                //TODO: clear buffer

                break;

        }
    
    }

    private void HandleHover()
    {
        byte x = buffer.ReadUInt8(4);
        byte y = buffer.ReadUInt8(5);

        if (controllerGameplay.isMyTurn)
        {
            //TODO: processBitmask
        }
        else
        {
            controllerGameplay.HoverPeice(x, y);
            buffer.Consume(6);
        }
    }

    public void Disconnect() {

        socket.Close(); //.Dispose();
    }

    async public void SendPacketToServer(Buffer packet) {

        if (!socket.Connected) return;
        //print("sending");

        await socket.GetStream().WriteAsync(packet.bytes, 0, packet.bytes.Length);

    
    }

    public void SendPlayPacket(int x, int y) {
        //Buffer packet = PacketBuilder.Play(x, y);

        //SendPacketToServer(PacketBuilder.Play(x, y));
        //packet.WriteString("PLAY", 0);
        


    }




}
