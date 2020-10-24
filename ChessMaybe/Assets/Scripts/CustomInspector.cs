using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        Board board = (Board)target;
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Build Board (dissabled)")) {
            //board.BuildBoard();
        }

        if (GUILayout.Button("Destroy Board"))
        {
            board.DeconstructBoard();
        }

        GUILayout.EndHorizontal();


    }
}
