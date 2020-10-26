using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PacketBuilder 
{

    public static Buffer Name(string username) {
        int packetLength = 5 + username.Length;
        Buffer packet = Buffer.Alloc(packetLength);

        packet.WriteString("NAME");
        packet.WriteUInt8((byte)username.Length, 4);
        packet.WriteString(username, 5);

        return packet;
    }
    public static Buffer Pass()
    {
        //int packetLength = 4;
        Buffer packet = Buffer.Alloc(4);

        packet.WriteString("PASS");
        //packet.WriteUInt8((byte)username.Length, 4);
        //packet.WriteString(username, 5);

        return packet;
    }


    public static Buffer Init(int desiredRole) {

        //int packetLength = 5 + username.Length;
        Buffer packet = Buffer.Alloc(5);

        packet.WriteString("INIT");
        packet.WriteUInt8((byte)desiredRole, 4);
        //packet.WriteString(username, 5);

        return packet;
    }



    public static Buffer Chat(string message) {
        int packetLength = 5 + message.Length;
        Buffer packet = Buffer.Alloc(packetLength);

        packet.WriteString("CHAT");
        packet.WriteUInt8((byte)message.Length, 4);
        packet.WriteString(message, 5);

        return packet;

    }


    public static Buffer Play(int currentX, int currentY, int targetX, int targetY)
    {
        //int packetLength = 5 + message.Length;
        Buffer packet = Buffer.Alloc(8);

        packet.WriteString("PLAY");
        packet.WriteUInt8((byte)currentX, 4);
        packet.WriteUInt8((byte)currentY, 5);
        packet.WriteUInt8((byte)targetX, 6);
        packet.WriteUInt8((byte)targetY, 7);
        //packet.WriteString(message, 5);

        return packet;

    }


    public static Buffer Hover(int x, int y) {

        Buffer packet = Buffer.Alloc(6);

        packet.WriteString("HOVR");
        packet.WriteUInt8((byte)x, 4);
        packet.WriteUInt8((byte)y, 5);


        return packet;

    }



}
