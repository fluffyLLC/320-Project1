using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public struct GridPOS {
    public int X;
    public int Y;
    public GridPOS(int X, int Y) {
        this.X = X;
        this.Y = Y;
    }

    public override string ToString()
    {
        return $"Grid Position {X}, {Y}";
    }

}

public class ButtonXO : MonoBehaviour
{
    // Start is called before the first frame update

    public GridPOS pos;
    public void init(GridPOS pos,UnityAction callback)
    {
        this.pos = pos;
        Button bttn = GetComponent<Button>();

        bttn.onClick.AddListener( callback );

    }

    public void ButtonClicked()
    {
        print("I've been clicked");
    }

}
