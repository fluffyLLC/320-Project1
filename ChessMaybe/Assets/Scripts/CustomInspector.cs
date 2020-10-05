using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardBuilder))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        BoardBuilder boardBuilder = (BoardBuilder)target;
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Board")) {
            boardBuilder.BuildBoard();
        }

        if (GUILayout.Button("Destroy Board"))
        {
            boardBuilder.DeconstructBoard();
        }

        GUILayout.EndHorizontal();


    }
}
