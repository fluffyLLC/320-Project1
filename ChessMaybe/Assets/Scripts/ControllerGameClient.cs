﻿using System;
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

            controllerGameplay.SwitchScreenState(ScreenState.Conection);
            
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
               // print("reading");
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
            
                //TODO: change which screen we are looking at

                //TODO: consume data

                buffer.Consume(5);
                break;
            case "UPDT":
                if (buffer.Length < 15) return;
                print("update recieved");

                byte whoseTurn = buffer.ReadUInt8(4);
                byte gameStatus = buffer.ReadUInt8(5);

                byte[] spaces = new byte[9];

                for (int i = 0; i < 9; i++) {

                    spaces[i] = buffer.ReadUInt8(6 + i);
                
                }

                controllerGameplay.SwitchScreenState(ScreenState.Game);
               


               // panelGameplay.UpdateFromServer(gameStatus, whoseTurn, spaces);

                

                //TODO: consume data
                buffer.Consume(15);

                break;
            case "CHAT":

                if (buffer.Length <= 7) return;
                
                byte usernameLength = buffer.ReadUInt8(4);

                ushort messageLength = buffer.ReadUInt16BE(5);

                int fullPacketLength = 7 + usernameLength + messageLength;

                if (buffer.Length < fullPacketLength) return;

                string username = buffer.ReadString(7, usernameLength);
                string chatMessage = buffer.ReadString(7 + usernameLength, messageLength);

                
                //panelGameplay.gameObject.SetActive(true);

                //TODO: consume data
                buffer.Consume(fullPacketLength);
                break;
            case "HOVR":
                if (buffer.Length <= 5) return;

                byte x = buffer.ReadUInt8(4);
                byte y = buffer.ReadUInt8(5);

                if (controllerGameplay.isMyTurn)
                {
                    //TODO: processBitmask
                }
                else {
                    controllerGameplay.HoverPeice(x, y);
                    buffer.Consume(6);
                }
                

                break;
            default:
                print("unknown packet identifyer...");

                //TODO: clear buffer

                break;

        }
    
    }


    async public void SendPacketToServer(Buffer packet) {

        if (!socket.Connected) return;
        print("sending");

        await socket.GetStream().WriteAsync(packet.bytes, 0, packet.bytes.Length);

    
    }

    public void SendPlayPacket(int x, int y) {
        //Buffer packet = PacketBuilder.Play(x, y);

        //SendPacketToServer(PacketBuilder.Play(x, y));
        //packet.WriteString("PLAY", 0);
        


    }




}
