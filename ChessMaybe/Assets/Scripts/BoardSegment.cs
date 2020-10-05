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
        
    public string x {
        get {

            return this.x;
        
        }
        set {
            switch (value)
            {
                case "0":
                    x = "A";
                    break;
                case "1":
                    x = "B";
                    break;
                case "2":
                    x = "C";
                    break;
                case "3":
                    x = "D";
                    break;
                case "4":
                    x = "E";
                    break;
                case "5":
                    x = "F";
                    break;
                case "6":
                    x = "G";
                    break;
                case "7":
                    x = "H";
                    break;
                default:
                    x = "Error: Invalid Index. x value beyond the scoope of 0-7";
                    break;
            }
        } 

    }
    public string y {
        get {
            return this.y;
        }
        set {
            if (value == "0" || value == "1" || value == "2" || value == "3" || value == "4" || value == "5" || value == "6" || value == "7")
            {
                y = value;

            }
            else {

                y = "Error: Invalid Index. y value beyond the scoope of 0-7";
                
            }
        }
    
    
    
    
    }



    public BoardPOS(int X, int Y)
    {
        this.indexX = X;
        this.indexY = Y;
        //this.x = X.ToString();
        //this.y = Y.ToString();
    }

    public override string ToString()
    {
        return $"Grid Position {indexX}, {indexY}";
    }

}


public class BoardSegment : MonoBehaviour
{

    BoardPOS pos;
    public Transform SnapPointPlaced;
    public Transform SnapPointHover;
    public Transform Glow;


    public void init(BoardPOS pos /*, UnityAction callback*/)
    {
        this.pos = pos;
        //Button bttn = GetComponent<Button>();

        //bttn.onClick.AddListener(callback);

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
