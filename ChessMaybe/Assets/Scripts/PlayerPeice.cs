using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Peices { 
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King,
    Empty
}




public class PlayerPeice : MonoBehaviour
{

    public static string[] meshPath = { "Mesh/Chess_Centered_Pawn", "Mesh/Chess_Centered_Rook", "Mesh/Chess_Centered_Knight", "Mesh/Chess_Centered_Bishop", "Mesh/Chess_Centered_Queen", "Mesh/Chess_Centered_King" };
    public GameObject mesh;
    public Peices peiceType;
    public int owner = 1;
    


    public void init(Transform location, Peices peice, Material mat, int owner){
        peiceType = peice;
        this.owner = owner;
        Quaternion rot = owner == 1 ? Quaternion.identity : Quaternion.Euler(0,180,0);
        //print(mat.name);
        switch (peice)
        {
            case Peices.Pawn:
                HandleInstantiateMesh(mat, rot, Peices.Pawn);
                break;
            case Peices.Rook:
                HandleInstantiateMesh(mat, rot, Peices.Rook);
                break;
            case Peices.Knight:
                HandleInstantiateMesh(mat, rot, Peices.Knight);
                break;
            case Peices.Bishop:
                HandleInstantiateMesh(mat, rot, Peices.Bishop);
                break;
            case Peices.Queen:
                HandleInstantiateMesh(mat, rot, Peices.Queen);
                break;
            case Peices.King:
                HandleInstantiateMesh(mat, rot, Peices.King);
                break;
            default:
                
                break;
        }



    }

    private void HandleInstantiateMesh(Material mat, Quaternion rot, Peices peice)
    {
        mesh = InstantiateMesh(meshPath[(int)peice], transform, rot);
        mesh.AddComponent<CapsuleCollider>();
 
       MeshRenderer matAcess =  mesh.GetComponent<MeshRenderer>();
        // print("Before Name: " + matAcess.materials[1].name);
        if (matAcess != null)
        {
            //print("we got acess bois");
            matAcess.material = mat;
        }

    }




    /*
    public GameObject InstantiateMesh(string path, Transform pos)
    {

        return InstantiateMesh(path, pos, Quaternion.identity);

    }
    */



    public GameObject InstantiateMesh(string path, Transform parent, Quaternion rotation)
    {

        return Instantiate(Resources.Load<GameObject>(path), parent.position, rotation,parent) as GameObject;

    }



}
